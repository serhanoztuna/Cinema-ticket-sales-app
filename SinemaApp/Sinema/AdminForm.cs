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
   
    public partial class AdminForm : Form
    {
        string secilen;
        int secilensalon;
        SqlConnection conn = new SqlConnection("Data Source = SKYWALKER\\SQLEXPRESS; Initial Catalog = Sinema; Integrated Security = True");
        string kayit = "";
        SqlCommand komut = null;
        string SeansSalonNO = "";
        int koltuksayisi;
        public AdminForm()
        {
            InitializeComponent();
        }
        void filmekle()
        {
            SqlCommand command = new SqlCommand("Select FilmAdi from Filmler", conn);
            conn.Open();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                filmBox.Items.Add(reader["FilmAdi"].ToString());
                secilen = reader["FilmAdi"].ToString();
            }
            reader.Close();
            conn.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            seansBox.Items.Clear();
            if (salonBox1.SelectedIndex == 0)
            {
                koltuksayisi = 10;
                secilensalon = 1;
            }
                
            if (salonBox1.SelectedIndex == 1)
            {
                secilensalon = 2;
                koltuksayisi = 5;
            }
                
            conn.Open();
            
            SqlCommand command = new SqlCommand("Select SalonNoSeansNo from Seanslar where SalonNoSeansNo like '"+secilensalon+"%' and SalonNo IS NULL", conn);
            
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
             
                seansBox.Items.Add(reader["SalonNoSeansNo"].ToString().Substring(2, 5));
                
            }
            reader.Close();
            conn.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            filmBox.Items.Clear();
            conn.Open();
            kayit = "insert into FilmBilgileri(FilmAdi,Türü,Dili,Firma,Ozet) values (@filmadi,@turu,@dili,@firma,@ozet)";
            komut = new SqlCommand(kayit, conn);
            komut.Parameters.AddWithValue("@filmadi", FilmAdı.Text);
            komut.Parameters.AddWithValue("@turu", FilmTürü.Text);
            komut.Parameters.AddWithValue("@dili", FilmDili.Text);
            komut.Parameters.AddWithValue("@firma", YayıncıFirma.Text);
            komut.Parameters.AddWithValue("@ozet", FilmÖzeti.Text);
            komut.ExecuteNonQuery();
            conn.Close();
            filmekle();
        }

        private void AdminForm_Load(object sender, EventArgs e)
        {
            filmekle();
        }

        private void filmBox_SelectedIndexChanged(object sender, EventArgs e)
        {
           
                salonBox1.Items.Add("1");
                salonBox1.Items.Add("2");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            conn.Open();
            label11.Text = salonBox1.SelectedText;
            kayit = "update Seanslar set SalonNo="+ secilensalon+ " , FilmAdi='"+filmBox.SelectedItem+"' where SalonNoSeansNo= '" + SeansSalonNO + "'";
            komut = new SqlCommand(kayit, conn);
            komut.ExecuteNonQuery();
            SqlCommand seansno = new SqlCommand("insert into KoltuklarSalon" + secilensalon + "(SalonNoSeansNo) values ('"+SeansSalonNO+"')", conn);
            seansno.ExecuteNonQuery();
            for (int i = 1; i <= koltuksayisi; i++)
            {
                kayit = "update KoltuklarSalon" + secilensalon + " set Koltuk" + i + "=0 where SalonNoSeansNo= '" + SeansSalonNO+"'";
                komut = new SqlCommand(kayit, conn);
                komut.ExecuteNonQuery();

            }
            conn.Close();
             

        }

        private void seansBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            SeansSalonNO = secilensalon + "," + seansBox.SelectedItem + ":00";
        }
    }
}
