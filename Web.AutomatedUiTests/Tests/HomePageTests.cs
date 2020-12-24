using System.Linq;
using Web.AutomatedUITests.Init;
using Web.AutomatedUITests.Pages;
using Web.Models;
using Xunit;

namespace Web.AutomatedUITests.Tests
{
    public class HomePageTests : IClassFixture<TestStartupInitializerDefault>
    {
        private readonly TestStartupInitializerDefault _initializer;
        private readonly HomePage _page;

        public HomePageTests(TestStartupInitializerDefault initializer)
        {
            _initializer = initializer;
            _page = new HomePage(initializer);
        }

        [Fact]
        public void TestPageTitle()
        {
            _page.Navigate();
            Assert.Equal("Home Page - Web", _page.Title);
        }

        [Fact]
        public void TestAddNewBook()
        {
            _initializer.EnsureServerRestart();
            _page.Navigate();

            var beforeAdd = _page.ExtractBooksTable();

            var toAdd = new AddBookModel() {AddName = "testName", AddCount = 2};
            _page.PopulateNewBook(toAdd);
            _page.ClickBookAddButton();

            var afterAdd = _page.ExtractBooksTable();

            Assert.Equal(beforeAdd.Count + 1, afterAdd.Count);
            Assert.Contains(afterAdd, a => a.count == toAdd.AddCount && a.name == toAdd.AddName);
        }

        [Fact]
        public void TestAddBookAlreadyExist()
        {
            _initializer.EnsureServerRestart();
            _page.Navigate();

            var beforeAdd = _page.ExtractBooksTable();
            var toAdd = new AddBookModel() {AddName = "testName", AddCount = 2};

            _page.PopulateNewBook(toAdd);
            _page.ClickBookAddButton();
            var afterFirstAdd = _page.ExtractBooksTable();

            _page.PopulateNewBook(toAdd);
            _page.ClickBookAddButton();
            var afterSecondAdd = _page.ExtractBooksTable();


            Assert.Equal(beforeAdd.Count + 1, afterFirstAdd.Count);
            Assert.Equal(afterFirstAdd.Count, afterSecondAdd.Count);

            // книги должны суммироваться
            Assert.Contains(afterSecondAdd, a => a.count == toAdd.AddCount * 2 && a.name == toAdd.AddName);
        }

        [Fact]
        public void TestAddClient()
        {
            _initializer.EnsureServerRestart();
            _page.Navigate();

            var beforeAdd = _page.ExtractClientTable();
            Assert.Empty(beforeAdd);

            var toAddName = "testClientName";
            _page.PopulateNewClientName(toAddName);
            _page.ClickNewClientAddButton();

            var afterAdd = _page.ExtractClientTable();
            Assert.Single(afterAdd);
            Assert.Contains(afterAdd, a => a.clientName == toAddName);
        }

        [Fact]
        public void TestGiveBookSuccess()
        {
            _initializer.EnsureServerRestart();
            _page.Navigate();

            // добавляем книгу
            var newBook = new AddBookModel() {AddName = "testName", AddCount = 2};
            _page.PopulateNewBook(newBook);
            _page.ClickBookAddButton();

            // добавляем клиента
            var newClientName = "testClientName";
            _page.PopulateNewClientName(newClientName);
            _page.ClickNewClientAddButton();

            var info = new GiveBookModel() {GiveBookName = newBook.AddName, GiveToClientId = 0};
            _page.PopulateGiveBookInfo(info);
            _page.ClickGiveBookButton();

            var afterAdd = _page.ExtractClientTable();
            Assert.Single(afterAdd);
            Assert.Contains(afterAdd, a => a.id == 0 && a.clientName == newClientName && a.bookList.Contains(newBook.AddName));
        }

        [Fact]
        public void TestGiveBookWrongClientId()
        {
            _initializer.EnsureServerRestart();
            _page.Navigate();

            // добавляем книгу
            var newBook = new AddBookModel() {AddName = "testName", AddCount = 2};
            _page.PopulateNewBook(newBook);
            _page.ClickBookAddButton();

            // добавляем клиента
            var newClientName = "testClientName";
            _page.PopulateNewClientName(newClientName);
            _page.ClickNewClientAddButton();

            // даём клиенту с ид 15, которого нет
            var info = new GiveBookModel() {GiveBookName = newBook.AddName, GiveToClientId = 15};
            _page.PopulateGiveBookInfo(info);
            _page.ClickGiveBookButton();

            // должна быть ошибка
            Assert.Contains("Данного клиента не существует", _page.ErrorTextString);
        }


        [Fact]
        public void TestGiveBookWrongBookName()
        {
            _initializer.EnsureServerRestart();
            _page.Navigate();

            // добавляем книгу
            var newBook = new AddBookModel() {AddName = "testName", AddCount = 2};
            _page.PopulateNewBook(newBook);
            _page.ClickBookAddButton();

            // добавляем клиента
            var newClientName = "testClientName";
            _page.PopulateNewClientName(newClientName);
            _page.ClickNewClientAddButton();

            // даём клиенту несуществующую книгу
            var info = new GiveBookModel() {GiveBookName = "asdasdasd", GiveToClientId = 0};
            _page.PopulateGiveBookInfo(info);
            _page.ClickGiveBookButton();

            // должна быть ошибка
            Assert.Contains("Данной книги не существует", _page.ErrorTextString);
        }

        [Fact]
        public void TestGiveBookNotAvailableBook()
        {
            _initializer.EnsureServerRestart();
            _page.Navigate();

            // добавляем книгу с количеством 0
            var newBook = new AddBookModel() {AddName = "testName", AddCount = 0};
            _page.PopulateNewBook(newBook);
            _page.ClickBookAddButton();

            // добавляем клиента
            var newClientName = "testClientName";
            _page.PopulateNewClientName(newClientName);
            _page.ClickNewClientAddButton();

            // даём клиенту эту книгу
            var info = new GiveBookModel() {GiveBookName = newBook.AddName, GiveToClientId = 0};
            _page.PopulateGiveBookInfo(info);
            _page.ClickGiveBookButton();

            // должна быть ошибка
            Assert.Contains("Данной книги нет в наличии", _page.ErrorTextString);
        }
    }
}