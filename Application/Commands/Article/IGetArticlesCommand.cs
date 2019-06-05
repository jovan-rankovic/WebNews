using Application.DataTransfer;
using Application.Interfaces;
using Application.Searches;
using System.Collections.Generic;

namespace Application.Commands.Article
{
    public interface IGetArticlesCommand : ICommand<ArticleSearch, IEnumerable<ArticleDto>> { }
}