using Application.Commands.Article;
using Application.DataTransfer;
using Application.Exceptions;
using EfDataAccess;
using System.Linq;

namespace EfCommands.Article
{
    public class EfEditArticleCommand : BaseEfCommand, IEditArticleCommand
    {
        public EfEditArticleCommand(WebNewsContext context) : base(context) { }

        public void Execute((int id, ArticleDto articleDto) request)
        {
            var article = Context.Articles.Find(request.id);

            if (article == null)
                throw new EntityNotFoundException("Article");

            if (article.Title != request.articleDto.Title.Trim())
                if (Context.Articles.Any(a => a.Title == request.articleDto.Title.Trim()))
                    throw new EntityAlreadyExistsException("Article");

            article.UpdatedAt = System.DateTime.Now;
            article.Title = request.articleDto.Title.Trim();
            article.Content = request.articleDto.Content.Trim();
            article.UserId = request.articleDto.AuthorId;

            if (request.articleDto.Image != null)
                article.ImagePath = request.articleDto.Image;

            if (request.articleDto.CategoryIds != null && request.articleDto.HashtagIds != null)
            {
                foreach (var category in Context.ArticleCategory.Where(ac => ac.ArticleId == request.id))
                    Context.ArticleCategory.Remove(category);

                foreach (var categoryId in request.articleDto.CategoryIds)
                    Context.ArticleCategory.Add(new Domain.ArticleCategory
                    {
                        ArticleId = request.id,
                        CategoryId = categoryId
                    });

                foreach (var hashtag in Context.ArticleHashtag.Where(ah => ah.ArticleId == request.id))
                    Context.ArticleHashtag.Remove(hashtag);

                foreach (var hashtagId in request.articleDto.HashtagIds)
                    Context.ArticleHashtag.Add(new Domain.ArticleHashtag
                    {
                        ArticleId = request.id,
                        HashtagId = hashtagId
                    });
            }

            Context.SaveChanges();
        }
    }
}