using NUnit.Framework;
using LibraryMenangmentKit;

namespace LMKTests
{
	[TestFixture]
	class Test
	{
		Librarykit lk;
		
		[SetUp]
		public void InitTest()
		{
			lk= new Librarykit();
		}
		[Test]
		public void testAddClient()
		{

		}
		[Test]
		public void testAddBook()
		{
			Assert.DoesNotThrow(() => { lk.addBook("Муму"); });//Добавляю книгу
			Assert.DoesNotThrow(() => { lk.addBook("Муму"); });//Добавляю книгу чтоб проверить что колличество книг с таким названием изменится
			Assert.AreEqual(2, lk.countBooksInLibrary());//проверяю что книги добавились
		}
		[Test]
		public void testTakeBook()
		{

		}
	}
}
