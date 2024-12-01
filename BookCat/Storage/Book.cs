using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BookCat.Util;

namespace BookCat.Storage
{
    [Serializable]
    public class Book
    {
        private string _title;
        private int _year;
        private string _author;
        private string _isbn;
        private string _annotation;

        private HashSet<string> _genres = new HashSet<string>();
        private HashSet<string> _annotationTokens = new HashSet<string>();
        private HashSet<string> _nameTokens = new HashSet<string>();
        private HashSet<string> _authorNameTokens = new HashSet<string>();

        // Конструктор с HashSet<string> для жанров
        public Book(string title, int year, string author, string isbn, string annotation, HashSet<string> genres)
        {
            _title = title.Trim();
            _year = year;
            _author = author.Trim();
            _isbn = isbn.Trim();
            _annotation = annotation.Trim();
            _genres = genres ?? new HashSet<string>();  // Если genres равен null, создаем новый HashSet
            Tokenizator.Tokenize(_annotationTokens, _annotation);
            Tokenizator.Tokenize(_nameTokens, _title);
            Tokenizator.Tokenize(_authorNameTokens, _author);
        }

        // Конструктор, принимающий строку жанров
        public Book(string title, int year, string author, string isbn, string annotation, string genres)
        {
            _title = title.Trim();
            _year = year;
            _author = author.Trim();
            _isbn = isbn.Trim();
            _annotation = annotation.Trim();

            // Преобразуем строку жанров в HashSet
            _genres = new HashSet<string>(genres.Split(',').Select(g => g.Trim().ToLowerInvariant()));
            
            Tokenizator.Tokenize(_annotationTokens, _annotation);
            Tokenizator.Tokenize(_nameTokens, _title);
            Tokenizator.Tokenize(_authorNameTokens, _author);
        }

        // Методы для получения свойств
        public string GetIsbn()
        {
            return _isbn;
        }

        public string GetTitle()
        {
            return _title;
        }

        public int GetTitleLen()
        {
            return _nameTokens.Count;
        }

        public string GetAuthor()
        {
            return _author;
        }

        public string GetAnnotation()
        {
            return _annotation;
        }

        public int? GetYear()
        {
            return _year;
        }

        // Получение жанров как HashSet
        public HashSet<string> GetGenres()
        {
            return _genres;
        }

        // Поиск по автору
        public bool FindAuthor(string author)
        {
            HashSet<string> name = new HashSet<string>();
            Tokenizator.Tokenize(name, author.ToLowerInvariant());

            foreach (var part in name)
            {
                if (!_authorNameTokens.Contains(part))
                {
                    return false;
                }
            }

            return true;
        }

        // Поиск по названию
        public bool FindTitle(string keyword)
        {
            return _nameTokens.Contains(keyword.ToLowerInvariant());
        }

        // Поиск по нескольким ключевым словам в названии
        public int FindTitle(HashSet<string> tokenList)
        {
            int counter = 0;
            foreach (var kw in tokenList)
            {
                if (_nameTokens.Contains(kw))
                {
                    counter++;
                }
            }

            return counter;
        }

        // Поиск по жанру
        public bool FindGenres(string keyword)
        {
            return _genres.Contains(keyword.ToLowerInvariant());
        }

        // Поиск по аннотации
        public bool FindKeywordInAnnotation(string keyword)
        {
            return _annotationTokens.Contains(keyword.ToLowerInvariant());
        }

        // Краткая информация о книге
        public string GetBriefInfo()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var g in _genres)
            {
                if (sb.Length > 0)
                {
                    sb.Append(", ");
                }

                sb.Append(g);
            }

            return $"{_title}, {_author}, {sb}, {_year}, ISBN: {_isbn}";
        }
    }
}
