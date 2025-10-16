using Bogus;
using ClosedXML.Excel;
using Dapper;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MovieReviewApi.Application.DTOs;
using MovieReviewApi.Application.Interfaces;
using MovieReviewApi.Application.Queries.Actor;
using MovieReviewApi.Domain.Common.Actors;
using System.Data;
using System.IO;
using System.Threading;

namespace MovieReviewApi.Infrastructure.Services
{
    public class ActorReportService : IActorReportService
    {

        private readonly IDbConnectionFactory _connection;

        public ActorReportService(IConfiguration configuration, IDbConnectionFactory connection)
        {
            _connection = connection;
        }

        public async Task<Result<ActorReportResultDto>> GenerateReportAsync(String format)
        {
            string fileName = String.Empty;
            string contentType = String.Empty;
            using var stream = new MemoryStream();

            var actorRatings = await GetActorRatingsAsync();

            if (String.Equals(format.ToString(), "excel", StringComparison.OrdinalIgnoreCase))
            {
                using var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("Actor Ratings");

                worksheet.Cell(1, 1).Value = "Actor Name";
                worksheet.Cell(1, 2).Value = "Average Movie Rating";

                for (int i = 0; i < actorRatings.Count; i++)
                {
                    worksheet.Cell(i + 2, 1).Value = actorRatings[i].ActorName;
                    worksheet.Cell(i + 2, 2).Value = actorRatings[i].AverageRating;
                }

                workbook.SaveAs(stream);

                fileName = $"ActorRatings_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
                contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            }
            else if (String.Equals(format.ToString(), "pdf", StringComparison.OrdinalIgnoreCase))
            {
                // PDF generation logic will go here later

                return Result<ActorReportResultDto>.Failure(ActorErrors.InvalidFileFormat);

            }
            else {
                return Result<ActorReportResultDto>.Failure(ActorErrors.InvalidFileFormat);
            }

            stream.Position = 0;

            var resultDto = new ActorReportResultDto
            {
                Content = stream.ToArray(),
                FileName = fileName,
                ContentType = contentType
            };

            return Result<ActorReportResultDto>.Success(resultDto);
        }

        private async Task<List<ActorRatingDto>> GetActorRatingsAsync()
        {
            var connection = await _connection.CreateConnectionAsync(CancellationToken.None);

            var data = (await connection.QueryAsync<ActorRatingDto>(
                "GetActorsRating",
                commandType: CommandType.StoredProcedure
            )).ToList();

            return data;
        }
    }
}
