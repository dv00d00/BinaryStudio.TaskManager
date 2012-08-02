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
using System.ComponentModel;
using System.Collections.ObjectModel;
using WPFTaskbarNotifier;

namespace WPFTaskbarNotifierExample
{
    /// <summary>
    /// This is just a mock object to hold something of interest. 
    /// </summary>
    public class NotifyObject
    {
        public NotifyObject(string message, string title)
        {
            this.message = message;
            this.title = title;
        }

        private string title;
        public string Title
        {
            get { return this.title; }
            set { this.title = value; }
        }

        private string message;
        public string Message
        {
            get { return this.message; }
            set { this.message = value; }
        }
    }

    /// <summary>
    /// This is a TaskbarNotifier that contains a list of NotifyObjects to be displayed.
    /// </summary>
    public partial class ExampleTaskbarNotifier : TaskbarNotifier
    {
        public ExampleTaskbarNotifier()
        {
            InitializeComponent();
        }

        private ObservableCollection<NotifyObject> notifyContent;
        /// <summary>
        /// A collection of NotifyObjects that the main window can add to.
        /// </summary>
        public ObservableCollection<NotifyObject> NotifyContent
        {
            get
            {
                if (this.notifyContent == null)
                {
                    // Not yet created.
                    // Create it.
                    this.NotifyContent = new ObservableCollection<NotifyObject>();
                }

                return this.notifyContent;
            }
            set
            {
                this.notifyContent = value;
            }
        }

        private void Item_Click(object sender, EventArgs e)
        {
            Hyperlink hyperlink = sender as Hyperlink;

            if(hyperlink == null)
                return;

            NotifyObject notifyObject = hyperlink.Tag as NotifyObject;
            if(notifyObject != null)
            {
                MessageBox.Show("\"" + notifyObject.Message + "\"" + " clicked!");
            }
        }

        private void HideButton_Click(object sender, EventArgs e)
        {
            this.ForceHidden();
        }
    }
}