using MediatR;
using MovieReviewApi.Application.Interfaces;
using MovieReviewApi.Application.Queries.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieReviewApi.Application.Handlers.Actor
{
    public class GetActorReportQueryHandler : IRequestHandler<GetActorReportQuery, byte[]>
    {
        private readonly IActorReportService _reportService;

        public GetActorReportQueryHandler(IActorReportService reportService)
        {
            _reportService = reportService;
        }

        public async Task<byte[]> Handle(GetActorReportQuery request, CancellationToken cancellationToken)
        {
            return await _reportService.GenerateReportAsync(request.Format);
        }
    }
}
