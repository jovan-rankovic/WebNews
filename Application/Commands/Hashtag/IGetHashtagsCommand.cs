using Application.DataTransfer;
using Application.Interfaces;
using Application.Searches;
using System.Collections.Generic;

namespace Application.Commands.Hashtag
{
    public interface IGetHashtagsCommand : ICommand<HashtagSearch, IEnumerable<HashtagDto>> { }
}