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
    /// Interaction logic for UserButton.xaml
    /// </summary>
    public partial class UserButton : UserControl
    {
        public UserButton()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        /// <summary>
        /// UserName of User that the Button is Addressed too.
        /// </summary>
        public string UserName { get; set; }
    }
}
