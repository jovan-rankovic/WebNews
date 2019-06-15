using Application.DataTransfer;
using Application.Interfaces;
using Application.Responses;
using Application.Searches;

namespace Application.Commands.Category
{
    public interface IGetCategoriesCommand : ICommand<CategorySearch, PagedResponse<CategoryDto>> { }
}