﻿using System;
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
    public partial class Form1 : Form
    {

        public static int kullaniciID;
        int profilTabfotoID;
        int anaTabfotoID;
        int arananKisiID;
        int isArananKisiID;
        int okulArananKisiID;
        string isArananKisiad;
        string tabloAdi = null;
       
        ArrayList arkadasID;
        ArrayList foto = new ArrayList();
        ArrayList kisiID = new ArrayList();
        ArrayList ID = new ArrayList();
        ArrayList fotoID = new ArrayList();
        veriTabani veri = new veriTabani();
        
        SqlConnection con = new SqlConnection("Data Source=.;Initial Catalog=instagram;Integrated Security=True");
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            instagramTab.Controls.Remove(anaSayfaTab);
            instagramTab.Controls.Remove(arkadaslarAraTab);
            instagramTab.Controls.Remove(profilTab);

            kayitgroupBox2.Visible = false;
            gecersizLabel.Visible = false;

            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
            }
            catch (DataException ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void girisBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                string ePosta = null;
                string sifre = null;
                int id = -1;
                profilTabYorumText.Text = "";
                SqlCommand command = new SqlCommand("select ID,ePosta,sifre from kullaniciTbl where ePosta='" + ePostaText.Text
                    + "' and sifre='" + sifreText.Text + "'", con);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    ePosta = reader.GetString(1);
                    sifre = reader.GetString(2);
                    id = reader.GetInt32(0);
                }
                reader.Close();
                //  MessageBox.Show("ID : (" + id + ") e posta : (" + ePosta + ") sifre : (" + sifre+")");
                if (ePosta == ePostaText.Text && sifre == sifreText.Text)
                {
                    kullaniciID = id;
                    instagramTab.Controls.Add(anaSayfaTab);
                  
                    instagramTab.SelectedTab = anaSayfaTab;
                    instagramTab.Controls.Remove(girisTab);
                    anaSayfaPaylasim();
                }
                else
                    MessageBox.Show("gecersiz giriş");
                con.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString());
            }
        }
        private bool arkadasSec(int ID)
        {
            for (int i = 0; i < arkadasID.Count; i++)
            {
                if (ID == Convert.ToInt32(arkadasID[i]))
                    return true;
            }
            return false;
        }
        private void anaSayfaPaylasim()
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
               
                arkadasID = new ArrayList();
                SqlCommand command = new SqlCommand("select kisiID from arkadastbl"+kullaniciID+" ", con);
                SqlDataReader reader = command.ExecuteReader();
                while(reader.Read())
                {
                    arkadasID.Add(reader.GetInt32(0));
                }
                reader.Close();
                command.CommandText="select * from paylasimTbl ";
                reader = command.ExecuteReader();
                yorumlarRichText.Text = "";
                while (reader.Read())
                {
                    if (reader.GetInt32(1)==kullaniciID || arkadasSec(reader.GetInt32(1)))
                    {
                        ID.Add(reader.GetInt32(0));
                        kisiID.Add(reader.GetInt32(1));
                        fotoID.Add(reader.GetInt32(2));
                        foto.Add(reader.GetString(3));
                    }
                }
                if (foto.Count == 0)
                {
                    numericUpDown1.Minimum = 0;
                    numericUpDown1.Maximum = 0;
                    paylasımKisilabel7.Text = "gösterilecek fotograf yok";
                }
                else
                {
                    numericUpDown1.Minimum = 0;
                    numericUpDown1.Maximum = foto.Count - 1;
                    reader.Close();
                    anaTabfotoID = Convert.ToInt32(kisiID[kisiID.Count - 1]);
                    command.CommandText = "select AdSoyad from kullaniciTbl where ID=" + Convert.ToInt32(kisiID[kisiID.Count - 1]) + "";
                    paylasımKisilabel7.Text = command.ExecuteScalar().ToString();
                    anaSayfapictureBox1.Image = Image.FromFile(Convert.ToString(foto[foto.Count - 1]));
                    anaSayfapictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                    //yorumlar
                    command.CommandText = "select yorum,kisiID from yorumTbl where fotoID=" + Convert.ToInt32(fotoID[fotoID.Count - 1]) + " ";
                    ArrayList yorum = new ArrayList();
                    ArrayList yorumKisiID = new ArrayList();
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        yorum.Add(reader.GetString(0));
                        yorumKisiID.Add(reader.GetInt32(1));
                    }
                    reader.Close();
                    int counter = 0;
                    while (counter < yorum.Count)
                    {
                        command.CommandText = "select AdSoyad from kullaniciTbl where ID=" + Convert.ToInt32(yorumKisiID[counter]) + "";
                        string ad = command.ExecuteScalar().ToString();
                        yorumlarRichText.Text += ad + "     : " + yorum[counter] + "\n";
                        counter++;
                    }
                }
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void KaydolBtn_Click(object sender, EventArgs e)
        {
            kayitgroupBox2.Visible = true;
        }
        private void profilBtn_Click(object sender, EventArgs e)
        {
            instagramTab.Controls.Add(profilTab);
            instagramTab.SelectedTab = profilTab;
            instagramTab.Controls.Remove(anaSayfaTab);
            profilFotoDoldur();
            tumArkDoldur();
            isArkGridDoldur();
            isArkDoldu();
            okulArkgridDoldur();
            okulArkDoldur();
            albumGroupBoxDoldur();

        }

        private void arkadasAraBtn_Click(object sender, EventArgs e)
        {
            arkadasAragroupBox8.Visible = true;
            istekgroupBox1.Visible = false;
            instagramTab.Controls.Add(arkadaslarAraTab);         
            instagramTab.Controls.Remove(anaSayfaTab);
            instagramTab.SelectedTab = arkadaslarAraTab;
        }

        private void isteklerBtn_Click(object sender, EventArgs e)
        {
            try
            {
                instagramTab.Controls.Add(arkadaslarAraTab);
                instagramTab.Controls.Remove(anaSayfaTab);
                arkadasAragroupBox8.Visible = false;
                istekgroupBox1.Visible = true;
                instagramTab.SelectedTab = arkadaslarAraTab;


                if (con.State == ConnectionState.Closed)
                    con.Open();


                SqlCommand command = new SqlCommand("select ID,AdSoyad from kullaniciTbl where ID in (select istekGonderenID from istekTbl where istekAlanID = " + kullaniciID + ")", con);


                ArrayList resimler = new ArrayList();
                ArrayList ad = new ArrayList();
                ArrayList ID = new ArrayList();


                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    ad.Add(reader.GetString(1));
                    ID.Add(reader.GetInt32(0));

                }
                reader.Close();

                int counter = 0;
                int counterRow = 0;
                int counterColoum = 0;
                // MessageBox.Show(ad.Count.ToString());

                while (counter < ad.Count)
                {

                    Label txt = new Label();

                    txt.Size = new Size(60, 20);
                    txt.Location = new Point(0 + counterColoum, 0 + counterRow);
                    istekgroupBox1.Controls.Add(txt);

                    txt.Text = Convert.ToString(ad[counter]);
                    PictureBox pic = new PictureBox();

                    try
                    {
                        pic.Image = Image.FromFile(veri.profilFotoSec(Convert.ToInt32(ID[counter])));
                    }
                    catch (Exception)
                    {

                    }
                    pic.SizeMode = PictureBoxSizeMode.StretchImage;
                    pic.Size = new Size(60, 60);
                    pic.Location = new Point(0 + counterColoum, 20 + counterRow);
                    istekgroupBox1.Controls.Add(pic);
                    Button btn = new Button();
                    btn.Size = new Size(60, 20);
                    btn.Name = Convert.ToString(ID[counter]);
                    btn.Location = new Point(0 + counterColoum, 80 + counterRow);
                    istekgroupBox1.Controls.Add(btn);
                    btn.Text = "ONAYLA";
                    btn.Click += new EventHandler(btnClick);
                    if (counter > 0 && counter % 4 == 0)
                    {
                        counterColoum += 60;
                        counterRow = 0;
                    }
                    else
                        counterRow += 100;

                    counter++;

                }
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void btnClick(object sender, EventArgs e)
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                ((Button)sender).Enabled = true;
                ((Button)sender).Text = "ONAYLA";
                SqlCommand command = new SqlCommand("insert into arkadastbl" + kullaniciID + " (kisiID,adSoyad )  values (" + Convert.ToInt32(((Button)sender).Name) + ",(select AdSoyad from kullaniciTbl where ID=" + Convert.ToInt32(((Button)sender).Name) + "))", con);
                command.ExecuteNonQuery();
                command.CommandText = "insert into arkadastbl" + ((Button)sender).Name + " (kisiID,adSoyad ) values (" + kullaniciID + ",(select AdSoyad from kullaniciTbl where ID=" + kullaniciID + "))";
                command.ExecuteNonQuery();
                ((Button)sender).Enabled = false;
                ((Button)sender).Text = "ONAYLANDI";
                command.CommandText = "delete from istekTbl where istekGonderenID=" + Convert.ToInt32(((Button)sender).Name) + "and istekAlanID=" + kullaniciID + " ";
                command.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString());
            }


        }

        private void cıkısBtn_Click(object sender, EventArgs e)
        {
            instagramTab.Controls.Add(girisTab);
            instagramTab.Controls.Remove(anaSayfaTab);
            instagramTab.Controls.Remove(arkadaslarAraTab);
            instagramTab.Controls.Remove(profilTab);
            arkadasID.Clear();
            arkadasID.TrimToSize();
            foto.Clear();
            foto.TrimToSize();
            kisiID.Clear();
            kisiID.TrimToSize();
            ID.Clear();
            ID.TrimToSize();
            fotoID.Clear();
            fotoID.TrimToSize();
            kayitgroupBox2.Visible = false;
            gecersizLabel.Visible = false;
            profilTabpictureBox3.Image = null;
            anaSayfapictureBox1.Image = null;
        }   

        private void fotoEkleBtn_Click(object sender, EventArgs e)
        {
            if (con.State == ConnectionState.Closed)
                con.Open();
            try
            {
                OpenFileDialog fdialog = new OpenFileDialog();//Resmi kullanıcıya seçtirmek için bir open file dialog oluşturuyoruz.
                fdialog.Filter = "Pictures|*.jpg";//Seçilecek dosyanın tipi sadece resim olacağı için resim dosya tiplerini filter olarak belirliyoruz.
                if (DialogResult.OK == fdialog.ShowDialog())// Kullanıcı bir resim seçmiş mi kontrol ediyoruz.
                {
                    string resimYol = fdialog.FileName; //Resmin yolunu alıyoruz.
                    veri.fotoEkle(resimYol,kullaniciID);
                    MessageBox.Show("Fotgraf eklenmiştir");
                    profilFotoDoldur();

                }
                con.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void profilFotoDoldur()
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                int count = fotografgroupBox5.Controls.Count;
                for (int i = 0; i < count; i++)
                {
                    fotografgroupBox5.Controls.Remove(fotografgroupBox5.Controls[0]);
                }
                int coloumCount = 0, rowCount = 0;
                int counter = 0;
                // MessageBox.Show(kullaniciID.ToString());
                SqlCommand command = new SqlCommand("select ID,foto from fototbl" + kullaniciID + " ", con);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    PictureBox pic = new PictureBox();
                    pic.Image = Image.FromFile(reader.GetString(1));
                    pic.SizeMode = PictureBoxSizeMode.StretchImage;
                    pic.Size = new Size(60, 60);
                    pic.Location = new Point(0 + coloumCount, 0 + rowCount);
                    fotografgroupBox5.Controls.Add(pic);
                    if (counter > 0 && (counter) % 6 == 0)
                    {
                        rowCount += 60;
                        coloumCount = 0;
                    }
                    else
                        coloumCount += 60;
                    counter++;
                    pic.Name = Convert.ToString(reader.GetInt32(0));
                    pic.Click += new EventHandler(picClick);

                }
                reader.Close();
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }


        }

        private void picClick(object sender, EventArgs e)
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                profilTabpictureBox3.Image = ((PictureBox)sender).Image;
                profilTabpictureBox3.Name = ((PictureBox)sender).Name;
                profilTabfotoID = Convert.ToInt32(((PictureBox)sender).Name);
                ArrayList yorum = new ArrayList();
                ArrayList kisiID = new ArrayList();
                profilTabYorumText.Text = "";
                SqlCommand command = new SqlCommand("select yorum,kisiID from yorumTbl where fotoID=" + ((PictureBox)sender).Name + "", con);
                // MessageBox.Show(((PictureBox)sender).Name);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    yorum.Add(reader.GetString(0));
                    kisiID.Add(reader.GetInt32(1));
                }
                reader.Close();
                int counter = 0;
                while (counter < yorum.Count)
                {
                    command.CommandText = "select AdSoyad from kullaniciTbl where ID=" + Convert.ToInt32(kisiID[counter]) + "";
                    string ad = command.ExecuteScalar().ToString();
                    profilTabYorumText.Text += ad + "     : " + yorum[counter] + "\n";
                    counter++;
                }
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
       



        private void kaydetBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                int id = 0;
               
                 SqlCommand command = new SqlCommand("insert into kullaniciTbl values ('" + kullaniciAdKaydetText.Text + "','" + sifreKaydettext.Text + "','" + ePostaKayitText.Text + "')", con);
                command.ExecuteNonQuery();
                command.CommandText = "select ID from kullaniciTbl where ePosta='" + ePostaKayitText.Text + "'";
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                    id = reader.GetInt32(0);
                reader.Close();
                tabloAdi = Convert.ToString(id);
                //  MessageBox.Show(tabloAdi);
                veri.arkadasTblOlustur(tabloAdi);
                veri.fotoTblOlustur(tabloAdi);
                MessageBox.Show("kayıt başarılı");
                con.Close();
            }
            catch (DataException ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString());
            }

        }

        private void profilTabpictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void yorumYapProfilBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                if (profilTabfotoID != 0)
                {
                    string fID = kullaniciID.ToString() + profilTabfotoID.ToString();
                    SqlCommand command = new SqlCommand("insert into yorumTbl values(" +Convert.ToInt32(fID) + "," + kullaniciID + ",'" + profilTabYorumYaprichTextBox1.Text + "')", con);
                    command.ExecuteNonQuery();
                    MessageBox.Show("Yorum yapılmıstır");
                    con.Close();
                }
                else
                    MessageBox.Show("yorum yapmak için bir resim secmediniz");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString());
            }

        }

        private void paylasBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                if (profilTabfotoID != 0)
                {
                    string fID = kullaniciID.ToString() + profilTabfotoID.ToString();
                    string fotoYol = null;
                    SqlCommand command1 = new SqlCommand("select foto from fototbl" + kullaniciID + " where ID =" + profilTabfotoID + "", con);
                    SqlDataReader reader = command1.ExecuteReader();
                    if (reader.Read())
                        fotoYol = reader.GetString(0);
                    reader.Close();
                    SqlCommand command = new SqlCommand("insert into paylasimTbl values (" + kullaniciID + ", " + Convert.ToInt32(fID) + ",'" + fotoYol + "')", con);
                    command.ExecuteNonQuery();
                    MessageBox.Show("Fotoğraf Paylaşılmştır");
                    con.Close();
                }
                else
                    MessageBox.Show("paylaşım yapmak için bir resim secmediniz");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void arkadasAraText_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                SqlCommand command = new SqlCommand("select AdSoyad from kullaniciTbl where AdSoyad like '" + arkadasAraText.Text + "%' " +
                    " and ID !=" + kullaniciID + " ", con);

                //and ID != (select kisiID from arkadastbl" + kullaniciID + ")

                SqlDataAdapter adpt = new SqlDataAdapter(command);
                DataTable data = new DataTable();
                adpt.Fill(data);
                aradasAramaListesidataGridView1.DataSource = data;
                con.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void okulArkgridDoldur()
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                SqlCommand command = new SqlCommand("select adSoyad from arkadastbl" + kullaniciID + " where okulArkadas=0", con);
                SqlDataAdapter adpt = new SqlDataAdapter(command);
                DataTable data = new DataTable();
                adpt.Fill(data);
                okulArkDataGridView.DataSource = data;
                con.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void isArkGridDoldur()
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                SqlCommand command = new SqlCommand("select adSoyad from arkadastbl" + kullaniciID + " where isArkadas = 0", con);
                SqlDataAdapter adpt = new SqlDataAdapter(command);
                DataTable data = new DataTable();
                adpt.Fill(data);
                isArkdataGridView.DataSource = data;
                con.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void tumArkDoldur()
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                int count = tumArkgroupBox1.Controls.Count;
                for (int i = 0; i < count; i++)
                {
                    tumArkgroupBox1.Controls.Remove(tumArkgroupBox1.Controls[0]);
                }

                ArrayList resimler = new ArrayList();
                ArrayList ad = new ArrayList();
                ArrayList ID = new ArrayList();

                SqlCommand command = new SqlCommand("select kisiID,adSoyad from arkadastbl" + kullaniciID + "", con);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    ad.Add(reader.GetString(1));
                    ID.Add(reader.GetInt32(0));

                }
                reader.Close();

                int counter = 0;
                int counterRow = 0;
                int counterColoum = 0;
                // MessageBox.Show(ad.Count.ToString());

                while (counter < ad.Count)
                {

                    Label txt = new Label();

                    txt.Size = new Size(60, 20);
                    txt.Location = new Point(0 + counterColoum, 0 + counterRow);
                    tumArkgroupBox1.Controls.Add(txt);

                    txt.Text = Convert.ToString(ad[counter]);
                    PictureBox pic = new PictureBox();
                    try
                    {
                        pic.Image = Image.FromFile(veri.profilFotoSec(Convert.ToInt32(ID[counter])));
                    }
                    catch (Exception)
                    {

                    }
                    pic.SizeMode = PictureBoxSizeMode.StretchImage;
                    pic.Size = new Size(60, 60);
                    pic.Location = new Point(0 + counterColoum, 20 + counterRow);
                    tumArkgroupBox1.Controls.Add(pic);

                    if (counter > 0 && counter % 4 == 0)
                    {
                        counterColoum += 60;
                        counterRow = 0;
                    }
                    else
                        counterRow += 80;

                    counter++;

                }
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void isArkDoldu()
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                int count = isArkgroupBox1.Controls.Count;
                for (int i = 0; i < count; i++)
                {
                    isArkgroupBox1.Controls.Remove(isArkgroupBox1.Controls[0]);
                }

                ArrayList resimler = new ArrayList();
                ArrayList ad = new ArrayList();
                ArrayList ID = new ArrayList();

                SqlCommand command = new SqlCommand("select kisiID,adSoyad from arkadastbl" + kullaniciID + " where isArkadas=1", con);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    ad.Add(reader.GetString(1));
                    ID.Add(reader.GetInt32(0));

                }
                reader.Close();

                int counter = 0;
                int counterRow = 0;
                int counterColoum = 0;
                // MessageBox.Show(ad.Count.ToString());

                while (counter < ad.Count)
                {

                    Label txt = new Label();

                    txt.Size = new Size(60, 20);
                    txt.Location = new Point(0 + counterColoum, 0 + counterRow);
                    isArkgroupBox1.Controls.Add(txt);

                    txt.Text = Convert.ToString(ad[counter]);
                    PictureBox pic = new PictureBox();
                    try
                    {
                        pic.Image = Image.FromFile(veri.profilFotoSec(Convert.ToInt32(ID[counter])));
                    }
                    catch (Exception)
                    {

                    }
                    pic.SizeMode = PictureBoxSizeMode.StretchImage;
                    pic.Size = new Size(60, 60);
                    pic.Location = new Point(0 + counterColoum, 20 + counterRow);
                    isArkgroupBox1.Controls.Add(pic);

                    if (counter > 0 && counter % 4 == 0)
                    {
                        counterColoum += 60;
                        counterRow = 0;
                    }
                    else
                        counterRow += 80;

                    counter++;

                }
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void okulArkDoldur()
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                int count = okulArkgroupBox1.Controls.Count;
                for (int i = 0; i < count; i++)
                {
                    okulArkgroupBox1.Controls.Remove(okulArkgroupBox1.Controls[0]);
                }

                ArrayList resimler = new ArrayList();
                ArrayList ad = new ArrayList();
                ArrayList ID = new ArrayList();

                SqlCommand command = new SqlCommand("select kisiID,adSoyad from arkadastbl" + kullaniciID + " where okulArkadas=1", con);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    ad.Add(reader.GetString(1));
                    ID.Add(reader.GetInt32(0));

                }
                reader.Close();

                int counter = 0;
                int counterRow = 0;
                int counterColoum = 0;
                // MessageBox.Show(ad.Count.ToString());

                while (counter < ad.Count)
                {

                    Label txt = new Label();

                    txt.Size = new Size(60, 20);
                    txt.Location = new Point(0 + counterColoum, 0 + counterRow);
                    okulArkgroupBox1.Controls.Add(txt);

                    txt.Text = Convert.ToString(ad[counter]);
                    PictureBox pic = new PictureBox();
                    try
                    {
                        pic.Image = Image.FromFile(veri.profilFotoSec(Convert.ToInt32(ID[counter])));
                    }
                    catch (Exception)
                    {

                    }
                    pic.SizeMode = PictureBoxSizeMode.StretchImage;
                    pic.Size = new Size(60, 60);
                    pic.Location = new Point(0 + counterColoum, 20 + counterRow);
                    okulArkgroupBox1.Controls.Add(pic);

                    if (counter > 0 && counter % 4 == 0)
                    {
                        counterColoum += 60;
                        counterRow = 0;
                    }
                    else
                        counterRow += 80;

                    counter++;

                }
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void aradasAramaListesidataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                int row = aradasAramaListesidataGridView1.CurrentCell.RowIndex;
                int column = aradasAramaListesidataGridView1.CurrentCell.ColumnIndex;
                string coloumName = aradasAramaListesidataGridView1.Columns[column].Name;
                string value = aradasAramaListesidataGridView1.Rows[row].Cells[column].Value.ToString();
                if (con.State == ConnectionState.Closed)
                    con.Open();
                SqlCommand command = new SqlCommand("select ID,AdSoyad from kullaniciTbl where " + coloumName + "= '" + value + "'", con);
                istekYollaBtn.Enabled = true;
                istekYollaBtn.Text = "istek yolla";
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    arananKisiID = reader.GetInt32(0);
                    arananKisiAdtext.Text = reader.GetString(1);
                }
                reader.Close();
                con.Close();
                try
                {
                    arananKisipictureBox2.Image = Image.FromFile(veri.profilFotoSec(arananKisiID));
                }
                catch (Exception)
                {

                }
                arananKisipictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
                if (!istekControl())
                {
                    istekYollaBtn.Enabled = false;
                    istekYollaBtn.Text = "ARKADAŞSIN";
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void istekYollaBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                if (arananKisiID == 0)
                    MessageBox.Show("bir kişi secmediniz");
                else
                {
                    SqlCommand command = new SqlCommand("insert into istekTbl values(" + kullaniciID + "," + arananKisiID + ")", con);
                    command.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("istek gönderilmiştir");
                    con.Close();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString());
            }

        }

        private bool istekControl()
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                SqlCommand command = new SqlCommand("select kisiID from arkadastbl" + kullaniciID + " ", con);
                SqlDataReader reaer = command.ExecuteReader();
                while (reaer.Read())
                {
                    if (reaer.GetInt32(0) == arananKisiID)
                    {
                        reaer.Close();
                        con.Close();
                        return false;
                    }
                }
                reaer.Close();
                con.Close();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            return false;
        }

        private void isArkdataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                int row = isArkdataGridView.CurrentCell.RowIndex;
                int column = isArkdataGridView.CurrentCell.ColumnIndex;
                string coloumName = isArkdataGridView.Columns[column].Name;
                string value = isArkdataGridView.Rows[row].Cells[column].Value.ToString();
                
                SqlCommand command = new SqlCommand("select kisiID , adSoyad from arkadastbl" + kullaniciID + " where " + coloumName + "='" + value + "'", con);

                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    isArananKisiID = reader.GetInt32(0);
                    isArananKisiad = reader.GetString(1);
                }
                try
                {
                    isArkSecpictureBox1.Image = Image.FromFile(veri.profilFotoSec(isArananKisiID));
                }
                catch (Exception)
                {

                }
                isArkSecpictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                reader.Close();
                con.Close();


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void isArkadasEkleBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                if (isArananKisiID == 0)
                    MessageBox.Show("bir kişi secmediniz");
                else
                {
                    //MessageBox.Show("ad : " + isArananKisiad + " ID : " + isArananKisiID.ToString());
                    SqlCommand command = new SqlCommand("update arkadastbl" + kullaniciID + " set isArkadas=1 where kisiID=" + isArananKisiID + "", con);
                    command.ExecuteNonQuery();
                    isArkDoldu();
                    isArkGridDoldur();
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }

        }

        private void okulArkDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                int row = okulArkDataGridView.CurrentCell.RowIndex;
                int column = okulArkDataGridView.CurrentCell.ColumnIndex;
                string coloumName = okulArkDataGridView.Columns[column].Name;
                string value = okulArkDataGridView.Rows[row].Cells[column].Value.ToString();

                SqlCommand command = new SqlCommand("select kisiID , adSoyad from arkadastbl" + kullaniciID + " where " + coloumName + "='" + value + "'", con);

                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    okulArananKisiID = reader.GetInt32(0);
                    isArananKisiad = reader.GetString(1);
                }
                try
                {
                    okulArkSecpictureBox1.Image = Image.FromFile(veri.profilFotoSec(okulArananKisiID));
                }
                catch (Exception)
                {

                }
                okulArkSecpictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                reader.Close();
                con.Close();


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void okulArkadasEkleBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                if (okulArananKisiID == 0)
                    MessageBox.Show("bir kişi secnediniz");
                else
                {
                   // MessageBox.Show("ad : " + isArananKisiad + " ID : " + okulArananKisiID.ToString());
                    SqlCommand command = new SqlCommand("update arkadastbl" + kullaniciID + " set okulArkadas=1 where kisiID=" + okulArananKisiID + "", con);
                    command.ExecuteNonQuery();
                    okulArkDoldur();
                    okulArkgridDoldur();
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void albumOlsturBtn_Click(object sender, EventArgs e)
        {
            try
            {
                albumEkleFormcs album = new albumEkleFormcs();
                album.Show();
              

                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
        public  void albumGroupBoxDoldur()
        {

            try
            {
              
                if (con.State == ConnectionState.Closed)
                    con.Open();
                int count = albumgroupBox1.Controls.Count;
                for (int i = 0; i < count; i++)
                {
                    albumgroupBox1.Controls.Remove(albumgroupBox1.Controls[0]);
                }
              
                ArrayList albumAd = new ArrayList();
               


                SqlCommand command = new SqlCommand("select albumAd from albumTbl where kisiID="+kullaniciID+"", con);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    albumAd.Add(reader.GetString(0));

                }
                reader.Close();

                int counter = 0;
              
                int counterColoum = 0;
                // MessageBox.Show(ad.Count.ToString());

                while (counter < albumAd.Count)
                {

                    Label txt = new Label();

                    txt.Size = new Size(100, 20);
                    txt.Location = new Point(0 + counterColoum, 110);
                    albumgroupBox1.Controls.Add(txt);
                    
                      txt.Text = Convert.ToString(albumAd[counter]);
                    PictureBox pic = new PictureBox();
                    pic.Image = albumOlsturpictureBox4.Image;
                    pic.SizeMode = PictureBoxSizeMode.StretchImage;
                    pic.Size = new Size(100, 100);
                    pic.Name= Convert.ToString(albumAd[counter]);
                    pic.Location = new Point(0 + counterColoum, 10);
                    pic.Click += new EventHandler(secilenAlbumDoldur);
                    albumgroupBox1.Controls.Add(pic);

                    Button btn = new Button();
                    btn.Name= Convert.ToString(albumAd[counter]);
                    btn.Size = new Size(100, 25);
                    btn.Text = "Fotograf Ekle";
                    btn.Location = new Point(0 + counterColoum, 130);
                    counterColoum += 100;
                    btn.Click += new EventHandler(albumeFotoEkle);
                    albumgroupBox1.Controls.Add(btn);
                    counter++;
                }

                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
       
       public void secilenAlbumDoldur(object sender, EventArgs e)
        {
            try
            {

                if (con.State == ConnectionState.Closed)
                    con.Open();
                SqlCommand command = new SqlCommand("select foto from fototbl" + kullaniciID + " where " + ((PictureBox)sender).Name + "=1", con);
                SqlDataReader reader = command.ExecuteReader();
                int counter = 0;
                ArrayList resim = new ArrayList();
                int counterColoum = 0, counterRow = 0;
                while (reader.Read())
                {
                    resim.Add(reader.GetString(0));

                }
                reader.Close();
                while (counter < resim.Count)
                {
                    PictureBox pic = new PictureBox();
                    pic.Image = Image.FromFile(Convert.ToString(resim[counter]));
                    pic.SizeMode = PictureBoxSizeMode.StretchImage;
                    pic.Size = new Size(100, 100);
                    pic.Location = new Point(0 + counterRow + counterColoum, 10);
                 
                    albumResimlergroupBox6.Controls.Add(pic);
                    if (counterColoum == 6)
                    {
                        counterRow += 100;
                        counterColoum = 0;
                    }
                    counterColoum += 100;
                    counter++;
                }
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void albumeFotoEkle(object sender,EventArgs e)
        {
            albumFotoEkleForm frm = new albumFotoEkleForm();
            frm.Text = ((Button)sender).Name;
            frm.Show();
        }

       

        private void yorumEkleBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                if (anaTabfotoID != 0)
                {
                    SqlCommand command = new SqlCommand("insert into yorumTbl values(" +(kullaniciID+ anaTabfotoID) + "," + kullaniciID + ",'" +anaSayfaYorumYaprichTextBox1.Text + "')", con);
                    command.ExecuteNonQuery();
                    MessageBox.Show("Yorum yapılmıstır");
                    con.Close();
                }
                else
                    MessageBox.Show("yorum yapmak için bir resim secmediniz");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void anaSayfapictureBox1_Click(object sender, EventArgs e)
        {

        }
      
        
      

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                if (numericUpDown1.Maximum == 0)
                {
                    
                }
                else
                {
                    yorumlarRichText.Text = "";
                    // MessageBox.Show(index.ToString());
                    SqlCommand command = new SqlCommand("select AdSoyad from kullaniciTbl where ID=" + Convert.ToInt32(kisiID[Convert.ToInt32(numericUpDown1.Value)]) + "", con);
                    paylasımKisilabel7.Text = command.ExecuteScalar().ToString();
                    anaSayfapictureBox1.Image = Image.FromFile(Convert.ToString(foto[Convert.ToInt32(numericUpDown1.Value)]));
                    anaSayfapictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;

                    command.CommandText = "select yorum,kisiID from yorumTbl where fotoID=" + Convert.ToInt32(fotoID[Convert.ToInt32(numericUpDown1.Value)]) + "";
                    ArrayList yorum = new ArrayList();
                    ArrayList yorumKisiID = new ArrayList();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        yorum.Add(reader.GetString(0));
                        yorumKisiID.Add(reader.GetInt32(1));
                    }
                    reader.Close();
                    int counter = 0;
                    while (counter < yorum.Count)
                    {
                        command.CommandText = "select AdSoyad from kullaniciTbl where ID=" + Convert.ToInt32(yorumKisiID[counter]) + "";
                        string ad = command.ExecuteScalar().ToString();
                        yorumlarRichText.Text += ad + "     : " + yorum[counter] + "\n";
                        counter++;
                    }
                }
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void geriBtn_Click(object sender, EventArgs e)
        {
            anaSayfaPaylasim();
            instagramTab.Controls.Add(anaSayfaTab);
            instagramTab.SelectedTab = anaSayfaTab;
            instagramTab.Controls.Remove(profilTab);
            instagramTab.Controls.Remove(arkadaslarAraTab);
        }
    }
}