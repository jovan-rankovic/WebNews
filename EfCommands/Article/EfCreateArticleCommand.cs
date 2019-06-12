using Application.Commands.Article;
using Application.DataTransfer;
using Application.Exceptions;
using Application.Interfaces;
using EfDataAccess;
using System.Linq;

namespace EfCommands.Article
{
    public class EfCreateArticleCommand : BaseEfCommand, ICreateArticleCommand
    {
        private readonly IEmailSender _emailSender;

        public EfCreateArticleCommand(WebNewsContext context, IEmailSender emailSender) : base(context)
        {
            _emailSender = emailSender;
        }

        public void Execute(ArticleDto request)
        {
            if (Context.Articles.Any(a => a.Title == request.Title.Trim()))
                throw new EntityAlreadyExistsException("Article");

            var article = new Domain.Article
            {
                Title = request.Title.Trim(),
                Content = request.Content.Trim(),
                ImagePath = request.Image,
                UserId = request.AuthorId
            };

            Context.Articles.Add(article);

            Context.SaveChanges();

            if (request.CategoryIds != null && request.HashtagIds != null)
            {
                foreach (var categoryId in request.CategoryIds)
                {
                    Context.ArticleCategory.Add(new Domain.ArticleCategory
                    {
                        ArticleId = article.Id,
                        CategoryId = categoryId
                    });
                }

                foreach (var hashtagId in request.HashtagIds)
                {
                    Context.ArticleHashtag.Add(new Domain.ArticleHashtag
                    {
                        ArticleId = article.Id,
                        HashtagId = hashtagId
                    });
                }
            }

            Context.SaveChanges();

            _emailSender.Subject = request.Title;
            _emailSender.Body = request.Content;
            _emailSender.ToEmail = "webnewsapi@gmail.com";
            _emailSender.Send();
        }
    }
}