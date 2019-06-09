﻿using Application.Commands.Article;
using Application.DataTransfer;
using Application.Exceptions;
using EfDataAccess;
using System.Linq;

namespace EfCommands.Article
{
    public class EfEditArticleCommand : BaseEfCommand, IEditArticleCommand
    {
        public EfEditArticleCommand(WebNewsContext context) : base(context) { }

        public void Execute((int id, ArticleDto articleDto) request)
        {
            var article = Context.Articles.Find(request.id);

            if (article == null)
                throw new EntityNotFoundException("Article");

            if (article.Title != request.articleDto.Title.Trim())
                if (Context.Articles.Any(a => a.Title == request.articleDto.Title.Trim()))
                    throw new EntityAlreadyExistsException("Article");

            article.UpdatedAt = System.DateTime.Now;
            article.Title = request.articleDto.Title.Trim();
            article.Content = request.articleDto.Content.Trim();
            article.ImagePath = request.articleDto.Image.Trim();

            Context.SaveChanges();
        }
    }
}