using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Reflection;

namespace numbers_analyzer
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int[,] input = new int[3, 5];
        Web NW1;
        OpenFileDialog openFileDialog1 = new OpenFileDialog();
        public MainWindow()
        {
            InitializeComponent();

            NW1 = new Web(3, 5, input);

            openFileDialog1.Title = "Укажите файл весов";
            openFileDialog1.ShowDialog();
            string s = openFileDialog1.FileName;
            StreamReader sr = File.OpenText(s);
            string line;
            string[] s1;
            int k = 0;
            while ((line = sr.ReadLine()) != null)
            {

                s1 = line.Split(' ');
                for (int i = 0; i < s1.Length; i++)
                {
                    list.Items.Add("");
                    if (k < 5)
                    {
                        NW1.weight[i, k] = Convert.ToInt32(s1[i]);
                        list.Items[k] += Convert.ToString(NW1.weight[i, k]);
                    }

                }
                k++;

            }
            sr.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            list.Items.Clear();

            openFileDialog1.Title = "Укажите тестируемый файл";
            openFileDialog1.ShowDialog();

            BitmapImage logo = new BitmapImage();
            logo.BeginInit();
            logo.UriSource = new Uri(@openFileDialog1.FileName, UriKind.Relative);
            logo.EndInit();

            image.Source = logo;

            Bitmap im = System.Drawing.Image.FromFile(openFileDialog1.FileName.ToString()) as Bitmap;

            for (var i = 0; i <= 5; i++) list.Items.Add(" ");

            for (var x = 0; x <= 2; x++)
            {
                for (var y = 0; y <= 4; y++)
                {
                    // list.Items.Add(Convert.ToString(im.GetPixel(x, y).R));
                    int n = (im.GetPixel(x, y).R);
                    if (n >= 250) n = 0;
                    else n = 1;
                    list.Items[y] = list.Items[y] + "  " + Convert.ToString(n);
                    input[x, y] = n;
                    //if (n == 0) input[x, y] = 1;
                }

            }

            recognize();
        }

        public void recognize()
        {
            NW1.mul_w();
            NW1.Sum();

            if (NW1.Rez()) list.Items.Add(" - True, Sum = " + Convert.ToString(NW1.sum));

            else list.Items.Add(" - False, Sum = " + Convert.ToString(NW1.sum));
        }

        class Web
        {
            public int[,] mul;
            public int[,] weight;
            public int[,] input;
            public int limit = 9;
            public int sum;

            public Web(int sizex, int sizey, int[,] inP)
            {
                weight = new int[sizex, sizey];
                mul = new int[sizex, sizey];

                input = new int[sizex, sizey];
                input = inP;
            }

            public void mul_w()
            {
                for (int x = 0; x <= 2; x++)
                {
                    for (int y = 0; y <= 4; y++)
                    {
                        mul[x, y] = input[x, y] * weight[x, y];
                    }
                }
            }

            public void Sum()
            {
                sum = 0;
                for (int x = 0; x <= 2; x++)
                {
                    for (int y = 0; y <= 4; y++)
                    {
                        sum += mul[x, y];
                    }
                }
            }

            public bool Rez()
            {
                if (sum >= limit)
                    return true;

                else return false;
            }
            public void incW(int[,] inP)
            {
                for (int x = 0; x <= 2; x++)
                {
                    for (int y = 0; y <= 4; y++)
                    {
                        weight[x, y] += inP[x, y];
                    }
                }
            }
            public void decW(int[,] inP)
            {
                for (int x = 0; x <= 2; x++)
                {
                    for (int y = 0; y <= 4; y++)
                    {
                        weight[x, y] -= inP[x, y];
                    }
                }
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            button2.IsEnabled = false;

            if (NW1.Rez() == false)
                NW1.incW(input);

            else NW1.decW(input);

            //Запись
            string s = "";
            string[] s1 = new string[5];

            System.IO.File.Delete("w.txt");
            FileStream FS = new FileStream("w.txt", FileMode.OpenOrCreate);
            StreamWriter SW = new StreamWriter(FS);

            for (int y = 0; y <= 4; y++)
            {
                s = Convert.ToString(NW1.weight[0, y]) + " " + Convert.ToString(NW1.weight[1, y]) + " " + Convert.ToString(NW1.weight[2, y]);

                s1[y] = s;

                SW.WriteLine(s);
            }
            SW.Close();
        }
    }

}
