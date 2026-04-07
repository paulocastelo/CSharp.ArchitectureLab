using System.ComponentModel.DataAnnotations;

namespace Pagination.Sample.Common;

public sealed record PaginationParams(
    [Range(1, int.MaxValue)] int Page = 1,
    [Range(1, 100)] int PageSize = 10);
