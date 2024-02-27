using Dapper;
using GraphQLDemo.Models;
using GraphQLDemo.Utilities;
using System.Data;
using System.Collections.Generic;
using System.Linq;

namespace GraphQLDemo.Services
{
    public class PostService : IPostService
    {
        private readonly DapperContext _dapperContext;

        public PostService(DapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }

        public Post GetPostById(int id)
        {
            using var connection = _dapperContext.CreateConnection();
            var post = connection.Query<Post, Author, Post>(
                "SELECT p.*, a.* FROM Posts p INNER JOIN Authors a ON p.AuthorId = a.Id WHERE p.Id = @Id",
                (post, author) =>
                {
                    post.Author = author;
                    return post;
                },
                param: new { Id = id },
                splitOn: "Id"
            ).FirstOrDefault();

            return post;
        }

        public List<Post> GetAllPosts()
        {
            using var connection = _dapperContext.CreateConnection();
            var postDictionary = new Dictionary<int, Post>();

            var posts = connection.Query<Post, Author, Post>(
                "SELECT p.*, a.* FROM Posts p INNER JOIN Authors a ON p.AuthorId = a.Id",
                (post, author) =>
                {
                    post.Author = author;
                    return post;
                },
                splitOn: "Id"
            ).ToList();

            return posts;
        }

        public Post AddPost(Post post)
        {
            using var connection = _dapperContext.CreateConnection();

            var authorExists = connection.QuerySingleOrDefault<Author>(
                "SELECT * FROM Authors WHERE Id = @AuthorId",
                new { AuthorId = post.AuthorId });

            if (authorExists == null)
            {
                throw new Exception($"Author with ID {post.AuthorId} not found.");
            }

            var sql = @"
                INSERT INTO Posts (Title, Content, AuthorId) VALUES (@Title, @Content, @AuthorId);
                SELECT CAST(SCOPE_IDENTITY() as int);
            ";

            var postId = connection.QuerySingle<int>(sql, post);
            var newPost = GetPostById(postId); // Re-fetch the post to include the Author

            return newPost;
        }
    }
}
