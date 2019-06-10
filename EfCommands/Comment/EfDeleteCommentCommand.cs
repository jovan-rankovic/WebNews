using Application.Commands.Comment;
using Application.Exceptions;
using EfDataAccess;

namespace EfCommands.Comment
{
    public class EfDeleteCommentCommand : BaseEfCommand, IDeleteCommentCommand
    {
        public EfDeleteCommentCommand(WebNewsContext context) : base(context) { }

        public void Execute(int request)
        {
            var comment = Context.Comments.Find(request);

            if (comment == null)
                throw new EntityNotFoundException("Comment");

            Context.Comments.Remove(comment);

            Context.SaveChanges();
        }
    }
}