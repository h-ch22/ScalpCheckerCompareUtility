using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Scalp_Checker_Compare_Utility
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int currnetIndex = 0;
        public int allIndex = 0;
        private List<string> dirs = new List<string>();

        public MainWindow()
        {
            InitializeComponent();
            this.currnetIndex = 0;
            this.allIndex = 0;
            init();
        }

        private void init()
        {
            var dirNames = Directory.GetDirectories(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\ScalpChecker");

            foreach(var dir in dirNames)
            {
                if(dir != Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\ScalpChecker\Results")
                {
                    var newDir = dir.Split("-")[0];
                    dirs.Add(newDir);
                }

            }

            dirs = dirs.Distinct().ToList();

            allIndex = dirs.Count;

            if(allIndex > 0)
            {
                show(0);
            }
        }

        public void onPreviousBtnClick(object sender, RoutedEventArgs e)
        {
            if(currnetIndex > 0)
            {
                currnetIndex--;
                show(currnetIndex);
            }
            else
            {
                MessageBox.Show("첫번째 이미지입니다.", "오류", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        public void onNextBtnClick(object sender, RoutedEventArgs e)
        {
            if (currnetIndex < allIndex-1)
            {
                currnetIndex++;
                show(currnetIndex);
            }
            else
            {
                MessageBox.Show("마지막 이미지입니다.", "오류", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        public string getSeverity(string id, string type)
        {
            var result_txt_root = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\ScalpChecker\Results\Results_" + id + ".txt";
            string results = System.IO.File.ReadAllText(result_txt_root);

            if(results != "")
            {
                string[] splited_results = results.Split('\n');

                foreach(string result in splited_results)
                {
                    string replaced = result.Replace("['", "");
                    replaced = replaced.Replace("']", "");
                    replaced = replaced.Replace("\n", "");

                    if (replaced.Contains(type))
                    {
                        string[] severity = replaced.Split(':');

                        return severity[2];
                    }
                }
            }

            return "";
        }

        public void show(int index)
        {
            Mouse.OverrideCursor = Cursors.Wait;

            DirectoryInfo d = new DirectoryInfo(dirs[index]);
            string[] types = { "-EfficientNet", "-ViT" };

            foreach (string type in types)
            {
                DirectoryInfo dirInfo = new DirectoryInfo(d.FullName + type);

                if(System.IO.Path.Exists(dirInfo.FullName))
                {
                    FileInfo[] files = dirInfo.GetFiles("*.png");

                    foreach (FileInfo file in files)
                    {

                        if (file.Name.Contains("Original"))
                        {
                            img_original.Source = new BitmapImage(new Uri(file.FullName));
                        }
                        else if (file.Name.Contains("EfficientNet"))
                        {
                            if (file.Name.Contains("BIDUM"))
                            {
                                img_dandruff_E.Source = new BitmapImage(new Uri(file.FullName));
                                txt_dandruff_E.Text = $"Dandruff - EfficientNet:\n{getSeverity(d.Name + type, "BIDUM")}";
                            }
                            else if (file.Name.Contains("FIJI"))
                            {
                                img_oiliness_E.Source = new BitmapImage(new Uri(file.FullName));
                                txt_oiliness_E.Text = $"Oiliness - EfficientNet:\n{getSeverity(d.Name + type, "FIJI")}";
                            }
                            else if (file.Name.Contains("HONGBAN"))
                            {
                                img_erythema_E.Source = new BitmapImage(new Uri(file.FullName));
                                txt_erythema_E.Text = $"Erythema - EfficientNet:\n{getSeverity(d.Name + type, "HONGBAN")}";
                            }
                            else if (file.Name.Contains("NONGPO"))
                            {
                                img_folicultis_E.Source = new BitmapImage(new Uri(file.FullName));
                                txt_folicultis_E.Text = $"Folicultis - EfficientNet:\n{getSeverity(d.Name + type, "NONGPO")}";
                            }
                            else if (file.Name.Contains("MISE"))
                            {
                                img_dryness_E.Source = new BitmapImage(new Uri(file.FullName));
                                txt_dryness_E.Text = $"Dryness - EfficientNet:\n{getSeverity(d.Name + type, "MISE")}";
                            }
                            else if (file.Name.Contains("TALMO"))
                            {
                                img_hairLoss_E.Source = new BitmapImage(new Uri(file.FullName));
                                txt_hairLoss_E.Text = $"Hair Loss - EfficientNet:\n{getSeverity(d.Name + type, "TALMO")}";
                            }
                        }
                        else if (file.Name.Contains("ViT"))
                        {
                            if (file.Name.Contains("BIDUM"))
                            {
                                img_dandruff_V.Source = new BitmapImage(new Uri(file.FullName));
                                txt_dandruff_V.Text = $"Dandruff - ViT:\n{getSeverity(d.Name + type, "BIDUM")}";
                            }
                            else if (file.Name.Contains("FIJI"))
                            {
                                img_oiliness_V.Source = new BitmapImage(new Uri(file.FullName));
                                txt_oilness_V.Text = $"Oiliness - ViT:\n{getSeverity(d.Name + type, "FIJI")}";
                            }
                            else if (file.Name.Contains("HONGBAN"))
                            {
                                img_erythema_V.Source = new BitmapImage(new Uri(file.FullName));
                                txt_erythema_V.Text = $"Erythema - ViT:\n{getSeverity(d.Name + type, "HONGBAN")}";
                            }
                            else if (file.Name.Contains("NONGPO"))
                            {
                                img_folicultis_V.Source = new BitmapImage(new Uri(file.FullName));
                                txt_folicultis_V.Text = $"Folicultis - ViT:\n{getSeverity(d.Name + type, "NONGPO")}";
                            }
                            else if (file.Name.Contains("MISE"))
                            {
                                img_dryness_V.Source = new BitmapImage(new Uri(file.FullName));
                                txt_dryness_V.Text = $"Dryness - ViT:\n{getSeverity(d.Name + type, "MISE")}";
                            }
                            else if (file.Name.Contains("TALMO"))
                            {
                                img_hairLoss_V.Source = new BitmapImage(new Uri(file.FullName));
                                txt_hairLoss_V.Text = $"Hair Loss - ViT:\n{getSeverity(d.Name + type, "TALMO")}";
                            }
                        }
                    }
                }
                
            }

            Mouse.OverrideCursor = Cursors.Arrow;

            txt_id.Text = $"Original\n({d.Name}) ({currnetIndex+1}/{allIndex})";
        }
    }
}