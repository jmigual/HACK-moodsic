using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Microsoft.Expression.Encoder.Devices;
using Microsoft.ProjectOxford.Emotion;
using Microsoft.ProjectOxford.Emotion.Contract;
using Microsoft.Win32;
using WebcamControl;

using System.Windows.Controls;
using System.Windows.Data;

//using AForge.Video;
//using AForge.Video.DirectShow;

namespace MoodsicApp
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private String imagePath;
        private String apiKey = "1487efd373034a61b500849db503e8f1";

        public MainWindow()
        {
            imagePath = "";
            InitializeComponent();

            this.console.FontFamily = new FontFamily("Consolas");


            //////////////////////////
            Binding binding_1 = new Binding("SelectedValue");
            binding_1.Source = VideoDevicesComboBox;
            WebcamCtrl.SetBinding(Webcam.VideoDeviceProperty, binding_1);

            imagePath = @"C:\Users\aleix\Desktop\HC\Fotos_videos";;
            if (!Directory.Exists(imagePath))
                Directory.CreateDirectory(imagePath);

            WebcamCtrl.ImageDirectory = imagePath;
            WebcamCtrl.FrameRate = 30;
            WebcamCtrl.FrameSize = new System.Drawing.Size(640, 480);

            var vidDevices = EncoderDevices.FindDevices(EncoderDeviceType.Video);
            VideoDevicesComboBox.ItemsSource = vidDevices;
            VideoDevicesComboBox.SelectedIndex = 0;

            // START CAPTURING
            try
            {
                // Display webcam video
                WebcamCtrl.StartPreview();
            }
            catch (Microsoft.Expression.Encoder.SystemErrorException ex)
            {
                MessageBox.Show("Device is in use by another application");
            }
            //////////////////////////

            //var vDevices = EncoderDevices.FindDevices(EncoderDeviceType.Video);
            //this.videoDevicesComboBox.ItemsSource = vDevices;
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

            imagePath = dialog.FileName;
            this.pathBox.Text = imagePath;
            scanAndPlay();
        }

        private async void scanAndPlay()
        {
            Emotion[] emotionResult = await UploadAndDetectEmotions();
            this.LogEmotionResult(emotionResult);


        }

        private async Task<Emotion[]> UploadAndDetectEmotions()
        {
            this.Log("Using " + imagePath + " file");

            EmotionServiceClient client = new EmotionServiceClient(apiKey);
            this.Log("Calling EmotionServiceClient.RecognizeAsync()...");

            try
            {
                using (Stream imageFileStream = File.OpenRead(imagePath))
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

        private void Log(String text)
        {
            this.console.Text += "\n" + text;
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
