using Application.DataTransfer;
using Application.Interfaces;
using Application.Responses;
using Application.Searches;

namespace Application.Commands.Article
{
    public interface IGetArticlesCommand : ICommand<ArticleSearch, PagedResponse<ArticleDto>> { }
}