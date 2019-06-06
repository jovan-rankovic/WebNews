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
            if (Context.Articles.Any(a => a.Title == request.Title))
                throw new EntityAlreadyExistsException("Article");

            Context.Articles.Add(new Domain.Article
            {
                Title = request.Title,
                Content = request.Content,
                ImagePath = request.Image,
                UserId = 1 // for testing
            });

            Context.SaveChanges();
        }
    }
}