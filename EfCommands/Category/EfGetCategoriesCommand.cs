using Application.Commands.Category;
using Application.DataTransfer;
using Application.Responses;
using Application.Searches;
using EfDataAccess;
using System.Linq;

namespace EfCommands.Category
{
    public class EfGetCategoriesCommand : BaseEfCommand, IGetCategoriesCommand
    {
        public EfGetCategoriesCommand(WebNewsContext context) : base(context) { }

        public PagedResponse<CategoryDto> Execute(CategorySearch request)
        {
            var query = Context.Categories.AsQueryable();

            if (request.Name != null)
                query = query.Where(c => c.Name.ToLower().Contains(request.Name.ToLower()));

            if (request.PageNumber != 0)
                query = query.Skip((request.PageNumber - 1) * request.PerPage).Take(request.PerPage);

            var totalCount = query.Count();
            var pagesCount = (int)System.Math.Ceiling((double)totalCount / request.PerPage);

            return new PagedResponse<CategoryDto>
            {
                TotalCount = totalCount,
                PagesCount = pagesCount,
                CurrentPage = request.PageNumber,
                Data = query.Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    ArticlesInCategory = c.CategoryArticles.Select(ac => ac.Article.Title)
                })
            };
        }
    }
}