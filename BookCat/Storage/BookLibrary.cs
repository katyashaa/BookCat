using System.Collections.ObjectModel;
using Npgsql;
using Dapper;

namespace BookCat.Storage
{
    public class BookLibrary
    {
        private readonly string _connectionString; // Строка подключения к базе данных

        // Конструктор, который принимает строку подключения
        public BookLibrary(string connectionString)
        {
            _connectionString = connectionString; // Сохраняем строку подключения
        }

        // Метод для добавления книги в библиотеку
        public bool AddBook(Book item)
        {
            if (!AddBookToDatabase(item))
            {
                return false; // Книга не добавлена
            }
            return true;
        }

        private bool AddBookToDatabase(Book item)
        {
            // Проверка на дублирование по названию книги
            if (IsBookInDatabase(item.GetTitle()))
            {
                return false; // Книга с таким названием уже есть в базе данных
            }

            using (var db = new NpgsqlConnection(_connectionString))
            {
                string sql = "INSERT INTO Books (Title, Year, Author, ISBN, Annotation, Genres) " +
                             "VALUES (@Title, @Year, @Author, @ISBN, @Annotation, @Genres)";

                db.Execute(sql, new
                {
                    Title = item.GetTitle(),
                    Year = item.GetYear(),
                    Author = item.GetAuthor(),
                    ISBN = item.GetIsbn(),
                    Annotation = item.GetAnnotation(),
                    Genres = string.Join(",", item.GetGenres())
                });
            }
            return true;
        }

        // Метод для проверки наличия книги в базе данных по названию
        private bool IsBookInDatabase(string title)
        {
            using (var db = new NpgsqlConnection(_connectionString))
            {
                string sql = "SELECT COUNT(1) FROM Books WHERE Title = @Title";
                return db.ExecuteScalar<int>(sql, new { Title = title }) > 0;
            }
        }

        // Метод для получения всех книг из базы данных
        public List<Book> GetBooksFromDatabase()
        {
            using (var db = new NpgsqlConnection(_connectionString))
            {
                string sql = "SELECT Title, Year, Author, ISBN, Annotation, Genres FROM Books";
                var books = db.Query<Book>(sql).ToList();
                return books;
            }
        }
        
        public ReadOnlyCollection<Book> GetLib()
        {
            // Загружаем книги из базы данных
            List<Book> books = GetBooksFromDatabase();
            // Преобразуем в ReadOnlyCollection
            return books.AsReadOnly();
        }
        
    }
}
