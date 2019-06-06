using Application.Commands.Article;
using Application.Exceptions;
using EfDataAccess;

namespace EfCommands.Article
{
    public class EfDeleteArticleCommand : BaseEfCommand, IDeleteArticleCommand
    {
        public EfDeleteArticleCommand(WebNewsContext context) : base(context) { }

        public void Execute(int request)
        {
            var article = Context.Articles.Find(request);

            if (article == null)
                throw new EntityNotFoundException("Article");

            Context.Articles.Remove(article);

            Context.SaveChanges();
        }
    }
}