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
using System.Timers;

namespace ShutDownTimer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string sCurrentTime = "Time String";
        public MainWindow()
        {
            InitializeComponent();

            sCurrentTime =  DateTime.Now.ToString("h:mm:ss tt");
            TimeLabel.Content = sCurrentTime;
        }
    }
}
