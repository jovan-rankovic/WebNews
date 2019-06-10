using Application.Commands.Hashtag;
using Application.DataTransfer;
using Application.Exceptions;
using EfDataAccess;
using System.Linq;

namespace EfCommands.Hashtag
{
    public class EfEditHashtagCommand : BaseEfCommand, IEditHashtagCommand
    {
        public EfEditHashtagCommand(WebNewsContext context) : base(context) { }

        public void Execute((int id, HashtagDto hashtagDto) request)
        {
            var hashtag = Context.Hashtags.Find(request.id);

            if (hashtag == null)
                throw new EntityNotFoundException("Hashtag");

            if (hashtag.Tag != request.hashtagDto.Tag.Trim())
                if (Context.Hashtags.Any(h => h.Tag == request.hashtagDto.Tag.Trim()))
                    throw new EntityAlreadyExistsException("Hashtag");

            hashtag.UpdatedAt = System.DateTime.Now;
            hashtag.Tag = request.hashtagDto.Tag;

            Context.SaveChanges();
        }
    }
}