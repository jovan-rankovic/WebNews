using Application.DataTransfer;
using Application.Interfaces;
using Application.Searches;
using System.Collections.Generic;

namespace Application.Commands.Category
{
    public interface IGetCategoriesCommand : ICommand<CategorySearch, IEnumerable<CategoryDto>> { }
}