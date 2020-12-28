using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using Web.AutomatedUITests.Init;
using Web.Models;

namespace Web.AutomatedUITests.Pages
{
    public class HomePage : TestPageBase
    {
        public override Uri Uri => new Uri(Initializer.RootUri, "/Home");

        public IWebElement BookAddNameInputElement => Initializer.Driver.FindElement(By.Name("addName"));
        public IWebElement BookAddCountInputElement => Initializer.Driver.FindElement(By.Name("addCount"));
        public IWebElement BookAddButtonElement => Initializer.Driver.FindElement(By.Id("btnAddBook"));

        public IWebElement BookRemoveNameInputElement => Initializer.Driver.FindElement(By.Name("rmvName"));
        public IWebElement BookRemoveCountInputElement => Initializer.Driver.FindElement(By.Name("rmvCount"));
        public IWebElement BookRemoveButtonElement => Initializer.Driver.FindElement(By.Id("btnRmvBook"));

        public IWebElement BookReturnNameInputElement => Initializer.Driver.FindElement(By.Name("returnBookName"));
        public IWebElement ClientIdInputElement => Initializer.Driver.FindElement(By.Name("returnFromClientId"));
        public IWebElement BookReturnButtonElement => Initializer.Driver.FindElement(By.Id("btnReturnBook"));

        public IWebElement NewClientNameInputElement => Initializer.Driver.FindElement(By.Id("newClientName"));
        public IWebElement NewClientAddButtonElement => Initializer.Driver.FindElement(By.Id("btnAddClient"));


        public IWebElement GiveBookNameInputElement => Initializer.Driver.FindElement(By.Name("giveBookName"));
        public IWebElement GiveToClientIdInputElement => Initializer.Driver.FindElement(By.Name("giveToClientId"));
        public IWebElement GiveBookButtonElement => Initializer.Driver.FindElement(By.Id("btnGiveBook"));

        public IWebElement BooksTableElement => Initializer.Driver.FindElement(By.Id("books"));
        public IWebElement ClientsTableElement => Initializer.Driver.FindElement(By.Id("clients"));

        public IWebElement ErrorTextElement => Initializer.Driver.FindElement(By.Id("error"));
        public string ErrorTextString => ErrorTextElement.Text;

        public IReadOnlyCollection<IWebElement> BooksTableAllRowsElements => BooksTableElement.FindElements(By.TagName("tr"));
        public IReadOnlyCollection<IWebElement> ClientsTableAllRowsElements => ClientsTableElement.FindElements(By.TagName("tr"));

        public void PopulateNewBook(AddBookModel model)
        {
            BookAddNameInputElement.SendKeys(model.AddName);

            BookAddCountInputElement.Clear();
            BookAddCountInputElement.SendKeys(model.AddCount.ToString());
        }

        public void PopulateRemoveBook(RemoveBookModel model)
        {
            BookRemoveNameInputElement.SendKeys(model.rmvName);

            BookRemoveCountInputElement.Clear();
            BookRemoveCountInputElement.SendKeys(model.rmvCount.ToString());
        }

        public void PopulateNewClientName(string name)
        {
            NewClientNameInputElement.SendKeys(name);
        }

        public void PopulateGiveBookInfo(GiveBookModel model)
        {
            GiveBookNameInputElement.SendKeys(model.GiveBookName);

            GiveToClientIdInputElement.Clear();
            GiveToClientIdInputElement.SendKeys(model.GiveToClientId.ToString());
        }


        public void ClickBookAddButton() => BookAddButtonElement.Click();
        public void ClickNewClientAddButton() => NewClientAddButtonElement.Click();
        public void ClickGiveBookButton() => GiveBookButtonElement.Click();

        public void ClickRemoveBook()=> BookRemoveButtonElement.Click();
        public void ClickReturnBookButton() => BookReturnButtonElement.Click();



        public List<(string name, int count)> ExtractBooksTable()
        {
            var res = new List<(string, int)>();

            // пропускаем заголовок
            foreach (var row in BooksTableAllRowsElements.Skip(1))
            {
                var columns = row.FindElements(By.TagName("td"));
                var t = columns[1].Text;
                res.Add((columns[0].Text, int.Parse(t)));
            }

            return res;
        }

        public List<(int id, string clientName, string bookList)> ExtractClientTable()
        {
            var res = new List<(int, string, string)>();

            // пропускаем заголовок
            foreach (var row in ClientsTableAllRowsElements.Skip(1))
            {
                var columns = row.FindElements(By.TagName("td"));
                res.Add((int.Parse(columns[0].Text), columns[1].Text, columns[2].Text));
            }

            return res;
        }


        public HomePage(ITestStartupInitializer initializer) : base(initializer)
        {
        }
    }
}