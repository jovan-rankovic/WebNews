using Application.DataTransfer;
using Application.Interfaces;

namespace Application.Commands.Article
{
    public interface IGetArticleCommand : ICommand<int, ArticleDto> { }
}