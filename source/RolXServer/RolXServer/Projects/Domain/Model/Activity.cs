// -----------------------------------------------------------------------
// <copyright file="Activity.cs" company="Christian Ewald">
// Copyright (c) Christian Ewald. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace RolXServer.Projects.Domain.Model;

/// <summary>
/// An activity in a subproject.
/// </summary>
public sealed class Activity
{
    private static readonly TimeSpan BudgetMin = TimeSpan.FromMinutes(1);
    private static readonly TimeSpan PlannedMin = TimeSpan.FromMinutes(1);

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
    /// Gets or sets the start date.
    /// </summary>
    public DateOnly StartDate { get; set; }

    /// <summary>
    /// Gets or sets the ended date.
    /// </summary>
    /// <remarks>
    /// This marks the first day this activity is closed for adding records.
    /// </remarks>
    public DateOnly? EndedDate { get; set; }

    /// <summary>
    /// Gets or sets the time budget.
    /// </summary>
    public TimeSpan? Budget { get; set; }

    /// <summary>
    /// Gets or sets the planned time.
    /// </summary>
    public TimeSpan? Planned { get; set; }

    /// <summary>
    /// Gets or sets the actual time worked on this activity.
    /// </summary>
    public TimeSpan? Actual { get; set; }

    /// <summary>
    /// Gets a value indicating whether the actual time is larger than the time budget.
    /// </summary>
    public bool IsOverBudget
    {
        get
        {
            return this.Budget is not null && this.Actual > this.Budget;
        }
    }

    /// <summary>
    /// Gets or sets the subproject.
    /// </summary>
    public Subproject? Subproject { get; set; }

    /// <summary>
    /// Gets or sets the billability.
    /// </summary>
    public DataAccess.Billability Billability { get; set; } = null!;

    /// <summary>
    /// Gets the full-qualified number of the specified activity.
    /// </summary>
    /// <returns>The full-qualified number.</returns>
    public string FullNumber
    {
        get
        {
            if (this.Subproject is null)
            {
                throw new ArgumentNullException("The activities subproject must not be null", this.GetType().Name);
            }

            return $"{this.Subproject.FullNumber}.{this.Number:D2}";
        }
    }

    /// <summary>
    /// Gets the full-qualified name of the specified activity.
    /// </summary>
    /// <returns>The full-qualified name.</returns>
    public string FullName
    {
        get
        {
            if (this.Subproject is null)
            {
                throw new ArgumentNullException("The activities subproject must not be null", this.GetType().Name);
            }

            return $"{this.Subproject.AllNames} - {this.Name} ({this.FullNumber})";
        }
    }

    /// <summary>
    /// Gets the numbered name of the specified activity.
    /// </summary>
    public string NumberedName
    {
        get { return $"{this.Name} ({this.Number:D2})"; }
    }

    /// <summary>
    /// Sanitizes the specified activity.
    /// </summary>
    internal void Sanitize()
    {
        this.ClearEmptyBudget();
        this.ClearEmptyPlanned();
    }

    private void ClearEmptyBudget()
    {
        if ((this.Budget ?? default) < BudgetMin)
        {
            this.Budget = null;
        }
    }

    private void ClearEmptyPlanned()
    {
        if ((this.Planned ?? default) < PlannedMin)
        {
            this.Planned = null;
        }
    }
}
