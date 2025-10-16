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

        public async Task<Result<byte[]>> GenerateReportAsync(ReportFormat format)
        {
            var actorRatings = await GetActorRatingsAsync();

            if (String.Equals(format.ToString(), ReportFormat.Excel.ToString(), StringComparison.OrdinalIgnoreCase))
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

                using var stream = new MemoryStream();
                workbook.SaveAs(stream);
                return Result<byte[]>.Success(stream.ToArray());
            }
            else if (String.Equals(format.ToString(), ReportFormat.Pdf.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                // PDF generation logic will go here later

                return Result<byte[]>.Failure(ActorErrors.InvalidFileFormat);

            }
            else {
                return Result<byte[]>.Failure(ActorErrors.InvalidFileFormat);
            }
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
