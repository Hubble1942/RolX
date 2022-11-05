using RolXServer.Common.Util;

namespace RolXServer.Reports.WebApi.Resource;

/// <summary>
/// Defines a filter for the creation of a <see cref="Report"/>.
/// </summary>
public record ReportFilter(
    DateRange DateRange,
    int? ProjectNumber,
    int? SubProjectNumber,
    IEnumerable<Guid> UserIds,
    string CommentFilter);
