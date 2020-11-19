using NUnit.Framework;
using LibraryMenangmentKit;
using System;

namespace LMKTests
{
	[TestFixture]
	class Test
	{
		Librarykit lk;

		[SetUp]
		public void InitTest()
		{
			lk = new Librarykit();
		}
		[Test]
		public void testAddClient()
		{
			Assert.DoesNotThrow(() => { lk.addClient("Никита Ильич Ильин"); });// Клиент добавится 
			Assert.DoesNotThrow(() => { lk.addClient("Никита Ильич Ильин"); });// Клиент добавится, но под другим id
			Assert.That(Assert.Throws<Exception>(() => lk.addClient(null)).Message, Is.EqualTo("Введено пустое поле"));//Пустое поле
		}
		[Test]
		public void testAddBook()
		{
			Assert.DoesNotThrow(() => { lk.addBook("Муму"); });//Добавляю книгу
			Assert.AreEqual(1, lk.countBooksInLibrary());//проверяю что книги добавились
			Assert.DoesNotThrow(() => { lk.addBook("Муму"); });//Добавляю книгу чтоб проверить что колличество книг с таким названием изменится
			Assert.AreEqual(2, lk.countBooksInLibrary());//проверяю что книги добавились
			
		}
		[Test]
		public void testGiveBook()
		{
			Assert.DoesNotThrow(() => { lk.addClient("Никита Ильич Ильин"); });
			Assert.DoesNotThrow(() => { lk.addBook("Муму"); });
			Assert.That(Assert.Throws<Exception>(() => lk.giveBook("Муka",0)).Message, Is.EqualTo("Данной книги не существует"));//книги не существует
			Assert.DoesNotThrow(() => { lk.giveBook("Муму", 0); });//дать книгу
			Assert.That(Assert.Throws<Exception>(() => lk.giveBook("Муму", 0)).Message, Is.EqualTo("Данной книги нет в наличии"));//книги нет в наличии
			Assert.DoesNotThrow(() => { lk.addBook("Муму"); });
			Assert.That(Assert.Throws<Exception>(() => lk.giveBook("Муму", 1)).Message, Is.EqualTo("Данного клиента не существует"));//дать книгу несуществующему

		}
		[Test]
		public void testRemoveBook()
		{
			Assert.DoesNotThrow(() => { lk.addBook("Муму"); });
			Assert.That(Assert.Throws<Exception>(() => lk.removeBook("Мум")).Message, Is.EqualTo("Книги не существует в библиотеке"));//книги нет в наличии
			Assert.DoesNotThrow(() => { lk.addClient("Никита Ильич Ильин"); });
			Assert.DoesNotThrow(() => { lk.giveBook("Муму", 0); });
			Assert.That(Assert.Throws<Exception>(() => lk.removeBook("Муму")).Message, Is.EqualTo("Данная книга занята"));//книги нет в наличии
			


		}
		[Test]
		public void testRemoveClient()
		{
			Assert.DoesNotThrow(() => { lk.addClient("Никита Ильич Ильин"); });
			Assert.That(Assert.Throws<Exception>(() => lk.removeClient(1)).Message, Is.EqualTo("Такой клиент не существует"));
			Assert.DoesNotThrow(() => { lk.addBook("Муму"); });
			Assert.DoesNotThrow(() => { lk.giveBook("Муму", 0); });
			Assert.That(Assert.Throws<Exception>(() => lk.removeClient(0)).Message, Is.EqualTo("За клиентом долги"));
			Assert.DoesNotThrow(() => { lk.addClient("Никита Ильич Ильин"); });
			Assert.DoesNotThrow(() => { lk.removeClient(1); });



		}
		[Test]
		public void voidTestSumCountInCollections()
		{
			Assert.DoesNotThrow(() => { lk.addBook("Муму"); });
			Assert.DoesNotThrow(() => { lk.addBook("Муму"); });
			Assert.DoesNotThrow(() => { lk.addBook("Муму"); });
			Assert.AreEqual(3, lk.countBooksInLibrary());

			Assert.DoesNotThrow(() => { lk.addClient("Никита Ильич Ильин"); });
			Assert.DoesNotThrow(() => { lk.giveBook("Муму", 0); });//дать книгу
			Assert.DoesNotThrow(() => { lk.giveBook("Муму", 0); });//дать книгу
			Assert.DoesNotThrow(() => { lk.giveBook("Муму", 0); });//дать книгу
			Assert.AreEqual(3, lk.countBusyBooks());

			Assert.AreEqual(0, lk.countBooksInLibrary());

		}
	}
}
