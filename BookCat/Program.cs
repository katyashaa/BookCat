using System;
using System.Collections.Generic;
using BookCat.Storage;

class Program
{
    static void Main(string[] args) 
    {
        // Подключение к базе данных
        string connectionString = "Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=1315";
        var library = new BookLibrary(connectionString);

        while (true)
        {
            Console.WriteLine("\nВведите информацию о книге:");

            Console.Write("Название: ");
            string title = Console.ReadLine()?.Trim();

            Console.Write("Год издания: ");
            if (!int.TryParse(Console.ReadLine(), out int year))
            {
                Console.WriteLine("Некорректный год. Попробуйте снова.");
                continue;
            }

            Console.Write("Автор: ");
            string author = Console.ReadLine()?.Trim();

            Console.Write("ISBN: ");
            string isbn = Console.ReadLine()?.Trim();

            Console.Write("Аннотация: ");
            string annotation = Console.ReadLine()?.Trim();

            Console.Write("Жанры (через запятую): ");
            string genresInput = Console.ReadLine()?.Trim();
            var genres = new HashSet<string>(genresInput.Split(',', StringSplitOptions.RemoveEmptyEntries));

            // Создаем объект книги
            var book = new Book(title, year, author, isbn, annotation, genres);

            // Добавляем книгу в библиотеку (и автоматически в базу данных)
            if (library.AddBook(book))
            {
                Console.WriteLine("Книга успешно добавлена в базу данных.");
            }
            else
            {
                Console.WriteLine("Книга с таким ISBN или названием уже существует.");
            }
        }
    }
}
