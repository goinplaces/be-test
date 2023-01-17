using System;
using System.Net.Http;
using System.Threading.Tasks;
using VacationRental.Common.Models;
using Xunit;

namespace VacationRental.Tests
{
    [Collection("Integration")]
    public class PostBookingTests
    {
        private readonly HttpClient _client;

        public PostBookingTests(IntegrationFixture fixture)
        {
            _client = fixture.Client;
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPostBooking_ThenAGetReturnsTheCreatedBooking()
        {
            var postRentalRequest = new RentalModel
            {
                Units = 4,
                PreparationTimeInDays = 2
            };

            ResourceIdViewModel postRentalResult;
            using (var postRentalResponse = await _client.PostAsJsonAsync($"/api/v1/rentals", postRentalRequest))
            {
                Assert.True(postRentalResponse.IsSuccessStatusCode);
                postRentalResult = await postRentalResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            var postBookingRequest = new BookingBindingModel
            {
                 RentalId = postRentalResult.Id,
                 Nights = 3,
                 Start = new DateTime(2024, 02, 02)
            };

            ResourceIdViewModel postBookingResult;
            using (var postBookingResponse = await _client.PostAsJsonAsync($"/api/v1/bookings", postBookingRequest))
            {
                Assert.True(postBookingResponse.IsSuccessStatusCode);
                postBookingResult = await postBookingResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

             using (var getBookingResponse = await _client.GetAsync($"/api/v1/bookings/{postBookingResult.Id}"))
             {
                 Assert.True(getBookingResponse.IsSuccessStatusCode);

                 var getBookingResult = await getBookingResponse.Content.ReadAsAsync<BookingViewModel>();
                 Assert.Equal(postBookingRequest.RentalId, getBookingResult.RentalId);
                 Assert.Equal(postBookingRequest.Nights, getBookingResult.Nights);
                 Assert.Equal(postBookingRequest.Start, getBookingResult.Start);
             }
        }

        [Theory]
        [InlineData(0)]
        [InlineData(2)]
        public async Task GivenCompleteRequest_WhenPostBooking_ThenAPostReturnsErrorWhenThereIsOverbooking(int preparationTimeInDays)
        {
            var postRentalRequest = new RentalModel
            {
                Units = 1,
                PreparationTimeInDays = preparationTimeInDays
            };

            ResourceIdViewModel postRentalResult;
            using (var postRentalResponse = await _client.PostAsJsonAsync("/api/v1/rentals", postRentalRequest))
            {
                Assert.True(postRentalResponse.IsSuccessStatusCode);
                postRentalResult = await postRentalResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            var postBooking1Request = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Nights = 3,
                Start = new DateTime(2023, 02, 01).AddDays(-preparationTimeInDays)
            };

            using (var postBooking1Response = await _client.PostAsJsonAsync("/api/v1/bookings", postBooking1Request))
            {
                Assert.True(postBooking1Response.IsSuccessStatusCode);
            }

            var postBooking2Request = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Nights = 1,
                Start = new DateTime(2023, 02, 02).AddDays(-preparationTimeInDays)
            };

            ErrorInfo postBookingResult;
            using (var postBooking1Response = await _client.PostAsJsonAsync("/api/v1/bookings", postBooking1Request))
            {
                postBookingResult = await postBooking1Response.Content.ReadAsAsync<ErrorInfo>();
                Assert.NotNull(postBookingResult);
                Assert.Equal(postBookingResult.Detail, AppConstants.NoAvailability);
            }
        }
    }
}
