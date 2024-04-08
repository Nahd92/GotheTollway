using Microsoft.EntityFrameworkCore.Migrations;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Collections.Generic;

#nullable disable

namespace GotheTollway.Database.Migrations
{
    /// <inheritdoc />
    public partial class adddummydataindatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Inserting dummy data into TollFees table
            // This is only for demonostration purposes
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
