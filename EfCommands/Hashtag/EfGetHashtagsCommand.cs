using Application.Commands.Hashtag;
using Application.DataTransfer;
using Application.Responses;
using Application.Searches;
using EfDataAccess;
using System.Linq;

namespace EfCommands.Hashtag
{
    public class EfGetHashtagsCommand : BaseEfCommand, IGetHashtagsCommand
    {
        public EfGetHashtagsCommand(WebNewsContext context) : base(context) { }

        public PagedResponse<HashtagDto> Execute(HashtagSearch request)
        {
            var query = Context.Hashtags.AsQueryable();

            if (request.Tag != null)
                query = query.Where(h => h.Tag.ToLower().Contains(request.Tag.ToLower()));

            if (request.PageNumber != 0)
                query = query.Skip((request.PageNumber - 1) * request.PerPage).Take(request.PerPage);

            var totalCount = query.Count();
            var pagesCount = (int)System.Math.Ceiling((double)totalCount / request.PerPage);

            return new PagedResponse<HashtagDto>
            {
                TotalCount = totalCount,
                PagesCount = pagesCount,
                CurrentPage = request.PageNumber,
                Data = query.Select(h => new HashtagDto
                {
                    Id = h.Id,
                    Tag = h.Tag,
                    ArticlesWithHashtag = h.HashtagArticles.Select(ah => ah.Article.Title)
                })
            };
        }
    }
}