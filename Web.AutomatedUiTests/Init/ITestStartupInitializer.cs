using System;
using OpenQA.Selenium;

namespace Web.AutomatedUITests.Init
{
    /**
     * Предоставляет функциональность для UI-тестирования веб-приложений.
     */
    public interface ITestStartupInitializer
    {
        /// <summary>
        /// Корневой адрес сервера для тестирования.
        /// </summary>
        Uri RootUri { get; }

        /// <summary>
        /// Драйвер для тестирования.
        /// </summary>
        IWebDriver Driver { get; }
    }
}