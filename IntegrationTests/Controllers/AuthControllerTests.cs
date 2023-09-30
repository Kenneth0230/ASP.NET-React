﻿using EUniversity.Core.Dtos.Auth;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
using System.Net;

namespace EUniversity.IntegrationTests.Controllers
{
	public class AuthControllerTests : ControllersTest
	{
		private const string DefaultUserName = "user";
		private const string DefaultPassword = "Password1!@Gs";
		private const string NewPassword = DefaultPassword + "2";

		[Test]
		public async Task LogIn_ValidAttempt_Succeeds()
		{
			// Arrange
			using var client = CreateUnauthorizedClient();
			LogInDto loginDto = new()
			{
				UserName = DefaultUserName,
				Password = DefaultPassword
			};
			WebApplicationFactory.AuthServiceMock
				.LogInAsync(Arg.Any<LogInDto>()).Returns(true);

			// Act
			var response = await client.PostAsJsonAsync("/api/auth/login", loginDto);

			// Assert
			response.EnsureSuccessStatusCode();
		}

		[Test]
		public async Task LogIn_InvalidAttempt_Fails()
		{
			// Arrange
			using var client = CreateUnauthorizedClient();
			LogInDto loginDto = new()
			{
				UserName = DefaultUserName,
				Password = DefaultPassword
			};
			WebApplicationFactory.AuthServiceMock
				.LogInAsync(loginDto).Returns(false);

			// Act
			var response = await client.PostAsJsonAsync("/api/auth/login", loginDto);

			// Assert
			Assert.That(response.IsSuccessStatusCode, Is.False);
		}

		[Test]
		public async Task LogIn_InvalidInput_Returns400BadRequest()
		{
			// Arrange
			using var client = CreateUnauthorizedClient();
			var loginDto = new LogInDto()
			{
				UserName = "",
				Password = ""
			};

			// Act
			var response = await client.PostAsJsonAsync("/api/auth/login", loginDto);

			// Assert
			Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
		}

		[Test]
		public async Task ChangePassword_ValidAttempt_Succeeds()
		{
			// Arrange
			string userId = Guid.NewGuid().ToString();
			using var client = CreateStudentClient(userId);
			var password = new ChangePasswordDto()
			{
				Current = DefaultPassword,
				New = NewPassword
			};
			WebApplicationFactory.AuthServiceMock
				.ChangePasswordAsync(userId, Arg.Any<ChangePasswordDto>())
				.Returns(IdentityResult.Success);

			// Act
			var response = await client.PostAsJsonAsync("/api/auth/password/change", password);

			// Assert
			response.EnsureSuccessStatusCode();
		}

		[Test]
		public async Task ChangePassword_InvalidInput_Returns400BadRequest()
		{
			// Arrange
			using var client = CreateStudentClient();
			var password = new ChangePasswordDto()
			{
				// Equal passwords shouldn't be allowed
				Current = DefaultPassword,
				New = DefaultPassword
			};

			// Act
			var response = await client.PostAsJsonAsync("/api/auth/password/change", password);

			// Assert
			Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
		}
	}
}
