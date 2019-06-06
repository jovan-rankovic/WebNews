using Application.Commands.Category;
using Application.Exceptions;
using EfDataAccess;

namespace EfCommands.Category
{
    public class EfDeleteCategoryCommand : BaseEfCommand, IDeleteCategoryCommand
    {
        public EfDeleteCategoryCommand(WebNewsContext context) : base(context) { }

        public void Execute(int request)
        {
            var category = Context.Categories.Find(request);

            if (category == null)
                throw new EntityNotFoundException("Category");

            Context.Categories.Remove(category);

            Context.SaveChanges();
        }
    }
}