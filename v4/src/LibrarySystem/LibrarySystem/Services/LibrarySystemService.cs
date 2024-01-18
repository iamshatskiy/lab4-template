using LibrarySystem.DTO;
using LibrarySystem.Interfaces;
using LibrarySystem.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Drawing;

namespace LibrarySystem.Services
{
    public class LibrarySystemService : ILibrarySystemService
    {
        private readonly HttpClient _httpClient;

        public LibrarySystemService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<PaginationResponse<LibraryResponse>> GetCityLibraries(int? page, int? size, string city)
        {
            var result = await _httpClient.GetFromJsonAsync<PaginationResponse<LibraryResponse>>($"http://library:8060/libraries?page={page}&size={size}&city={city}");

            return result;
        }

        public async Task<PaginationResponse<LibraryBookResponse>> GetLibraryBooks(int? page, int? size, Guid libraryGuid, bool? allShow = false)
        {
            var result = await _httpClient.GetFromJsonAsync<PaginationResponse<LibraryBookResponse>>($"http://library:8060/libraries/{libraryGuid}/books?page={page}&size={size}&allShow={allShow}");

            return result;
        }

        public async Task<RatingResponse?> GetRatingResponseByUserName(string userName)
        {
            using var ratingRequest = new HttpRequestMessage(HttpMethod.Get, $"http://rating:8050/rating");
            ratingRequest.Headers.Add("X-User-Name", userName);
            using var ratingResult = await _httpClient.SendAsync(ratingRequest);
            var rating = await ratingResult.Content.ReadFromJsonAsync<RatingResponse>();

            return rating;
        }

        public async Task<RentInfoResponse> CreateBookReservation(string userName, RentingRequest request)
        {

            var bookCheck = await _httpClient.GetFromJsonAsync<CheckResponse>($"http://library:8060/library/checkBookAvailable/?libraryUid={request.libraryUid}&bookUid={request.bookUid}");

            if (bookCheck?.check == false)
            {
                return null;
            }

            using var reserveCountRequest = new HttpRequestMessage(HttpMethod.Get, $"http://reservation:8070/reservationsCount");
            reserveCountRequest.Headers.Add("X-User-Name", userName);
            using var reserveCountResult = await _httpClient.SendAsync(reserveCountRequest);
            reserveCountResult.EnsureSuccessStatusCode();
            var reservationCount = await reserveCountResult.Content.ReadFromJsonAsync<ReservationCountResponse>();

            using var ratingRequest = new HttpRequestMessage(HttpMethod.Get, $"http://rating:8050/rating");
            ratingRequest.Headers.Add("X-User-Name", userName);
            using var ratingResult = await _httpClient.SendAsync(ratingRequest);
            var rating = await ratingResult.Content.ReadFromJsonAsync<RatingResponse>();

            if (reservationCount.reservationCount >= rating.Stars)
            {
                return null;
            }

            var reserveRequest = new HttpRequestMessage(HttpMethod.Post, "http://reservation:8070/reservation/create");
            reserveRequest.Headers.Add("X-User-Name", userName);
            reserveRequest.Content = JsonContent.Create(request, typeof(RentingRequest));

            var reserveResult = await _httpClient.SendAsync(reserveRequest);
            reserveResult.EnsureSuccessStatusCode();
            var reservation = await reserveResult.Content.ReadFromJsonAsync<ReservationResponse>();
            
            var libraryRequestBody = new RentRequest {bookUid = request.bookUid, libraryUid = request.libraryUid };
            var action = true; //Флаг взятия книги

            var libraryRequest = new HttpRequestMessage(HttpMethod.Post, $"http://library:8060/library/rentBook?action={action}");
            libraryRequest.Content = JsonContent.Create(libraryRequestBody, typeof(RentRequest));
            var libraryResult = await _httpClient.SendAsync(libraryRequest);
            libraryResult.EnsureSuccessStatusCode();

            var library = await _httpClient.GetFromJsonAsync<LibraryResponse>($"http://library:8060/library/{request.libraryUid}");
            var book = await _httpClient.GetFromJsonAsync<BookResponse>($"http://library:8060/book/{request.bookUid}");

            var result = new RentInfoResponse {
                reservationUid = reservation.Reservation_uid,
                startDate = reservation.Start_date,
                tillDate = reservation.Till_date,
                status = reservation.Status,
                book = book,
                library = library,
                rating = rating
            };

            return result;
        }

        public async Task<CloseReservationResponse> CloseBookReservation(string userName, Guid reservationUid, ReturnBookRequest request)
        {
            //Возврат внутри Reservation
            var requestReturnBody = new ReturnRequest { reservationGuid = reservationUid.ToString(), returnDate = request.date };

            using var reserveCloseRequest = new HttpRequestMessage(HttpMethod.Post, $"http://reservation:8070/reservation/return");
            reserveCloseRequest.Content = JsonContent.Create(requestReturnBody, typeof(ReturnRequest));
            using var reserveCloseResult = await _httpClient.SendAsync(reserveCloseRequest);

            if (reserveCloseResult.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return new CloseReservationResponse { isReturned = false };
            }

            var reservation = await reserveCloseResult.Content.ReadFromJsonAsync<ReservationResponse?>();

            if (reservation is null)
            {
                return new CloseReservationResponse { isReturned = false };
            }

            //Увеличение счетчика
            var libraryRequestBody = new RentRequest { bookUid = reservation.Book_uid.ToString(), libraryUid = reservation.Library_uid.ToString() };
            var action = false; //Флаг возврата книги

            var libraryRequest = new HttpRequestMessage(HttpMethod.Post, $"http://library:8060/library/rentBook?action={action}");
            libraryRequest.Content = JsonContent.Create(libraryRequestBody, typeof(RentRequest));
            var libraryResult = await _httpClient.SendAsync(libraryRequest);
            libraryResult.EnsureSuccessStatusCode();

            
            //Изменение рейтинга

            var book = await _httpClient.GetFromJsonAsync<Books>($"http://library:8060/bookInfo?bookUid={reservation.Book_uid.ToString()}");
            
            var ratingRequestBody = new RatingUpdateRequest
            {
                laterDate = reservation.Till_date < request.date,
                lessCond = request.condition != book.Condition
            };

            using var ratingUpdateRequest = new HttpRequestMessage(HttpMethod.Post, $"http://rating:8050/rating");
            ratingUpdateRequest.Headers.Add("X-User-Name", userName);
            ratingUpdateRequest.Content = JsonContent.Create(ratingRequestBody, typeof(RatingUpdateRequest));
            using var ratingUpdateResult = await _httpClient.SendAsync(ratingUpdateRequest);
            ratingUpdateResult.EnsureSuccessStatusCode();

            //Если состояние книги изменилось
            if (book.Condition != request.condition)
            {
                var condChangeBody = new ChangeCondRequest {condition = request.condition, guid = reservation.Book_uid.ToString() };
                var condChangeRequest = new HttpRequestMessage(HttpMethod.Post, $"http://library:8060/book/condition/change");
                condChangeRequest.Content = JsonContent.Create(condChangeBody, typeof(ChangeCondRequest));
                var condChangeResult = await _httpClient.SendAsync(condChangeRequest);
                condChangeResult.EnsureSuccessStatusCode();
            }

            return new CloseReservationResponse { isReturned = true };
        }

        public async Task<IEnumerable<OpenReservationResponse>> GetBookReservations(string userName)
        {
            var reservationsRequest = new HttpRequestMessage(HttpMethod.Get, $"http://reservation:8070/reservations/get");
            reservationsRequest.Headers.Add("X-User-Name", userName);
            using var reservationsResult = await _httpClient.SendAsync(reservationsRequest);

            var reservationRespones = await reservationsResult.Content.ReadFromJsonAsync<IEnumerable<ReservationResponse>>();
            var responses = new List<OpenReservationResponse>();

            foreach (var reservation in reservationRespones)
            {
                var library = await _httpClient.GetFromJsonAsync<LibraryResponse>($"http://library:8060/library/{reservation.Library_uid.ToString()}");
                var book = await _httpClient.GetFromJsonAsync<BookResponse>($"http://library:8060/book/{reservation.Book_uid.ToString()}");
                responses.Add(new OpenReservationResponse {
                              reservationUid = reservation.Reservation_uid,
                              startDate = reservation.Start_date,
                              tillDate = reservation.Till_date,
                              status = reservation.Status,
                              book = book,
                              library = library
                });
            }
            return responses;
        }
    }
}
