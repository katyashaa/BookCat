namespace BookCat.Storage
{
    public interface IMemory
    {
        Task Book(string title, int year, string author, string isbn, string annotation, string genres);
        Task FindAuthor(string author);
        Task FindTitle(string keyword);
        Task FindKeywordInAnnotation(string keyword);
    }
}

