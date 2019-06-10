using Application.Commands.Hashtag;
using Application.Exceptions;
using EfDataAccess;

namespace EfCommands.Hashtag
{
    public class EfDeleteHashtagCommand : BaseEfCommand, IDeleteHashtagCommand
    {
        public EfDeleteHashtagCommand(WebNewsContext context) : base(context) { }
        public void Execute(int request)
        {
            var hashtag = Context.Hashtags.Find(request);

            if (hashtag == null)
                throw new EntityNotFoundException("Hashtag");

            Context.Hashtags.Remove(hashtag);

            Context.SaveChanges();
        }
    }
}