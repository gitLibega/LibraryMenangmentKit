using System;
using System.Collections.Generic;
using System.Linq;

namespace LibraryMenangmentKit
{
    public class Librarykit
    {
        public Dictionary<string, int> Books { get; } = new Dictionary<string, int>();

        public Dictionary<int, string> Clients { get; } = new Dictionary<int, string>();

        public Dictionary<int, List<string>> BusyBooks { get; } = new Dictionary<int, List<string>>();


        /// <summary>
        /// добавить книгу
        /// </summary>
        /// <param name="nameBook"></param>
        public void addBook(string nameBook, int count = 1)
        {
            if (nameBook == "" || nameBook == " ")
            {
                throw new Exception("Не правильный вариант ввода названия");
            }

            if (Books.ContainsKey(nameBook) && Books[nameBook] == int.MaxValue)
            {
                throw new Exception("Место на складе закончилось");
            }

            if (!Books.ContainsKey(nameBook))
            {
                Books.Add(nameBook, count);
                return;
            }

            if (Books.ContainsKey(nameBook))
            {
                Books[nameBook] += count;
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
            if (Books.ContainsKey(nameBook) && Books[nameBook] > 0 && Clients.ContainsKey(idClient))
            {
                Books[nameBook]--;
                if (BusyBooks.ContainsKey(idClient))
                {
                    BusyBooks[idClient].Add(nameBook);
                }
                else
                {
                    BusyBooks.Add(idClient, new List<string>());
                    BusyBooks[idClient].Add(nameBook);
                }

                return;
            }

            if (Books.ContainsKey(nameBook) && Books[nameBook] < 1)
            {
                throw new Exception("Данной книги нет в наличии");
            }

            if (!Books.ContainsKey(nameBook))
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
            if (Books.ContainsKey(nameBook) && Clients.ContainsKey(idClient) && BusyBooks.ContainsKey(idClient))
            {
                Books[nameBook]++;
                BusyBooks[idClient].Remove(nameBook);
                if(BusyBooks[idClient].Count<1)
				{
                    BusyBooks.Remove(idClient);
				}
                return;
            }

            if (!BusyBooks.ContainsKey(idClient))
            {
                throw new Exception("Данного клиента не существует в списке должников");
            }

            if (!Books.ContainsKey(nameBook))
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

        public IEnumerable<string> GetClientBooks(int id)
        {
            if (!BusyBooks.ContainsKey(id)) return new string[0];
            return BusyBooks[id];
        }

        /// <summary>
        /// удалить клиента
        /// </summary>
        /// <param name="idClient"></param>
        public void removeClient(int idClient)
        {
            if (Clients.ContainsKey(idClient) && BusyBooks.ContainsKey(idClient) && BusyBooks[idClient].Count < 1)
            {
                Clients.Remove(idClient);
            }

            if (Clients.ContainsKey(idClient) && BusyBooks.ContainsKey(idClient) && BusyBooks[idClient].Count > 0)
                throw new Exception("За клиентом долги");
            if (!Clients.ContainsKey(idClient))
                throw new Exception("Такой клиент не существует");
        }


        /// <summary>
        /// удалить книгу
        /// </summary>
        /// <param name="nameBook"></param>
        public void removeBook(string nameBook,int count)
        {
            var book = 0;
            foreach (var v in BusyBooks.Keys)
            {
                foreach (var bok in BusyBooks[v])

                {
                    if (bok == nameBook)
                    {
                        book++;
                    }
                }
            }

            
            if (count != 0)
            {
                if (Books.ContainsKey(nameBook) && book==0  && Books[nameBook] - count > 0)
                {
                    Books[nameBook] = Books[nameBook] - count;
                    return;
                }
                if (Books.ContainsKey(nameBook) && book==0 && Books[nameBook] - count <= 0)
                {
                    Books.Remove(nameBook);
                    return;
                }
                if (Books.ContainsKey(nameBook) && book>0 && Books[nameBook] - count <= 0)
                {
                    Books[nameBook] = 0;
                    return;
                }
            }
          

            if (!Books.ContainsKey(nameBook))
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

            foreach (var v in Books.Keys)
            {
                count += Books[v];
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

            foreach (var v in BusyBooks.Keys)
            {
                count += BusyBooks[v].Count;
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
            var current = 1;
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