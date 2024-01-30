using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp3
{
    internal class Comment
    {
        public int id { get; set; }
        public int userid { get; set; }
        public int mediaid { get; set; }
        public string comment { get; set; }
        public DateTime date { get; set; }
        public int like { get; set; }
        public int deslike { get; set; }
    }
    internal class MetodsComment
    {
        List<Comment> comments = new List<Comment>();
        private NpgsqlConnection con = new NpgsqlConnection(
        connectionString: ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
        public void SourseMedia(ref List<Comment> SourseComment)
        {
            SourseComment = comments;
        }

        public MetodsComment()
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
                cmd.CommandText = $"CREATE TABLE comments (id SERIAL PRIMARY KEY," +
                    "userid integer NOT NULL," +
                    "mediaid integer NOT NULL," +
                    "date timestamp NOT NULL," +
                    "comment text NOT NULL," +
                    "likecomm integer DEFAULT 0," +
                    "deslikecomm integer DEFAULT 0 )";
                cmd.ExecuteNonQueryAsync();
            }
            catch
            {
                MessageBox.Show("Exit", "Попробуйте снова");
            }
            cmd.Dispose();
            return true;
        }
        public bool ADDBd(Comment comment)
        {
            using var cmd = new NpgsqlCommand();
            try
            {
                cmd.Connection = con;
                cmd.CommandText = $"INSERT INTO comments (userid,mediaid,date,comment)" +
                    $" VALUES ({comment.userid},{comment.mediaid},'{comment.date}'," +
                    $"'{comment.comment}')";
                cmd.ExecuteNonQueryAsync();
            }
            catch
            {
                MessageBox.Show("Exit", "Попробуйте снова");
            }
            cmd.Dispose();
            return true;
        }

        public bool PRINTBd(int m_id)
        {
            using var cmd = new NpgsqlCommand();
            cmd.Connection = con;

            cmd.CommandText = $"SELECT * FROM comments WHERE mediaid = {m_id} ORDER BY date DESC";
            NpgsqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                comments.Add(
                    new Comment()
                    {
                        id = (int)reader["id"],
                        userid = (int)reader["userid"],
                        mediaid = (int)reader["mediaid"],
                        comment = (string)reader["comment"],
                        date = (DateTime)reader["date"],
                        like = (int)reader["likecomm"],
                        deslike = (int)reader["deslikecomm"]
                    }
                );
            }

            cmd.Dispose();
            return true;
        }

        //public void UPDATEBd(MediaFiles user)
        //{
        //    con.Open();
        //    using var cmd = new NpgsqlCommand();
        //    cmd.Connection = con;

        //    cmd.CommandText = $"UPDATE polzovatel" +
        //    $" SET first_name='{user.Name}'," +
        //    $"last_name='{user.Surname}'," +
        //    $"email='{user.email}'," +
        //    $"password='{user.password}'" +
        //    $" WHERE id = {user.id}";
        //    cmd.ExecuteNonQueryAsync();

        //    cmd.Dispose();
        //    con.Close();
        //    con.Dispose();
        //}
        ~MetodsComment()
        {
            con.Close();
            con.Dispose();
        }
    }
}