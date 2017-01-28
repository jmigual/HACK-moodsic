using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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
        }

        private void scanAndPlay()
        {
            Emotion[] emotionResult = await UploadAndDetectEmotions();


        }

        private async Task<Emotion[]> UploadAndDetectEmotions()
        {
            EmotionServiceClient client = new EmotionServiceClient(apiKey);
            this.Log("Calling EmotionServiceClient.RecognizeAsync()...");
        }

        private void Log(String text)
        {
            this.console.Text += "\n" + text;
        }
    }
}
