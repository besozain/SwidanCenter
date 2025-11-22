using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Presistence.Migrations
{
    /// <inheritdoc />
    public partial class SPstats : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = @"CREATE OR ALTER PROCEDURE GetDashboardStats
                @Today date = NULL
            AS
            BEGIN
                SET NOCOUNT ON;
            
                -- لو مش مبعوت، خليه تاريخ النهارده
                IF @Today IS NULL SET @Today = CONVERT(date, GETDATE());
            
                DECLARE @Start datetime2(0) = CAST(@Today AS datetime2(0));
                DECLARE @End datetime2(0) = DATEADD(day, 1, @Start);
            
                SELECT
                    (SELECT COUNT(DISTINCT v.MedicalExaminationId)
                     FROM Visits v
                     WHERE v.[Date] >= @Start AND v.[Date] < @End) AS CurrentDayPatients,
                    (SELECT COUNT(*)
                     FROM Operations o
                     WHERE o.OperationDate >= @Start AND o.OperationDate < @End) AS CurrentDayOperations,
                    (SELECT COUNT(*)
                     FROM Patients p
                     WHERE p.CreatedAt >= @Start AND p.CreatedAt < @End) AS NewPatientsToday,
                    (SELECT COUNT(*) FROM Patients) AS TotalPatients;
            END;
            GO";
            migrationBuilder.Sql(sql); 
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetDashboardStats;");
        }
    }
}
