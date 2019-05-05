using System;
using System.Collections.Generic;
using System.Drawing;
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
using System.Windows.Shapes;
using TsudaKageyu;

namespace Right_Click_Commands.Views.WPF
{
    /// <summary>
    /// Interaction logic for IconPicker.xaml
    /// </summary>
    public partial class IconPicker : Window
    {
        public IconPicker()
        {
            InitializeComponent();

            IconExtractor iconExtractor = new IconExtractor(@"C:\WINDOWS\system32\imageres.dll");

            Icon[] allIcons = iconExtractor.GetAllIcons();

            foreach (Icon icon in allIcons)
            {
                using (Bitmap bmp = icon.ToBitmap())
                {
                    var stream = new MemoryStream();
                    bmp.Save(stream, System.Drawing.Imaging.ImageFormat.Png);

                    System.Windows.Controls.Image image = new System.Windows.Controls.Image();
                    image.Source = BitmapFrame.Create(stream);
                    image.Width = 32;
                    image.Height = 32;

                    SP.Children.Add(image);
                }
            }
            Console.WriteLine("done");
        }
    }
}
