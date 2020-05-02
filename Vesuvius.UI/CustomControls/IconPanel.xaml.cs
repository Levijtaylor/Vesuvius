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

namespace Vesuvius.UI.CustomControls
{
    /// <summary>
    /// Interaction logic for IconPanel.xaml
    /// </summary>
    public partial class IconPanel : UserControl
    {
        public IconPanel()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        /// <summary>
        /// Number of Notificaiton for a specific Icon
        /// </summary>
        public int UserNotificationCount { get; set; }

        /// <summary>
        /// Number of Notifications for specific icon
        /// </summary>
        public int ChannelNotificationCount { get; set; }

        /// <summary>
        /// Visibility property for Channe; Icon's Notification Bubble
        /// </summary>
        public Visibility ChannelNotificationVisibility { get; set; }

        /// <summary>
        /// Visibility property for User Icon's Notification Bubble
        /// </summary>
        public Visibility UserNotificationVisibility { get; set; }

        /// <summary>
        /// Refreshes the Control and allows for changes to be seen
        /// </summary>
        public void Refresh()
        {
            txtBlockChannelNotification.Text = ChannelNotificationCount.ToString();
            txtBlockUserNotification.Text = UserNotificationCount.ToString();
            
        }
    }
}
