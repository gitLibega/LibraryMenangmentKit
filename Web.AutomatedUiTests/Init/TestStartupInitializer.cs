using System;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Web.AutomatedUITests.Init
{
    /// <summary>
    /// Представляет собой класс, который инициализирует окружение для UI-тестирования веб-приложений.
    /// </summary>
    public abstract class TestStartupInitializer : ITestStartupInitializer, IDisposable
    {
        private Lazy<Uri> _rootUriInitializer;
        private Lazy<IWebDriver> _driverInitializer;

        public Uri RootUri => _rootUriInitializer.Value;
        public IWebDriver Driver => _driverInitializer.Value;
        public IHost Host { get; set; }

        protected TestStartupInitializer()
        {
            InitLazy();
        }

        private void InitLazy()
        {
            _rootUriInitializer = new Lazy<Uri>(StartAndGetRootUri);
            _driverInitializer = new Lazy<IWebDriver>(CreateWebDriver);
        }

        protected static void RunInBackgroundThread(Action action)
        {
            var isDone = new ManualResetEvent(false);
            ExceptionDispatchInfo edi = null;
            new Thread(() =>
            {
                try
                {
                    action();
                }
                catch (Exception ex)
                {
                    edi = ExceptionDispatchInfo.Capture(ex);
                }

                isDone.Set();
            }).Start();

            if (!isDone.WaitOne(TimeSpan.FromSeconds(10)))
                throw new TimeoutException("Timed out waiting for: " + action);

            if (edi != null)
                throw edi.SourceException;
        }

        protected virtual Uri StartAndGetRootUri()
        {
            Host = CreateWebHost();
            RunInBackgroundThread(Host.Start);

            var uriString = Host.Services.GetRequiredService<IServer>().Features
                .Get<IServerAddressesFeature>()
                .Addresses.Single();
            return new Uri(uriString);
        }

        public void Dispose() => Dispose(true);

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Driver?.Dispose();
                Host?.Dispose();
                Host?.StopAsync();
            }
        }

        public void EnsureServerRestart()
        {
            Dispose();
            InitLazy();
        }

        protected abstract IHost CreateWebHost();

        protected abstract IWebDriver CreateWebDriver();
    }

    public class TestStartupInitializerDefault : TestStartupInitializer
    {
        /// <inheritdoc />
        protected override IHost CreateWebHost()
        {
            return new HostBuilder()
                .ConfigureWebHost(webHostBuilder => webHostBuilder
                    .UseKestrel()
                    .UseSolutionRelativeContentRoot(typeof(Startup).Assembly.GetName().Name)
                    .UseStaticWebAssets()
                    .UseStartup<Startup>()
                    .UseUrls($"http://127.0.0.1:0")) // :0 allows to choose a port automatically
                .Build();
        }

        /// <inheritdoc />
        protected override IWebDriver CreateWebDriver()
        {
            var service = ChromeDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true;

            var options = new ChromeOptions();
            options.AddArgument("headless");


            var driver = new ChromeDriver(service, options);
            return driver;
        }
    }
}