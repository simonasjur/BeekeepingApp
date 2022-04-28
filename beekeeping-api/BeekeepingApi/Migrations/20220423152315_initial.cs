using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BeekeepingApi.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Farms",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Farms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Manufacturers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Manufacturers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    PasswordKey = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DefaultFarmId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Apiaries",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    FarmId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Apiaries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Apiaries_Farms_FarmId",
                        column: x => x.FarmId,
                        principalTable: "Farms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BeeFamilies",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsNucleus = table.Column<bool>(type: "bit", nullable: false),
                    Origin = table.Column<int>(type: "int", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false),
                    FarmId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BeeFamilies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BeeFamilies_Farms_FarmId",
                        column: x => x.FarmId,
                        principalTable: "Farms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Beehives",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<int>(type: "int", nullable: false),
                    IsEmpty = table.Column<bool>(type: "bit", nullable: false),
                    No = table.Column<int>(type: "int", nullable: true),
                    AcquireDay = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Color = table.Column<int>(type: "int", nullable: true),
                    MaxNestCombs = table.Column<int>(type: "int", nullable: true),
                    NestCombs = table.Column<int>(type: "int", nullable: true),
                    MaxHoneyCombsSupers = table.Column<int>(type: "int", nullable: true),
                    FarmId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Beehives", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Beehives_Farms_FarmId",
                        column: x => x.FarmId,
                        principalTable: "Farms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Foods",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FarmId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Foods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Foods_Farms_FarmId",
                        column: x => x.FarmId,
                        principalTable: "Farms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Invitations",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FarmId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invitations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invitations_Farms_FarmId",
                        column: x => x.FarmId,
                        principalTable: "Farms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Queens",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Breed = table.Column<int>(type: "int", nullable: false),
                    HatchingDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MarkingColor = table.Column<int>(type: "int", nullable: true),
                    IsFertilized = table.Column<bool>(type: "bit", nullable: false),
                    BroodStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    State = table.Column<int>(type: "int", nullable: false),
                    FarmId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Queens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Queens_Farms_FarmId",
                        column: x => x.FarmId,
                        principalTable: "Farms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FarmWorkers",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    FarmId = table.Column<long>(type: "bigint", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Role = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FarmWorkers", x => new { x.UserId, x.FarmId });
                    table.ForeignKey(
                        name: "FK_FarmWorkers_Farms_FarmId",
                        column: x => x.FarmId,
                        principalTable: "Farms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FarmWorkers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApiaryBeeFamilies",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ArriveDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DepartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApiaryId = table.Column<long>(type: "bigint", nullable: false),
                    BeeFamilyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiaryBeeFamilies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApiaryBeeFamilies_Apiaries_ApiaryId",
                        column: x => x.ApiaryId,
                        principalTable: "Apiaries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApiaryBeeFamilies_BeeFamilies_BeeFamilyId",
                        column: x => x.BeeFamilyId,
                        principalTable: "BeeFamilies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Harvests",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Product = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Quantity = table.Column<double>(type: "float", nullable: false),
                    FarmId = table.Column<long>(type: "bigint", nullable: false),
                    ApiaryId = table.Column<long>(type: "bigint", nullable: true),
                    BeeFamilyId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Harvests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Harvests_Apiaries_ApiaryId",
                        column: x => x.ApiaryId,
                        principalTable: "Apiaries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Harvests_BeeFamilies_BeeFamilyId",
                        column: x => x.BeeFamilyId,
                        principalTable: "BeeFamilies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Harvests_Farms_FarmId",
                        column: x => x.FarmId,
                        principalTable: "Farms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NestExpansions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FrameType = table.Column<int>(type: "int", nullable: false),
                    CombSheets = table.Column<int>(type: "int", nullable: false),
                    Combs = table.Column<int>(type: "int", nullable: false),
                    BeefamilyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NestExpansions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NestExpansions_BeeFamilies_BeefamilyId",
                        column: x => x.BeefamilyId,
                        principalTable: "BeeFamilies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NestReductions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StayedCombs = table.Column<int>(type: "int", nullable: false),
                    StayedHoney = table.Column<double>(type: "float", nullable: false),
                    StayedBroodCombs = table.Column<int>(type: "int", nullable: false),
                    RequiredFoodForWinter = table.Column<double>(type: "float", nullable: false),
                    BeefamilyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NestReductions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NestReductions_BeeFamilies_BeefamilyId",
                        column: x => x.BeefamilyId,
                        principalTable: "BeeFamilies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TodoItems",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DueDate = table.Column<DateTime>(type: "date", nullable: false),
                    IsComplete = table.Column<bool>(type: "bit", nullable: false),
                    FarmId = table.Column<long>(type: "bigint", nullable: false),
                    ApiaryId = table.Column<long>(type: "bigint", nullable: true),
                    BeeFamilyId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TodoItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TodoItems_Apiaries_ApiaryId",
                        column: x => x.ApiaryId,
                        principalTable: "Apiaries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TodoItems_BeeFamilies_BeeFamilyId",
                        column: x => x.BeeFamilyId,
                        principalTable: "BeeFamilies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TodoItems_Farms_FarmId",
                        column: x => x.FarmId,
                        principalTable: "Farms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BeehiveBeeFamilies",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ArriveDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DepartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BeehiveId = table.Column<long>(type: "bigint", nullable: false),
                    BeeFamilyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BeehiveBeeFamilies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BeehiveBeeFamilies_BeeFamilies_BeeFamilyId",
                        column: x => x.BeeFamilyId,
                        principalTable: "BeeFamilies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BeehiveBeeFamilies_Beehives_BeehiveId",
                        column: x => x.BeehiveId,
                        principalTable: "Beehives",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BeehiveComponents",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Position = table.Column<int>(type: "int", nullable: true),
                    InstallationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BeehiveId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BeehiveComponents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BeehiveComponents_Beehives_BeehiveId",
                        column: x => x.BeehiveId,
                        principalTable: "Beehives",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Feedings",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Quantity = table.Column<double>(type: "float", nullable: false),
                    FoodId = table.Column<long>(type: "bigint", nullable: false),
                    BeeFamilyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feedings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Feedings_BeeFamilies_BeeFamilyId",
                        column: x => x.BeeFamilyId,
                        principalTable: "BeeFamilies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Feedings_Foods_FoodId",
                        column: x => x.FoodId,
                        principalTable: "Foods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BeeFamilyQueens",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InsertDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TakeOutDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    QueenId = table.Column<long>(type: "bigint", nullable: false),
                    BeeFamilyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BeeFamilyQueens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BeeFamilyQueens_BeeFamilies_BeeFamilyId",
                        column: x => x.BeeFamilyId,
                        principalTable: "BeeFamilies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BeeFamilyQueens_Queens_QueenId",
                        column: x => x.QueenId,
                        principalTable: "Queens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QueensRaisings",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LarvaCount = table.Column<int>(type: "int", nullable: false),
                    DevelopmentPlace = table.Column<int>(type: "int", nullable: false),
                    CappedCellCount = table.Column<int>(type: "int", nullable: false),
                    QueensCount = table.Column<int>(type: "int", nullable: false),
                    MotherId = table.Column<long>(type: "bigint", nullable: false),
                    BeefamilyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QueensRaisings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QueensRaisings_BeeFamilies_BeefamilyId",
                        column: x => x.BeefamilyId,
                        principalTable: "BeeFamilies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QueensRaisings_Queens_MotherId",
                        column: x => x.MotherId,
                        principalTable: "Queens",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Apiaries_FarmId",
                table: "Apiaries",
                column: "FarmId");

            migrationBuilder.CreateIndex(
                name: "IX_ApiaryBeeFamilies_ApiaryId",
                table: "ApiaryBeeFamilies",
                column: "ApiaryId");

            migrationBuilder.CreateIndex(
                name: "IX_ApiaryBeeFamilies_BeeFamilyId",
                table: "ApiaryBeeFamilies",
                column: "BeeFamilyId");

            migrationBuilder.CreateIndex(
                name: "IX_BeeFamilies_FarmId",
                table: "BeeFamilies",
                column: "FarmId");

            migrationBuilder.CreateIndex(
                name: "IX_BeeFamilyQueens_BeeFamilyId",
                table: "BeeFamilyQueens",
                column: "BeeFamilyId");

            migrationBuilder.CreateIndex(
                name: "IX_BeeFamilyQueens_QueenId",
                table: "BeeFamilyQueens",
                column: "QueenId");

            migrationBuilder.CreateIndex(
                name: "IX_BeehiveBeeFamilies_BeeFamilyId",
                table: "BeehiveBeeFamilies",
                column: "BeeFamilyId");

            migrationBuilder.CreateIndex(
                name: "IX_BeehiveBeeFamilies_BeehiveId",
                table: "BeehiveBeeFamilies",
                column: "BeehiveId");

            migrationBuilder.CreateIndex(
                name: "IX_BeehiveComponents_BeehiveId",
                table: "BeehiveComponents",
                column: "BeehiveId");

            migrationBuilder.CreateIndex(
                name: "IX_Beehives_FarmId",
                table: "Beehives",
                column: "FarmId");

            migrationBuilder.CreateIndex(
                name: "IX_FarmWorkers_FarmId",
                table: "FarmWorkers",
                column: "FarmId");

            migrationBuilder.CreateIndex(
                name: "IX_Feedings_BeeFamilyId",
                table: "Feedings",
                column: "BeeFamilyId");

            migrationBuilder.CreateIndex(
                name: "IX_Feedings_FoodId",
                table: "Feedings",
                column: "FoodId");

            migrationBuilder.CreateIndex(
                name: "IX_Foods_FarmId",
                table: "Foods",
                column: "FarmId");

            migrationBuilder.CreateIndex(
                name: "IX_Harvests_ApiaryId",
                table: "Harvests",
                column: "ApiaryId");

            migrationBuilder.CreateIndex(
                name: "IX_Harvests_BeeFamilyId",
                table: "Harvests",
                column: "BeeFamilyId");

            migrationBuilder.CreateIndex(
                name: "IX_Harvests_FarmId",
                table: "Harvests",
                column: "FarmId");

            migrationBuilder.CreateIndex(
                name: "IX_Invitations_FarmId",
                table: "Invitations",
                column: "FarmId");

            migrationBuilder.CreateIndex(
                name: "IX_NestExpansions_BeefamilyId",
                table: "NestExpansions",
                column: "BeefamilyId");

            migrationBuilder.CreateIndex(
                name: "IX_NestReductions_BeefamilyId",
                table: "NestReductions",
                column: "BeefamilyId");

            migrationBuilder.CreateIndex(
                name: "IX_Queens_FarmId",
                table: "Queens",
                column: "FarmId");

            migrationBuilder.CreateIndex(
                name: "IX_QueensRaisings_BeefamilyId",
                table: "QueensRaisings",
                column: "BeefamilyId");

            migrationBuilder.CreateIndex(
                name: "IX_QueensRaisings_MotherId",
                table: "QueensRaisings",
                column: "MotherId");

            migrationBuilder.CreateIndex(
                name: "IX_TodoItems_ApiaryId",
                table: "TodoItems",
                column: "ApiaryId");

            migrationBuilder.CreateIndex(
                name: "IX_TodoItems_BeeFamilyId",
                table: "TodoItems",
                column: "BeeFamilyId");

            migrationBuilder.CreateIndex(
                name: "IX_TodoItems_FarmId",
                table: "TodoItems",
                column: "FarmId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApiaryBeeFamilies");

            migrationBuilder.DropTable(
                name: "BeeFamilyQueens");

            migrationBuilder.DropTable(
                name: "BeehiveBeeFamilies");

            migrationBuilder.DropTable(
                name: "BeehiveComponents");

            migrationBuilder.DropTable(
                name: "FarmWorkers");

            migrationBuilder.DropTable(
                name: "Feedings");

            migrationBuilder.DropTable(
                name: "Harvests");

            migrationBuilder.DropTable(
                name: "Invitations");

            migrationBuilder.DropTable(
                name: "Manufacturers");

            migrationBuilder.DropTable(
                name: "NestExpansions");

            migrationBuilder.DropTable(
                name: "NestReductions");

            migrationBuilder.DropTable(
                name: "QueensRaisings");

            migrationBuilder.DropTable(
                name: "TodoItems");

            migrationBuilder.DropTable(
                name: "Beehives");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Foods");

            migrationBuilder.DropTable(
                name: "Queens");

            migrationBuilder.DropTable(
                name: "Apiaries");

            migrationBuilder.DropTable(
                name: "BeeFamilies");

            migrationBuilder.DropTable(
                name: "Farms");
        }
    }
}
