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
    internal class Sub
    {
        public int id { get; set; }
        public int userid { get; set; }
        public int blogerid { get; set; }
        public DateTime date { get; set; }
    }
    internal class MetodsSub
    {
        List<Sub> sub = new List<Sub>();
        public void SourseLDS(ref List<Sub> SourseLDS)
        {
            SourseLDS = sub;
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
        public bool ADDBd(Sub dS, string text)
        {
            if (CheckConnection())
            {
                using var cmd = new NpgsqlCommand();
                try
                {
                    cmd.Connection = con;
                    cmd.CommandText = $"INSERT INTO {text} (userid,blogerid,date)" +
                        $" VALUES ({dS.userid},{dS.blogerid},'{dS.date}')";
                    cmd.ExecuteNonQueryAsync();
                }
                catch
                {
                    MessageBox.Show("Exit", "Попробуйте снова");
                    return false;
                }
                cmd.Dispose();
            }
            else { MessageBox.Show("Exit", "Попробуйте снова"); return false; }
            return true;
        }
        public bool PRINTBd(Sub ds, string text)
        {
            if (CheckConnection())
            {
                LDS lDS = new LDS();
                using var cmd = new NpgsqlCommand();
                NpgsqlDataReader reader;
                try
                {
                    cmd.Connection = con;
                    cmd.CommandText = $"SELECT * FROM {text} WHERE userid = {ds.userid} AND blogerid = {ds.blogerid}";
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        lDS.id = (int)reader["id"];
                        lDS.userid = (int)reader["userid"];
                        lDS.mediaid = (int)reader["blogerid"];
                        lDS.date = (DateTime)reader["date"];
                    }
                    if (lDS.mediaid == 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch
                {
                    MessageBox.Show("Exit", "Попробуйте снова");
                    return false;
                }
            }
            else { return false; }
        }
        public bool PRINTBd(string text)
        {
            if (CheckConnection())
            {
                using var cmd = new NpgsqlCommand();
                try
                {
                    cmd.Connection = con;
                    cmd.CommandText = $"SELECT * FROM {text} ";
                    NpgsqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        sub.Add(
                            new Sub()
                            {
                                id = (int)reader["id"],
                                userid = (int)reader["userid"],
                                blogerid = (int)reader["blogerid"],
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
                con.Dispose();
                con.Close();
            }
            else { return false; }
            return true;
        }
        public bool DROPBd(Sub lDS, string text)
        {
            if (CheckConnection())
            {
                using var cmd = new NpgsqlCommand();
                try
                {
                    cmd.Connection = con;
                    cmd.CommandText = $"DELETE FROM {text} WHERE userid = {lDS.userid} AND blogerid = {lDS.blogerid}";
                    cmd.ExecuteNonQueryAsync();
                }
                catch
                {
                    MessageBox.Show("Exit", "Попробуйте снова");
                    return false;
                }
            }
            else { return false; }
            return true;

        }
        ~MetodsSub()
        {
            con.Dispose();
            con.Close();
        }
    }
}

