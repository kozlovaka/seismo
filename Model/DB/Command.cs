using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilencePauseApp.Model.DB
{
    public class Command : ICommand
    {
        //Строка подключения к БД
        private string SQLConnection = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\SilencePausesDB.mdf;Integrated Security=False";
        private SqlConnection connection;

        public Command()
        {
            connection = new SqlConnection(SQLConnection);
        }

        //Метод для заполнения данных
        public void InitialTable(out DataTable dt, string sqlCommand)
        {
            dt = new DataTable();
            dt.Clear();
            SqlCommand command = new SqlCommand(sqlCommand, connection);
            SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
            dataAdapter.Fill(dt);
        }

        //открываем подключение к БД
        public void Open()
        {
            connection.Open();
        }

        //Закрывем подключение
        public void Close()
        {
            connection.Close();
        }

        /// <summary>
        /// Метод для выполнения sql запроса к БД
        /// </summary>
        /// <param name="sqlComand"></param>
        public void Execute(string sqlComand)
        {
            connection.Open();
            SqlCommand command = new SqlCommand(sqlComand, connection);
            command.ExecuteNonQuery();
            connection.Close();
        }

        public void ExecuteImage(string sqlCommand, string path)
        {
            using (SqlConnection con = new SqlConnection(SQLConnection))
            {
                using (SqlCommand com = new SqlCommand(sqlCommand, con))
                {
                    con.Open();
                    byte[] imageData = File.ReadAllBytes(path);
                    com.Parameters.AddWithValue("@IM", imageData);
                    com.ExecuteNonQuery();
                }
            }
        }
    }
}
