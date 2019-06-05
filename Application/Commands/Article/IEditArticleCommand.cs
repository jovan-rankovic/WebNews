using Application.DataTransfer;
using Application.Interfaces;

namespace Application.Commands.Article
{
    public interface IEditArticleCommand : ICommand<(int id, ArticleDto articleDto)> { }
}