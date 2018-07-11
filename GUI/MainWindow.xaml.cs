using AudioAnalyze;
using NAudio.Dsp;
using NAudio.Wave;
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
using System.Windows.Threading;

namespace GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        AudioAnalyzer analyzer;
        DispatcherTimer timer;

        public MainWindow()
        {

            InitializeComponent();

            analyzer = new AudioAnalyzer(128);
            analyzer.Bands.Add(new AudioBand(20, 100, AudioBand.Fade.Fade, TimeSpan.FromSeconds(1)));
            analyzer.StartRecording();

            Console.ReadLine();
            timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(25)
            };
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            p1.Value = analyzer.Bands[0].Value * 1000;
        }
    }
}
