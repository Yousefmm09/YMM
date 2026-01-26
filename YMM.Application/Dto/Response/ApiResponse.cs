using System;
using System.Collections.Generic;
using System.Text;

namespace YMM.Application.Dto.Response
{
    public record ApiResponse<T>(
     bool Success,
     string Message,
     T? Data = default,
     object? Errors = null,
     string? TraceId = null
      );
}
