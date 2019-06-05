using Application.DataTransfer;
using Application.Interfaces;

namespace Application.Commands.Category
{
    public interface IGetCategoryCommand : ICommand<int, CategoryDto> { }
}