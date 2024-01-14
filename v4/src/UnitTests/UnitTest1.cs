using LibrarySystem.Controllers;
using LibrarySystem.DTO;
using LibrarySystem.Interfaces;
using Moq;

namespace UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task TestGetCityLibrariesAsync()
        {
            var librarySystemMock = new Mock<ILibrarySystemService>();

            Task<bool> healthCheckTask = Task.Run(() => true);
            librarySystemMock.Setup(a => a.HealthCheckAsync("library:8060")).Returns(healthCheckTask);


            int? page = 1, size = 5;
            Task<PaginationResponse<LibraryResponse>?> returnLibrariesTask = Task.Run(() => Builder.BuildLibraryPages(page, size));

            librarySystemMock.Setup(a => a.GetCityLibraries(page, size, "Москва")).Returns(returnLibrariesTask);

            LibrarySystemController controller = new LibrarySystemController(librarySystemMock.Object);

            var responseTask = controller.GetCityLibraries("Москва", page, size);

            var response = await responseTask;

            librarySystemMock.Verify(mock => mock.GetCityLibraries(page, size, "Москва"), Times.Once());
            Assert.IsTrue(responseTask.IsCompletedSuccessfully);
            Assert.IsTrue(response.Value.TotalElements.Equals(2));
        }

        [TestMethod]
        public async Task TestGetLibraryBooksAllShow()
        {
            var librarySystemMock = new Mock<ILibrarySystemService>();
            int? page = 1, size = 5;
            Guid guid = Guid.NewGuid();
            Task<bool> healthCheckTask = Task.Run(() => true);
            librarySystemMock.Setup(a => a.HealthCheckAsync("library:8060")).Returns(healthCheckTask);

            Task<PaginationResponse<LibraryBookResponse>?> returnLibraryTask = Task.Run(() => Builder.BuildBookPages(page, size, guid));
            librarySystemMock.Setup(a => a.GetLibraryBooks(page, size, guid, true)).Returns(returnLibraryTask);

            LibrarySystemController controller = new LibrarySystemController(librarySystemMock.Object);

            var responseTask = controller.GetLibraryBooks(guid.ToString(), page, size, true);

            var response = await responseTask;

            librarySystemMock.Verify(mock => mock.GetLibraryBooks(page, size, guid, true), Times.Once());
            Assert.IsTrue(responseTask.IsCompletedSuccessfully);
            Assert.IsTrue(response.Value.TotalElements.Equals(2));
        }

        [TestMethod]
        public async Task TestGetLibraryBooks()
        {
            var librarySystemMock = new Mock<ILibrarySystemService>();
            int? page = 1, size = 5;
            Guid guid = Guid.NewGuid();
            Task<bool> healthCheckTask = Task.Run(() => true);
            librarySystemMock.Setup(a => a.HealthCheckAsync("library:8060")).Returns(healthCheckTask);

            Task<PaginationResponse<LibraryBookResponse>?> returnLibraryTask = Task.Run(() => Builder.BuildBookPagesNotAll(page, size, guid));
            librarySystemMock.Setup(a => a.GetLibraryBooks(page, size, guid, false)).Returns(returnLibraryTask);

            LibrarySystemController controller = new LibrarySystemController(librarySystemMock.Object);

            var responseTask = controller.GetLibraryBooks(guid.ToString(), page, size, false);

            var response = await responseTask;

            librarySystemMock.Verify(mock => mock.GetLibraryBooks(page, size, guid, false), Times.Once());
            Assert.IsTrue(responseTask.IsCompletedSuccessfully);
            Assert.IsTrue(response.Value.TotalElements.Equals(1));
        }

        [TestMethod]
        public async Task TestGetUserRating()
        {
            var librarySystemMock = new Mock<ILibrarySystemService>();
            string uname = "Фома";
            Guid guid = Guid.NewGuid();
            Task<bool> healthCheckTask = Task.Run(() => true);
            librarySystemMock.Setup(a => a.HealthCheckAsync("rating:8050")).Returns(healthCheckTask);

            Task<RatingResponse?> returnLibraryTask = Task.Run(() => Builder.UserInfo());
            librarySystemMock.Setup(a => a.GetRatingResponseByUserName(uname)).Returns(returnLibraryTask);

            LibrarySystemController controller = new LibrarySystemController(librarySystemMock.Object);

            var responseTask = controller.GetUserRating(uname);

            var response = await responseTask;

            librarySystemMock.Verify(mock => mock.GetRatingResponseByUserName(uname), Times.Once());
            Assert.IsTrue(responseTask.IsCompletedSuccessfully);
        }

        [TestMethod]
        public async Task TestGetReservaions()
        {


            var librarySystemMock = new Mock<ILibrarySystemService>();

            Task<bool> healthCheckTask = Task.Run(() => true);
            librarySystemMock.Setup(a => a.HealthCheckAsync("reservation:8070")).Returns(healthCheckTask);


            Task<IEnumerable<OpenReservationResponse>?> returnLibraryTask = Task.Run(() => Builder.Reservations());
            var userName = "Фома";

            librarySystemMock.Setup(a => a.GetBookReservations(userName)).Returns(returnLibraryTask);

            LibrarySystemController controller = new LibrarySystemController(librarySystemMock.Object);

            var responseTask = controller.GetBookReservations(userName);

            var response = await responseTask;

            librarySystemMock.Verify(mock => mock.GetBookReservations(userName), Times.Once());
            Assert.IsTrue(responseTask.IsCompletedSuccessfully);
        }
    }
}