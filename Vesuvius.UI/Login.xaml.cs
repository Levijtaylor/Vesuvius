using System.Windows;
using Vesuvius.Models;

namespace Vesuvius.UI
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
             InitializeComponent();
            AppContext.Current.SessionContext = new SessionContext();

#if DEBUG
            passboxPassword.Password = "Kodiak2468?";
            txtboxUsername.Text = "AlaskanOrigins";
#endif
        }

        /// <summary>
        /// Evaluating the Credentials of Login Information
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            var response = AppContext.Current.SessionContext.Login(new LoginRequest
            {
                Password = passboxPassword.Password,
                UserName = txtboxUsername.Text
            });

            if(response == true)
            {
                var main = new MainWindow();
                main.Show();
                this.Close();
            }
            else
            {
                txtblockIncorrect.Text = "Incorrect UserName or Password.";
            }

        }

        /// <summary>
        /// Create new User
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            gridNewUser.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Saving the new user to the database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            //If password is the same in box password boxes
            if (passboxConfirm.Password == passboxPass.Password)
            {
                var loginResponse = AppContext.Current.SessionContext.AddUser(new Models.User
                {
                    Alias = txtboxAlias.Text,
                    Email = txtboxEmail.Text,
                    Name = txtboxFirst.Text + " " + txtboxLast.Text,
                    Password = passboxConfirm.Password
                });

                MessageBox.Show("Your Account was Succesfully Created. Please Login");
                gridNewUser.Visibility = Visibility.Hidden;
            }
            else
            {
                borderIncorrectPass.Visibility = Visibility.Visible;
                lblIncorrect.Visibility = Visibility.Visible;
                lblIncorrect1.Visibility = Visibility.Visible;
            }
        }
    }
}
