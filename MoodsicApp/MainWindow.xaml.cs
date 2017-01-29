using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Collections.Generic;
using System.Windows.Media;
using Microsoft.Expression.Encoder.Devices;
using Microsoft.ProjectOxford.Emotion;
using Microsoft.ProjectOxford.Emotion.Contract;
using Microsoft.ProjectOxford.Common;
using Microsoft.Win32;
using WebcamControl;

using System.Windows.Controls;
using System.Windows.Data;


namespace MoodsicApp
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private String m_imagePath;
        private String m_picturesDefaultPath;
        private String m_apiKey = "1487efd373034a61b500849db503e8f1";
        private System.Windows.Forms.Timer m_timer;

        private const int kInterval = 5000;

        public MainWindow()
        {
            m_imagePath = "";
            InitializeComponent();

            Binding binding_1 = new Binding("SelectedValue");
            binding_1.Source = VideoDevicesComboBox;
            WebcamCtrl.SetBinding(Webcam.VideoDeviceProperty, binding_1);

            m_picturesDefaultPath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

            WebcamCtrl.ImageDirectory = m_picturesDefaultPath;
            m_picturesDefaultPath += @"\snapshot.jpg";
            WebcamCtrl.FrameRate = 30;
            WebcamCtrl.FrameSize = new System.Drawing.Size(640, 480);

            var vidDevices = EncoderDevices.FindDevices(EncoderDeviceType.Video);
            VideoDevicesComboBox.ItemsSource = vidDevices;
            VideoDevicesComboBox.SelectedIndex = 0;

            startCapturing();
        }

        private void startCapturing()
        {
            try
            {
                // Display webcam video
                WebcamCtrl.StartPreview();
            }
            catch (Microsoft.Expression.Encoder.SystemErrorException ex)
            {
                MessageBox.Show("Device is in use by another application");
            }

            m_timer = new System.Windows.Forms.Timer();
            m_timer.Tick += new EventHandler(Timer_handle);
            m_timer.Interval = kInterval;
            m_timer.Start();
        }

        private void TakeSnapshotButton_Click(object sender, RoutedEventArgs e)
        {
            // Take snapshot of webcam video.
            WebcamCtrl.TakeSnapshot();
            m_imagePath = m_picturesDefaultPath;
            scanAndPlay();
        }

        private void pathButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Images|*.jpg;*.png";

            bool? result = dialog.ShowDialog(this);

            if (!(bool) result)
            {
                return;
            }

            m_imagePath = dialog.FileName;
            this.pathBox.Text = m_imagePath;
            scanAndPlay();
        }

        private void Timer_handle(object sender, EventArgs e)
        {
            if (!(bool) this.useWebcam.IsChecked)
            {
                return;
            }
            WebcamCtrl.TakeSnapshot();
            m_imagePath = m_picturesDefaultPath;
            scanAndPlay();
        }

        private async void scanAndPlay()
        {
            Emotion[] emotionResult = await UploadAndDetectEmotions();
            LogEmotionResult(emotionResult);
            DetectedResult emotion = selectEmotion(emotionResult);
            Mood mood = emotion.toMood();

            // Get 2nd API mood
            String[] songs = SongLoader.GetMusic(((int)mood).ToString());
            foreach (String song in songs)
            {
                String s = SongLoader.GetYoutubeId(song);
                if (s != null)
                    SongLoader.DownloadVideo(s);
            }
        }

        private async Task<Emotion[]> UploadAndDetectEmotions()
        {
            Log("Using " + m_imagePath + " file");

            EmotionServiceClient client = new EmotionServiceClient(m_apiKey);
            Log("Calling EmotionServiceClient.RecognizeAsync()...");

            try
            {
                using (Stream imageFileStream = File.OpenRead(m_imagePath))
                {
                    // Detect the emotions
                    return await client.RecognizeAsync(imageFileStream);
                }
            }
            catch (Exception exception)
            {
                this.Log(exception.ToString());
                return null;
            }
        }

        private DetectedResult selectEmotion(Emotion[] emotionResult)
        {
            DetectedResult res = new DetectedResult();
            
            // Select the biggest rectangle
            if (emotionResult.Length <= 0 || emotionResult == null)
            {
                res.emotion = EmotionEnum.NONE;
                res.value = -1;
                return res;
            }

            Scores faceResult = null;
            int space = -1;
            foreach(Emotion emotion in emotionResult)
            {
                Rectangle fRect = emotion.FaceRectangle;
                int auxSpace = fRect.Width + fRect.Height;

                if (auxSpace > space)
                {
                    space = auxSpace;
                    faceResult = emotion.Scores;
                }
            }

            List<float> values = new List<float>(8);

            values.Add(faceResult.Anger);
            values.Add(faceResult.Contempt);
            values.Add(faceResult.Disgust);
            values.Add(faceResult.Fear);
            values.Add(faceResult.Happiness);
            values.Add(faceResult.Neutral);
            values.Add(faceResult.Sadness);
            values.Add(faceResult.Surprise);

            List<DetectedResult> emotionResults = new List<DetectedResult>(values.Count);
            for (int i = 0; i < values.Count; ++i)
            {
                emotionResults.Add(new DetectedResult((EmotionEnum)(i + 1), values[i]));
            }
            emotionResults.Sort();


            int j = emotionResults.Count - 1;
            analysisResult.Content = "Detected emotion: " + emotionResults[j].emotion.ToString() + 
                "  Value: " + emotionResults[j].value.ToString();
            Log("Detected emotion: " + emotionResults[j].emotion.ToString());

            bool found = false;
            while (!found && j >= emotionResults.Count - 3)
            {
                res = emotionResults[j];
                if (res.emotion != EmotionEnum.CONTEMPT && res.emotion != EmotionEnum.DISGUST && 
                    res.emotion != EmotionEnum.FEAR)
                {
                    found = true;
                }
                --j;
            }

            return res;
        }

        private void Log(String text)
        {
            Console.WriteLine(text);
        }

        private void LogEmotionResult(Emotion[] emotions)
        {
            int emotionResultCount = 0;
            if (emotions == null || emotions.Length <= 0)
            {
                Log("No emotion is detected. This might be due to:\n" +
                    "    image is too small to detect faces\n" +
                    "    no faces are in the images\n" +
                    "    faces poses make it difficult to detect emotions\n" +
                    "    or other factors");
                return;
            }
            foreach (Emotion emotion in emotions)
            {
                Log("Emotion[" + emotionResultCount + "]");
                Log("  .FaceRectangle = left: " + emotion.FaceRectangle.Left
                            + ", top: " + emotion.FaceRectangle.Top
                            + ", width: " + emotion.FaceRectangle.Width
                            + ", height: " + emotion.FaceRectangle.Height);

                Log("  Anger    : " + emotion.Scores.Anger.ToString());
                Log("  Contempt : " + emotion.Scores.Contempt.ToString());
                Log("  Disgust  : " + emotion.Scores.Disgust.ToString());
                Log("  Fear     : " + emotion.Scores.Fear.ToString());
                Log("  Happiness: " + emotion.Scores.Happiness.ToString());
                Log("  Neutral  : " + emotion.Scores.Neutral.ToString());
                Log("  Sadness  : " + emotion.Scores.Sadness.ToString());
                Log("  Surprise : " + emotion.Scores.Surprise.ToString());
                Log("");
                emotionResultCount++;
            }
        }


    }
}
