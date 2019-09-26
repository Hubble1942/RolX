﻿// -----------------------------------------------------------------------
// <copyright file="SignInService.cs" company="Christian Ewald">
// Copyright (c) Christian Ewald. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;
using Google.Apis.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RolXServer.Auth.DataAccess;
using RolXServer.Auth.Domain.Model;
using RolXServer.Common.DataAccess;

namespace RolXServer.Auth.Domain.Detail
{
    /// <summary>
    /// Service for signing users in.
    /// </summary>
    public sealed class SignInService : ISignInService
    {
        private readonly IRepository<User> userRepository;
        private readonly BearerTokenFactory bearerTokenFactory;
        private readonly Settings settings;
        private readonly IMapper mapper;
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="SignInService" /> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="bearerTokenFactory">The bearer token factory.</param>
        /// <param name="settingsAccessor">The settings accessor.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="logger">The logger.</param>
        public SignInService(
            IRepository<User> userRepository,
            BearerTokenFactory bearerTokenFactory,
            IOptions<Settings> settingsAccessor,
            IMapper mapper,
            ILogger<SignInService> logger)
        {
            this.userRepository = userRepository;
            this.bearerTokenFactory = bearerTokenFactory;
            this.settings = settingsAccessor.Value;
            this.mapper = mapper;
            this.logger = logger;
        }

        /// <summary>
        /// Authenticates with the specified google identifier token.
        /// </summary>
        /// <param name="googleIdToken">The google identifier token.</param>
        /// <returns>
        /// The authenticated user or <c>null</c> if authentication failed.
        /// </returns>
        public async Task<AuthenticatedUser?> Authenticate(string googleIdToken)
        {
            try
            {
                var payload = await GoogleJsonWebSignature.ValidateAsync(googleIdToken);
                if (payload is null)
                {
                    return null;
                }

                if (!this.IsAllowedDomain(payload.HostedDomain))
                {
                    this.logger.LogWarning("Sign-in from foreign domain refused: {0}", payload.HostedDomain);
                    return null;
                }

                return this.Authenticate(await this.EnsureUser(payload));
            }
            catch (InvalidJwtException e)
            {
                this.logger.LogWarning(e, "While validating googleIdToken");
                return null;
            }
        }

        /// <summary>
        /// Extends the authentication for user with the specified identifier.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>
        /// The authenticated user.
        /// </returns>
        public async Task<AuthenticatedUser?> Extend(Guid userId)
        {
            var user = await this.userRepository.Entities.FindAsync(userId);
            if (user is null)
            {
                this.logger.LogWarning("Unknown user tries to extend authentication: {0}", userId);
                return null;
            }

            return this.Authenticate(user);
        }

        private bool IsAllowedDomain(string domain)
        {
            return this.settings.GoogleHostedDomainWhitelist.Length == 0
                || this.settings.GoogleHostedDomainWhitelist.Any(d => d == domain);
        }

        private async Task<User> EnsureUser(GoogleJsonWebSignature.Payload payload)
        {
            var user = await this.userRepository.Entities.SingleOrDefaultAsync(u => u.GoogleId == payload.Subject);
            if (user is null)
            {
                this.logger.LogInformation("Adding yet unknown user {0}", payload.Name);

                user = new User { GoogleId = payload.Subject };
                this.userRepository.Entities.Add(user);
            }

            user.FirstName = payload.GivenName;
            user.LastName = payload.FamilyName;
            user.Email = payload.Email;
            user.AvatarUrl = payload.Picture;

            await this.userRepository.SaveChanges();

            return user;
        }

        private AuthenticatedUser Authenticate(User user)
        {
            var authenticatedUser = this.mapper.Map<AuthenticatedUser>(user);
            authenticatedUser.BearerToken = this.bearerTokenFactory.ProduceFor(user);
            return authenticatedUser;
        }
    }
}