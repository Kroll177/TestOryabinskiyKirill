using System.Data;
using System.Diagnostics;
using System.Threading.Tasks.Dataflow;
using TestOryabinkiyApp.Connection;
using TestOryabinkiyApp.Tables;

namespace TestOryabinkiyApp
{
	internal class Program
	{
		static void Main(string[] args)
		{
			var stopwatch = new Stopwatch();
			var stopwatchFaster = new Stopwatch();

			Random rand = new Random();
			string server="", user="", password="", choise;
			int choiseNumber;
			UserTable UT=new UserTable();
			DBConnection DBC = new DBConnection();

			Console.WriteLine("Здравствуйте, уважаемая компания ПТМК, " +
				"меня зовут Орябинский Кирилл " +
				"представляю своё тестовое задание, " +
				"выполненное на языке C# " +
				"с использованием СУБД MySQL, для запуска " +
				"приложения необходимо импортировать БД(testBD.sql) " +
				"на сервер MySQL 8.0, надеюсь на обратную связь, " +
				"БД импользует кодировку utf8mb4 " +
				"\nДля продолжения назмите любую клавишу");
			Console.ReadKey();
			do
			{
				Console.Write("Введите адресс сервера MySql: ");
				server = Console.ReadLine();
				Console.Write("Введите имя пользователя: ");
				user = Console.ReadLine();
				Console.Write("Введите пароль пользователя: ");
				password = Console.ReadLine();
				Console.Clear();
			} while (DBConnection.TryDBConnection(server,
			user,
			password)!=true);
			Console.WriteLine("Успешное подключение к БД");
			do
			{
				
				Console.WriteLine("1 - Создать таблицу users " +
					"\n2 - Создание записи " +
					"\n3 - Вывод всех строк с уникальным значением ФИО+дата, отсортированным по ФИО" +
					"\n4 - Заполнение автоматически 1000000 строк." +
					"\n5 - Результат выборки из таблицы по критерию: пол мужской, ФИО начинается с \"F\"." +
					"\n6 - Замер скорости" +
					"\n7 - Выход");
				choise = Console.ReadLine();
				while (!int.TryParse(choise, out choiseNumber))
				{
					Console.WriteLine("Неправильный формат ввода");
					choise = Console.ReadLine();
				}

				switch(choiseNumber)
				{
					case 1:

						UT.CreateTable();
						Console.ReadKey();
						Console.Clear();
						break;

					case 2:

						string fio, gender, birthdayChoise="";
						DateTime birthday;
						bool isDateValid = false;

						Console.Write("Введите ФИО: ");
						fio=Console.ReadLine();
						do
						{
							Console.Write("Введите пол(м/ж): ");
							gender = Console.ReadLine();
						} while (gender != "м" && gender != "ж");

						while (!isDateValid)
						{
							Console.Write("Введите дату рождения(дд.мм.гггг): ");
							birthdayChoise = Console.ReadLine();

							if (DateTime.TryParseExact(birthdayChoise, "dd.MM.yyyy", 
								null, 
								System.Globalization.DateTimeStyles.None, 
								out birthday))
							{
								isDateValid = true;
							}
							else
							{
								Console.WriteLine("Неправильный формат даты. Попробуйте снова.");
							}
						}

						UT.Add(fio,gender,birthdayChoise);
						Console.ReadKey();
						Console.Clear();
						break;

					case 3:
						if(UT.View())
						{
							foreach(DataRow row in UT.userDT.Rows)
							{
								foreach (DataColumn column in UT.userDT.Columns)
								{
									Console.Write(row[column] + "\t");
								}
								Console.WriteLine();
							}
							Console.ReadKey();
							Console.Clear();
						}
						break;

					case 4:
						string[] genderArray = {"м","ж"};
						for (int i=0;i<1000100;i++)
						{
							Lists.Lists lists = new Lists.Lists();

							string resultName;
							if (i < 1000000)
							{
								int indexGender = rand.Next(2);
								string resultGender = genderArray[indexGender];

								int indexSurname = rand.Next(20);
								string resultSurname = lists.surnames[indexSurname];

								int indexName = rand.Next(10);
								if (genderArray[indexGender] == "м")
									resultName = lists.maleNames[indexName];
								else
									resultName = lists.femaleNames[indexName];

								int indexMiddleName = rand.Next(10);
								string resultMiddleName = lists.middleNames[indexMiddleName];

								string fio1000 = $"{resultSurname} {resultName} {resultMiddleName}";

								DateTime start = new DateTime(1960, 1, 1);
								int range = (DateTime.Today - start).Days;
								DateTime dateResult = start.AddDays(rand.Next(range));

								UT.Add(fio1000, resultGender, dateResult.ToString("yyyy-MM-dd"));
							}
							else
							{
								int indexSurnameF = rand.Next(10);
								string resultSurnameF = lists.surnamesF[indexSurnameF];

								int indexName = rand.Next(10);
								resultName = lists.maleNames[indexName];

								int indexMiddleName = rand.Next(10);
								string resultMiddleName = lists.middleNames[indexMiddleName];

								string fio1000 = $"{resultSurnameF} {resultName} {resultMiddleName}";

								DateTime start = new DateTime(1960, 1, 1);
								int range = (DateTime.Today - start).Days;
								DateTime dateResult = start.AddDays(rand.Next(range));


								UT.Add(fio1000, "м", dateResult.ToString("yyyy-MM-dd"));
							}
						}
						Console.ReadKey();
						Console.Clear();
						break;
					case 5:
						
						stopwatch.Start();
						UT.ViewF();
						stopwatch.Stop();
						foreach (DataRow row in UT.userDT.Rows)
						{
							foreach (DataColumn column in UT.userDT.Columns)
							{
								Console.Write(row[column] + "\t");
							}
							Console.WriteLine();
						}
						Console.WriteLine($"Метод по запросу к БД выполнился за: {stopwatch.ElapsedMilliseconds} мс");

						break;
					case 6:

						stopwatchFaster.Start();
						UT.ViewFaster();
						stopwatchFaster.Stop();
						foreach (DataRow row in UT.userDT.Rows)
						{
							foreach (DataColumn column in UT.userDT.Columns)
							{
								Console.Write(row[column] + "\t");
							}
							Console.WriteLine();
						}
						Console.WriteLine($"Метод по запросу к БД выполнился за: {stopwatchFaster.ElapsedMilliseconds} мс");
						Console.WriteLine($"Метод из 5-го пункта {stopwatch.ElapsedMilliseconds} " +
							$"\nМетод из 6-го пункта {stopwatchFaster.ElapsedMilliseconds}");
						Console.WriteLine("В данном случае, в методе из пятого пункта в запросе используется выборка SELECT *, " +
							"в то время как в 6-ом пункте указаны только необходимые столбцы. " +
							"Также во втором случае используется limit 100, то есть ограничение на выборку в 100 записей. " +
							"Сложно ускорить запрос, который обращается к одной таблице, " +
							"но для увеличения скорости так же можно использовать индексацию, " +
							"то есть добавление индексов на необходимые поля или например обновления базы данных. " +
							"Благодарю за предоставленную возможность проявить себя, очень надеюсь на обратную связь, спасибо");
						break;
				};

			} while (choiseNumber != 7);
			DBC.DBConnectionClose();
		}
	}
}