using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WPFTaskbarNotifierExample
{
    /// <summary>
    /// An example window that adds content to the TaskbarNotifier and changes
    /// its settings.
    /// </summary>
    public partial class Window1 : System.Windows.Window
    {
        private bool reallyCloseWindow = false;

        public Window1()
        {
            this.taskbarNotifier = new ExampleTaskbarNotifier();

            InitializeComponent();

            this.taskbarNotifier.Show();
        }

        private ExampleTaskbarNotifier taskbarNotifier;
        public ExampleTaskbarNotifier TaskbarNotifier
        {
            get { return this.taskbarNotifier; }
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs args)
        {
            // In WPF it is a challenge to hide the window's close box in the title bar.
            // When the user clicks this, we don't want to exit the app, but rather just
            // put it back into hiding.  Unfortunately, this is a challenge too.
            // The follow code works around the issue.

            if (!this.reallyCloseWindow)
            {
                // Don't close, just Hide.
                args.Cancel = true;
                // Trying to hide
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, (DispatcherOperationCallback)delegate(object o)
                {
                    this.Hide();
                    return null;
                }, null);
            }
            else
            {
                // Actually closing window.

                this.NotifyIcon.Visibility = Visibility.Collapsed;

                // Close the taskbar notifier too.
                if (this.taskbarNotifier != null)
                    this.taskbarNotifier.Close();
            }
        }

        private void Add_Click(object sender, EventArgs e)
        {
            string title = this.TitleTextBox.Text.Trim();
            string message = this.MessageTextBox.Text.Trim();

            if ((title != string.Empty) && (message != string.Empty))
            {
                // The title and message are both not empty.

                // Add the new title and message to the TaskbarNotifier's content.
                this.taskbarNotifier.NotifyContent.Add(new NotifyObject(message, title));

                // Clear the textboxes.
                this.ClearTextBoxes();

                // Tell the TaskbarNotifier to open.
                this.taskbarNotifier.Notify();
            }
        }

        private void ClearTextBoxes()
        {
            this.TitleTextBox.Text = string.Empty;
            this.MessageTextBox.Text = string.Empty;
        }

        private void ClearAll_Click(object sender, EventArgs e)
        {
            // Clear all content in the TaskbarNotifier
            this.taskbarNotifier.NotifyContent.Clear();
        }

        private void NotifyIcon_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                // Open the TaskbarNotifier
                this.taskbarNotifier.Notify();
            }
        }

        private void NotifyIconOpen_Click(object sender, RoutedEventArgs e)
        {
            // Open the TaskbarNotifier
            this.taskbarNotifier.Notify();
        }

        private void NotifyIconConfigure_Click(object sender, RoutedEventArgs e)
        {
            // Show this window
            this.Show();
            this.Activate();          
        }

        private void NotifyIconExit_Click(object sender, RoutedEventArgs e)
        {
            // Close this window.
            this.reallyCloseWindow = true;
            this.Close();
        }
    }
}