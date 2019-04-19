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
    public partial class Acilis : Form
    {
        int girdi = 1;
        Form1 frm1 = new Form1();
        AdminForm admnfrm = new AdminForm();
        SqlConnection conn = new SqlConnection("Data Source = SKYWALKER\\SQLEXPRESS; Initial Catalog = Sinema; Integrated Security = True");
        public SqlDataReader verisorgula(string sorgu)
        {
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            SqlCommand sorgula = new SqlCommand(sorgu, conn);
            SqlDataReader dr = sorgula.ExecuteReader();

            return dr;
        }
        public void sorgucalistir(string sorgu)
        {
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            SqlCommand cmd = new SqlCommand(sorgu, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }
       
       
        public Acilis()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            string sorgu="Select tipi from kullanicilar where kullaniciadi='" + kullanicitext.Text + "' and sifre='"+sifretext.Text+"'", conn;
            SqlDataReader dr = verisorgula(sorgu);

            if (dr.Read())
            {
                if ("y" == dr["tipi"].ToString())
                { MessageBox.Show("Giriş Başarılı Yönetici Sayfasına Yönlendiriliyorsunuz..", "Error Title", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); admnfrm.Show(); this.Hide(); }
                    

                if ("p" == dr["tipi"].ToString())
                { MessageBox.Show("Giriş Başarılı Kullanıcı Sayfasına Yönlendiriliyorsunuz..", "Error Title", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); frm1.Show();this.Hide(); }
                   
                
                dr.Close();
                girdi = 0;
            }
            else if (girdi == 1)
            {
                kullanicitext.Clear();
                sifretext.Clear();
                MessageBox.Show("Hatalı giriş", "Error Title", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                dr.Close();
            }
        }

        private void Acilis_Load(object sender, EventArgs e)
        {
            sifretext.PasswordChar= (char)42;
        }
    }
}
