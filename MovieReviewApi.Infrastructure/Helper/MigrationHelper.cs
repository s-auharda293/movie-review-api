using Microsoft.EntityFrameworkCore.Migrations;

namespace MovieReviewApi.Infrastructure.Helper
{
    public class MigrationHelper
    {
        public static void RunSqlScript(MigrationBuilder migrationBuilder, string resourceName, bool suppressTransaction=false)
        {
            var assembly = typeof(MigrationHelper).Assembly;
            using var stream = assembly.GetManifestResourceStream(resourceName);
            using var reader = new StreamReader(stream!);
            var sql = reader.ReadToEnd();
            migrationBuilder.Sql(sql, suppressTransaction:suppressTransaction);
        }
    }
}
