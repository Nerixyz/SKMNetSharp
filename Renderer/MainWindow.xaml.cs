using SKMNET.Client;
using SKMNET.Client.Networking.Client;
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

namespace Renderer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        LightingConsole console;
        public MainWindow()
        {
            console = new LightingConsole("127.0.0.1");
            console.Query(new MonitorSelect(1));
            InitializeComponent();
        }

        private void canvas_Initialized(object sender, EventArgs e)
        {
            canvas.dra
        }
    }
}
