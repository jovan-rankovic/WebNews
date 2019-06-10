using Application.Commands.Hashtag;
using Application.DataTransfer;
using Application.Exceptions;
using EfDataAccess;
using System.Linq;

namespace EfCommands.Hashtag
{
    public class EfCreateHashtagCommand : BaseEfCommand, ICreateHashtagCommand
    {
        public EfCreateHashtagCommand(WebNewsContext context) : base(context) { }

        public void Execute(HashtagDto request)
        {
            if (Context.Hashtags.Any(h => h.Tag == request.Tag.Trim()))
                throw new EntityAlreadyExistsException("Hashtag");

            Context.Hashtags.Add(new Domain.Hashtag
            {
                Tag = request.Tag.Trim()
            });

            Context.SaveChanges();
        }
    }
}