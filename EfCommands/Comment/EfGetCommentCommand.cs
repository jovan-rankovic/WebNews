using Application.Commands.Comment;
using Application.DataTransfer;
using Application.Exceptions;
using EfDataAccess;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace EfCommands.Comment
{
    public class EfGetCommentCommand : BaseEfCommand, IGetCommentCommand
    {
        public EfGetCommentCommand(WebNewsContext context) : base(context) { }

        public CommentDto Execute(int request)
        {
            var comment = Context.Comments
                .Include(c => c.Article)
                .Include(c => c.User)
                .First(c => c.Id == request);

            if (comment == null)
                throw new EntityNotFoundException("Comment");

            return new CommentDto
            {
                Id = comment.Id,
                Text = comment.Text,
                CreatedAt = comment.CreatedAt,
                ArticleId = comment.ArticleId,
                UserId = comment.UserId,
                Article = comment.Article.Title,
                User = comment.User.FirstName + " " + comment.User.LastName
            };
        }
    }
}