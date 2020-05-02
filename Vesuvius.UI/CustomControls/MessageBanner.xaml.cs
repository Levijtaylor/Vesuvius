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
using Vesuvius.Models;
using System.Windows.Shapes;

namespace Vesuvius.UI.CustomControls
{
    /// <summary>
    /// Interaction logic for MessageBanner.xaml
    /// </summary>
    public partial class MessageBanner : UserControl
    {
        public MessageBanner(DateTime timeStamp)
        {
            InitializeComponent();
            this.DataContext = this;

            txtBlockDate.Text = timeStamp.ToString("MMMM dd, yyyy");

        }

    }
}
