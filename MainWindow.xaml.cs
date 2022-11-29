using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using System.Text.RegularExpressions;
using Path = System.IO.Path;
using Xceed.Wpf.Toolkit;
using System.Drawing;
using System.Globalization;
using Mcolor = System.Drawing.Color;
using Dcolor = System.Windows.Media.Color;
using DcolorConverter = System.Windows.Media.ColorConverter;
using Aspose.Svg;
using System.Runtime.InteropServices;
using System.Windows.Media;
using static WpfApp1.ColorHelper;
using System.Windows.Controls.Primitives;
using System.Collections;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private void Button_Clicked(object sender, RoutedEventArgs e)
        {
            UpdateSourceFiles();
        }
        private void UpdateSourceFiles()
        {   
        }
        public MainWindow()
        {
            ArrayList test = new ArrayList();
            InitializeComponent();
            var initList = InitializeList();
            int i = 0;
            foreach (var b in initList)
            {
                int x = ColorDetector(b.ToString());
                var btn = new Button();
                btn.Height = 50;
                btn.Width = 100;
                var list2 = new ListBox();
                var text = new ListBoxItem();
                Dcolor color;
                if (x == 1)
                {
                    color = (Dcolor)DcolorConverter.ConvertFromString(b);
                    btn.Background = new SolidColorBrush(color);
                }
                else if (x == 2)
                {
                    var rgb = b.Substring(4, b.Length - 5).Split(',').Select(int.Parse).ToArray();
                    Dcolor colour = Dcolor.FromRgb(((byte)rgb[0]), ((byte)rgb[1]), ((byte)rgb[2]));
                    btn.Background = new SolidColorBrush(colour);
                }
                else if (x == 3)
                {
                    var xd = ColorHelper.ParseColor(b);
                    btn.Background = new SolidColorBrush(xd);
                }
                text.Content = b;
                list2.Width = 200;
                list2.Items.Add(i);
                list2.Items.Add(text);
                list2.Name = "xdddd";
                list2.Items.Add(btn);
                this.GenerateFGridPanel.Items.Add(list2);
                i++;
            }
            i = 0;
            foreach (var b in initList)
            { 
                int x = ColorDetector(b.ToString());
                var btn = new ColorPicker();
                var text = new ListBoxItem();
                Dcolor color;
                if (x == 1)
                {
                    color = (Dcolor)DcolorConverter.ConvertFromString(b);
                    btn.SelectedColor = color;
                }
                else if (x == 2)
                {
                    var rgb = b.Substring(4, b.Length - 5).Split(',').Select(int.Parse).ToArray();
                    Dcolor colour = Dcolor.FromRgb(((byte)rgb[0]), ((byte)rgb[1]), ((byte)rgb[2]));
                    btn.SelectedColor = colour;
                }
                else if (x == 3)
                {
                    btn.SelectedColor = ColorHelper.ParseColor(b);
                };
                test.Add(i);
                test.Add(text);
                text.Content = b;
                GenerateFGridPanel2.Items.Add(i);
                text.Height = 50;
                text.Width = 150;
                GenerateFGridPanel2.Items.Add(text);
                GenerateFGridPanel2.Items.Add(btn);
                i++;
            }
        }
        // logika
        public int ColorDetector(string col)
        {
            switch (col)
            {
                case string a when col.StartsWith("#"): return 1;
                case string b when !col.StartsWith("rgba"): return 2;
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
    public static class ColorHelper
    {
        public static dynamic ParseColor(string cssColor)
        {
            cssColor = cssColor.Trim();

            if (cssColor.StartsWith("#"))
            {
                return ColorTranslator.FromHtml(cssColor);
            }
            else if (cssColor.StartsWith("rgb")) //rgb or argb
            {
                int left = cssColor.IndexOf('(');
                int right = cssColor.IndexOf(')');

                if (left < 0 || right < 0)
                    throw new FormatException("rgba format error");
                string noBrackets = cssColor.Substring(left + 1, right - left - 1);

                string[] parts = noBrackets.Split(',');

                int r = int.Parse(parts[0], CultureInfo.InvariantCulture);
                int g = int.Parse(parts[1], CultureInfo.InvariantCulture);
                int b = int.Parse(parts[2], CultureInfo.InvariantCulture);

                if (parts.Length == 3)
                {
                    return Mcolor.FromArgb(r, g, b);
                }
                else if (parts.Length == 4)
                {
                    float a = float.Parse(parts[3], CultureInfo.InvariantCulture);
                    Dcolor colorB = Dcolor.FromRgb((byte)(255 - (255 - r) * a), (byte)(255 - (255 - g) * a), (byte)(255 - (255 - b) * a));
                    return colorB;
                }
            }
            throw new FormatException("Not rgb, rgba or hexa color string");
        }
        public static Dcolor ToMediaColor(this Mcolor color)
        {
            return Dcolor.FromArgb(color.A, color.R, color.G, color.B);
        }
        internal static class ConsoleAllocator
        {
            [DllImport(@"kernel32.dll", SetLastError = true)]
            static extern bool AllocConsole();

            [DllImport(@"kernel32.dll")]
            static extern IntPtr GetConsoleWindow();

            [DllImport(@"user32.dll")]
            static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

            const int SwHide = 0;
            const int SwShow = 5;


            public static void ShowConsoleWindow()
            {
                var handle = GetConsoleWindow();

                if (handle == IntPtr.Zero)
                {
                    AllocConsole();
                }
                else
                {
                    ShowWindow(handle, SwShow);
                }
            }

            public static void HideConsoleWindow()
            {
                var handle = GetConsoleWindow();

                ShowWindow(handle, SwHide);
            }
        }
    }
}