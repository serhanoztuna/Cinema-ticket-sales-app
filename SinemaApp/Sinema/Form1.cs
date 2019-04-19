using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Sinema
{
    public partial class Form1 : Form
    {
        SqlConnection conn = new SqlConnection("Data Source = SKYWALKER\\SQLEXPRESS; Initial Catalog = Sinema; Integrated Security = True");
        string secilenfilm;
        string secilenSalon;
        string secilenSeans;
        string secilenKoltuk;
        int koltukSayisi;
        Button[] btn;
        bool[] koltuk;
        int secilenKoltukSayisi = 0;
        Bilet bilet = new Bilet();
        int biletÜcreti = 21;


        public Form1()

        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            SqlCommand command = new SqlCommand("exec TümFilmler", conn);
            conn.Open();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                listBox1.Items.Add(reader["FilmAdi"].ToString());
            }
            reader.Close();
            conn.Close();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox2.Items.Clear();
            secilenfilm = listBox1.Items[listBox1.SelectedIndex].ToString();
            bilet.FilmAdiAta(secilenfilm);
            SqlCommand slnno = new SqlCommand("Select SalonNo from Filmler where FilmAdi= '" + secilenfilm + "'", conn);
            conn.Open();
            SqlDataReader reader = slnno.ExecuteReader();
            while (reader.Read())
                secilenSalon = reader["SalonNo"].ToString();
            reader.Close();

            SqlCommand seanslr = new SqlCommand("Select SalonNoSeansNo from Seanslar where SalonNo= '" + secilenSalon + "'and FilmAdi= '"+secilenfilm+"'", conn);

            SqlDataReader reader2 = seanslr.ExecuteReader();

            while (reader2.Read())
            {
                label3.Text = reader2["SalonNoSeansNo"].ToString();
                label3.Text = label3.Text.Substring(2, 5);
                listBox2.Items.Add(label3.Text);
            }
            reader2.Close();
            conn.Close();

        }

        private void button1_Click(object sender, EventArgs e)
        {


        }
        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            KoltukDoldur();
        }



        void butonOlustur(int Ksayisi, bool[] dolu = null)
        {
            {
                groupBox1.Controls.Clear();
                secilenKoltukSayisi = 0;

                int sayac = 0, x = 55, y = 50;
                btn = new Button[Ksayisi];

                for (int i = 0; i < (Ksayisi / 5); i++)
                {
                    for (int j = 0; j < 5; j++)
                    {

                        btn[sayac] = new Button();
                        if (dolu[sayac] == true)
                        {
                            btn[sayac].BackColor = Color.OrangeRed;
                            btn[sayac].Enabled = false;
                        }

                        else
                        {
                            btn[sayac].BackColor = Color.LimeGreen;
                        }
                        btn[sayac].Text = "";
                        btn[sayac].Name = "Koltuk" + (sayac + 1);
                        btn[sayac].Width = 25;
                        btn[sayac].Height = 20;
                        btn[sayac].Left = x;
                        btn[sayac].Top = y;
                        x = x + 50;
                        groupBox1.Controls.Add(btn[sayac]);
                        btn[sayac].Click += new EventHandler(btn_Click);


                        sayac++;
                    }
                    x = 55;
                    y = y + 40;
                }
            }
        }
        private void button1_Click_1(object sender, EventArgs e)


        {
            conn.Open();

            secilenSeans = listBox2.Items[listBox2.SelectedIndex].ToString();
            string[] koltukTutma = new string[koltukSayisi];
            string kayit = "";
            SqlCommand komut = null;
            for (int i = 0; i < koltukTutma.Length; i++)
            {
                koltukTutma[i] = btn[i].Name;
                if (btn[i].BackColor == Color.OrangeRed && btn[i].Enabled == true)
                {
                    kayit = "update KoltuklarSalon" + secilenSalon + " set " + koltukTutma[i] + "=1 where SalonNoSeansNo= '" + secilenSalon + "," + secilenSeans + ":00'";
                    komut = new SqlCommand(kayit, conn);
                    komut.ExecuteNonQuery();
                    secilenKoltuk += "Koltuk" + i + "\n";
                    secilenKoltukSayisi++;
                }

            }
            conn.Close();
            bilet.ÜcretAta(secilenKoltukSayisi * biletÜcreti);
            bilet.KoltukAta(secilenKoltuk);
            bilet.ShowDialog();
            KoltukDoldur();


        }
        private void btn_Click(object sender, EventArgs e)
        {

            if (((Button)sender).BackColor == Color.OrangeRed)
            {

                ((Button)sender).BackColor = Color.LimeGreen;
            }
            else if (((Button)sender).BackColor == Color.LimeGreen)
                ((Button)sender).BackColor = Color.OrangeRed;

        }
        void KoltukDoldur()
        {
            conn.Open();
            string koltukno = "";
            secilenSeans = listBox2.Items[listBox2.SelectedIndex].ToString();
            bilet.SeansAta(secilenSeans);
            bilet.SalonNoAta(Convert.ToInt32(secilenSalon));
            SqlCommand seansno = new SqlCommand("Select * from KoltuklarSalon" + secilenSalon + " where SalonNoSeansNo= '" + secilenSalon + "," + secilenSeans + ":00'", conn);
            SqlDataReader reader3 = seansno.ExecuteReader();

            if (secilenSalon == "1")
                koltukSayisi = 10;
            else if (secilenSalon == "2")
                koltukSayisi = 5;
            koltuk = new bool[koltukSayisi];
            while (reader3.Read())
            {
                groupBox1.Text = "Salon " + reader3["SalonNoSeansNo"].ToString().Substring(0, 1);
                for (int i = 0; i < koltuk.Length; i++)
                {
                    koltukno = "Koltuk" + (i + 1);
                    if (reader3[koltukno] == null)
                        koltuk[i] = false;
                    else
                        koltuk[i] = reader3.GetBoolean(reader3.GetOrdinal(koltukno));
                }


            }


            butonOlustur(koltukSayisi, koltuk);
            reader3.Close();
            //if salon 1 koltuksayısı 10 salon2 koltuk sayısı 5
            conn.Close();


        }
    }
}

