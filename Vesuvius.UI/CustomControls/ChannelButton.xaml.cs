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
    /// Interaction logic for ChannelButton.xaml
    /// </summary>
    public partial class ChannelButton : UserControl
    {
        public ChannelButton()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public int NotificationCount { get; set; }

        public string ChannelName { get; set; }

        public Visibility NotificationsVisible { get; set; }

        public delegate void CustomClickEventHandler(object sender, RoutedEventArgs e);
        public event CustomClickEventHandler CustomClick;

        private void BtnChannel_Click(object sender, RoutedEventArgs e)
        {
            OnCustomClick(e);
        }

        protected virtual void OnCustomClick(RoutedEventArgs e)
        {
            if (CustomClick != null)
                CustomClick(this, e);
        }

        public void Refresh()
        {
            txtBlockNotifications.Text = NotificationCount.ToString();
            borderNotifications.Visibility = NotificationsVisible;
        }

        private void BtnChannel_MouseEnter(object sender, MouseEventArgs e)
        {
        }
    }
}
