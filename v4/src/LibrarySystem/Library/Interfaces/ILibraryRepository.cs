using Library.DTO;
using Library.Entities;

namespace Library.Interfaces
{
    public interface ILibraryRepository
    {
        Task<IEnumerable<Books>> GetAllBooks();
        Task<Books> GetBookById(int Id);
        Task<BookResponse> GetBookByGuid(Guid guid);

        Task<Books> GetFullBookByGuid(Guid guid);
        Task<IEnumerable<Libraries>> GetAllLibraries();
        Task<Libraries> GetLibraryById(int Id);
        Task<LibraryResponse> GetLibraryByGuid(Guid guid);

        Task<PaginationResponse<LibraryResponse>> GetCityLibraries(int? page, int? size, string city);

        Task<PaginationResponse<LibraryBookResponse>> GetLibraryBooks(int? page, int? size, Guid libraryGuid, bool? allShow= false);

        Task<LibraryBooks> GetLibraryBookAsync(Guid bookGuid, Guid libraryGuid);
        Task<bool> CheckLibraryBookCount(Guid bookGuid, Guid libraryGuid);

        void RentBook(LibraryBooks libraryBooks);
        void BookUpdate(Books book);
        Task SaveAsync();
    }
}
