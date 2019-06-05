using Application.DataTransfer;
using Application.Interfaces;

namespace Application.Commands.Article
{
    public interface ICreateUserCommand : ICommand<ArticleDto> { }
}