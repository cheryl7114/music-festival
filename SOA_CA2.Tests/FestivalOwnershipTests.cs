using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using SOA_CA2.DTOs;
using Xunit;

namespace SOA_CA2.Tests;

public class FestivalOwnershipTests
{
    [Fact]
    public async Task GetFestival_RequiresTheSignedInOwner()
    {
        var directory = Directory.CreateTempSubdirectory("soa-festival-tests-");
        try
        {
            var databasePath = Path.Combine(directory.FullName, "festivals.db");
            await using var factory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder => builder.UseSetting(
                    "ConnectionStrings:Festivals", $"Data Source={databasePath};Pooling=False"));

            var options = new WebApplicationFactoryClientOptions
            {
                BaseAddress = new Uri("https://localhost"),
                AllowAutoRedirect = false
            };

            using var owner = factory.CreateClient(options);
            using var other = factory.CreateClient(options);
            using var anonymous = factory.CreateClient(options);
            await SignInAsync(owner, "owner@example.com");
            await SignInAsync(other, "other@example.com");

            var request = new FestivalRequestDto
            {
                Name = "Summer Sessions",
                Location = "Dundalk",
                StartDate = new DateOnly(2027, 6, 18),
                EndDate = new DateOnly(2027, 6, 20)
            };
            using var created = await owner.PostAsJsonAsync("/api/Festival", request);
            Assert.Equal(HttpStatusCode.Created, created.StatusCode);
            var festival = Assert.IsType<FestivalDto>(await created.Content.ReadFromJsonAsync<FestivalDto>());
            var path = $"/api/Festival/{festival.Id}";

            // 200 for the festival's owner
            using var ownerResponse = await owner.GetAsync(path);
            Assert.Equal(HttpStatusCode.OK, ownerResponse.StatusCode);
            Assert.Equal(festival, await ownerResponse.Content.ReadFromJsonAsync<FestivalDto>());

            // 404 as the festival belongs to someone else
            using var otherResponse = await other.GetAsync(path);
            Assert.Equal(HttpStatusCode.NotFound, otherResponse.StatusCode);

            // 401 if the auth token is missing
            using var anonymousResponse = await anonymous.GetAsync(path);
            Assert.Equal(HttpStatusCode.Unauthorized, anonymousResponse.StatusCode);
        }
        finally
        {
            directory.Delete(recursive: true);
        }
    }

    private static async Task SignInAsync(HttpClient client, string email)
    {
        var credentials = new { Email = email, Password = "Festival integration test passphrase" };
        using var registration = await client.PostAsJsonAsync("/api/Organiser/register", credentials);
        Assert.Equal(HttpStatusCode.Created, registration.StatusCode);

        using var login = await client.PostAsJsonAsync("/api/Organiser/login", credentials);
        Assert.Equal(HttpStatusCode.OK, login.StatusCode);
        var token = Assert.IsType<LoginToken>(await login.Content.ReadFromJsonAsync<LoginToken>());
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
    }

    private record LoginToken(string AccessToken);
}
