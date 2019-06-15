using Application.Commands.Article;
using Application.DataTransfer;
using Application.Interfaces;
using Application.Responses;
using Application.Searches;
using EfDataAccess;
using System.Linq;

namespace EfCommands.Article
{
    public class EfGetArticlesCommand : BaseEfCommand, IGetArticlesCommand
    {
        public EfGetArticlesCommand(WebNewsContext context) : base(context) { }

        public PagedResponse<ArticleDto> Execute(ArticleSearch request)
        {
            var query = Context.Articles.AsQueryable();

            if (request.Title != null)
                query = query.Where(a => a.Title.ToLower().Contains(request.Title.ToLower()));

            if (request.Hashtag != null)
                query = query.Where(a => a.ArticleHashtags.Any(ah => ah.Hashtag.Tag.ToLower().Contains(request.Hashtag.ToLower())));

            if (request.CategoryId.HasValue)
                query = query.Where(a => a.ArticleCategories.Any(ac => ac.CategoryId == request.CategoryId));

            if (request.PageNumber != 0)
                query = query.Skip((request.PageNumber - 1) * request.PerPage).Take(request.PerPage);

            var totalCount = query.Count();
            var pagesCount = (int)System.Math.Ceiling((double)totalCount / request.PerPage);

            return new PagedResponse<ArticleDto>
            {
                TotalCount = totalCount,
                PagesCount = pagesCount,
                CurrentPage = request.PageNumber,
                Data = query.Select(a => new ArticleDto
                {
                    Id = a.Id,
                    Title = a.Title,
                    Content = a.Content,
                    Image = a.ImagePath,
                    CreatedAt = a.CreatedAt,
                    UpdatedAt = a.UpdatedAt,
                    AuthorId = a.UserId,
                    CategoryIds = a.ArticleCategories.Select(ac => ac.CategoryId),
                    HashtagIds = a.ArticleHashtags.Select(ah => ah.HashtagId),
                    Author = a.User.FirstName + " " + a.User.LastName,
                    Comments = a.Comments.Select(c => c.Text),
                    CategoriesForArticle = a.ArticleCategories.Select(ac => ac.Category.Name),
                    HashtagsForArticle = a.ArticleHashtags.Select(ah => ah.Hashtag.Tag)
                })
            };
        }
    }
}