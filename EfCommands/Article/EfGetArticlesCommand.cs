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

            if (request.Hashtag != null)
                query = query.Where(a => a.ArticleHashtags.Any(ah => ah.Hashtag.Tag == request.Hashtag));

            if (request.CategoryId.HasValue)
                query = query.Where(a => a.ArticleCategories.Any(ac => ac.CategoryId == request.CategoryId));

            return query.Select(a => new ArticleDto
            {
                Id = a.Id,
                Title = a.Title,
                Content = a.Content,
                Image = a.ImagePath,
                CreatedAt = a.CreatedAt,
                UpdatedAt = a.UpdatedAt,
                AuthorId = a.UserId,
                Author = a.User.FirstName + " " + a.User.LastName,
                Comments = a.Comments.Select(c => c.Text),
                CategoriesForArticle = a.ArticleCategories.Select(ac => ac.Category.Name),
                HashtagsForArticle = a.ArticleHashtags.Select(ah => ah.Hashtag.Tag)
            });
        }
    }
}