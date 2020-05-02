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
using System.Windows.Shapes;
using Vesuvius.Models;

namespace Vesuvius.UI
{
    /// <summary>
    /// Interaction logic for ServerCreation.xaml
    /// </summary>
    public partial class ChannelCreation : Window
    {
        public ChannelCreation()
        {
            InitializeComponent();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            var newChannel = new Channel()
            {
                Name = txtboxServerName.Text,
                Users = new List<User>()
            };
            newChannel.Users.Add(new User
            {
                Id = AppContext.Current.SessionContext.User.Id
            });

            AppContext.Current.SessionContext.AddChannel(newChannel);
            this.Close();

        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
