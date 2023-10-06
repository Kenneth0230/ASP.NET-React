﻿using EUniversity.Core.Dtos.Auth;
using Microsoft.AspNetCore.Identity;

namespace EUniversity.Core.Services
{
    /// <summary>
    /// Authentication service.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Registers a user.
        /// </summary>
        /// <param name="register">Data needed for register.</param>
        /// <param name="userName">Optional username. If <see langword="null" /> then will be autogenerated.</param>
        /// <param name="password">Optional password. If <see langword="null" /> then temporary one will be created.</param>
        /// <param name="roles">Optional roles of the user.</param>
        /// <returns>
        /// Result of the operation.
        /// </returns>
        Task<RegisterResult> RegisterAsync(RegisterDto register, string? userName = null,
            string? password = null, params string[] roles);

        /// <summary>
        /// Registers many users with auto-generated usernames and passwords.
        /// </summary>
        /// <param name="users">Users to register.</param>
        /// <param name="roles">Optional roles of all users.</param>
        /// <returns>
        /// Results of the operation.
        /// </returns>
        IAsyncEnumerable<RegisterResult> RegisterManyAsync(IEnumerable<RegisterDto> users, params string[] roles);

        /// <summary>
        /// Logs in a user.
        /// </summary>
        /// <param name="login">Data needed for login.</param>
        /// <returns>
        /// <see langword="true" /> on success;
        /// otherwise <see langword="false" />.
        /// </returns>
        Task<bool> LogInAsync(LogInDto login);

        /// <summary>
        /// Changes a password for the user.
        /// </summary>
        /// <param name="userId">
        /// ID of the user whose password will be changed.
        /// </param>
        /// <param name="password">
        /// Old and new passwords.
        /// </param>
        /// <returns>
        /// Result of the operation.
        /// </returns>
        Task<IdentityResult> ChangePasswordAsync(string userId, ChangePasswordDto password);
    }
}
