using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using TestOryabinkiyApp.Connection;

namespace TestOryabinkiyApp.Tables
{
	internal class UserTable
	{
		public DataTable userDT = new DataTable();
		public void CreateTable()
		{
			try
			{
				DBConnection.cmd.CommandText = @$"CREATE TABLE IF NOT EXISTS 
					user (
						id INT AUTO_INCREMENT PRIMARY KEY,
						FIO VARCHAR(255) not null,
						gender char not null,
						birthday date not null
					);";
				DBConnection.cmd.ExecuteNonQuery();
				Console.WriteLine("Таблица создана и готова к заполнению");
			}
			catch(Exception e) 
			{
				Console.WriteLine(e.Message);
			}
		}

		public void Add(string FIO, string gender, string birthday)
		{
			try
			{
				DBConnection.cmd.CommandText = $"insert into user values(null," +
					$"'{FIO}','{gender}','{birthday}')";
				DBConnection.cmd.ExecuteNonQuery();
				Console.WriteLine("Запись добавлена");
			}
			catch(Exception e)
			{
				Console.WriteLine(e.Message);	
			}
		}

		public bool View()
		{
			try
			{
				DBConnection.cmd.CommandText = $"SELECT " +
					$"FIO," +
					$"gender," +
					$"DATE_FORMAT(birthday, '%d.%m.%Y'), " +
					$"TIMESTAMPDIFF(YEAR, birthday, CURDATE()) AS currentAge " +
					$"FROM user Order by FIO;";
				userDT.Clear();
				DBConnection.adp.Fill(userDT);
				return true;
			}
			catch(Exception e) 
			{
				Console.WriteLine (e.Message);
				return false;
			}
		}

		public void ViewF()
		{
			try
			{
				DBConnection.cmd.CommandText = $"SELECT " +
					$"* FROM user " +
					$"where gender='м' " +
					$"and fio like 'F%';";
				userDT.Clear();
				DBConnection.adp.Fill(userDT);
			}
			catch( Exception e ) 
			{
				Console.WriteLine (e.Message);
			}
		}

		public void ViewFaster()
		{
			try
			{
				DBConnection.cmd.CommandText = $"SELECT FIO, " +
					$"gender, " +
					$"birthday " +
					$"FROM user " +
					$"where gender='м' " +
					$"and fio like 'F%' " +
					$"limit 100";
				userDT.Clear();
				DBConnection.adp.Fill(userDT);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}

	}
}
