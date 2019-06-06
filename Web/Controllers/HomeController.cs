using Application.Commands.Article;
using Application.Commands.Category;
using Application.Searches;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Web.Models;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IGetArticlesCommand _searchArticlesCommand;
        private readonly IGetCategoriesCommand _searchCategoriesCommand;

        public HomeController(IGetArticlesCommand searchArticlesCommand, IGetCategoriesCommand searchCategoriesCommand)
        {
            _searchArticlesCommand = searchArticlesCommand;
            _searchCategoriesCommand = searchCategoriesCommand;
        }

        public IActionResult Index([FromQuery] ArticleSearch articleSearch, CategorySearch categorySearch)
        {
            var viewModel = new HomeViewModel
            {
                Articles = _searchArticlesCommand.Execute(articleSearch),
                Categories = _searchCategoriesCommand.Execute(categorySearch)
            };

            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}