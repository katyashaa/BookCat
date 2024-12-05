using Microsoft.Extensions.Configuration;
namespace BookCat.Storage
{
    public class Memory : IMemory
    {
        private readonly IMemory _repo;

        public Memory(IMemory repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        public static Memory RepoStorage;
        
        public static void Initialize(IConfiguration configuration)
        {
            RepoStorage = new Memory(new Database(configuration));
        }

        public async Task Book(string title, int year, string author, string isbn, string annotation, string genres)
        {
            await _repo.Book(title, year, author, isbn, annotation, genres);
        }

        public async Task FindAuthor(string author)
        {
            await _repo.FindAuthor(author);
        }

        public async Task FindTitle(string keyword)
        {
            await _repo.FindTitle(keyword);
        }

        public async Task FindKeywordInAnnotation(string keyword)
        {
            await _repo.FindKeywordInAnnotation(keyword);
        }
    }
}