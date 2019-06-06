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

            if (article.Title != request.articleDto.Title)
                if (Context.Articles.Any(a => a.Title == request.articleDto.Title))
                    throw new EntityAlreadyExistsException("Article");

            article.UpdatedAt = System.DateTime.Now;
            article.Title = request.articleDto.Title;
            article.Content = request.articleDto.Content;
            article.ImagePath = request.articleDto.Image;

            Context.SaveChanges();
        }
    }
}