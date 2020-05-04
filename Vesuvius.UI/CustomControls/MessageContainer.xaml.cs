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

namespace Vesuvius.UI
{
    /// <summary>
    /// Interaction logic for MessageContainer.xaml
    /// </summary>
    public partial class MessageContainer : UserControl
    {
        /// <summary>
        /// Sender determines the color of labelUsername
        /// </summary>
        /// <param name="IsSender"></param>
        public MessageContainer(bool isActiveUser, bool userNameVisible)
        {
            InitializeComponent();
            this.DataContext = this;

            if (userNameVisible == true)
            {
                if (isActiveUser == true)
                    lblUserName.Foreground = Brushes.Teal;
                else
                    lblUserName.Foreground = Brushes.NavajoWhite;
            }
            else
            {
                lblUserName.Visibility = Visibility.Hidden;
            }
        }
        
        public MessageContainer()
        {

        }

        public void AddTextToMessage(string text)
        {
            stackPanel.Children.Add(new TextBlock()
            {
                Text = text
            });
        }


    /// <summary>
    /// Sender of the Message
    /// </summary>
    public string Sender { get; set; }

    /// <summary>
    /// The Message Block
    /// </summary>
    public string Message { get; set; }

}
}
