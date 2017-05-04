using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using System.Data.SqlClient;

namespace _14331017MerveCandir
{
    public partial class albumEkleFormcs : Form
    {
        string albumAd;
        SqlConnection con = new SqlConnection("Data Source=.;Initial Catalog=instagram;Integrated Security=True");
        public albumEkleFormcs()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                albumAd = albumAdText.Text.Trim();
                if (albumAd == "")
                    MessageBox.Show("album için bir isim giriniz");
                else
                {
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    SqlCommand command = new SqlCommand("insert into albumTbl (kisiID,albumAd) values(" + Form1.kullaniciID + ",'" + albumAd + "')", con);
                    command.ExecuteNonQuery();
                    command.CommandText = "alter table fototbl"+Form1.kullaniciID+" add " + albumAd + " INT NOT NULL DEFAULT 0";
                    command.ExecuteNonQuery();
                    MessageBox.Show("album olusturulmustur");

                    Form1 frm = new Form1();
                  
                    frm.albumGroupBoxDoldur();
                    con.Close();
                    this.Close();
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            con.Close();
            this.Close();
        }

        private void albumEkleFormcs_Load(object sender, EventArgs e)
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString());
            }
        }
    }
}
