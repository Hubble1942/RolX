﻿// -----------------------------------------------------------------------
// <copyright file="ServiceCollectionExtensions.cs" company="Christian Ewald">
// Copyright (c) Christian Ewald. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace RolXServer.Auth
{
    /// <summary>
    /// Extension methods for <see cref="IServiceCollection"/> instances.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the services of the Auth package.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns>
        /// The service collection.
        /// </returns>
        public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
        {
            var settingsSection = configuration.GetSection("Auth");
            services.Configure<Settings>(configuration.GetSection("Auth"));

            services.AddScoped<Domain.ISignInService, Domain.Detail.SignInService>();
            services.AddSingleton<Domain.Detail.BearerTokenFactory>();

            var settings = settingsSection.Get<Settings>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; // TODO: check if required
            }).AddJwtBearer(options => new Domain.Detail.BearerTokenFactory(settingsSection.Get<Settings>()).Configure(options));

            return services;
        }
    }
}