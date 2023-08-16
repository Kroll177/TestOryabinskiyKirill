using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace TestOryabinkiyApp.Connection
{
    internal class DBConnection
    {
        static public MySqlConnection conn;
        static public MySqlCommand cmd;
        static public MySqlDataAdapter adp;
		/// <summary>
		/// Метод для подключению к БД
		/// </summary>
		/// <param name="server">Название сервера в СУБД MySQL</param>
		/// <param name="user">Название пользователя</param>
		/// <param name="password">Пароль для подключения</param>
		/// <returns>Статус подключения к БД</returns>
		static public bool TryDBConnection
            (string server,
            string user,
            string password)
        {
            try
            {
                string connectionString = $"database = test_oryabinskiy_kirill;" +
                    $"datasource={server};" +
                    $"user={user};" +
                    $"password={password};" +
                    $"charset=utf8mb4";
                conn = new MySqlConnection(connectionString);
                conn.Open ();

                cmd = new MySqlCommand();
                cmd.Connection = conn;

                adp=new MySqlDataAdapter(cmd);
                return true;
            }
            catch(Exception e)
            {
                Console.WriteLine($"Ошибка подключения: {e.Message}");
                return false;
            }
        }

        public void DBConnectionClose()
        {
            try
            {
                conn.Close();
            }
            catch(Exception e)
			{
				Console.WriteLine($"Ошибка отключения: {e.Message}");
			}
        }
    }
}
