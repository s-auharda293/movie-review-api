using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieReviewApi.Application.Queries.Actor
{
    public enum ReportFormat
    {
        Excel,
        Pdf
    }

    public record GetActorReportQuery(ReportFormat Format): IRequest<byte[]>;
}
