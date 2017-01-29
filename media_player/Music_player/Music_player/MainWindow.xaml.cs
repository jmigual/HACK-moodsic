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
using WMPLib;
using System.Windows.Controls.Primitives;


namespace Music_player
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //private double selectedMarker = 0;
        WindowsMediaPlayer player;
        public MainWindow()
        {
            InitializeComponent();
            player = new WindowsMediaPlayer();
            player.URL = @"C:\Users\Maria\Desktop\pc antic\Musica\Vídeoclip  M'agrada el Ping pong.mp3";
            player.controls.stop();
        }

        void Play(object sender, RoutedEventArgs args)
        {
            ToggleButton tb = (ToggleButton)sender;
            Console.WriteLine((bool)tb.IsChecked);
            if ((bool)tb.IsChecked)
            {
                player.controls.play();
            }
            else {
                player.controls.pause();
            }
        }
        void Next(object sender, RoutedEventArgs args)
        {
            player.controls.stop();
            //player.URL = getPATH;
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //player.controls.currentPosition =Convert.ToDouble(e);
        }
    }
}
