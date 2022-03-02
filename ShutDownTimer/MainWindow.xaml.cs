#region Using / Includes
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using Microsoft.Toolkit.Uwp.Notifications;
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

        /// <summary>
        /// Main entry point
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            //Initialise the start timer to when application is first loaded
            tsTimeTimerStarted = new TimeSpan(0, Int32.Parse(DateTime.Now.Hour.ToString()), Int32.Parse(DateTime.Now.Minute.ToString()), Int32.Parse(DateTime.Now.Second.ToString()));

            // Timer to update system time text
            tTimer.Interval = TimeSpan.FromSeconds(1);
            tTimer.Tick += UpdateTime;
            tTimer.Start();

            //Initalise shutdown timer text and update interval
            tsTime = new TimeSpan(0,Int32.Parse(HourLabel.Text), Int32.Parse(MinLabel.Text), 0);
            tShutdownTimer.Interval = tsTime;
            tShutdownTimer.Tick += ExecuteShutdown;
            
        }

        /// <summary>
        /// Executes and updates time text when app is loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimeLabel_Loaded(object sender, RoutedEventArgs e)
        {
            sCurrentTime = DateTime.Now.ToString("h:mm:ss tt");
            TimeLabel.Content = "The Time Is " + sCurrentTime;
        }

        /// <summary>
        /// Updates all text on app, also enables / disables start button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void UpdateTime(object? sender, EventArgs? e)
        {
            sCurrentTime = DateTime.Now.ToString("h:mm:ss tt");
            TimeLabel.Content = "Current Time : " + sCurrentTime;
            UpdateShutdownTimeIn();
            UpdateShutdownTimer();


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

        /// <summary>
        /// Will update / reset shutdown timer text
        /// </summary>
        public void UpdateShutdownTimer()
        {
            if (HourLabel != null)
            {
                if (MinLabel != null)
                {
                    int result;
                    if (int.TryParse(HourLabel.Text, out result) && int.TryParse(MinLabel.Text, out result))
                    {
                        if (IsTextAllowed(HourLabel.Text) && IsTextAllowed(MinLabel.Text))
                        {
                            tsTime = new TimeSpan(0, Int32.Parse(HourLabel.Text), Int32.Parse(MinLabel.Text), 0);
                            tShutdownTimer.Interval = tsTime;
                        }

                    }
                    else
                    {
                        HourLabel.Text = "0";
                        MinLabel.Text = "0";
                    }
                }
                tsTimeTimerStarted = new TimeSpan(0, Int32.Parse(DateTime.Now.Hour.ToString()), Int32.Parse(DateTime.Now.Minute.ToString()), Int32.Parse(DateTime.Now.Second.ToString()));
                tsTimeLeft = tsTimeTimerStarted - tsTime;
                tsWillShutdownAt = tsTimeTimerStarted + tsTime;
            }

        }

        private static readonly System.Text.RegularExpressions.Regex _regex = new System.Text.RegularExpressions.Regex("[^0-9.-]+"); //regex that contains all illegal text
        private static bool IsTextAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }

        /// <summary>
        /// Controlls "system shutting down in " text and visibility
        /// </summary>
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
                if (tsWillShutdownAt.Days > 0)
                {
                    if (tsWillShutdownAt.Hours > 12)
                    {
                        if (tsWillShutdownAt.Minutes < 10)
                        {
                            ShutdownInTimeLabel.Content = "System Shutting Down " + tsWillShutdownAt.Days + " Day(s) From Now At " + tsWillShutdownAt.Hours + ":" +  "0" + tsWillShutdownAt.Minutes + " PM";
                        }
                        else
                        {
                            ShutdownInTimeLabel.Content = "System Shutting Down " + tsWillShutdownAt.Days + " Day(s) From Now At " + tsWillShutdownAt.Hours + ":" + tsWillShutdownAt.Minutes + " PM";
                        }
                    }
                    else
                    {
                        if (tsWillShutdownAt.Minutes < 10)
                        {
                            ShutdownInTimeLabel.Content = "System Shutting Down " + tsWillShutdownAt.Days + " Day(s) From Now At " + tsWillShutdownAt.Hours + ":" + "0" + tsWillShutdownAt.Minutes + " AM";
                        }
                        else
                        {
                            ShutdownInTimeLabel.Content = "System Shutting Down " + tsWillShutdownAt.Days + " Day(s) From Now At " + tsWillShutdownAt.Hours + ":" + tsWillShutdownAt.Minutes + " AM";
                        }
                    }
                }
                else
                {
                    if (tsWillShutdownAt.Hours > 12)
                    {
                        if (tsWillShutdownAt.Minutes < 10)
                        {
                            ShutdownInTimeLabel.Content = "System Shutting Down At " + tsWillShutdownAt.Hours + ":" + "0" + tsWillShutdownAt.Minutes + " PM";
                        }
                        else
                        {
                            ShutdownInTimeLabel.Content = "System Shutting Down At " + tsWillShutdownAt.Hours + ":" + tsWillShutdownAt.Minutes + " PM";
                        }
                    }
                    else
                    {
                        if (tsWillShutdownAt.Minutes < 10)
                        {
                            ShutdownInTimeLabel.Content = "System Shutting Down At " + tsWillShutdownAt.Hours + ":" + "0" + tsWillShutdownAt.Minutes + " AM";
                        }
                        else
                        {
                            ShutdownInTimeLabel.Content = "System Shutting Down At " + tsWillShutdownAt.Hours + ":" + tsWillShutdownAt.Minutes + " AM";
                        }
                    }
                }

                ShutdownInTimeLabel.Foreground = new SolidColorBrush(Colors.Red);
                ShutdownInTimeLabel.FontSize = 20;
            }
        }

        /// <summary>
        /// Starts shutdown process in background
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ExecuteShutdown(object? sender, EventArgs? e)
        {
            var psi = new ProcessStartInfo("shutdown", "/s /t 0");
            psi.CreateNoWindow = true;
            psi.UseShellExecute = false;
            Process.Start(psi);

        }

        /// <summary>
        /// Starts shutdown timer aswell as disabling all text fields and start button.
        /// Also minimizes program
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Cancels shutdown timer and enables text fields and start button to reset.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            tShutdownTimer.Stop();
            if (HourLabel.Text == "0" && MinLabel.Text == "0")
            {
                btnStart.IsEnabled = false;
            }
            else
            {
                btnStart.IsEnabled = true;
            }
            HourLabel.IsEnabled = true;
            MinLabel.IsEnabled= true;

            HourLabel.Text = "0";
            MinLabel.Text = "0";

            bTimerStarted = false;
        }

        /// <summary>
        /// Updates shutdown timer text once user changes hour text
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HourLabel_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateShutdownTimer();
        }

        /// <summary>
        /// Updates shutdown timer text once user changes minutes text
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MinLabel_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateShutdownTimer();
        }
    }
}
