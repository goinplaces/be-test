using System;
using System.Net.Http;
using System.Threading.Tasks;
using VacationRental.Common.Models;
using Xunit;

namespace VacationRental.Tests
{
    [Collection("Integration")]
    public sealed class PutRentalTests
    {
        private readonly HttpClient _client;

        public PutRentalTests(IntegrationFixture fixture)
        {
            _client = fixture.Client;
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPutRental_ThenAGetReturnsTheUpdatedRental()
        {
            var request = new RentalModel
            {
                Units = 2,
                PreparationTimeInDays = 2
            };

            ResourceIdViewModel postResult;
            using (var postResponse = await _client.PostAsJsonAsync("/api/v1/rentals", request))
            {
                Assert.True(postResponse.IsSuccessStatusCode);
                postResult = await postResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            await CreateNewBookings(postResult.Id);

            request = new RentalModel
            {
                Units = 4,
                PreparationTimeInDays = 3
            };
            using (var putResponse = await _client.PutAsJsonAsync($"/api/v1/rentals/{postResult.Id}", request))
            {
                Assert.True(putResponse.IsSuccessStatusCode);
            }

            using var getResponse = await _client.GetAsync($"/api/v1/rentals/{postResult.Id}");
            Assert.True(getResponse.IsSuccessStatusCode);

            var getResult = await getResponse.Content.ReadAsAsync<RentalViewModel>();
            Assert.Equal(request.Units, getResult.Units);
            Assert.Equal(request.PreparationTimeInDays, getResult.PreparationTimeInDays);

            using var getCalendarResponse = await _client.GetAsync($"/api/v1/calendar?rentalId={postResult.Id}&start=2023-02-02&nights=3");
            Assert.True(getCalendarResponse.IsSuccessStatusCode);
            var getCalendarResult = await getCalendarResponse.Content.ReadAsAsync<CalendarViewModel>();
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPutRental_ThenAPostReturnsErrorWhenThereIsOverlapping()
        {
            var request = new RentalModel
            {
                Units = 2,
                PreparationTimeInDays = 1
            };

            ResourceIdViewModel postResult;
            using (var postResponse = await _client.PostAsJsonAsync("/api/v1/rentals", request))
            {
                Assert.True(postResponse.IsSuccessStatusCode);
                postResult = await postResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            await CreateNewBookings(postResult.Id);

            request = new RentalModel
            {
                Units = 2,
                PreparationTimeInDays = 4
            };

            ErrorInfo putResult;
            using (var putResponse = await _client.PutAsJsonAsync($"/api/v1/rentals/{postResult.Id}", request))
            {
                putResult = await putResponse.Content.ReadAsAsync<ErrorInfo>();
                Assert.NotNull(putResult);
            }
          
        }

        private async Task CreateNewBookings(int rentalId)
        {
            var postBookingRequest = new BookingBindingModel
            {
                RentalId = rentalId,
                Nights = 4,
                Start = new DateTime(2023, 02, 02)
            };

            using (var postBookingResponse = await _client.PostAsJsonAsync("/api/v1/bookings", postBookingRequest))
            {
                Assert.True(postBookingResponse.IsSuccessStatusCode);
            }

            postBookingRequest = new BookingBindingModel
            {
                RentalId = rentalId,
                Nights = 4,
                Start = new DateTime(2024, 02, 02)
            };

            using (var postBookingResponse = await _client.PostAsJsonAsync("/api/v1/bookings", postBookingRequest))
            {
                Assert.True(postBookingResponse.IsSuccessStatusCode);
            }

            postBookingRequest = new BookingBindingModel
            {
                RentalId = rentalId,
                Nights = 6,
                Start = new DateTime(2024, 02, 07)
            };

            using (var postBookingResponse = await _client.PostAsJsonAsync("/api/v1/bookings", postBookingRequest))
            {
                Assert.True(postBookingResponse.IsSuccessStatusCode);
            }

           
        }
    }
}
