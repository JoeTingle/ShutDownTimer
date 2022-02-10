using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace ShutDownTimer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string sCurrentTime = "Time String";
        DispatcherTimer timer = new();
        DispatcherTimer shutdownTimer = new();
        TimeSpan _time = new TimeSpan(0,0,0,0);
        public MainWindow()
        {
            InitializeComponent();

            // Timer to update system time
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += UpdateTime;
            timer.Start();

            //shutdown
            _time = new TimeSpan(0,Int32.Parse(HourLabel.Text), Int32.Parse(MinLabel.Text), 0);
            shutdownTimer.Interval = _time;
            shutdownTimer.Tick += ExecuteShutdown;
        }
        private void TimeLabel_Loaded(object sender, RoutedEventArgs e)
        {
            sCurrentTime = DateTime.Now.ToString("h:mm:ss tt");
            TimeLabel.Content = "System Time : " + sCurrentTime;
        }

        public void UpdateTime(object sender, EventArgs e)
        {
            sCurrentTime = DateTime.Now.ToString("h:mm:ss tt");
            TimeLabel.Content = "System Time : " + sCurrentTime;
            UpdateShutdownTimeIn();
        }

        public void UpdateShutdownTimer()
        {
            if (HourLabel != null)
            {
                if (MinLabel != null)
                {
                    _time = new TimeSpan(0, Int32.Parse(HourLabel.Text), 0, Int32.Parse(MinLabel.Text));
                    shutdownTimer.Interval = _time;
                }
            }

        }

        public void UpdateShutdownTimeIn()
        {
            if (_time.TotalMinutes == 0)
            {
                ShutdownInTimeLabel.Content = "Press Start Timer To Begin ...";
            }
            else
            {
                ShutdownInTimeLabel.Content = "System Shutting Down In " + _time.Hours + " Hours " + _time.Minutes + " Minutes";
            }
        }

        public void ExecuteShutdown(object sender, EventArgs e)
        {
            var psi = new ProcessStartInfo("shutdown", "/s /t 0");
            psi.CreateNoWindow = true;
            psi.UseShellExecute = false;
            Process.Start(psi);

        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            //Timer to execute shutdown
            shutdownTimer.Start();
            btnStart.IsEnabled = false;
            HourLabel.IsEnabled = false;
            MinLabel.IsEnabled = false;
            WindowState = WindowState.Minimized;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            shutdownTimer.Stop();
            btnStart.IsEnabled = true;
            HourLabel.IsEnabled = true;
            MinLabel.IsEnabled= true;
        }

        private void HourLabel_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateShutdownTimer();
        }

        private void MinLabel_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateShutdownTimer();
        }
    }
}
