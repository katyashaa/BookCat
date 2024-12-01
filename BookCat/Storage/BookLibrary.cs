using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using BookCat.Util;
using Npgsql;
using Dapper;

namespace BookCat.Storage
{
    public class BookLibrary
    {
        private readonly List<Book> _books = new List<Book>();
        private readonly string _connectionString; // Строка подключения к базе данных

        // Конструктор, который принимает строку подключения
        public BookLibrary(string connectionString)
        {
            _connectionString = connectionString; // Сохраняем строку подключения
        }

        // Метод для добавления книги в библиотеку
        public bool AddBook(Book item)
        {
            if (!AddLocalBook(item))
            {
                return false; // Книга не добавлена
            }

            // Если нужно, сохраняем данные в базу данных
            SaveBooks();
            return true;
        }

        // Локальный метод для добавления книги в список
        public bool AddLocalBook(Book item)
        {
            // Проверка на дублирование по названию книги
            HashSet<string> name = new HashSet<string>();
            Tokenizator.Tokenize(name, item.GetTitle());

            foreach (var b in _books)
            {
                if (b.FindTitle(name) == b.GetTitleLen()) // Книга с таким названием уже есть
                {
                    return false;
                }
            }

            _books.Add(item); // Добавляем книгу в список
            return true;
        }

        // Возвращает список всех книг в библиотеке как ReadOnlyCollection
        public ReadOnlyCollection<Book> GetLib()
        {
            return _books.AsReadOnly();
        }

        // Метод для сохранения данных в базу данных
        private void SaveBooks()
        {
            // Реализуйте логику сохранения в базу данных
            // Например, используя библиотеку Npgsql для PostgreSQL
            using (var db = new NpgsqlConnection(_connectionString))
            {
                string sql = "INSERT INTO Books (Title, Year, Author, ISBN, Annotation, Genres) " +
                             "VALUES (@Title, @Year, @Author, @ISBN, @Annotation, @Genres)";

                foreach (var book in _books)
                {
                    db.Execute(sql, new
                    {
                        Title = book.GetTitle(),
                        Year = book.GetYear(),
                        Author = book.GetAuthor(),
                        ISBN = book.GetIsbn(),
                        Annotation = book.GetAnnotation(),
                        Genres = string.Join(",", book.GetGenres())
                    });
                }
            }
        }
    }
}
