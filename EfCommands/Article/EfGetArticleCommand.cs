using Application.Commands.Article;
using Application.DataTransfer;
using Application.Exceptions;
using EfDataAccess;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace EfCommands.Article
{
    public class EfGetArticleCommand : BaseEfCommand, IGetArticleCommand
    {
        public EfGetArticleCommand(WebNewsContext context) : base(context) { }

        public ArticleDto Execute(int request)
        {
            var article = Context.Articles
                .Include(a => a.User)
                .Include(a => a.ArticleCategories)
                .ThenInclude(ac => ac.Category)
                .First(a => a.Id == request);

            if (article == null)
                throw new EntityNotFoundException("Article");

            return new ArticleDto
            {
                Id = article.Id,
                Title = article.Title,
                Content = article.Content,
                Image = article.ImagePath,
                Author = article.User.FirstName + " " + article.User.LastName,
                CategoriesForArticle = article.ArticleCategories.Select(ac => ac.Category.Name)
            };
        }
    }
}