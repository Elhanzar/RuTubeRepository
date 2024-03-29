﻿using Microsoft.VisualBasic;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using static System.Net.Mime.MediaTypeNames;

namespace WpfApp3
{
    [Serializable]
    public class User
    {
        public int id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string PhotoUser { get; set; }
        public int subscriber { get; set; }

    }
    public class MetodsUser
    {
        User UserUser = new User();
        private NpgsqlConnection con = new NpgsqlConnection(
        connectionString: ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);

        public void SourseUser(ref User SourseUser)
        {
            SourseUser = UserUser;
        }
        public MetodsUser()
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

        public bool CreateBd()
        {
            using var cmd = new NpgsqlCommand();
            try
            {
                cmd.Connection = con;
                cmd.CommandText = ($"DROP TABLE IF EXISTS polzovatel");
                cmd.ExecuteNonQueryAsync();
                cmd.CommandText = "CREATE TABLE polzovatel (id SERIAL PRIMARY KEY," +
                    "first_name text NOT NULL," +
                    "last_name text NOT NULL," +
                    "email text NOT NULL UNIQUE CHECK(email!='')," +
                    "password text NOT NULL)";
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

        public bool ADDBd(User user)
        {
            using var cmd = new NpgsqlCommand();
            try
            {
                cmd.Connection = con;
                cmd.CommandText = $"INSERT INTO polzovatel (first_name,last_name,email,password,\"FotoUser\") VALUES ('{user.first_name}', '{user.last_name}', '{user.email}', '{user.password}', '{user.PhotoUser}')";
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

        public bool PRINTBd(User user)
        {
            using var cmd = new NpgsqlCommand();
            try
            {
                cmd.Connection = con;
                cmd.CommandText = $"SELECT * FROM polzovatel WHERE password = '{user.password}'" +
                    $" AND first_name = '{user.first_name}'" +
                    $" AND last_name = '{user.last_name}'" +
                    $" AND email = '{user.email}'";
                NpgsqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    UserUser.id = (int)reader["id"];
                    UserUser.first_name = (string)reader["first_name"]; // column index can be used
                    UserUser.last_name = (string)reader["last_name"]; // another syntax option
                    UserUser.email = (string)reader["email"];
                    UserUser.password = (string)reader["password"];
                    UserUser.PhotoUser = (string)reader["FotoUser"];
                    UserUser.subscriber = (int)reader["subscriber"];
                }
                if (UserUser.id == 0)
                {
                    MessageBox.Show("We don't have this user");
                    return false;
                }
            }
            catch
            {
                return false;
            }
            cmd.Dispose();
            return true;
        }
        public bool PRINTBd(int u, ref User SourseUser)
        {
            using var cmd = new NpgsqlCommand();
            try
            {
                cmd.Connection = con;
                cmd.CommandText = $"SELECT * FROM polzovatel WHERE id = {u}";
                NpgsqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    UserUser.id = (int)reader["id"];
                    UserUser.first_name = (string)reader[1];
                    UserUser.last_name = (string)reader[2];
                    UserUser.PhotoUser = (string)reader["FotoUser"];
                    UserUser.subscriber = (int)reader["subscriber"];
                }
                SourseUser = UserUser;

            }
            catch
            {
                return false;
            }
            return true;
        }
        public bool UPDATEBd(User user)
        {
            using var cmd = new NpgsqlCommand();
            try
            {
                cmd.Connection = con;
                cmd.CommandText = $"UPDATE polzovatel SET subscriber = {user.subscriber} WHERE id = {user.id}";
                cmd.ExecuteNonQuery();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
            cmd.Dispose();
            return true;
        }
        ~MetodsUser()
        {
            try
            {
                con.Close();
            }
            catch { }
            con.Dispose();
        }
    }

}