using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GotheTollway.Database.Migrations
{
    /// <inheritdoc />
    public partial class Adddummydata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Inserting dummy data into TollFees table
            // This is only for demonstration purposes
            // We should not insert data like this in production.
            migrationBuilder.Sql(@"
                    INSERT INTO TollFee (IsActive, StartTime, EndTime, Fee) VALUES
                    (1, '06:00', '06:29', 8),
                    (1, '06:30', '06:59', 13),
                    (1, '07:00', '07:59', 18),
                    (1, '08:00', '08:29', 13),
                    (1, '08:30', '14:59', 8),
                    (1, '15:00', '15:29', 13),
                    (1, '15:30', '16:59', 18),
                    (1, '17:00', '17:59', 13),
                    (1, '18:00', '18:29', 8),
                    (1, '18:30', '23:59', 0),
                    (1, '00:00', '05:59', 0);
                ");

            //Inserting dummy data into TollExemptions table
            // This is only for demonstration purposes
            migrationBuilder.Sql(@"
            INSERT INTO TollExemptions(Description, IsActive, ExemptionStartPeriod, ExemptionEndPeriod, ExemptedDayOfWeek, ExemptionStartTime, ExemptionEndTime)
                 VALUES ('Exemption for Saturdays', 1, NULL, NULL, 6, '00:00:00', '23:59:59'),
                       ('Exemption for Sunday', 1, NULL, NULL, 0, '00:00:00', '23:59:59'),
                       ('Exemption for holidays', 1, NULL, NULL, NULL, NULL, NULL),
                       ('Exemption for days before holidays', 1, NULL, NULL, NULL, NULL, NULL),
                       ('Exemption for July', 1, '2024-07-01', '2024-07-31', NULL, NULL, NULL);
                        ");

            //Inserting dummy data into ExemptedVehicleTypes table
            // This is only for demonstration purposes
            // We should not insert data like this in production.
            migrationBuilder.Sql(@"INSERT INTO ExemptedVehicleTypes(VehicleType)
            VALUES (0),   
                   (1),  
                   (2),  
                   (3),  
                   (4),  
                   (5),
                   (6),
                   (7);");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
