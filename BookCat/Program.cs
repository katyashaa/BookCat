using System.Collections;
using BookCat.Menu;
using BookCat.Storage;

class Program
{
    static void Main(string[] args)
    {
        string connectionString = "Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=1315";
        var library = new BookLibrary(connectionString);
        
        var options = new ArrayList
        {
            new AddBook(library), 
            new FindName(library), 
            new FindAuthor(library), 
            new FindISBN(library), 
            new FindKeyword(library) 
        };
        
        var startMenu = new Start(library, options);
        startMenu.Exec();
    }
}