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
    public partial class Bilet : Form
    {
        SqlConnection conn = new SqlConnection("Data Source = SKYWALKER\\SQLEXPRESS; Initial Catalog = Sinema; Integrated Security = True");
        string kayit = "";
        SqlCommand komut = null;
        int z;
        public Bilet()
        {
            InitializeComponent();
        }

        public void FilmAdiAta(string a)
        {
            FilmAdi.Text = a;
        }
        public void SalonNoAta(int a)
        {
            z = a;
            SalonNo.Text = "Salon" + a;
        }
        public void SeansAta(string a)
        {
            Seans.Text = a;
        }
        public void KoltukAta(string a)
        {
            KoltukNo.Text = a;
        }
        public void ÜcretAta(int a)
        {
            Ücret.Text = a + " TL";
        }
        private void Bilet_Load(object sender, EventArgs e)
        {
            conn.Open();
            kayit = "insert into Biletler(SalonID,koltukNo,ücret,SeansNo,FilmAdi) values (@salon,@koltuk,@ücret,@SeansNo,@FilmAdi)";
            komut = new SqlCommand(kayit, conn);
            //Sorgumuzu ve baglantimizi parametre olarak alan bir SqlCommand nesnesi oluşturuyoruz.
            komut.Parameters.AddWithValue("@salon", z);
            komut.Parameters.AddWithValue("@koltuk", KoltukNo.Text);
            komut.Parameters.AddWithValue("@ücret", Ücret.Text);
            komut.Parameters.AddWithValue("@SeansNo", Seans.Text);
            komut.Parameters.AddWithValue("@FilmAdi", FilmAdi.Text);
            komut.ExecuteNonQuery();

            SqlCommand slnno = new SqlCommand("Select BiletID from Biletler where FilmAdi= '" + FilmAdi + "'", conn);
            SqlDataReader reader = slnno.ExecuteReader();
            while (reader.Read())
                BiletID.Text = reader["BiletID"].ToString();
            reader.Close();
            conn.Close();
        }
    }
}
