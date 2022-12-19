// -----------------------------------------------------------------------
// <copyright file="Subproject.cs" company="Christian Ewald">
// Copyright (c) Christian Ewald. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using RolXServer.Common.Util;
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

    /// <summary>
    /// Gets the budget time.
    /// </summary>
    public TimeSpan Budget => this.Activities
        .Where(a => a.Budget.HasValue)
        .Sum(a => a.Budget!.Value);

    /// <summary>
    /// Gets the planned time.
    /// </summary>
    public TimeSpan Planned => this.Activities
        .Where(a => a.Planned.HasValue)
        .Sum(a => a.Planned!.Value);

    /// <summary>
    /// Gets the actual time.
    /// </summary>
    public TimeSpan Actual => this.Activities
        .Where(a => a.Actual.HasValue)
        .Sum(a => a.Actual!.Value);

    /// <summary>
    /// Gets the ratio of actual time to budgeted time.
    /// </summary>
    public double? BudgetConsumedFraction
        => this.Activities.Any(a => a.Actual.HasValue) && this.Activities.Any(a => a.Budget.HasValue)
        ? this.Actual.TotalSeconds / this.Budget.TotalSeconds
        : null;

    /// <summary>
    /// Gets the ratio of actual time to planned time.
    /// </summary>
    public double? PlannedConsumedFraction
        => this.Activities.Any(a => a.Actual.HasValue) && this.Activities.Any(a => a.Planned.HasValue)
        ? this.Actual.TotalSeconds / this.Planned.TotalSeconds
        : null;

    /// <summary>
    /// Gets a value indicating whether the actual time is larger than the time budget.
    /// </summary>
    public bool IsOverBudget
        => this.Activities.Any(a => a.Budget.HasValue)
        && this.Actual > this.Budget;

    /// <summary>
    /// Gets a value indicating whether the actual time is larger than the planned time.
    /// </summary>
    public bool IsOverPlanned
        => this.Activities.Any(a => a.Planned.HasValue)
        && this.Actual > this.Planned;
}
