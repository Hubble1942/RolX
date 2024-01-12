// -----------------------------------------------------------------------
// <copyright file="RecordEntry.cs" company="Christian Ewald">
// Copyright (c) Christian Ewald. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using RolXServer.Projects.Domain.Model;

namespace RolXServer.Records.Domain.Model;

/// <summary>
/// An entry in a <see cref="Record"/>.
/// </summary>
public sealed class RecordEntry
{
    /// <summary>
    /// Gets or sets the activity identifier.
    /// </summary>
    public int ActivityId { get; set; }

    /// <summary>
    /// Gets or sets the full number of the activity.
    /// </summary>
    public Activity? Activity { get; set; }

    /// <summary>
    /// Gets or sets the duration.
    /// </summary>
    public TimeSpan Duration { get; set; }

    /// <summary>
    /// Gets or sets the begin time.
    /// </summary>
    public TimeOnly? Begin { get; set; }

    /// <summary>
    /// Gets or sets the pause duration.
    /// </summary>
    public TimeSpan? Pause { get; set; }

    /// <summary>
    /// Gets or sets the comment.
    /// </summary>
    public string Comment { get; set; } = string.Empty;
}
