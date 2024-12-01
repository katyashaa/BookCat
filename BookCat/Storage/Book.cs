using System;
using System.Collections.Generic;
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

        public Book(string title, int year, string author, string isbn, string annotation, HashSet<string> genres)
        {
            _title = title.Trim();
            _year = year;
            _author = author.Trim();
            _isbn = isbn.Trim();
            _annotation = annotation.Trim();
            _genres = genres;
            Tokenizator.Tokenize(_annotationTokens, _annotation);
            Tokenizator.Tokenize(_nameTokens, _title);
            Tokenizator.Tokenize(_authorNameTokens, _author);
        }

        public Book(string title, int year, string author, string isbn, string annotation, string genres)
        {
            _title = title.Trim();
            _year = year;
            _author = author.Trim();
            _isbn = isbn.Trim();
            _annotation = annotation.Trim();
            Tokenizator.Tokenize(_annotationTokens, _annotation);
            Tokenizator.Tokenize(_nameTokens, _title);
            Tokenizator.Tokenize(_genres, genres);
            Tokenizator.Tokenize(_authorNameTokens, _author);
        }

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

        public bool FindTitle(string keyword)
        {
            return _nameTokens.Contains(keyword.ToLowerInvariant());
        }

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

        public bool FindGenres(string keyword)
        {
            return _genres.Contains(keyword.ToLowerInvariant());
        }

        public bool FindKeywordInAnnotation(string keyword)
        {
            return _annotationTokens.Contains(keyword.ToLowerInvariant());
        }

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
