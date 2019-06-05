using Application.DataTransfer;
using Application.Interfaces;

namespace Application.Commands.Category
{
    public interface IEditCategoryCommand : ICommand<(int id, CategoryDto categoryDto)> { }
}