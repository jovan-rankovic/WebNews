using EfDataAccess;

namespace Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var context = new WebNewsContext();

            context.Roles.Add(new Domain.Role
            {
                Name = "Admin"
            });
            context.Roles.Add(new Domain.Role
            {
                Name = "Reader"
            });

            context.SaveChanges();

            context.Users.Add(new Domain.User
            {
                FirstName = "Jovan",
                LastName = "Rankovic",
                Email = "jovan.rankovic.145.14@ict.edu.rs",
                Password = "jovan2019",
                RoleId = 1
            });
            context.Users.Add(new Domain.User
            {
                FirstName = "Luka",
                LastName = "Lukic",
                Email = "luka.lukic@ict.edu.rs",
                Password = "asp2019",
                RoleId = 1
            });
            context.Users.Add(new Domain.User
            {
                FirstName = "Pera",
                LastName = "Peric",
                Email = "pera@gmail.com",
                Password = "pera2019",
                RoleId = 2
            });

            context.Categories.Add(new Domain.Category
            {
                Name = "Technology"
            });
            context.Categories.Add(new Domain.Category
            {
                Name = "Science"
            });
            context.Categories.Add(new Domain.Category
            {
                Name = "Lifestyle"
            });

            context.Hashtags.Add(new Domain.Hashtag
            {
                Tag = "#programming"
            });
            context.Hashtags.Add(new Domain.Hashtag
            {
                Tag = "#space"
            });
            context.Hashtags.Add(new Domain.Hashtag
            {
                Tag = "#nature"
            });

            context.SaveChanges();

            context.Articles.Add(new Domain.Article
            {
                Title = "How to stay calm during exams",
                Content = "Eos impedit copiosae te, dolor incorrupte definitionem ex eam. Te vel aeque laboramus scribentur, cu omnes feugait conceptam eam, vis nusquam commune et. Vis modus graeci ne, duo copiosae scaevola euripidis te. Ex his vocibus mentitum accusamus. Ad bonorum gubergren adversarium his, his prima mnesarchum honestatis an. Esse tantas sea.",
                ImagePath = "img/article/ls.jpg",
                UserId = 1
            });
            context.Articles.Add(new Domain.Article
            {
                Title = "Rocket launched today",
                Content = "Eos impedit copiosae te, dolor incorrupte definitionem ex eam. Te vel aeque laboramus scribentur, cu omnes feugait conceptam eam, vis nusquam commune et. Vis modus graeci ne, duo copiosae scaevola euripidis te. Ex his vocibus mentitum accusamus. Ad bonorum gubergren adversarium his, his prima mnesarchum honestatis an. Esse tantas sea.",
                ImagePath = "img/article/sci.jpg",
                UserId = 2
            });
            context.Articles.Add(new Domain.Article
            {
                Title = "New chip series released",
                Content = "Eos impedit copiosae te, dolor incorrupte definitionem ex eam. Te vel aeque laboramus scribentur, cu omnes feugait conceptam eam, vis nusquam commune et. Vis modus graeci ne, duo copiosae scaevola euripidis te. Ex his vocibus mentitum accusamus. Ad bonorum gubergren adversarium his, his prima mnesarchum honestatis an. Esse tantas sea.",
                ImagePath = "img/article/tech.jpg",
                UserId = 1
            });

            context.SaveChanges();

            context.Comments.Add(new Domain.Comment
            {
                Text = "Great article.",
                ArticleId = 3,
                UserId = 3
            });
            context.Comments.Add(new Domain.Comment
            {
                Text = "Thank you.",
                ArticleId = 3,
                UserId = 1
            });
            context.Comments.Add(new Domain.Comment
            {
                Text = "This is very interesting.",
                ArticleId = 2,
                UserId = 3,
                UpdatedAt = System.DateTime.Now.AddMinutes(1)
            });

            context.ArticleCategory.Add(new Domain.ArticleCategory
            {
                ArticleId = 3,
                CategoryId = 1
            });
            context.ArticleCategory.Add(new Domain.ArticleCategory
            {
                ArticleId = 2,
                CategoryId = 2
            });
            context.ArticleCategory.Add(new Domain.ArticleCategory
            {
                ArticleId = 1,
                CategoryId = 3
            });
            context.ArticleCategory.Add(new Domain.ArticleCategory
            {
                ArticleId = 3,
                CategoryId = 2
            });

            context.ArticleHashtag.Add(new Domain.ArticleHashtag
            {
                ArticleId = 3,
                HashtagId = 1
            });
            context.ArticleHashtag.Add(new Domain.ArticleHashtag
            {
                ArticleId = 2,
                HashtagId = 2
            });
            context.ArticleHashtag.Add(new Domain.ArticleHashtag
            {
                ArticleId = 2,
                HashtagId = 1
            });
            context.ArticleHashtag.Add(new Domain.ArticleHashtag
            {
                ArticleId = 1,
                HashtagId = 3
            });

            context.SaveChanges();
        }
    }
}