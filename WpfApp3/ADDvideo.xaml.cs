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

namespace WpfApp3
{
    /// <summary>
    /// Логика взаимодействия для ADDvideo.xaml
    /// </summary>
    public partial class ADDvideo : Window
    {
        public ADDvideo()
        {
            InitializeComponent();
        }
        
        User User = new User();

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            MetodsMediaFiles MetodsMediaFiles = new MetodsMediaFiles();
            MediaFiles MediaFiles = new MediaFiles();

            new MainWindow().SourseUser(ref User);

            MediaFiles.name = MediaName.Text.ToString();
            MediaFiles.URI = SourseURI.Text.ToString();
            MediaFiles.date = DateTime.UtcNow;
            MediaFiles.like = 0;
            MediaFiles.deslike = 0;
            MediaFiles.userid = User.id;
            if (MetodsMediaFiles.ADDBd(MediaFiles))
            {
                Close();
            }
            else { MessageBox.Show("Error"); }
        }
    }
}
