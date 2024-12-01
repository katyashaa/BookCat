using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using BookCat.Util;

namespace BookCat.Storage
{
    public class BookLibrary
    {
        private readonly List<Book> _books = new List<Book>();

        public bool AddBook(Book item)
        {
            if (!AddLocalBook(item))
            {
                return false;
            }

            // Данные можно сохранить другим способом, если потребуется
            SaveBooks();
            return true;
        }

        public bool AddLocalBook(Book item)
        {
            HashSet<string> name = new HashSet<string>();
            Tokenizator.Tokenize(name, item.GetTitle());
            foreach (var b in _books)
            {
                if (b.FindTitle(name) == b.GetTitleLen())
                {
                    return false; // Книга уже существует
                }
            }
            _books.Add(item);
            return true;
        }

        // Возвращает список всех книг в библиотеке как ReadOnlyCollection
        public ReadOnlyCollection<Book> GetLib()
        {
            return _books.AsReadOnly();
        }

        // Метод для сохранения данных (если необходимо)
        private void SaveBooks()
        {
            // Реализуйте логику сохранения, если нужно
            // Например, сохранять данные в базу данных
        }
    }
}