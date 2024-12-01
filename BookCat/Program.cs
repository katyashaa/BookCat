using System.Collections;
using BookCat.Menu;
using BookCat.Storage;

class Program
{
    static void Main(string[] args)
    {
        // Подключение к базе данных
        string connectionString = "Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=1315";
        var library = new BookLibrary(connectionString);

        // Список доступных действий
        var options = new ArrayList
        {
            new AddBook(library), // Добавление книги
            new FindName(library), // Поиск по названию
            new FindAuthor(library), // Поиск по автору
            new FindISBN(library), // Поиск по ISBN
            new FindKeyword(library) // Поиск по ключевым словам
        };

        // Создание стартового меню
        var startMenu = new Start(library, options);

        // Запуск меню
        startMenu.Exec();
    }
}