using Application.Commands.Category;
using Application.DataTransfer;
using Application.Exceptions;
using EfDataAccess;
using System.Linq;

namespace EfCommands.Category
{
    public class EfCreateCategoryCommand : BaseEfCommand, ICreateCategoryCommand
    {
        public EfCreateCategoryCommand(WebNewsContext context) : base(context) { }

        public void Execute(CategoryDto request)
        {
            if (Context.Categories.Any(c => c.Name == request.Name.Trim()))
                throw new EntityAlreadyExistsException("Category");

            Context.Categories.Add(new Domain.Category
            {
                Name = request.Name.Trim()
            });

            Context.SaveChanges();
        }
    }
}