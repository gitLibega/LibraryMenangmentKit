using System;
using Web.AutomatedUITests.Init;

namespace Web.AutomatedUITests.Pages
{
    public abstract class TestPageBase
    {
        protected readonly ITestStartupInitializer Initializer;

        protected TestPageBase(ITestStartupInitializer initializer)
        {
            this.Initializer = initializer;
        }

        public abstract Uri Uri { get; }
        public string Title => Initializer.Driver.Title;

        public void Navigate()
        {
            Initializer.Driver.Navigate().GoToUrl(Uri);
        }
    }
}