using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Expression.Encoder.Devices;
using Microsoft.ProjectOxford.Emotion;
using Microsoft.ProjectOxford.Emotion.Contract;
using Microsoft.Win32;

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

        }
    }
}
