using Application.Commands.Comment;
using Application.DataTransfer;
using Application.Responses;
using Application.Searches;
using EfDataAccess;
using System.Linq;

namespace EfCommands.Comment
{
    public class EfGetCommentsCommand : BaseEfCommand, IGetCommentsCommand
    {
        public EfGetCommentsCommand(WebNewsContext context) : base(context) { }

        public PagedResponse<CommentDto> Execute(CommentSearch request)
        {
            var query = Context.Comments.AsQueryable();

            if (request.Text != null)
                query = query.Where(c => c.Text.ToLower().Contains(request.Text.ToLower()));

            if (request.UserFirstName != null)
                query = query.Where(c => c.User.FirstName.ToLower() == request.UserFirstName.ToLower());

            if (request.UserLastName != null)
                query = query.Where(c => c.User.LastName.ToLower() == request.UserLastName.ToLower());

            if (request.PageNumber != 0)
                query = query.Skip((request.PageNumber - 1) * request.PerPage).Take(request.PerPage);

            var totalCount = query.Count();
            var pagesCount = (int)System.Math.Ceiling((double)totalCount / request.PerPage);

            return new PagedResponse<CommentDto>
            {
                TotalCount = totalCount,
                PagesCount = pagesCount,
                CurrentPage = request.PageNumber,
                Data = query.Select(c => new CommentDto
                {
                    Id = c.Id,
                    Text = c.Text,
                    CreatedAt = c.CreatedAt,
                    UpdatedAt = c.UpdatedAt,
                    ArticleId = c.ArticleId,
                    UserId = c.UserId,
                    Article = c.Article.Title,
                    User = c.User.FirstName + " " + c.User.LastName
                })
            };
        }
    }
}