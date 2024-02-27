using Dapper;
using GraphQLDemo.Models;
using GraphQLDemo.Utilities;
using System.Data;
using System.Collections.Generic;

namespace GraphQLDemo.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly DapperContext _dapperContext;

        public AuthorService(DapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }

        public Author GetAuthorById(int id)
        {
            using var connection = _dapperContext.CreateConnection();
            var authorDictionary = new Dictionary<int, Author>();

            var authors = connection.Query<Author, Post, Author>(
                "SELECT a.*, p.* FROM Authors a LEFT JOIN Posts p ON a.Id = p.AuthorId WHERE a.Id = @Id",
                (author, post) =>
                {
                    if (!authorDictionary.TryGetValue(author.Id, out var authorEntry))
                    {
                        authorEntry = author;
                        authorEntry.Posts = new List<Post>();
                        authorDictionary.Add(authorEntry.Id, authorEntry);
                    }

                    if (post != null)
                    {
                        authorEntry.Posts.Add(post);
                    }

                    return authorEntry;
                },
                param: new { Id = id },
                splitOn: "Id"
            ).FirstOrDefault();

            return authors;
        }

        public List<Author> GetAllAuthors()
        {
            using var connection = _dapperContext.CreateConnection();
            var authorDictionary = new Dictionary<int, Author>();

            var authors = connection.Query<Author, Post, Author>(
                "SELECT a.*, p.* FROM Authors a LEFT JOIN Posts p ON a.Id = p.AuthorId",
                (author, post) =>
                {
                    if (!authorDictionary.TryGetValue(author.Id, out var authorEntry))
                    {
                        authorEntry = author;
                        authorEntry.Posts = new List<Post>();
                        authorDictionary.Add(authorEntry.Id, authorEntry);
                    }

                    if (post != null)
                    {
                        authorEntry.Posts.Add(post);
                    }

                    return authorEntry;
                },
                splitOn: "Id"
            ).Distinct().ToList();

            return authors;
        }
    }
}
