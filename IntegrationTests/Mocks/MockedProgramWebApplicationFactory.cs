﻿using EUniversity.Core.Models;
using EUniversity.Core.Services;
using EUniversity.Extensions;
using EUniversity.Infrastructure.Data;
using EUniversity.Infrastructure.Services.University;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using System.Net.Http.Headers;

namespace EUniversity.IntegrationTests.Mocks
{
    /// <summary>
    /// <see cref="WebApplicationFactory{}" /> for testing with mocked services and authentication.
    /// </summary>
    public class MockedProgramWebApplicationFactory : WebApplicationFactory<Program>
    {
        public TestClaimsProvider ClaimsProvider { get; private set; } = null!;
        public IAuthService AuthServiceMock { get; private set; } = null!;
        public UserManager<ApplicationUser> UserManagerMock { get; private set; } = null!;
        public IClassroomsService ClassroomsServiceMock { get; private set; } = null!;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            ClaimsProvider = new();
            AuthServiceMock = Substitute.For<IAuthService>();

            var mockedUserStore = Substitute.For<IUserStore<ApplicationUser>>();
            UserManagerMock = Substitute.For<UserManager<ApplicationUser>>(
                mockedUserStore, null, null, null, null, null, null, null, null
                );

            ClassroomsServiceMock = Substitute.For<IClassroomsService>();

            builder.ConfigureTestServices(services =>
            {
                services.AddDbContext<ApplicationDbContext>(o => o.UseInMemoryDatabase("Endpoints tests DB")
                    .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning)));

                services.AddScoped(_ => ClaimsProvider);

                services.AddAuthentication(defaultScheme: "TestScheme")
                    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                        "TestScheme", options => { });
                services.AddCustomizedAuthorization("TestScheme");

                services.AddScoped(_ => AuthServiceMock);
                services.AddScoped(_ => UserManagerMock);
                services.AddScoped(_ => ClassroomsServiceMock);
            });
        }

        public HttpClient CreateCustomClient()
        {
            var client = CreateClient(new()
            {
                AllowAutoRedirect = false
            });

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("TestScheme");

            return client;
        }
    }
}
