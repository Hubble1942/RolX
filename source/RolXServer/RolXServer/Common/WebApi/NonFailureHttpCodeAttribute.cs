// -----------------------------------------------------------------------
// <copyright file="NonFailureHttpCodeAttribute.cs" company="Christian Ewald">
// Copyright (c) Christian Ewald. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace RolXServer.Common.WebApi;

/// <summary>
/// Specifies additional HTTP status codes that should not be treated as failures.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class NonFailureHttpCodeAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NonFailureHttpCodeAttribute"/> class.
    /// </summary>
    /// <param name="statusCode">The allowed status code.</param>
    public NonFailureHttpCodeAttribute(int statusCode) => this.StatusCode = statusCode;

    /// <summary>
    /// Gets the allowed status code.
    /// </summary>
    public int StatusCode { get; }
}
