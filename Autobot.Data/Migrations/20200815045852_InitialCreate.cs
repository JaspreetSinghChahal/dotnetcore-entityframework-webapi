using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Autobot.Data.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Brand",
                columns: table => new
                {
                    BrandId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BrandName = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brand", x => x.BrandId);
                });

            migrationBuilder.CreateTable(
                name: "PromotionMessage",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PromotionText = table.Column<string>(nullable: false),
                    PromotionFileName = table.Column<string>(nullable: false),
                    LastUpdatedOn = table.Column<DateTime>(nullable: false, defaultValueSql: "GetDate()"),
                    LastUpdatedBy = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromotionMessage", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RefreshToken",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Token = table.Column<string>(nullable: false),
                    Expires = table.Column<DateTime>(nullable: true),
                    UserId = table.Column<string>(nullable: false),
                    RemoteIpAddress = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshToken", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TermsAndConditions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "0, 1"),
                    TermsAndConditionsText = table.Column<string>(nullable: false),
                    LastUpdatedOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TermsAndConditions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserPointReset",
                columns: table => new
                {
                    UserPointResetId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "0, 1"),
                    UserId = table.Column<string>(nullable: false),
                    PointsReset = table.Column<double>(nullable: false),
                    ResetDateTime = table.Column<DateTime>(nullable: false),
                    LastUpdatedByUserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPointReset", x => x.UserPointResetId);
                });

            migrationBuilder.CreateTable(
                name: "PromoCodeBatch",
                columns: table => new
                {
                    BatchId = table.Column<Guid>(nullable: false),
                    BatchName = table.Column<string>(maxLength: 50, nullable: false),
                    BrandId = table.Column<int>(nullable: false),
                    NoOfPromoCodes = table.Column<int>(nullable: false),
                    LoyaltyPoints = table.Column<float>(nullable: false),
                    ExpirationDateTime = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedByUserId = table.Column<string>(nullable: true),
                    LastUpdatedOn = table.Column<DateTime>(nullable: false, defaultValueSql: "GetDate()"),
                    LastUpdatedBy = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromoCodeBatch", x => x.BatchId);
                    table.CheckConstraint("ck_NoOfPromoCodes", "NoOfPromoCodes > 0");
                    table.ForeignKey(
                        name: "FK_PromoCodeBatch_Brand_BrandId",
                        column: x => x.BrandId,
                        principalTable: "Brand",
                        principalColumn: "BrandId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PromoCodes",
                columns: table => new
                {
                    PromoCodeNumber = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1000, 1"),
                    BatchId = table.Column<Guid>(nullable: false),
                    PromoCodeId = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    LastUpdatedOn = table.Column<DateTime>(nullable: false, defaultValueSql: "GetDate()"),
                    LastUpdatedBy = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromoCodes", x => x.PromoCodeNumber);
                    table.ForeignKey(
                        name: "FK_PromoCodes_PromoCodeBatch_BatchId",
                        column: x => x.BatchId,
                        principalTable: "PromoCodeBatch",
                        principalColumn: "BatchId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserScans",
                columns: table => new
                {
                    UserScanId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "0, 1"),
                    UserId = table.Column<string>(nullable: false),
                    PromoCodeNumber = table.Column<long>(nullable: false),
                    ScannedDateTime = table.Column<DateTime>(nullable: false),
                    IsSuccess = table.Column<bool>(nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserScans", x => x.UserScanId);
                    table.ForeignKey(
                        name: "FK_UserScans_PromoCodes_PromoCodeNumber",
                        column: x => x.PromoCodeNumber,
                        principalTable: "PromoCodes",
                        principalColumn: "PromoCodeNumber",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PromoCodeBatch_BrandId",
                table: "PromoCodeBatch",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_PromoCodes_BatchId",
                table: "PromoCodes",
                column: "BatchId");

            migrationBuilder.CreateIndex(
                name: "IX_UserScans_PromoCodeNumber",
                table: "UserScans",
                column: "PromoCodeNumber");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PromotionMessage");

            migrationBuilder.DropTable(
                name: "RefreshToken");

            migrationBuilder.DropTable(
                name: "TermsAndConditions");

            migrationBuilder.DropTable(
                name: "UserPointReset");

            migrationBuilder.DropTable(
                name: "UserScans");

            migrationBuilder.DropTable(
                name: "PromoCodes");

            migrationBuilder.DropTable(
                name: "PromoCodeBatch");

            migrationBuilder.DropTable(
                name: "Brand");
        }
    }
}
