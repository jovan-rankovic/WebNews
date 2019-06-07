using Application.Commands.Article;
using Application.DataTransfer;
using Application.Searches;
using EfDataAccess;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace EfCommands.Article
{
    public class EfGetArticlesCommand : BaseEfCommand, IGetArticlesCommand
    {
        public EfGetArticlesCommand(WebNewsContext context) : base(context) { }

        public IEnumerable<ArticleDto> Execute(ArticleSearch request)
        {
            var query = Context.Articles
                .Include(a => a.ArticleCategories)
                .ThenInclude(ac => ac.Category)
                .AsQueryable();

            if (request.Title != null)
                query = query.Where(a => a.Title.ToLower().Contains(request.Title.ToLower()));

            if (request.ThisMonth == true)
                query = query.Where(a => a.CreatedAt.Month == System.DateTime.Now.Month);

            if (request.ByJovan == true)
                query = query.Where(a => a.User.FirstName == "Jovan" && a.User.LastName == "Rankovic");

            if (request.ByJovan == false)
                query = query.Where(a => a.User.FirstName != "Jovan" && a.User.LastName != "Rankovic");

            return query.Select(a => new ArticleDto
            {
                Id = a.Id,
                Title = a.Title,
                Content = a.Content,
                Image = a.ImagePath,
                CreatedAt = a.CreatedAt,
                Author = a.User.FirstName + " " + a.User.LastName,
                CategoriesForArticle = a.ArticleCategories.Select(ac => ac.Category.Name)
            });
        }
    }
}