using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryMenangmentKit
{
    public class Librarykit
    {
		private Dictionary<string, int> books = new Dictionary<string, int>();

		private Dictionary<int, string> Clients = new Dictionary<int, string>();

		private Dictionary<int, List<string>> busyBooks = new Dictionary<int, List<string>>();


		/// <summary>
		/// добавить книгу
		/// </summary>
		/// <param name="nameBook"></param>
		public void addBook(string nameBook)
		{
			if (!books.ContainsKey(nameBook))
			{
				books.Add(nameBook, 1);
				return;
			}

			if (books.ContainsKey(nameBook))
			{
				books[nameBook]++;
				return;
			}
		}

		/// <summary>
		/// взять книгу
		/// </summary>
		/// <param name="nameBook"></param>
		/// <param name="idClient"></param>
		public void giveBook(string nameBook, int idClient)
		{
			if (books.ContainsKey(nameBook) && books[nameBook] > 0 && Clients.ContainsKey(idClient))
			{
				books[nameBook]--;
				if (busyBooks.ContainsKey(idClient))
				{
					busyBooks[idClient].Add(nameBook);
				}
				else
				{
					busyBooks.Add(idClient,new List<string>());
					busyBooks[idClient].Add(nameBook);
				}
				return;
			}
			if (books.ContainsKey(nameBook) && books[nameBook] < 1 )
			{
				throw new Exception("Данной книги нет в наличии");

			}
			if (!books.ContainsKey(nameBook))
			{
				throw new Exception("Данной книги не существует");
			}
			if (!Clients.ContainsKey(idClient))
			{
				throw new Exception("Данного клиента не существует");

			}

		}
		/// <summary>
		/// возврат книги
		/// </summary>
		/// <param name="nameBook"></param>
		/// <param name="idClient"></param>
		public void returnBook(string nameBook, int idClient)
		{
			if (books.ContainsKey(nameBook) && Clients.ContainsKey(idClient))
			{
				books[nameBook]++;
				busyBooks[idClient].Remove(nameBook);
				return;
			}
			if (!Clients.ContainsKey(idClient))
			{
				throw new Exception("Данного клиента не существует");

			}
			if (!books.ContainsKey(nameBook))
			{
				throw new Exception("Данной книги не существет");
			}
		}

		/// <summary>
		/// добавить клиента
		/// </summary>
		/// <param name="nameClient"></param>
		public void addClient(string nameClient)
		{
			if (nameClient != null)
			{
				Clients.Add(generateIdClient(), nameClient);
			}
			else
			{
				throw new Exception("Введено пустое поле");
			}

		}
		/// <summary>
		/// удалить клиента
		/// </summary>
		/// <param name="idClient"></param>
		public void removeClient(int idClient)
		{
			if (Clients.ContainsKey(idClient) && busyBooks.ContainsKey(idClient)&& busyBooks[idClient].Count < 1)
			{
				Clients.Remove(idClient);
			}
			if (Clients.ContainsKey(idClient) &&   busyBooks.ContainsKey(idClient)&& busyBooks[idClient].Count > 0)
				throw new Exception("За клиентом долги");
			if (!Clients.ContainsKey(idClient))
				throw new Exception("Такой клиент не существует");
		}


		/// <summary>
		/// удалить книгу
		/// </summary>
		/// <param name="nameBook"></param>
		public void removeBook(string nameBook)
		{
			var book = 0;
			foreach (var v in busyBooks.Keys)
			{
				foreach (var bok in busyBooks[v])

				{
					if (bok == nameBook)
					{
						book++;
					}
				}
			}
			if (books.ContainsKey(nameBook) && book < 1)
			{
				books.Remove(nameBook);
				return;
			}
			if (!books.ContainsKey(nameBook))
			{
				throw new Exception("Книги не существует в библиотеке");
			}
			if (book > 0)
			{
				throw new Exception("Данная книга занята");
			}
		}

		/// <summary>
		/// количество книг в библиотеке
		/// </summary>
		/// <returns></returns>
		public int countBooksInLibrary()
		{
			var count = 0;

			foreach (var v in books.Keys)
			{
				count += books[v];
			}
			return count;
		}

		/// <summary>
		/// количество занятых книг
		/// </summary>
		/// <returns></returns>
		public int countBusyBooks()
		{
			var count = 0;

			foreach (var v in busyBooks.Keys)
			{
				count += busyBooks[v].Count;
			}
			return count;


		}

		/// <summary>
		/// генерация id
		/// </summary>
		/// <returns></returns>
		/// 
		private int generateIdClient()
		{
			var current = 0;
			if (Clients.ContainsKey(current))
			{
				return Clients.Last().Key + 1;
			}
			else
			{
				return current;
			}
			
		}
		
	
	}
}
