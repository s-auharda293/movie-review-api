using MediatR;
using MovieReviewApi.Application.Commands.Actor;
using MovieReviewApi.Application.DTOs;
using MovieReviewApi.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieReviewApi.Application.Handlers.Actor
{
    public class GetActorReportQueryHandler : IRequestHandler<ExportActorsWithRatingsCommand, Result<ActorReportResultDto>>
    {
        private readonly IActorReportService _reportService;

        public GetActorReportQueryHandler(IActorReportService reportService)
        {
            _reportService = reportService;
        }

        public async Task<Result<ActorReportResultDto>> Handle(ExportActorsWithRatingsCommand request, CancellationToken cancellationToken)
        {
            return await _reportService.GenerateReportAsync(request.dto.ActorIds,request.dto.Format.ToLower());
        }
    }
}
