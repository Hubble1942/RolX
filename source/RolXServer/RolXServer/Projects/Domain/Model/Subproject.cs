// -----------------------------------------------------------------------
// <copyright file="Subproject.cs" company="Christian Ewald">
// Copyright (c) Christian Ewald. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using RolXServer.Users.DataAccess;

namespace RolXServer.Projects.Domain.Model;

/// <summary>
/// A subproject we are working on.
/// </summary>
public sealed class Subproject
{
    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the number.
    /// </summary>
    public int Number { get; set; }

    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the project number.
    /// </summary>
    public int ProjectNumber { get; set; }

    /// <summary>
    /// Gets the full-qualified number of the specified subproject.
    /// </summary>
    public string FullNumber
    {
        get { return $"#{this.ProjectNumber:D4}.{this.Number:D3}"; }
    }

    /// <summary>
    /// Gets or sets the project name.
    /// </summary>
    public string ProjectName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the name of the customer.
    /// </summary>
    public string CustomerName { get; set; } = string.Empty;

    /// <summary>
    /// Gets all names of the specified subproject.
    /// </summary>
    public string AllNames
    {
        get { return $"{this.CustomerName} - {this.ProjectName} - {this.Name}"; }
    }

    /// <summary>
    /// Gets the full-qualified name of the specified subproject.
    /// </summary>
    public string FullName
    {
        get { return $"{this.AllNames} ({this.FullNumber})"; }
    }

    /// <summary>
    /// Gets or sets the activities.
    /// </summary>
    public List<Activity> Activities { get; set; } = new List<Activity>();

    /// <summary>
    /// Gets a value indicating whether this instance is closed.
    /// </summary>
    public bool IsClosed
    {
        get
        {
            return this.Activities
                .All(activity => activity.EndedDate != null && activity.EndedDate <= DateOnly.FromDateTime(DateTime.Now));
        }
    }

    /// <summary>
    /// Gets or sets the manager identifier.
    /// </summary>
    public Guid? ManagerId { get; set; }

    /// <summary>
    /// Gets or sets the manager.
    /// </summary>
    public User? Manager { get; set; }
}
