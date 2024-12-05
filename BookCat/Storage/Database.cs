using System.Data;
using Microsoft.Extensions.Configuration;
using Dapper;
using Npgsql;

namespace BookCat.Storage
{
    public class Database : IMemory
    {
        private readonly string _connectionString;

        // Constructor that accepts IConfiguration as a dependency
        public Database(IConfiguration configuration)
        {
            // Retrieve the connection string from the "Settings" section of the appsettings.json file
            _connectionString = configuration["Settings:ConnectionString"];
        }

        private async Task<IDbConnection> GetConnection()
        {
            var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();
            return connection;
        }

        public async Task Book(string title, int year, string author, string isbn, string annotation, string genres)
        {
            var connection = await GetConnection();
            const string query = """INSERT INTO BOOKS (Title, Year, Author, ISBN, Annotation, Genres) VALUES (@Title, @Year, @Author, @ISBN, @Annotation, @Genres)""";
            await connection.ExecuteAsync(query, new
            {
                Title = title,
                Year = year,
                Author = author,
                ISBN = isbn,
                Annotation = annotation,
                Genres = genres
            });
        }

        public async Task FindAuthor(string author)
        {
            var connection = await GetConnection();
            var query = "SELECT * FROM BOOKS WHERE AUTHOR LIKE @Author";
            var books = await connection.QueryAsync<Book>(query, new { Author = "%" + author + "%" });
        }

        public async Task FindTitle(string keyword)
        {
            var connection = await GetConnection();
            var query = "SELECT * FROM BOOKS WHERE LOWER(TITLE) LIKE LOWER(@Keyword)";
            var books = await connection.QueryAsync<Book>(query, new { Keyword = "%" + keyword.ToLower() + "%" });
        }

        public async Task FindKeywordInAnnotation(string keyword)
        {
            var connection = await GetConnection();
            var query = "SELECT * FROM BOOKS WHERE LOWER(ANNOTATION) LIKE LOWER(@Keyword)";
            var books = await connection.QueryAsync<Book>(query, new { Keyword = "%" + keyword.ToLower() + "%" });
        }
    }
}


