using Library.DTO;
using Library.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Library.Controllers
{
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class LibraryController : Controller
    {
        readonly ILibraryService _libraryService;


        public LibraryController(ILibraryService libraryService)
        {
            _libraryService = libraryService;
        }

        
        [HttpGet("libraries")]
        public async Task<IActionResult> GetCityLibraries([FromQuery, Required] string city, [FromQuery] int? page, [FromQuery] int? size)
        {
            var availableLibraries = await _libraryService.GetCityLibraries(page, size, city);

            return Ok(availableLibraries);
        }

        [HttpGet("libraries/{libraryUid}/books")]
        public async Task<IActionResult> GetLibraryBooks([FromRoute] string libraryUid, [FromQuery] int? page, [FromQuery] int? size, [FromQuery] bool allShow = false)
        {
            var books = await _libraryService.GetLibraryBooks(page, size, Guid.Parse(libraryUid), allShow);

            return Ok(books);
        }

        [HttpGet("library/{libraryUid}")]
        public async Task<IActionResult> GetLibraryByGuid([FromRoute] string libraryUid)
        {
            var library = await _libraryService.GetLibraryByGuid(Guid.Parse(libraryUid));

            return Ok(library);
        }

        [HttpGet("library")]
        public async Task<IActionResult> GetLibraryById([FromQuery, Required] int libraryId)
        {
            var library = await _libraryService.GetLibraryById(libraryId);

            return Ok(library);
        }

        [HttpGet("book/{bookUid}")]
        public async Task<IActionResult> GetBookByGuid([FromRoute] string bookUid)
        {
            var book = await _libraryService.GetBookByGuid(Guid.Parse(bookUid));

            return Ok(book);
        }

        [HttpGet("book")]
        public async Task<IActionResult> GetBookById([FromQuery, Required] int bookId)
        {
            var book = await _libraryService.GetBookById(bookId);

            return Ok(book);
        }

        [HttpGet("library/checkBookAvailable")]
        public async Task<IActionResult> CheckLibraryBookAvailable([FromQuery, Required] string libraryUid, [FromQuery, Required] string bookUid)
        {
            var check = await _libraryService.CheckLibraryBookCount(Guid.Parse(bookUid), Guid.Parse(libraryUid));

            return Ok(check);
        }

        [HttpPost("library/rentBook")]
        public async Task<IActionResult> RentBookAction([FromBody, Required] RentRequest request, [FromQuery, Required] bool action)
        {
            var check = await _libraryService.RentBookAsync(Guid.Parse(request.bookUid), Guid.Parse(request.libraryUid), action);

            if (check.check)
                return Ok();
            else
                return BadRequest();
        }


        [HttpGet("bookInfo")]
        public async Task<IActionResult> GetBookInfo([FromQuery, Required] string bookUid)
        {
            var book = await _libraryService.GetBookFullByGuid(Guid.Parse(bookUid));

            return Ok(book);
        }

        [HttpPost("book/condition/change")]
        public async Task<IActionResult> ChangeBookCondition([FromBody, Required] ChangeCondRequest request)
        {
            var check = await _libraryService.ChangeBookCondAsync(Guid.Parse(request.guid), request.condition);

            if (check.check)
                return Ok();
            else
                return BadRequest();
        }
    }
}