using System;
using System.Drawing;
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
using System.IO;
using System.Text.RegularExpressions;
using Path = System.IO.Path;
using System.Security.Cryptography.X509Certificates;
using Xceed.Wpf.Toolkit;
using static System.Net.Mime.MediaTypeNames;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var initList = InitializeList();
            foreach(var b in initList)
            {
                int i = 0;
                int x = ColorDetector(b.ToString());
                var btn = new ColorPicker();
                var list2 = new ListBox();
                var text = new ListBoxItem();
                Color color;
                if (x == 1)
                {
                   color = (Color)ColorConverter.ConvertFromString(b);
                    btn.SelectedColor= color;
                } else if (x == 2)
                {
                    var rgb = b.Substring(4, b.Length - 5).Split(',').Select(int.Parse).ToArray();
                    Color colour = Color.FromArgb(rgb[0], rgb[1], rgb[2]);

                }
                text.Content = b;
                list2.Width = 200;
                list2.Items.Add(i.ToString());
                list2.Items.Add(text);
                list2.Items.Add(btn);
                this.GenerateFGridPanel.Items.Add(list2);
                i++;
            }
            foreach (var b in initList)
            {
                int i = 0;
                var btn = new ColorPicker();
                var list2 = new ListBox();
                var text = new ListBoxItem();
                text.Content = b;
                list2.Width = 200;
                list2.Items.Add(i.ToString());
                list2.Items.Add(text);
                list2.Items.Add(btn);
                this.GenerateFGridPanel2.Items.Add(list2);
                i++;
            }
        }

        public int ColorDetector(string col)
        {
            switch (col)
            {
                case string a when col.StartsWith("#"): return 1;
                case string b when col.StartsWith("rgb"): return 2;
                case string c when col.StartsWith("rgba"): return 3;
            }
            return 0;
        }
        public List<String> InitializeList()
        {
  
            Regex isValidColor = new Regex(@"(#(?:[0-9a-f]{2}){2,4}|#[0-9a-f]{3}|(?:rgba?|hsla?)\((?:\d+%?(?:deg|rad|grad|turn)?(?:,|\s)+){2,3}[\s\/]*[\d\.]+%?\))");
            var extensions = new List<string> { ".txt", ".xml", ".css", ".html", ".js" };
            List<String> finalList = new List<String>();
            var location = Directory.EnumerateFiles(@"C:\Users\daniel.loranc\Desktop\ZadaniePodmianaKolorowCSS", "*.*", SearchOption.AllDirectories).Where(f => extensions.IndexOf(Path.GetExtension(f)) >= 0);
            foreach (string fileName in location)
            {
                string currentFile = File.ReadAllText(fileName);
                currentFile.Split(new String[] { ";", "\n", }, StringSplitOptions.None);
                MatchCollection matches = isValidColor.Matches(currentFile);
                if (matches.Count == 0)
                {
                }
                else
                {

                    for (int i = 0; i < matches.Count; i++)
                    {
                        if (matches[i].Length != 4 && matches[i].Length != 5)
                        {
                            finalList.Add(matches[i].ToString());
                        }
                    }
                }
            }
            return finalList;
        }
    }

}
