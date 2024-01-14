using LibrarySystem.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    internal class Builder
    {
        public static PaginationResponse<LibraryResponse>? BuildLibraryPages(int? page, int? pageSize)
        {
            var libraries = new List<LibraryResponse>();

            libraries.Add(new LibraryResponse()
            {
                libraryUid = Guid.NewGuid(),
                Address = "ул. Пушкина, д. Колотушкина",
                City = "Москва",
                Name = "Библиотека у Билли"
            });

            libraries.Add(new LibraryResponse()
            {
                libraryUid = Guid.NewGuid(),
                Address = "ул. Колотушкина, д. Пушкина",
                City = "Москва",
                Name = "Продам гараж"
            });

            var total = libraries.Count();

            var response = new PaginationResponse<LibraryResponse>()
            {
                Page = page.Value,
                PageSize = pageSize.Value,
                TotalElements = total,
                Items = libraries
            };
            return response;
        }

        public static PaginationResponse<LibraryBookResponse>? BuildBookPages(int? page, int? pageSize, Guid guid)
        {
            var books = new List<LibraryBookResponse>();

            books.Add(new LibraryBookResponse()
            {
                bookUid = Guid.NewGuid(),
                Author = "Анатолий Палено",
                Genre = "Романтика",
                Name = "Кто я?",
                AvailableCount = 1,
                Condition = "EXCELLENT"
            });

            books.Add(new LibraryBookResponse()
            {
                bookUid = Guid.NewGuid(),
                Author = "Геннадий Горин",
                Genre = "Фантастика",
                Name = "Мой камыш",
                AvailableCount = 0,
                Condition = "GOOD"
            });

            var total = books.Count();

            var response = new PaginationResponse<LibraryBookResponse>()
            {
                Page = page.Value,
                PageSize = pageSize.Value,
                TotalElements = total,
                Items = books
            };
            return response;
        }
        public static PaginationResponse<LibraryBookResponse>? BuildBookPagesNotAll(int? page, int? pageSize, Guid guid)
        {
            var books = new List<LibraryBookResponse>();

            books.Add(new LibraryBookResponse()
            {
                bookUid = Guid.NewGuid(),
                Author = "Анатолий Палено",
                Genre = "Романтика",
                Name = "Кто я?",
                AvailableCount = 1,
                Condition = "EXCELLENT"
            });

            var total = books.Count();

            var response = new PaginationResponse<LibraryBookResponse>()
            {
                Page = page.Value,
                PageSize = pageSize.Value,
                TotalElements = total,
                Items = books
            };
            return response;
        }

        public static RatingResponse? UserInfo()
        {
            var response = new RatingResponse()
            {
                Stars = 60
            };
            return response;
        }

        public static RentInfoResponse? ReservationInfo()
        {
            var libraries = Builder.BuildLibraryPages(1, 2);
            var books = Builder.BuildBookPages(1, 2, libraries.Items.LastOrDefault().libraryUid);
            var book = new BookResponse()
            {
                Author = books.Items.LastOrDefault().Author,
                bookUid = books.Items.LastOrDefault().bookUid,
                Genre = "Фантастика",
                Name = "Дом"
            };
            var response = new RentInfoResponse()
            {
                book = book,
                library = libraries.Items.LastOrDefault(),
                rating = Builder.UserInfo(),
                reservationUid = Guid.NewGuid(),
                startDate = new DateOnly(),
                tillDate = new DateOnly(),
                status = "RENTED"
            };
            return response;
        }

        public static IEnumerable<OpenReservationResponse> Reservations()
        {
            var reservations = new List<OpenReservationResponse>();
            var libraries = Builder.BuildLibraryPages(1, 2);
            var books = Builder.BuildBookPages(1, 2, libraries.Items.LastOrDefault().libraryUid);

            reservations.Add(new OpenReservationResponse()
            {
                book = new BookResponse()
                {
                    Author = "Гена Букин",
                    bookUid = Guid.NewGuid(),
                    Genre = "Спорт",
                    Name = "Кожаный мяч"
                },
                library = libraries.Items.LastOrDefault(),
                reservationUid = Guid.NewGuid(),
                startDate = new DateOnly(),
                status = "RENTED",
                tillDate = new DateOnly()
            });

            return reservations;
        }

    }
}
