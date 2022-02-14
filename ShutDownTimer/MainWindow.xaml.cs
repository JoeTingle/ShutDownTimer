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
        TimeSpan timeTimerStarted = new TimeSpan(0,0,0,0);
        TimeSpan timeLeft = new TimeSpan(0,0,0,0); 
        public MainWindow()
        {
            InitializeComponent();

            timeTimerStarted = new TimeSpan(0, Int32.Parse(DateTime.Now.Hour.ToString()), Int32.Parse(DateTime.Now.Minute.ToString()), Int32.Parse(DateTime.Now.Second.ToString()));

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
            TimeLabel.Content = "The Time Is " + sCurrentTime;
        }

        public void UpdateTime(object sender, EventArgs e)
        {
            sCurrentTime = DateTime.Now.ToString("h:mm:ss tt");
            TimeLabel.Content = "The Time Is " + sCurrentTime;
            UpdateShutdownTimeIn();

            if (HourLabel.Text == "0" && MinLabel.Text == "0")
            {
                btnStart.IsEnabled = false;
            }
            else
            {
                btnStart.IsEnabled=true;
            }
        }

        public void UpdateShutdownTimer()
        {
            if (HourLabel != null)
            {
                if (MinLabel != null)
                {
                    _time = new TimeSpan(0, Int32.Parse(HourLabel.Text), Int32.Parse(MinLabel.Text), 0);
                    shutdownTimer.Interval = _time;
                }
            }
            timeLeft = timeTimerStarted - _time;
        }

        public void UpdateShutdownTimeIn()
        {
            if (_time.TotalMinutes == 0 && _time.TotalHours == 0)
            {
                ShutdownInTimeLabel.Content = "Press Start Timer To Begin ...";
            }
            else
            {
                //ShutdownInTimeLabel.Content = "System Shutting Down In " + _time.Hours + " Hours " + _time.Minutes + " Minutes " + _time.Seconds + " Seconds";
                //ShutdownInTimeLabel.Content = "System Shutting Down In " + timeLeft.Hours + " Hours " + timeLeft.Minutes + " Minutes " + timeLeft.Seconds + " Seconds";
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

            timeTimerStarted = new TimeSpan(0, Int32.Parse(DateTime.Now.Hour.ToString()), Int32.Parse(DateTime.Now.Minute.ToString()), Int32.Parse(DateTime.Now.Second.ToString()));


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
