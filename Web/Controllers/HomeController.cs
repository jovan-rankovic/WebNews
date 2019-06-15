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
            articleSearch.PageNumber = 0;
            var articles = _searchArticlesCommand.Execute(articleSearch);

            categorySearch.PageNumber = 0;
            var categories = _searchCategoriesCommand.Execute(categorySearch);

            var viewModel = new HomeViewModel
            {
                Articles = articles.Data,
                Categories = categories.Data
            };

            return View(viewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}