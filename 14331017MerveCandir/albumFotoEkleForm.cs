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
using System.Collections;

namespace _14331017MerveCandir
{
    public partial class albumFotoEkleForm : Form
    {
        public albumFotoEkleForm()
        {
            InitializeComponent();
        }
       
        SqlConnection con = new SqlConnection("Data Source=.;Initial Catalog=instagram;Integrated Security=True");
        private void albumFotoEkleForm_Load(object sender, EventArgs e)
        {
            ArrayList ID = new ArrayList();
            ArrayList foto = new ArrayList();
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
               
                SqlCommand command = new SqlCommand("select ID,foto from fototbl"+Form1.kullaniciID+" where "+Text+"!=1 ", con);
                SqlDataReader reader = command.ExecuteReader();
                
                while(reader.Read())
                {
                    ID.Add(reader.GetInt32(0));
                    foto.Add(reader.GetString(1));
                }
                reader.Close();

                for (int i = 0; i < tumFotogroupBox1.Controls.Count; i++)
                {
                    tumFotogroupBox1.Controls.Remove(tumFotogroupBox1.Controls[0]); 
                }
               
                int coloumCount = 0, rowCount = 0;
                int counter = 0;
                while(counter<foto.Count)
                {
                    PictureBox pic = new PictureBox();
                    pic.Size = new Size(60, 60);
                    pic.Image = Image.FromFile(Convert.ToString(foto[counter]));
                    pic.Name = Convert.ToString(ID[counter]);
                    pic.Location = new Point(0 + coloumCount, 10 + rowCount);
                    pic.SizeMode = PictureBoxSizeMode.StretchImage;
                    pic.Click += new EventHandler(picClick);
                    tumFotogroupBox1.Controls.Add(pic);
                    if(counter>0 && counter%9==0)
                    {
                        coloumCount = 0;
                        rowCount += 60;
                    }
                    coloumCount += 60;
                    counter++;
                }
              
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void picClick(object sender, EventArgs e)
        {
            secilenFotopictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            secilenFotopictureBox1.Image = ((PictureBox)sender).Image;
           secilenFotopictureBox1.Name = ((PictureBox)sender).Name;
           // MessageBox.Show(secilenFotopictureBox1.Name);
            
        }

        private void ekleBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                SqlCommand command = new SqlCommand("update fototbl"+Form1.kullaniciID+" set "+this.Text+"=1 where ID="+Convert.ToInt32( secilenFotopictureBox1.Name)+"", con);
                command.ExecuteNonQuery();
                MessageBox.Show("FOROGRAF EKLENMİŞTİR");
                Form1 frm = new Form1();
              
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("bir fotograf secmediniz");
            }
        }
    }
}
