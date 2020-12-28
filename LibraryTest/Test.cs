using NUnit.Framework;
using LibraryMenangmentKit;
using System;

namespace LibraryTest
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
			Assert.That(Assert.Throws<Exception>(() => lk.addBook(" ")).Message, Is.EqualTo("Не правильный вариант ввода названия"));//книги не существует
			Assert.AreEqual(1, lk.countBooksInLibrary());//проверяю что книги добавились
			Assert.DoesNotThrow(() => { lk.addBook("Муму",int.MaxValue-1); });
			Assert.AreEqual(int.MaxValue, lk.countBooksInLibrary());//проверяю что книги добавились
			Assert.That(Assert.Throws<Exception>(() => lk.addBook("Муму")).Message, Is.EqualTo("Место на складе закончилось"));//книги не существует
			Assert.AreEqual(int.MaxValue, lk.countBooksInLibrary());//проверяю что книги добавились
		
			
		}
		[Test]
		public void testGiveBook()
		{
			Assert.DoesNotThrow(() => { lk.addClient("Никита Ильич Ильин"); });
			Assert.DoesNotThrow(() => { lk.addBook("Муму"); });
			Assert.That(Assert.Throws<Exception>(() => lk.giveBook("Муka",0)).Message, Is.EqualTo("Данной книги не существует"));//книги не существует
			Assert.DoesNotThrow(() => { lk.giveBook("Муму", 1); });//дать книгу
			Assert.AreEqual(0, lk.countBooksInLibrary());
			Assert.That(Assert.Throws<Exception>(() => lk.giveBook("Муму", 0)).Message, Is.EqualTo("Данной книги нет в наличии"));//книги нет в наличии
			Assert.DoesNotThrow(() => { lk.addBook("Муму"); });
			Assert.That(Assert.Throws<Exception>(() => lk.giveBook("Муму", 2)).Message, Is.EqualTo("Данного клиента не существует"));//дать книгу несуществующему

		}
		[Test]
	
		public void testRemoveClient()
		{
			Assert.DoesNotThrow(() => { lk.addClient("Никита Ильич Ильин"); });
			Assert.That(Assert.Throws<Exception>(() => lk.removeClient(5)).Message, Is.EqualTo("Такой клиент не существует"));
			Assert.DoesNotThrow(() => { lk.addBook("Муму"); });
		
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
			Assert.DoesNotThrow(() => { lk.giveBook("Муму", 1); });//дать книгу
			Assert.DoesNotThrow(() => { lk.giveBook("Муму", 1); });//дать книгу
			Assert.DoesNotThrow(() => { lk.giveBook("Муму", 1); });//дать книгу
			Assert.AreEqual(3, lk.countBusyBooks());

			Assert.AreEqual(0, lk.countBooksInLibrary());

		}
	}
}
