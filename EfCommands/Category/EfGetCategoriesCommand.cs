using Application.Commands.Category;
using Application.DataTransfer;
using Application.Searches;
using EfDataAccess;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace EfCommands.Category
{
    public class EfGetCategoriesCommand : BaseEfCommand, IGetCategoriesCommand
    {
        public EfGetCategoriesCommand(WebNewsContext context) : base(context) { }

        public IEnumerable<CategoryDto> Execute(CategorySearch request)
        {
            var query = Context.Categories
                .Include(c => c.CategoryArticles)
                .ThenInclude(ac => ac.Article)
                .AsQueryable();

            if (request.Name != null)
                query = query.Where(c => c.Name.ToLower().Contains(request.Name.ToLower()));

            return query.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                ArticlesInCategory = c.CategoryArticles.Select(ac => ac.Article.Title)
            });
        }
    }
}