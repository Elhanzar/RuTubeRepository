using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static System.Net.Mime.MediaTypeNames;

namespace WpfApp3
{
    internal class LDS
    {
        public int id { get; set; }
        public int userid { get; set; }
        public int mediaid { get; set; }
        public DateTime date { get; set; }
    }
    internal class MetodsLDS
    {
        List<LDS> lds = new List<LDS>();
        public void SourseLDS(ref List<LDS> SourseLDS)
        {
            SourseLDS = lds;
        }

        public MetodsLDS()
        {
            CheckConnection();
        }

        private NpgsqlConnection con = new NpgsqlConnection(
        connectionString: ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);

        private bool CheckConnection()
        {
            try
            {
                con.Open();
            }
            catch (NpgsqlException)
            {
                MessageBox.Show("Error");
                return false;
            }
            return true;
        }
        public bool ADDBd(LDS dS, string text)
        {
            using var cmd = new NpgsqlCommand();
            try
            {
                cmd.Connection = con;
                cmd.CommandText = $"INSERT INTO {text} (userid,mediaid,date)" +
                    $" VALUES ({dS.userid},{dS.mediaid},'{dS.date}')";
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
        public bool PRINTBd(LDS ds, string text)
        {
            LDS lDS = new LDS();
            using var cmd = new NpgsqlCommand();
            NpgsqlDataReader reader;

            cmd.Connection = con;
            cmd.CommandText = $"SELECT * FROM {text} WHERE userid = {ds.userid} AND mediaid = {ds.mediaid}";
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lDS.id = (int)reader["id"];
                lDS.userid = (int)reader["userid"];
                lDS.mediaid = (int)reader["mediaid"];
                lDS.date = (DateTime)reader["date"];
            }
            cmd.Dispose();
            if (lDS.mediaid == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool PRINTBd(string text)
        {
            using var cmd = new NpgsqlCommand();
            try
            {
                cmd.Connection = con;
                cmd.CommandText = $"SELECT * FROM {text} ";
                NpgsqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lds.Add(
                        new LDS()
                        {
                            id = (int)reader["id"],
                            userid = (int)reader["userid"],
                            mediaid = (int)reader["mediaid"],
                            date = (DateTime)reader["date"]
                        }
                    );
                }
            }
            catch
            {
                MessageBox.Show("Exit", "Попробуйте снова");
                return false;
            }

            cmd.Dispose();
            return true;
        }
        public bool DROPBd(LDS lDS, string text)
        {
            using var cmd = new NpgsqlCommand();
            try
            {
                cmd.Connection = con;
                cmd.CommandText = $"DELETE FROM {text} WHERE userid = {lDS.userid} AND mediaid = {lDS.mediaid}";
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
        ~MetodsLDS()
        {
            con.Close();
            con.Dispose();
        }
    }
}