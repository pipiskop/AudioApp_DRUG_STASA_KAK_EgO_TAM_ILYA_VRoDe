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
using System.Media;
using Microsoft.Win32;
using System.Windows.Threading;
using System.Threading;
using System.IO;
using AudioApp.Service;

namespace AudioApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    
    public partial class MainWindow : Window
    {
        private FileReadMusic nameMus;
        private FileReadMusic nameList;
       private MediaPlayer mediaPlayer = null;
       private readonly string sourceDir = $"{Environment.CurrentDirectory}\\AudioList.json";
       private  readonly string sorce = $"{Environment.CurrentDirectory}\\AudioName.json";
       private DispatcherTimer timer;
       private event EventHandler Times;
       private Random random = null;
       private bool replay = true;
       private bool property = true;
        bool ra = true;
        bool vk = true;
        private List<string> path;
        private List<string> file;
        private List<string> save = new List<string>();
        private List<string> choice = new List<string>();
        private int index = 0;
        public MainWindow()
        {
            InitializeComponent();
            
            


        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.InitialDirectory = sourceDir;
            openFileDialog.Filter = "Audio files|*.mp3";

            while (true)
            {
                if (openFileDialog.ShowDialog() == true)
                {
                    var fileSafeName = openFileDialog.SafeFileNames.ToList();
                    var pathName = openFileDialog.FileNames.ToList();
                   
                    foreach(var i in fileSafeName)
                    {
                        if (file!= null)
                        {
                            if (file.Contains(i))
                            {
                                continue;
                            }
                            
                                file.Add(i);
                                FileNameLB.Items.Add(i);
                            
                        }
                        else
                        {
                            file = new List<string>();
                            file.Add(i);
                            FileNameLB.Items.Add(i);
                        }
                    }
                    foreach(var n in pathName)
                    {
                        if (path!=null)
                        {
                            if (path.Contains(n))
                            {
                                continue;
                            }
                            
                                path.Add(n);
                            
                        }
                        else
                        {
                            path = new List<string>();
                            path.Add(n);
                        }
                    }
                                          
                    
                }
                if (path.Count == 0)
                {
                    return;
                }
                else
                    break;
            }
            nameList.ToSave(path);
            nameMus.ToSave(file);
            
                mediaPlayer.Open(new Uri(path[index]));
                ra = false;
                Times?.Invoke(this, new EventArgs());
                timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromMilliseconds(100);
                timer.Tick += new EventHandler(Timer_Tick);
                timer.Start();
            

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Pause();
           
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (path == null)
            {
                MessageBox.Show("Please, add Music");
                return;
            }
            mediaPlayer.Play();
                       
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            mediaPlayer.Volume = (double)slider.Value;
            valueVolume.Text = Convert.ToInt32(slider.Value).ToString();
        }

        private void Slider_ValueChanged_1(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            mediaPlayer.Position = TimeSpan.FromSeconds(slider2.Value);
            
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Stop();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            if (path == null)
            {
                MessageBox.Show("Please, add Music");
                return;
            }
                
            if (index == path.Count-1)
            {
                index = 0;
            }
            else
            {
                index++;
            }
            mediaPlayer.Open(new Uri(path[index]));
            Times?.Invoke(this, new EventArgs());
            mediaPlayer.Play();
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            if (path == null)
            {
                MessageBox.Show("Please, add Music");
                return;
            }
            if (index == 0)
            {
                index = path.Count-1;
            }
            else
            {
                index--;
            }
            mediaPlayer.Open(new Uri(path[index]));
            Times?.Invoke(this, new EventArgs());
            mediaPlayer.Play();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            slider2.Value = mediaPlayer.Position.TotalSeconds;
            valueSlider.Text = mediaPlayer.Position.ToString(@"mm\:ss");
            musicName.Text = "Now playing "+file[index];
            if(slider2.Value == slider2.Maximum && replay == true)
            {
                if (index == path.Count-1)
                {
                    index = 0;
                }
                else
                {
                    index++;
                }
                slider2.Value = 0;
                mediaPlayer.Open(new Uri(path[index]));
                Times?.Invoke(this, new EventArgs());
                mediaPlayer.Play();
            }
            if(slider2.Value == slider2.Maximum && replay == false)
            {
                slider2.Value = 0;
                mediaPlayer.Play();
            }
            
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            if (path == null)
            {
                MessageBox.Show("Please, add Music");
                return;
            }
            if (vk == true)
            {
                for (int i = 0; i < path.Count - 1; i++)
                {

                    save.Add(path[i]);
                    choice.Add(path[i]);
                }
                 
               for(int i=0; i<path.Count-1; i++)
                {
                    int ind = random.Next(0, choice.Count);
                    path[i] = choice[ind];
                    choice.RemoveAt(ind);
                    
                }
                vk = false;
            }
            else if (vk == false)
            {
                for (int i = 0; i < path.Count - 1; i++)
                {

                    path[i] = save[i];
                }

                vk = true;
            }
            mediaPlayer.Open(new Uri(path[index]));
            Times?.Invoke(this, new EventArgs());
            mediaPlayer.Play();
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            if (path == null)
            {
                MessageBox.Show("Please, add Music");
                return;
            }
            if (property == true)
            {
                replay = false;
                property = false;
                checkRan.Tag = true;
            }
            else if(property == false)
            {
                replay = true;
                property =true;
                checkRan.Tag = false;
            }
        }
       
        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            if (path == null)
            {
                MessageBox.Show("Please, add Music");
                return;
            }
            path.RemoveAt(index);
            FileNameLB.Items.RemoveAt(index);
            file.RemoveAt(index);
            nameList.ToSave(path);
            nameMus.ToSave(file);              
             index = 0;
           
            mediaPlayer.Open(new Uri(path[index]));
            Times?.Invoke(this, new EventArgs());
            mediaPlayer.Play();

        }

        private void TimeS(object sender, EventArgs e)
        {
            
            try
            {
                while (mediaPlayer.NaturalDuration.HasTimeSpan == false)
                {

                }
                TimeSpan ts = mediaPlayer.NaturalDuration.TimeSpan;
                valueSong.Text = mediaPlayer.NaturalDuration.TimeSpan.ToString(@"mm\:ss");
                slider2.Maximum = ts.TotalSeconds;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            mediaPlayer = new MediaPlayer();
            random = new Random();
            Times += TimeS;
            mediaPlayer.Volume = 1;
            slider.Value = (double)mediaPlayer.Volume;
            nameMus = new FileReadMusic(sourceDir);
            nameList = new FileReadMusic(sorce);
            try
            {
                path = nameList.ToLoad();
                file = nameMus.ToLoad();
                if (file!=null&&path!=null)
                {

                    foreach (var i in file)
                    {
                        FileNameLB.Items.Add(i);
                    }
                    mediaPlayer.Open(new Uri(path[index]));
                    ra = false;
                    Times?.Invoke(this, new EventArgs());
                    timer = new DispatcherTimer();
                    timer.Interval = TimeSpan.FromMilliseconds(100);
                    timer.Tick += new EventHandler(Timer_Tick);
                    timer.Start();

                }

               
                
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
               
            

        }

       
    }
}
