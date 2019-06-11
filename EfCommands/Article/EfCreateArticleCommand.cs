using Application.Commands.Article;
using Application.DataTransfer;
using Application.Exceptions;
using EfDataAccess;
using System.Linq;

namespace EfCommands.Article
{
    public class EfCreateArticleCommand : BaseEfCommand, ICreateArticleCommand
    {
        public EfCreateArticleCommand(WebNewsContext context) : base(context) { }

        public void Execute(ArticleDto request)
        {
            if (Context.Articles.Any(a => a.Title == request.Title.Trim()))
                throw new EntityAlreadyExistsException("Article");

            Context.Articles.Add(new Domain.Article
            {
                Title = request.Title.Trim(),
                Content = request.Content.Trim(),
                ImagePath = request.Image.Trim(),
                UserId = request.AuthorId
            });

            foreach (var categoryId in request.CategoryIds)
            {
                Context.ArticleCategory.Add(new Domain.ArticleCategory
                {
                    ArticleId = request.Id,
                    CategoryId = categoryId
                });
            }

            foreach (var hashtagId in request.HashtagIds)
            {
                Context.ArticleHashtag.Add(new Domain.ArticleHashtag
                {
                    ArticleId = request.Id,
                    HashtagId = hashtagId
                });
            }

            Context.SaveChanges();
        }
    }
}