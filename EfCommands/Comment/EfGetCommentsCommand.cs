using Application.Commands.Comment;
using Application.DataTransfer;
using Application.Searches;
using EfDataAccess;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace EfCommands.Comment
{
    public class EfGetCommentsCommand : BaseEfCommand, IGetCommentsCommand
    {
        public EfGetCommentsCommand(WebNewsContext context) : base(context) { }

        public IEnumerable<CommentDto> Execute(CommentSearch request)
        {
            var query = Context.Comments
                .Include(c => c.Article)
                .Include(c => c.User)
                .AsQueryable();

            if (request.Text != null)
                query = query.Where(c => c.Text.ToLower().Contains(request.Text.ToLower()));

            if (request.UserFirstName != null)
                query = query.Where(c => c.User.FirstName.ToLower() == request.UserFirstName.ToLower());

            if (request.UserLastName != null)
                query = query.Where(c => c.User.LastName.ToLower() == request.UserLastName.ToLower());

            return query.Select(c => new CommentDto
            {
                Id = c.Id,
                Text = c.Text,
                CreatedAt = c.CreatedAt,
                ArticleId = c.ArticleId,
                UserId = c.UserId,
                Article = c.Article.Title,
                User = c.User.FirstName + " " + c.User.LastName
            });
        }
    }
}