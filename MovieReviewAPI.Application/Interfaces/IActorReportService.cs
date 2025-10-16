using MovieReviewApi.Application.DTOs;
using MovieReviewApi.Application.Queries.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieReviewApi.Application.Interfaces
{
    public interface IActorReportService
    {
        Task<Result<byte[]>> GenerateReportAsync(ReportFormat format);
    }
}
