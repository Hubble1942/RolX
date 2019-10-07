﻿// -----------------------------------------------------------------------
// <copyright file="ServiceCollectionExtensions.cs" company="Christian Ewald">
// Copyright (c) Christian Ewald. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RolXServer.Common.DataAccess;

namespace RolXServer.Database
{
    /// <summary>
    /// Extension methods for <see cref="IServiceCollection"/> instances.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the database services.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns>The service collection.</returns>
        public static IServiceCollection AddDatabase(this IServiceCollection services)
        {
            services.AddDbContext<RolXContext>(options => options.UseSqlite("Data Source=rolx.sqlite"));

            services.AddScoped<IRepository<Account.DataAccess.Customer>, RolXRepository>();
            services.AddScoped<IRepository<Account.DataAccess.Project>, RolXRepository>();
            services.AddScoped<IRepository<Auth.DataAccess.User>, RolXRepository>();
            services.AddScoped<IRepository<WorkRecord.DataAccess.UserSetting>, RolXRepository>();

            return services;
        }
    }
}
