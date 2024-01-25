using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
using WpfApp3.enums;
using System.Windows.Threading;
using Npgsql;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Runtime.Serialization;
using System.Xml.Linq;
using static System.Net.WebRequestMethods;


namespace WpfApp3
{    
    public partial class MainWindow : Window
    {
        DispatcherTimer timer = new DispatcherTimer();

        List<MediaFiles> resultMedia = new List<MediaFiles>();
        List<Comment> resultComment = new List<Comment>();
        List<LDS> likes = new List<LDS>();
        List<LDS> dislikes = new List<LDS>();
        List<LDS> subscribes = new List<LDS>();
        User user = new User();
        ~MainWindow(){
            resultMedia.Clear();
            resultComment.Clear();
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.Shutdown();
        }
        public void SourseUser(ref User SourseUser)
        {
            SourseUser = user;
        }

        void Userusing()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream fs = new FileStream("people.dat", FileMode.OpenOrCreate);
            if (fs.Length != 0)
            {
                user = (User)formatter.Deserialize(fs);
            }
            fs.Close();
        }
        public MainWindow()
        {
            InitializeComponent();
            Userusing();
            GenerationMediaPanels();
        }
        bool Poiskmediabool = false;
        private void GenerationMediaPanels()
        {
            MetodsMediaFiles metodsMediaFiles = new MetodsMediaFiles();
            if (Poiskovik.Text == null)
            {
                Poiskmediabool = false;
            }
            if (Poiskmediabool)
            {
                metodsMediaFiles.PRINTBdWithPoiskovik(Poiskovik.Text);
            }
            else { metodsMediaFiles.PRINTBd(); }
            metodsMediaFiles.SourseMedia(ref resultMedia);
            
            for(int i=0;i<resultMedia.Count;i++)
            {
                var mediaPlayer = new MediaElement(){ 
                    Width = 300,
                    Height=200
                };
                TextBlock textBlock = new TextBlock();
                textBlock.TextWrapping = TextWrapping.Wrap;
                textBlock.HorizontalAlignment = HorizontalAlignment.Left;
                textBlock.Background = Brushes.Transparent;
                textBlock.Text = resultMedia[i].name;
                textBlock.Width = 150;
                textBlock.Height = 200;
                textBlock.Margin = new Thickness(10,5,0,0);

                mediaPlayer.Source = new Uri($"{resultMedia[i].URI}", UriKind.Absolute);
                mediaPlayer.Tag = resultMedia[i];
                mediaPlayer.HorizontalAlignment = HorizontalAlignment.Center;
                mediaPlayer.VerticalAlignment = VerticalAlignment.Top;
                mediaPlayer.LoadedBehavior = MediaState.Manual;
                mediaPlayer.MouseMove += MediaPlayer_MouseMove;
                mediaPlayer.MouseLeave += MediaPlayer_MouseLeave;
                mediaPlayer.MouseDown += MediaElement_MouseDown;
                mediaPlayer.Stop();

                DockPanel dockPanel = new DockPanel();
                dockPanel.Height = 210;
                dockPanel.Width = 410;
                dockPanel.Children.Add(mediaPlayer);
                dockPanel.Children.Add(textBlock);
                dockPanel.HorizontalAlignment = HorizontalAlignment.Right;
                dockPanel.Margin = new Thickness(20);

                Border border = new Border();
                border.CornerRadius = new CornerRadius(8,8,8,8);
                border.BorderBrush = Brushes.Gray;
                border.BorderThickness = new Thickness(3, 3, 3, 3);
                border.Margin = new Thickness(0,20,0,0);
                border.Child = (dockPanel);
                stackPanel.Children.Add(border);
            }
        }
        private void GeneratorCommentPanels()
        {
            MediaFiles mediaFiles = (MediaFiles)element.Tag;
            MetodsComment metodscomment = new MetodsComment();
            metodscomment.PRINTBd(mediaFiles.id);
            metodscomment.SourseMedia(ref resultComment);
            MetodsUser Metuser = new MetodsUser();
            User uuComm = new User();
            for (int i = 0; i < resultComment.Count; i++)
            {
                TextBlock textBlock = new TextBlock();
                textBlock.Width = 60;
                textBlock.TextWrapping = TextWrapping.Wrap;
                textBlock.HorizontalAlignment = HorizontalAlignment.Left;
                textBlock.Text = resultComment[i].date.ToString();

                TextBlock textBlock1 = new TextBlock();
                textBlock1.Width = 150;
                textBlock1.TextWrapping = TextWrapping.Wrap;
                textBlock1.HorizontalAlignment = HorizontalAlignment.Left;
                textBlock1.Text = resultComment[i].comment;

                Metuser.PRINTBd(resultComment[i].userid);
                Metuser.SourseUser(ref uuComm);
                TextBlock textBlock2 = new TextBlock();
                textBlock2.Margin = new Thickness(10,0, 0, 0);
                textBlock2.Width = 50;
                textBlock2.TextWrapping = TextWrapping.Wrap;
                textBlock2.HorizontalAlignment = HorizontalAlignment.Left;
                textBlock2.Text = uuComm.first_name;
                
                Image myImage = new Image();
                myImage.Width = 50;
                myImage.HorizontalAlignment= HorizontalAlignment.Left;
                myImage.VerticalAlignment = VerticalAlignment.Top;
                if (uuComm.PhotoUser != null)
                {
                    BitmapImage myBitmapImage = new BitmapImage();
                    myBitmapImage.BeginInit();
                    myBitmapImage.UriSource = new Uri($"{uuComm.PhotoUser}");
                    myBitmapImage.DecodePixelWidth = 1080;
                    myBitmapImage.EndInit();
                    if (myBitmapImage != null)
                    {
                        myImage.Source = myBitmapImage;
                    }
                }

                DockPanel dockPanel = new DockPanel();
                dockPanel.Height = 60;
                dockPanel.Children.Add(myImage);
                dockPanel.Children.Add(textBlock2);
                dockPanel.Children.Add(textBlock1);
                dockPanel.Children.Add(textBlock);
                CommentPanel.Children.Add(dockPanel);
            }
        }
        private MediaElement element = new MediaElement();
        private void MediaPlayer_MouseLeave(object sender, MouseEventArgs e)
        {
            if (element != null)
            {
                element = (MediaElement)sender;
                element.Stop();
            }
        }


        private void MediaPlayer_MouseMove(object sender, MouseEventArgs e)
        {
            element = (MediaElement)sender;
            element.Play();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ADDvideo ADDvideo = new ADDvideo();
            ADDvideo.Show();
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            OpenUser openUser = new OpenUser();
            openUser.Show();
            Menu.IsExpanded = false;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            ADDuser aDDuser = new ADDuser();
            aDDuser.Show();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            stackPanel.Children.Clear();
            GenerationMediaPanels();
        }

        private void MediaElement_MouseDown(object sender, MouseButtonEventArgs e)
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += new EventHandler(timerTick);


            element = (MediaElement)sender;
            GGScrol.Visibility = Visibility.Collapsed;
            GGMediaDock.Visibility = Visibility.Visible;
            GGMedia.Source = element.Source;
            GGMedia.Play();
            GGMedia.Pause();
            Volum.Value = 10;
            GeneratorCommentPanels();
            MediaFiles mediaFiles = (MediaFiles)element.Tag;
            MetodsUser metodsUser = new MetodsUser();
            User bloger = new User();
            metodsUser.PRINTBd(mediaFiles.userid);
            metodsUser.SourseUser(ref bloger);

            if (bloger.PhotoUser != null)
            {
                BitmapImage myBitmapImage = new BitmapImage();
                myBitmapImage.BeginInit();
                myBitmapImage.UriSource = new Uri($"{bloger.PhotoUser}");
                myBitmapImage.DecodePixelWidth = 1080;
                myBitmapImage.EndInit();
                if (myBitmapImage != null)
                {
                    GGSubImage.ImageSource = myBitmapImage;
                }
            }
            GGSubTextBlock.Text = bloger.first_name;
            GGSubBlock_withSUSI.Text = bloger.subscriber.ToString();

            Sub dSub = new Sub();
            dSub.userid = user.id;
            dSub.blogerid = mediaFiles.userid;
            dSub.date = DateTime.UtcNow;
            if (!new MetodsSub().PRINTBd(dSub, "sub"))
            {
                SubButton.Background = Brushes.Gray;
                GGSubBlock_withSUSI.Text = bloger.subscriber.ToString();
            }
            else
            {
                SubButton.Background = Brushes.Red;
                GGSubBlock_withSUSI.Text = bloger.subscriber.ToString();
            }

            LDS dS = new LDS();
            dS.userid = user.id;
            dS.mediaid = mediaFiles.id;
            dS.date = DateTime.UtcNow;
            if (!new MetodsLDS().PRINTBd(dS, "favorits"))
            {
                BitmapImage myBitmapImage = new BitmapImage();
                myBitmapImage.BeginInit();
                myBitmapImage.UriSource = new Uri($"/ImgFls/icons8-палец-вверх-48 (1).png", UriKind.Relative);
                myBitmapImage.DecodePixelWidth = 1080;
                myBitmapImage.EndInit();
                likeimage.Source = myBitmapImage;
            }
            else
            {
                BitmapImage myBitmapImage = new BitmapImage();
                myBitmapImage.BeginInit();
                myBitmapImage.UriSource = new Uri($"/ImgFls/icons8-палец-вверх-48.png", UriKind.Relative);
                myBitmapImage.DecodePixelWidth = 1080;
                myBitmapImage.EndInit();
                likeimage.Source = myBitmapImage;
            }
            if (!new MetodsLDS().PRINTBd(dS, "dislakes"))
            {
                BitmapImage myBitmapImage = new BitmapImage();
                myBitmapImage.BeginInit();
                myBitmapImage.UriSource = new Uri($"/ImgFls/icons8-палец-вверх-48 (1).png", UriKind.Relative);
                myBitmapImage.DecodePixelWidth = 1080;
                myBitmapImage.EndInit();
                deslikeimage.Source = myBitmapImage;
            }
            else
            {
                BitmapImage myBitmapImage = new BitmapImage();
                myBitmapImage.BeginInit();
                myBitmapImage.UriSource = new Uri($"/ImgFls/icons8-палец-вверх-48.png", UriKind.Relative);
                myBitmapImage.DecodePixelWidth = 1080;
                myBitmapImage.EndInit();
                deslikeimage.Source = myBitmapImage;
            }
            Likometr.Content = "like" + mediaFiles.like + " | dislakes" + mediaFiles.deslike;
        }
        void timerTick(object sender, EventArgs e)
        {
            MedPosTS.Value = GGMedia.Position.TotalSeconds;
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            GGMedia.Play();
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            GGMedia.Pause();
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            GGMedia.Stop();
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            GGMedia.Volume = (double)Volum.Value;
        }

        private void Slider_ValueChanged_1(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            GGMedia.Position = TimeSpan.FromSeconds(MedPosTS.Value);
        }

        private void GGMedia_MediaOpened(object sender, RoutedEventArgs e)
        {
            TimeSpan ts = GGMedia.NaturalDuration.TimeSpan;
            MedPosTS.Maximum = ts.TotalSeconds;
            timer.Start();
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            GGMedia.Stop();
            CommentPanel.Children.Clear();
            GGScrol.Visibility = Visibility.Visible;
            GGMediaDock.Visibility = Visibility.Collapsed;
        }

        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            MetodsComment metodsComment = new MetodsComment();
            Comment comment = new Comment();
            MediaFiles mediaFiles = (MediaFiles)element.Tag;
            comment.userid = user.id;
            comment.mediaid = mediaFiles.id;
            comment.comment = textboxComment.Text;
            comment.date = DateTime.UtcNow;
            //MessageBox.Show($"{comment.userid}\n{comment.mediaid}\n{comment.comment}\n{comment.date.ToString("dd.MM.yyyy")}");
            metodsComment.ADDBd(comment);
            textboxComment.Text="";
            CommentPanel.Children.Clear();
            GeneratorCommentPanels();
        }

        private void Expander_Expanded(object sender, RoutedEventArgs e)
        {
            Userusing();
            GGUserTextBlock.Text = user.first_name + " " + user.last_name;
            if (user.PhotoUser != null)
            {
                BitmapImage myBitmapImage = new BitmapImage();
                myBitmapImage.BeginInit();
                myBitmapImage.UriSource = new Uri($"{user.PhotoUser}");
                myBitmapImage.DecodePixelWidth = 1080;
                myBitmapImage.EndInit();
                if (myBitmapImage != null)
                {
                    GGUserImage.ImageSource = myBitmapImage;
                }
            }
            //labelprob.Content = user.Favorit[0] + " " + user.Favorit[1];
        }

        private void Button_Click_Like(object sender, RoutedEventArgs e)
        {
            MediaFiles mediaFiles = (MediaFiles)element.Tag;
            var mMF = new MetodsMediaFiles();
            LDS dS = new LDS();
            dS.userid = user.id;
            dS.mediaid = mediaFiles.id;
            dS.date = DateTime.UtcNow;
            if (new MetodsLDS().PRINTBd(dS,"favorits"))
            {
                mediaFiles.like += 1;
                mMF.UPDATEBd(mediaFiles);
                new MetodsLDS().ADDBd(dS, "favorits");
                BitmapImage myBitmapImage = new BitmapImage();
                myBitmapImage.BeginInit();
                myBitmapImage.UriSource = new Uri($"/ImgFls/icons8-палец-вверх-48 (1).png", UriKind.Relative);
                myBitmapImage.DecodePixelWidth = 1080;
                myBitmapImage.EndInit();
                likeimage.Source = myBitmapImage;
            }
            else
            {
                mediaFiles.like -= 1;
                mMF.UPDATEBd(mediaFiles);
                new MetodsLDS().DROPBd(dS, "favorits");
                BitmapImage myBitmapImage = new BitmapImage();
                myBitmapImage.BeginInit();
                myBitmapImage.UriSource = new Uri($"/ImgFls/icons8-палец-вверх-48.png", UriKind.Relative);
                myBitmapImage.DecodePixelWidth = 1080;
                myBitmapImage.EndInit();
                likeimage.Source = myBitmapImage;
            }
            Likometr.Content = "like" + mediaFiles.like + " | dislakes" + mediaFiles.deslike;
        }

        private void Button_Click_Dislike(object sender, RoutedEventArgs e)
        {
            MediaFiles mediaFiles = (MediaFiles)element.Tag;
            var mMF = new MetodsMediaFiles();

            LDS dS = new LDS();
            dS.userid = user.id;
            dS.mediaid = mediaFiles.id;
            dS.date = DateTime.UtcNow;

            if (new MetodsLDS().PRINTBd(dS, "dislakes"))
            {
                mediaFiles.deslike += 1;
                mMF.UPDATEBd(mediaFiles);
                new MetodsLDS().ADDBd(dS, "dislakes");

                BitmapImage myBitmapImage = new BitmapImage();
                myBitmapImage.BeginInit();
                myBitmapImage.UriSource = new Uri($"/ImgFls/icons8-палец-вверх-48 (1).png", UriKind.Relative);
                myBitmapImage.DecodePixelWidth = 1080;
                myBitmapImage.EndInit();
                deslikeimage.Source = myBitmapImage;
            }
            else
            {
                mediaFiles.deslike -= 1;
                mMF.UPDATEBd(mediaFiles);
                new MetodsLDS().DROPBd(dS, "dislakes");
                BitmapImage myBitmapImage = new BitmapImage();
                myBitmapImage.BeginInit();
                myBitmapImage.UriSource = new Uri($"/ImgFls/icons8-палец-вверх-48.png", UriKind.Relative);
                myBitmapImage.DecodePixelWidth = 1080;
                myBitmapImage.EndInit();
                deslikeimage.Source = myBitmapImage;
            }
            Likometr.Content = "like" + mediaFiles.like + " | dislakes" + mediaFiles.deslike;
        }

        private void Button_Click_9(object sender, MouseButtonEventArgs e)
        {
            Poiskmediabool = true;
            stackPanel.Children.Clear();
            GenerationMediaPanels();
        }
        bool ggMedPanelPPuS = false;
        private void GGMedia_MouseDown(object sender, MouseButtonEventArgs e)
        {
            while (true)
            {
                if (!ggMedPanelPPuS)
                {
                    Panel.SetZIndex(PanelPPUS, -2);
                    ggMedPanelPPuS = true;
                    break;
                }
                if (ggMedPanelPPuS)
                {
                    Panel.SetZIndex(PanelPPUS, +2);
                    ggMedPanelPPuS = false;
                    break;
                }
            }
        }

        private void Button_Click_sub(object sender, RoutedEventArgs e)
        {
            MediaFiles mediaFiles = (MediaFiles)element.Tag;
            MetodsUser metodsUser = new MetodsUser();
            User bloger = new User();
            metodsUser.PRINTBd(mediaFiles.userid);
            metodsUser.SourseUser(ref bloger);

            Sub dS = new Sub();
            dS.userid = user.id;
            dS.blogerid = mediaFiles.userid;
            dS.date = DateTime.UtcNow;
            if (new MetodsSub().PRINTBd(dS, "sub"))
            {
                bloger.subscriber += 1;
                metodsUser.UPDATEBd(bloger);
                new MetodsSub().ADDBd(dS, "sub");
                SubButton.Background = Brushes.Gray;
                GGSubBlock_withSUSI.Text = bloger.subscriber.ToString();
            }
            else
            {
                bloger.subscriber -= 1;
                metodsUser.UPDATEBd(bloger);
                new MetodsSub().DROPBd(dS, "sub");
                SubButton.Background = Brushes.Red;
                GGSubBlock_withSUSI.Text = bloger.subscriber.ToString();
            }
        }
    }
}
