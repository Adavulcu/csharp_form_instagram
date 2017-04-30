using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace _14331017MerveCandir
{
    class veriTabani
    {
        SqlConnection con = new SqlConnection("Data Source=.;Initial Catalog=instagram;Integrated Security=True");
        SqlCommand command = new SqlCommand();

        public void arkadasTblOlustur(string tabloAdi)
        {
            try
            {
               
 ;                if (con.State == ConnectionState.Closed)
                    con.Open();
                command.Connection = con;
                command.CommandText = "create table  arkadastbl"+tabloAdi+" ("+
                    "ID INT IDENTITY (1,1) PRIMARY KEY NOT NULL ," +
                    "kisiID INT NOT NULL,"+
                    "adSoyad NVARCHAR (MAX) NOT NULL," +
                    "isArkadas INT DEFAULT 0 ," +
                    "okulArkadas INT DEFAULT 0 )";
                   
                command.ExecuteNonQuery();
                con.Close();
            }
          
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }
        public void fotoEkle( string resimYol, int kisiID)
        {

            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                SqlCommand command = new SqlCommand("INSERT INTO fototbl"+kisiID+" (foto) values ('" + resimYol + "')", con);
                command.ExecuteNonQuery();
                con.Close();
              
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void fotoTblOlustur(string tabloAdi)
        {
            try
            {

                 if (con.State == ConnectionState.Closed)
                    con.Open();
                command.Connection = con;
                command.CommandText = "create table  fototbl"+ tabloAdi +" (" +
                    "ID INT IDENTITY (1,1) PRIMARY KEY NOT NULL ," +
                    "foto NVARCHAR (MAX) NOT NULL)";
                command.ExecuteNonQuery();
                con.Close();
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }
       
        public string profilFotoSec(int ID)
        {
            if (con.State == ConnectionState.Closed)
                con.Open();
            command.Connection = con;
            command.CommandText = "select foto from fototbl"+ID+" where ID=1";
            int kontrol = command.ExecuteNonQuery();
            
            if (kontrol == 0)
                return null;
            else
            {
                string foto = null;
                foto = command.ExecuteScalar().ToString();
                return foto;
            }
        }
        
    }
}
