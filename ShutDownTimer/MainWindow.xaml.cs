#region Using / Includes
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
#endregion

namespace ShutDownTimer
{
    /// <summary>
    /// Interaction logic / Code for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Variables
        //Variable Declares
        /// <summary>
        /// Contains current system time to be displayed in XAML
        /// </summary>
        string sCurrentTime = "Time String";
        /// <summary>
        /// Determines how often time will update
        /// </summary>
        DispatcherTimer tTimer = new();
        /// <summary>
        /// Determines how often shutdown text will update in XAML
        /// </summary>
        DispatcherTimer tShutdownTimer = new();
        TimeSpan tsTime = new TimeSpan(0,0,0,0);
        TimeSpan tsTimeTimerStarted = new TimeSpan(0,0,0,0);
        TimeSpan tsWillShutdownAt = new TimeSpan(0, 0, 0, 0);
        TimeSpan tsTimeLeft = new TimeSpan(0,0,0,0);

        bool bTimerStarted = false;
        #endregion

        public MainWindow()
        {
            InitializeComponent();

            tsTimeTimerStarted = new TimeSpan(0, Int32.Parse(DateTime.Now.Hour.ToString()), Int32.Parse(DateTime.Now.Minute.ToString()), Int32.Parse(DateTime.Now.Second.ToString()));

            // Timer to update system time
            tTimer.Interval = TimeSpan.FromSeconds(1);
            tTimer.Tick += UpdateTime;
            tTimer.Start();

            //shutdown
            tsTime = new TimeSpan(0,Int32.Parse(HourLabel.Text), Int32.Parse(MinLabel.Text), 0);
            tShutdownTimer.Interval = tsTime;
            tShutdownTimer.Tick += ExecuteShutdown;
        }
        private void TimeLabel_Loaded(object sender, RoutedEventArgs e)
        {
            sCurrentTime = DateTime.Now.ToString("h:mm:ss tt");
            TimeLabel.Content = "The Time Is " + sCurrentTime;
        }

        public void UpdateTime(object sender, EventArgs e)
        {
            sCurrentTime = DateTime.Now.ToString("h:mm:ss tt");
            TimeLabel.Content = "Current Time : " + sCurrentTime;
            UpdateShutdownTimeIn();

            if (HourLabel.Text == "0" && MinLabel.Text == "0")
            {
                btnStart.IsEnabled = false;
            }
            else
            {
                if (bTimerStarted == false)
                {
                    btnStart.IsEnabled = true;
                }
            }
        }

        public void UpdateShutdownTimer()
        {
            if (HourLabel != null)
            {
                if (MinLabel != null)
                {
                    if (IsTextAllowed(HourLabel.Text) && IsTextAllowed(MinLabel.Text))
                    {
                        tsTime = new TimeSpan(0, Int32.Parse(HourLabel.Text), Int32.Parse(MinLabel.Text), 0);
                        tShutdownTimer.Interval = tsTime;
                    }
                    else
                    {
                        HourLabel.Text = "0";
                        MinLabel.Text = "0";
                    }
                }
            }
            tsTimeLeft = tsTimeTimerStarted - tsTime;
            tsWillShutdownAt = tsTimeTimerStarted + tsTime;
        }

        private static readonly System.Text.RegularExpressions.Regex _regex = new System.Text.RegularExpressions.Regex("[^0-9.-]+"); //regex that contains all illegal text
        private static bool IsTextAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }

        public void UpdateShutdownTimeIn()
        {
            if (tsTime.TotalMinutes == 0 && tsTime.TotalHours == 0)
            {
                ShutdownInTimeLabel.Visibility = Visibility.Hidden;
                //ShutdownInTimeLabel.Content = "Press Start Timer To Begin ...";
            }
            else
            {
                ShutdownInTimeLabel.Visibility = Visibility.Visible;
                ShutdownInTimeLabel.Content = "System Shutting Down At " + tsWillShutdownAt;
                ShutdownInTimeLabel.Foreground = new SolidColorBrush(Colors.Red);
                ShutdownInTimeLabel.FontSize = 20;
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
            tShutdownTimer.Start();
            btnStart.IsEnabled = false;
            HourLabel.IsEnabled = false;
            MinLabel.IsEnabled = false;
            WindowState = WindowState.Minimized;

            tsTimeTimerStarted = new TimeSpan(0, Int32.Parse(DateTime.Now.Hour.ToString()), Int32.Parse(DateTime.Now.Minute.ToString()), Int32.Parse(DateTime.Now.Second.ToString()));

            bTimerStarted = true;

        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            tShutdownTimer.Stop();
            btnStart.IsEnabled = true;
            HourLabel.IsEnabled = true;
            MinLabel.IsEnabled= true;

            HourLabel.Text = "0";
            MinLabel.Text = "0";

            bTimerStarted = false;
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
