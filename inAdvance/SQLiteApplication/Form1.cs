using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;
using System.Diagnostics;
using System.Data.SQLite;

namespace SQLiteApplication
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void buttonCreateDatabase_Click(object sender, EventArgs e)
        {
            var sqliteFile = "app.sqlite";
            if ( ! File.Exists(sqliteFile))
            {
                SQLiteConnection.CreateFile(sqliteFile);
            }
            
        }

        private void buttonCreateTable_Click(object sender, EventArgs e)
        {
            using (SQLiteConnection con = new SQLiteConnection("Data Source=app.sqlite;Version=3;"))
            {
                con.Open();
                string createSql = "create table if not exists settings( ssection varchar(64), skey varchar(64), svalue varchar(256), primary key(ssection, skey));";
                SQLiteCommand command = new SQLiteCommand(createSql, con);
                command.ExecuteNonQuery();
            }
        }

        private void buttonInsertData_Click(object sender, EventArgs e)
        {
            using (SQLiteConnection con = new SQLiteConnection("Data Source=app.sqlite;Version=3;"))
            {
                try
                {
                    con.Open();
                    string sql = "insert into settings(ssection, skey, svalue) values ('Main', 'percentOfWidth', '60')," +
                        "('Main', 'percentOfHeight', '60');";
                    SQLiteCommand command = new SQLiteCommand(sql, con);
                    command.ExecuteNonQuery();
                } catch ( SQLiteException ex) when ( ex.Message.Contains("constraint failed"))
                {
                    // 主制約違反の場合はスルー
                }
            }
        }

        private void buttonSelectData_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            using (SQLiteConnection con = new SQLiteConnection("Data Source=app.sqlite;Version=3;"))
            {
                con.Open();
                string sql = "select * from settings;";

                SQLiteCommand command = new SQLiteCommand(sql, con);
                SQLiteDataReader sdr = command.ExecuteReader();
                while (sdr.Read() == true)
                {
                    result.Add((string)sdr["skey"], (string)sdr["svalue"]);
                    System.Diagnostics.Debug.WriteLine((string)sdr["svalue"]);
                }
                sdr.Close();
            }

            System.Diagnostics.Debug.WriteLine(string.Join(",", result.Select(kvp => $"{kvp.Key} : {kvp.Value}")));
            foreach ( var kvp in result)
            {
                var value = kvp.Value;
                System.Diagnostics.Debug.WriteLine($"{kvp.Key} : {value}");
            }
        }
    }
}
