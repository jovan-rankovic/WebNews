using Application.Commands.Hashtag;
using Application.DataTransfer;
using Application.Searches;
using EfDataAccess;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace EfCommands.Hashtag
{
    public class EfGetHashtagsCommand : BaseEfCommand, IGetHashtagsCommand
    {
        public EfGetHashtagsCommand(WebNewsContext context) : base(context) { }

        public IEnumerable<HashtagDto> Execute(HashtagSearch request)
        {
            var query = Context.Hashtags
                .Include(h => h.HashtagArticles)
                .ThenInclude(ah => ah.Article)
                .AsQueryable();

            if (request.Tag != null)
                query = query.Where(h => h.Tag.ToLower().Contains(request.Tag.ToLower()));

            return query.Select(h => new HashtagDto
            {
                Id = h.Id,
                Tag = h.Tag,
                ArticlesWithHashtag = h.HashtagArticles.Select(ah => ah.Article.Title)
            });
        }
    }
}