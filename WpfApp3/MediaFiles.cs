using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WpfApp3.MainWindow;
using System.Windows;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;
using System.Xml;
using System.Security.Policy;
using System.Windows.Shapes;
using System.Configuration;

namespace WpfApp3
{
    internal class MediaFiles
    {
        public int id { get; set; }
        public string URI { get; set; }
        public string name { get; set; }
        public DateTime date { get; set; }
        public int like { get; set; }
        public int deslike { get; set; }
        public int userid { get; set; }
    }
    internal class MetodsMediaFiles
    {
        List<MediaFiles> MedFiles = new List<MediaFiles>();
        private NpgsqlConnection con = new NpgsqlConnection(
        connectionString: ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);

        public void SourseMedia(ref List<MediaFiles> SourseMedia)
        {
            SourseMedia = MedFiles;
        }

        public MetodsMediaFiles()
        {
            CheckConnection();
        }

        private bool CheckConnection()
        {
            try
            {
                con.Open();
            }
            catch
            {
                MessageBox.Show("Error");
                return false;
            }
            return true;
        }

        //https://drive.google.com/uc?export=download&confirm=no_antivirus&id=1IBKD6eM8CpWzu5NpVtujjwIpp3qm0nnD povezlo
        //https://drive.google.com/uc?export=download&confirm=no_antivirus&id=13nyFLhs8wP2FfPoRP0s2UYEM2vpawUUC night
        //https://drive.google.com/uc?export=download&confirm=no_antivirus&id=1ExCGtP-eZ_Kz0fkEvMQTnrQLgrLaxT82 ti viigral
        //https://drive.google.com/uc?export=download&confirm=no_antivirus&id=1gKuuhDjPebccv_yVmkZxqCLw9LGyNHeb cyberpubk
        public bool CreateBd()
        {
            using var cmd = new NpgsqlCommand();
            try
            {
                cmd.Connection = con;
                cmd.CommandText = ($"DROP TABLE IF EXISTS mediafile");
                cmd.ExecuteNonQueryAsync();
                cmd.CommandText = "CREATE TABLE mediafile(id SERIAL PRIMARY KEY, " +
                    "uri text NOT NULL UNIQUE CHECK(uri!=''), " +
                    "name text NOT NULL UNIQUE CHECK(name!=''), " +
                    "date timestamp NOT NULL, " +
                    "likeonly integer DEFAULT 0," +
                    "deslikeonly integer DEFAULT 0," +
                    "userid integer NOT NULL CHECK(userid!=null))";
                cmd.ExecuteNonQueryAsync();
            }
            catch
            {
                MessageBox.Show("Exit", "Попробуйте снова");
            }
            cmd.Dispose();
            return true;
        }

        public bool ADDBd(MediaFiles mediaFiles)
        {

            using var cmd = new NpgsqlCommand();
            try
            {
                cmd.Connection = con;
                cmd.CommandText = $"INSERT INTO mediafile (uri,name,date,userid)" +
                    $" VALUES ('{mediaFiles.URI}','{mediaFiles.name}','{mediaFiles.date}'," +
                    $"{mediaFiles.userid})";
                cmd.ExecuteNonQueryAsync();
            }
            catch
            {
                MessageBox.Show("Exit", "Попробуйте снова");
                return false;
            }
            cmd.Dispose();
            return true;
        }

        public bool PRINTBd()
        {
            using var cmd = new NpgsqlCommand();
            cmd.Connection = con;
            cmd.CommandText = $"SELECT * FROM mediafile ORDER BY likeonly ,date DESC";
            NpgsqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                MedFiles.Add(
                    new MediaFiles()
                    {
                        id = (int)reader["id"],
                        URI = (string)reader["uri"],
                        name = (string)reader["name"],
                        date = (DateTime)reader["date"],
                        like = (int)reader["likeonly"],
                        deslike = (int)reader["deslikeonly"],
                        userid = (int)reader["userid"]
                    }
                );
            }
            if (MedFiles.Count == 0)
            {
                return false;
            }

            cmd.Dispose();
            return true;
        }
        public bool PRINTBdWithPoiskovik(string text)
        {
            using var cmd = new NpgsqlCommand();
            cmd.Connection = con;
            cmd.CommandText = $"SELECT * FROM mediafile WHERE name LIKE '{text}%'";
            NpgsqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                MedFiles.Add(
                    new MediaFiles()
                    {
                        id = (int)reader["id"],
                        URI = (string)reader["uri"],
                        name = (string)reader["name"],
                        date = (DateTime)reader["date"],
                        like = (int)reader["likeonly"],
                        deslike = (int)reader["deslikeonly"],
                        userid = (int)reader["userid"]
                    }
                );
            }
            if (MedFiles.Count == 0)
            {
                return false;
            }
            cmd.Dispose();
            return true;
        }
        public bool UPDATEBd(MediaFiles mediaFiles)
        {
            using var cmd = new NpgsqlCommand();
            try
            {
                cmd.Connection = con;
                cmd.CommandText = $"UPDATE mediafile" +
                $" SET likeonly='{mediaFiles.like}'," +
                $" deslikeonly='{mediaFiles.deslike}'" +
                $" WHERE id = {mediaFiles.id}";
                cmd.ExecuteNonQuery();
            }
            catch
            {
                MessageBox.Show("Exit", "Попробуйте снова");
                return false;
            }
            cmd.Dispose();
            return true;
        }
        ~MetodsMediaFiles()
        {
            con.Close();
            con.Dispose();
        }
    }
}