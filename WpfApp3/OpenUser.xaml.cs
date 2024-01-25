using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
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
using System.Xml.Linq;
using System.Runtime.Serialization.Formatters.Binary;


namespace WpfApp3
{
    /// <summary>
    /// Логика взаимодействия для OpenUser.xaml
    /// </summary>
    public partial class OpenUser : Window
    {
        public OpenUser()
        {
            InitializeComponent();
        }
        User userFromOpen = new User();
        public void SourseUser(ref User User)
        {
            User = userFromOpen;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            userFromOpen.first_name = name.Text.ToString();
            userFromOpen.last_name = Surname.Text.ToString();
            userFromOpen.email = Mail.Text.ToString();
            userFromOpen.password = Password.Password.ToString();
            MetodsUser metoduser = new MetodsUser();
            if (metoduser.PRINTBd(userFromOpen))
            {
                Close(); 
            }
            else { MessageBox.Show("Error","Не удалось подключится к серверу"); }
        }
    }
}
