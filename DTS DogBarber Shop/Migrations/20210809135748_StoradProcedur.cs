using Microsoft.EntityFrameworkCore.Migrations;

namespace DTS_DogBarber_Shop.Migrations
{
    public partial class StoradProcedur : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string StoredProcedureSQL = @"CREATE OR ALTER PROCEDURE dbo.SPGetCustomerQueue
                                            AS
                                                BEGIN 
                                                    SELECT * FROM Queue
                                                END";
            migrationBuilder.Sql(StoredProcedureSQL);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
