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

        private void button2_Click(object sender, EventArgs e)
        {
            Bitmap image = new Bitmap(pictureBox1.Image);
            Bitmap gri = griYap(image);

            pictureBox2.Image = gri;
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

        private void button3_Click(object sender, EventArgs e)
        {
            Bitmap image = new Bitmap(pictureBox1.Image);
            Bitmap binary = binaryYap(image);
            pictureBox2.Image = binary;

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

        private void button4_Click(object sender, EventArgs e)
        {
            Bitmap image = new Bitmap(pictureBox1.Image);
            Bitmap sobel = sobelEdgeDetection(image);
            pictureBox1.Image = sobel;
        }

        private Bitmap sobelEdgeDetection(Bitmap image)
        {
            Bitmap gri = griYap(image);
            Bitmap buffer=new Bitmap(image.Height,)

            return image;
        }
    }
}
