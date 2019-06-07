using Application.Commands.Category;
using Application.DataTransfer;
using Application.Exceptions;
using EfDataAccess;
using System.Linq;

namespace EfCommands.Category
{
    public class EfEditCategoryCommand : BaseEfCommand, IEditCategoryCommand
    {
        public EfEditCategoryCommand(WebNewsContext context) : base(context) { }

        public void Execute((int id, CategoryDto categoryDto) request)
        {
            var category = Context.Categories.Find(request.id);

            if (category == null)
                throw new EntityNotFoundException("Category");

            if (category.Name != request.categoryDto.Name.Trim())
                if (Context.Categories.Any(c => c.Name == request.categoryDto.Name.Trim()))
                    throw new EntityAlreadyExistsException("Category");

            category.UpdatedAt = System.DateTime.Now;
            category.Name = request.categoryDto.Name.Trim();

            Context.SaveChanges();
        }
    }
}