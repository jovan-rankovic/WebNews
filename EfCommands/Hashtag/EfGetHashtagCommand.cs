using Application.Commands.Hashtag;
using Application.DataTransfer;
using Application.Exceptions;
using EfDataAccess;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace EfCommands.Hashtag
{
    public class EfGetHashtagCommand : BaseEfCommand, IGetHashtagCommand
    {
        public EfGetHashtagCommand(WebNewsContext context) : base(context) { }

        public HashtagDto Execute(int request)
        {
            var hashtag = Context.Hashtags
                .Include(h => h.HashtagArticles)
                .ThenInclude(ah => ah.Article)
                .First(h => h.Id == request);

            if (hashtag == null)
                throw new EntityNotFoundException("Hashtag");

            return new HashtagDto
            {
                Id = hashtag.Id,
                Tag = hashtag.Tag,
                ArticlesWithHashtag = hashtag.HashtagArticles.Select(ah => ah.Article.Title)
            };
        }
    }
}