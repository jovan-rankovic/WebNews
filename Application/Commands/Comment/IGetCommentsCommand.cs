using Application.DataTransfer;
using Application.Interfaces;
using Application.Searches;
using System.Collections.Generic;

namespace Application.Commands.Comment
{
    public interface IGetCommentsCommand : ICommand<CommentSearch, IEnumerable<CommentDto>> { }
}