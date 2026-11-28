using System;
using System.IO;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using SOA_CA2.DTOs;
using Xunit;

namespace SOA_CA2.Tests;

public class PerformerBookingOverlapTests
{
    [Fact]
    public async Task CreateBooking_RejectsOverlapAndAllowsAdjacentSlot()
    {
        var directory = Directory.CreateTempSubdirectory("soa-booking-tests-");
        try
        {
            var databasePath = Path.Combine(directory.FullName, "festivals.db");
            await using var factory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder => builder.UseSetting(
                    "ConnectionStrings:Festivals", $"Data Source={databasePath};Pooling=False"));
            using var client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                BaseAddress = new Uri("https://localhost"),
                AllowAutoRedirect = false
            });

            var credentials = new { Email = "booker@example.com", Password = "Booking integration test passphrase" };
            using var registration = await client.PostAsJsonAsync("/api/Organiser/register", credentials);
            Assert.Equal(HttpStatusCode.Created, registration.StatusCode);
            using var login = await client.PostAsJsonAsync("/api/Organiser/login", credentials);
            Assert.Equal(HttpStatusCode.OK, login.StatusCode);
            var token = Assert.IsType<LoginToken>(await login.Content.ReadFromJsonAsync<LoginToken>());
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);

            using var festivalResponse = await client.PostAsJsonAsync("/api/Festival", new FestivalRequestDto
            {
                Name = "Summer Sessions",
                Location = "Dundalk",
                StartDate = new DateOnly(2027, 6, 18),
                EndDate = new DateOnly(2027, 6, 20)
            });
            Assert.Equal(HttpStatusCode.Created, festivalResponse.StatusCode);
            var festival = Assert.IsType<FestivalDto>(await festivalResponse.Content.ReadFromJsonAsync<FestivalDto>());

            using var performerResponse = await client.PostAsJsonAsync("/api/Performer",
                new PerformerRequestDto { Name = "The Harbour Lights" });
            Assert.Equal(HttpStatusCode.Created, performerResponse.StatusCode);
            var performer = Assert.IsType<PerformerDto>(await performerResponse.Content.ReadFromJsonAsync<PerformerDto>());

            const string path = "/api/PerformerBooking";
            var start = new DateTime(2027, 6, 18, 19, 0, 0, DateTimeKind.Utc);
            var slot = new
            {
                FestivalId = festival.Id,
                PerformerId = performer.Id,
                StartsAtUtc = start,
                EndsAtUtc = start.AddHours(1),
                Fee = 250m
            };

            using var created = await client.PostAsJsonAsync(path, slot);
            Assert.Equal(HttpStatusCode.Created, created.StatusCode);

            // The original slot ends at 20:00, so 19:30 overlaps.
            using var overlap = await client.PostAsJsonAsync(path,
                slot with { StartsAtUtc = start.AddMinutes(30), EndsAtUtc = start.AddMinutes(90) });
            Assert.Equal(HttpStatusCode.Conflict, overlap.StatusCode);

            // Starting exactly at 20:00 is allowed.
            using var adjacent = await client.PostAsJsonAsync(path,
                slot with { StartsAtUtc = slot.EndsAtUtc, EndsAtUtc = start.AddHours(2) });
            Assert.Equal(HttpStatusCode.Created, adjacent.StatusCode);

            var bookings = await client.GetFromJsonAsync<PerformerBookingDto[]>(path);
            Assert.Equal(2, Assert.IsType<PerformerBookingDto[]>(bookings).Length);
        }
        finally
        {
            directory.Delete(recursive: true);
        }
    }

    private record LoginToken(string AccessToken);
}
