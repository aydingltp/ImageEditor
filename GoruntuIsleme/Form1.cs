using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GoruntuIsleme
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog sfd = new OpenFileDialog();
            //sfd.Filter = "resimler|*.bmp|All Files|*.*";
            //sfd.Filter = " JPEG Dosyaları| *.jpg | Bütün Dosyalar(*.*) | *.*";
            sfd.Filter = "resimler|*.BMP*.JPG; *|All Files|*.*";

            if (sfd.ShowDialog() != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }
            pictureBox1.ImageLocation = sfd.FileName;

        }

        private Bitmap griYap(Bitmap bmp)
        {
            for (int i = 0; i < bmp.Height - 1; i++)
            {
                for (int j = 0; j < bmp.Width - 1; j++)
                {
                    int deger = (bmp.GetPixel(j, i).R + bmp.GetPixel(j, i).G + bmp.GetPixel(j, i).B) / 3;
                    Color renk;
                    renk = Color.FromArgb(deger, deger, deger);

                    bmp.SetPixel(j, i, renk);

                }

            }

            return bmp;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


        private Bitmap binaryYap(Bitmap image)
        {
            Bitmap gri = griYap(image);
            int esik = 128;
            Color siyah = Color.FromArgb(0, 0, 0);
            Color beyaz = Color.FromArgb(255, 255, 255);

            for (int i = 0; i < gri.Height - 1; i++)
            {
                for (int j = 0; j < gri.Width - 1; j++)
                {
                    if (gri.GetPixel(i, j).G <= esik)
                    {
                        gri.SetPixel(i, j, siyah);
                    }
                    else
                    {
                        gri.SetPixel(i, j, beyaz);
                    }
                }
            }
            return gri;
        }

        private Bitmap sobelEdgeDetection(Bitmap image)
        {
            Bitmap gri = griYap(image);

            //Bitmap gri = new Bitmap(image,image.Width,image.Height);
            Bitmap buffer = new Bitmap(image.Height, image.Width);
            Color renk;
            int valX, valY, gradient;

            int[,] gX = new int[3, 3];
            int[,] gY = new int[3, 3];

            //Yatay yönde kenar     
            gX[0, 0] = -1; gX[0, 1] = 0; gX[0, 2] = 1;
            gX[1, 0] = -2; gX[1, 1] = 0; gX[1, 2] = 2;
            gX[2, 0] = -1; gX[2, 1] = 0; gX[2, 2] = 1;

            //Dikey yönde kenar
            gY[0, 0] = -1; gY[0, 1] = -2; gY[0, 2] = -1;
            gY[1, 0] = 0; gY[1, 1] = 0; gY[1, 2] = 0;
            gY[2, 0] = 1; gY[2, 1] = 2; gY[2, 2] = 1;


            for (int i = 0; i < gri.Height; i++)
            {
                for (int j = 0; j < gri.Width; j++)
                {
                    if (i == 0 || i == gri.Height - 1 || j == 0 || j == gri.Width - 1)
                    {
                        renk = Color.FromArgb(255, 255, 255);
                        buffer.SetPixel(i, j, renk);
                        valX = 0;
                        valY = 0;
                    }
                    else
                    {
                        valX =
                         gri.GetPixel(i - 1, j - 1).R * gX[0, 0] + gri.GetPixel(i - 1, j).R * gX[0, 1] + gri.GetPixel(i - 1, j + 1).R * gX[0, 2] +
                         gri.GetPixel(i, j - 1).R * gX[1, 0] + gri.GetPixel(i, j).R * gX[1, 1] + gri.GetPixel(i, j + 1).R * gX[1, 2] +
                         gri.GetPixel(i + 1, j - 1).R * gX[2, 0] + gri.GetPixel(i + 1, j).R * gX[2, 1] + gri.GetPixel(i + 1, j + 1).R * gX[2, 2];

                        valY =
                         gri.GetPixel(i - 1, j - 1).R * gY[0, 0] + gri.GetPixel(i - 1, j).R * gY[0, 1] + gri.GetPixel(i - 1, j + 1).R * gY[0, 2] +
                         gri.GetPixel(i, j - 1).R * gY[1, 0] + gri.GetPixel(i, j).R * gY[1, 1] + gri.GetPixel(i, j + 1).R * gY[1, 2] +
                         gri.GetPixel(i + 1, j - 1).R * gY[2, 0] + gri.GetPixel(i + 1, j).R * gY[2, 1] + gri.GetPixel(i + 1, j + 1).R * gY[2, 2];

                        gradient = (int)(Math.Abs(valX) + Math.Abs(valY));

                        if (gradient < 0) { gradient = 0; }
                        if (gradient > 255) { gradient = 255; }
                        renk = Color.FromArgb(gradient, gradient, gradient);
                        buffer.SetPixel(i, j, renk);
                    }
                }

            }

            return buffer;
        }



        private void btn_uygula_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0) // Gri seçildi.
            {
                Bitmap image = new Bitmap(pictureBox1.Image);
                Bitmap gri = griYap(image);

                pictureBox2.Image = gri;
            }
            if (comboBox1.SelectedIndex == 1) // Binary seçildi.
            {
                Bitmap image = new Bitmap(pictureBox1.Image);
                Bitmap binary = binaryYap(image);
                pictureBox2.Image = binary;
            }
            if (comboBox1.SelectedIndex == 2) // Sobel Kenar seçildi.
            {
                Bitmap image = new Bitmap(pictureBox1.Image);
                Bitmap sobel = sobelEdgeDetection(image);

                pictureBox2.Image = sobel;
            }
            if (comboBox1.SelectedIndex == 3) // Sobel Kenar seçildi.
            {
                Bitmap image = new Bitmap(pictureBox1.Image);
                Bitmap median = medianFilter(image);

                pictureBox2.Image = median;
            }
            if (comboBox1.SelectedIndex == 4) // Sepia Fitreleme seçildi.
            {
                // load an image
                System.Drawing.Bitmap image = new Bitmap(pictureBox1.Image);
                // create filter
                AForge.Imaging.Filters.Sepia filter = new AForge.Imaging.Filters.Sepia();
                // apply filter
                System.Drawing.Bitmap newImage = filter.Apply(image);
              

                pictureBox2.Image = newImage;
            }
            if (comboBox1.SelectedIndex == 5) // Blur Fitreleme seçildi.
            {
                // load an image
                System.Drawing.Bitmap image = new Bitmap(pictureBox1.Image);
                // create filter
                AForge.Imaging.Filters.Blur filter = new AForge.Imaging.Filters.Blur();
                // apply filter
                System.Drawing.Bitmap newImage = filter.Apply(image);


                pictureBox2.Image = newImage;
            }
            if (comboBox1.SelectedIndex == 6) // Keskinleştirme Fitreleme seçildi.
            {
                // load an image
                System.Drawing.Bitmap image = new Bitmap(pictureBox1.Image);
                // create filter
                AForge.Imaging.Filters.Sharpen filter = new AForge.Imaging.Filters.Sharpen();
                // apply filter
                System.Drawing.Bitmap newImage = filter.Apply(image);

                pictureBox2.Image = newImage;
            }
            if (comboBox1.SelectedIndex == 7) // Hue Modifier Fitreleme seçildi.
            {
                // load an image
                System.Drawing.Bitmap image = new Bitmap(pictureBox1.Image);
                // create filter
                AForge.Imaging.Filters.HueModifier filter = new AForge.Imaging.Filters.HueModifier();
                // apply filter
                System.Drawing.Bitmap newImage = filter.Apply(image);

                pictureBox2.Image = newImage;
            }
        }

        private Bitmap medianFilter(Bitmap image)
        {
            Bitmap buffer = new Bitmap(image.Width, image.Height);
            Color renk;

            for (int i = 0; i < image.Height; i++)
            {
                for (int j = 0; j < image.Width; j++)
                {
                    if (i == 0 || i == image.Height - 1 || j == 0 || j == image.Width - 1)
                    {
                        continue;
                    }
                    else
                    {
                        int ortanca = ortancayiBul(image, j, i);
                        renk = Color.FromArgb(ortanca, ortanca, ortanca);
                        buffer.SetPixel(j, i, renk);
                    }

                }
            }


            return buffer;
        }

        private int ortancayiBul(Bitmap image, int j, int i)
        {
            int[] dizi = new int[9];
            Color renk;
            int solust, ust, sagust, sol, sag, solalt, alt, sagalt;
            sag = image.GetPixel(j + 1, i).R;
            sagust = image.GetPixel(j + 1, i - 1).R;
            ust = image.GetPixel(j, i - 1).R;
            solust = image.GetPixel(j - 1, i - 1).R;
            sol = image.GetPixel(j - 1, i).R;
            solalt = image.GetPixel(j - 1, i + 1).R;
            alt = image.GetPixel(j, i + 1).R;
            sagalt = image.GetPixel(j + 1, i + 1).R;

            dizi[0] = image.GetPixel(j, i).R;
            dizi[1] = ust;
            dizi[2] = sagust;
            dizi[3] = sol;
            dizi[4] = sag;
            dizi[5] = solalt;
            dizi[6] = alt;
            dizi[7] = sagalt;

            for (int k = 0; k < 8; k++)
            {
                for (int m = k + 1; m < 9; m++)
                {
                    if (dizi[k] < dizi[m])
                    {
                        continue;
                    }
                    else
                    {
                        int temp = dizi[m];
                        dizi[m] = dizi[k];
                        dizi[k] = temp;

                    }
                }
            }

            return dizi[4];
        }
    }
}
