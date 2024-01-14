using Library.DTO;
using Library.Entities;
using Library.Interfaces;
using Microsoft.AspNetCore.Components.Web;
using System;

namespace Library.Services
{
    public class LibraryService : ILibraryService
    {
        readonly ILibraryRepository _libraryRepository;

        public LibraryService(ILibraryRepository libraryRepository)
        {
            _libraryRepository = libraryRepository;
        }
        public async Task<IEnumerable<Books>> GetAllBooks()
        {
            return await _libraryRepository.GetAllBooks();
        }

        public async Task<IEnumerable<Libraries>> GetAllLibraries()
        {
            return await _libraryRepository.GetAllLibraries();
        }

        public async Task<BookResponse> GetBookByGuid(Guid guid)
        {
            return await _libraryRepository.GetBookByGuid(guid);
        }

        public async Task<Books> GetBookById(int Id)
        {
            return await _libraryRepository.GetBookById(Id);
        }

        public async Task<PaginationResponse<LibraryResponse>> GetCityLibraries(int? page, int? size, string city)
        {
            return await _libraryRepository.GetCityLibraries(page, size, city);
        }

        public async Task<PaginationResponse<LibraryBookResponse>> GetLibraryBooks(int? page, int? size, Guid libraryGuid, bool? allShow = false)
        {
            return await _libraryRepository.GetLibraryBooks(page, size, libraryGuid, allShow);
        }

        public async Task<LibraryResponse> GetLibraryByGuid(Guid guid)
        {
            return await _libraryRepository.GetLibraryByGuid(guid);
        }

        public async Task<Libraries> GetLibraryById(int Id)
        {
            return await _libraryRepository.GetLibraryById(Id);
        }

        public async Task<CheckResponse> CheckLibraryBookCount(Guid bookGuid, Guid libraryGuid)
        {
            return new CheckResponse { check = await _libraryRepository.CheckLibraryBookCount(bookGuid, libraryGuid) };
        }

        public async Task<CheckResponse> RentBookAsync(Guid bookGuid, Guid libraryGuid, bool rent)
        {
            var library = await _libraryRepository.GetLibraryByGuid(libraryGuid);
            var book = await _libraryRepository.GetBookByGuid(bookGuid);

            if (library == null || book == null)
                return new CheckResponse { check = false };

            var libraryBook = await _libraryRepository.GetLibraryBookAsync(bookGuid, libraryGuid);
            if (libraryBook == null)
                return new CheckResponse { check = false };

            if (rent)
            {
                libraryBook.Available_count -= 1;
            }
            else
            {
                libraryBook.Available_count += 1;
            }

            _libraryRepository.RentBook(libraryBook);
            await _libraryRepository.SaveAsync();

            return new CheckResponse { check = true };
        }

        public async Task<CheckResponse> ChangeBookCondAsync(Guid bookGuid, string newCondition)
        {
            var book = await _libraryRepository.GetFullBookByGuid(bookGuid);

            if (book == null)
                return new CheckResponse { check = false };
            
            book.Condition = newCondition;

            _libraryRepository.BookUpdate(book);
            await _libraryRepository.SaveAsync();

            return new CheckResponse { check = true };
        }

        public async Task<Books> GetBookFullByGuid(Guid guid)
        {
            return await _libraryRepository.GetFullBookByGuid(guid);
        }
    }
}
