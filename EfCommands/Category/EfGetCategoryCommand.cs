using Application.Commands.Category;
using Application.DataTransfer;
using Application.Exceptions;
using EfDataAccess;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace EfCommands.Category
{
    public class EfGetCategoryCommand : BaseEfCommand, IGetCategoryCommand
    {
        public EfGetCategoryCommand(WebNewsContext context) : base(context) { }
        public CategoryDto Execute(int request)
        {
            var category = Context.Categories
                .Include(c => c.CategoryArticles)
                .ThenInclude(ac => ac.Article)
                .First(c => c.Id == request);

            if (category == null)
                throw new EntityNotFoundException("Category");

            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                ArticlesInCategory = category.CategoryArticles.Select(ac => ac.Article.Title)
            };
        }
    }
}