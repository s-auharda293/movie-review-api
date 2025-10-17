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
using PuppeteerSharp;
using PuppeteerSharp.Media;
using System.Data;
using System.IO;
using System.Text;
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
                var htmlContent = @"
                <!DOCTYPE html>
                <html>
                <head>
                    <meta charset='UTF-8'>
                    <title>Actor Ratings</title>
                    <style>
                        body {
                            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
                            margin: 20px;
                        }

                        h1 {
                            text-align: center;
                            color: #333;
                        }

                        table {
                            width: 100%;
                            border-collapse: collapse;
                            margin-top: 20px;
                            box-shadow: 0 2px 8px rgba(0,0,0,0.1);
                        }

                        th, td {
                            padding: 12px 15px;
                            text-align: left;
                        }

                        th {
                            background-color: #fafafa;
                            border-bottom: 2px solid #f0f0f0;
                            color: #555;
                            font-weight: 600;
                        }

                        tr:nth-child(even) {
                            background-color: #f9f9f9;
                        }

                        tr:hover {
                            background-color: #f1f5f9;
                        }

                        td {
                            border-bottom: 1px solid #f0f0f0;
                        }
                    </style>
                </head>
                <body>
                    <h1>Actor Ratings Report</h1>
                    <table>
                        <thead>
                            <tr>
                                <th>S.N</th>
                                <th>Actor Name</th>
                                <th>Average Rating</th>
                            </tr>
                        </thead>
                        <tbody>";

                for (int i = 0; i < actorRatings.Count; i++)
                {
                    var actor = actorRatings[i];
                    htmlContent += $@"
                    <tr>
                        <td>{i + 1}</td>
                        <td>{actor.ActorName}</td>
                        <td>{actor.AverageRating:F1}</td>
                    </tr>";
                }


                // launch headless browser
                var browserFetcher = new BrowserFetcher();
                await browserFetcher.DownloadAsync();
                await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true });

                await using var page = await browser.NewPageAsync();

                await page.SetContentAsync(htmlContent.ToString());

                var pdfStream = await page.PdfStreamAsync(new PdfOptions
                {
                    Format = PaperFormat.A4,
                    MarginOptions = new MarginOptions { Top = "25px", Bottom = "25px" }
                });

                await pdfStream.CopyToAsync(stream);

                fileName = $"ActorRatings_{DateTime.Now:yyyyMMddHHmmss}.pdf";
                contentType = "application/pdf";
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
