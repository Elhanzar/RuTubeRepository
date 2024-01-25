using Npgsql;
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

namespace WpfApp3
{
    /// <summary>
    /// Логика взаимодействия для ADDVIDEO.xaml
    /// </summary>
    public partial class ADDuser : Window
    {
        public ADDuser()
        {
            InitializeComponent();
        }
        User user = new User();
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            user.first_name = name.Text.ToString();
            user.last_name = Surname.Text.ToString();
            user.email = Mail.Text.ToString();
            user.password = Password.Password.ToString();
            user.PhotoUser = "https://drive.google.com/uc?export=download&confirm=no_antivirus&id=1tGIs5hxYfUXzSN_zEc-4Vk2W3LytexUJ";
            MetodsUser metoduser = new MetodsUser();
            if (metoduser.ADDBd(this.user))
            {
                Close();
            }
            else { MessageBox.Show("Error"); }
        }
    }
}
