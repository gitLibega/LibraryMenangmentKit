using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Web.Models;
using Web.Repository;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly DataContainer _dataContainer;
        private readonly ILogger<HomeController> _logger;

        public HomeController(DataContainer dataContainer, ILogger<HomeController> logger)
        {
            _dataContainer = dataContainer;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View(_dataContainer.Kit);
        }

        [HttpPost]
        public IActionResult AddBook(AddBookModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _dataContainer.Kit.addBook(model.AddName, model.AddCount);
                }
            }
            catch (Exception e)
            {
                TempData["Error"] = e.Message;
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult GiveBook(GiveBookModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _dataContainer.Kit.giveBook(model.GiveBookName, model.GiveToClientId);
                }
            }
            catch (Exception e)
            {
                TempData["Error"] = e.Message;
            }


            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult AddClient(string newClientName)
        {
            try
            {
                _dataContainer.Kit.addClient(newClientName);
            }
            catch (Exception e)
            {
                TempData["Error"] = e.Message;
            }

            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}