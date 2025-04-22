using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FoodCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "T_Menues",
                columns: table => new
                {
                    IdMenu = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Numero = table.Column<int>(type: "int", nullable: false),
                    Precio = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ValidFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ValidTo = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_Menues", x => x.IdMenu);
                });

            migrationBuilder.CreateTable(
                name: "T_Comidas",
                columns: table => new
                {
                    IdComida = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Comida = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CodComida = table.Column<int>(type: "int", nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_Comidas", x => x.IdComida);
                    table.ForeignKey(
                        name: "FK_T_Comidas_FoodCategories_CodComida",
                        column: x => x.CodComida,
                        principalTable: "FoodCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "T_Usuarios",
                columns: table => new
                {
                    IdAdmin = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Usuario = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Contrasena = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IdRol = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastLogin = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_Usuarios", x => x.IdAdmin);
                    table.ForeignKey(
                        name: "FK_T_Usuarios_Roles_IdRol",
                        column: x => x.IdRol,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "T_DetalleMenu",
                columns: table => new
                {
                    IdDetalleMenu = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdMenu = table.Column<int>(type: "int", nullable: false),
                    IdComida = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_DetalleMenu", x => x.IdDetalleMenu);
                    table.ForeignKey(
                        name: "FK_T_DetalleMenu_T_Comidas_IdComida",
                        column: x => x.IdComida,
                        principalTable: "T_Comidas",
                        principalColumn: "IdComida",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_T_DetalleMenu_T_Menues_IdMenu",
                        column: x => x.IdMenu,
                        principalTable: "T_Menues",
                        principalColumn: "IdMenu",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "T_Clientes",
                columns: table => new
                {
                    IdCliente = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreApellido = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Direccion = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_Clientes", x => x.IdCliente);
                    table.ForeignKey(
                        name: "FK_T_Clientes_T_Usuarios_UserId",
                        column: x => x.UserId,
                        principalTable: "T_Usuarios",
                        principalColumn: "IdAdmin");
                });

            migrationBuilder.CreateTable(
                name: "T_Pedidos",
                columns: table => new
                {
                    IdPedido = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdCliente = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DeliveryAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentMethod = table.Column<int>(type: "int", nullable: false),
                    IsPaid = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_Pedidos", x => x.IdPedido);
                    table.ForeignKey(
                        name: "FK_T_Pedidos_T_Clientes_IdCliente",
                        column: x => x.IdCliente,
                        principalTable: "T_Clientes",
                        principalColumn: "IdCliente",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "T_DetallePedidos",
                columns: table => new
                {
                    IdDetallePedido = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdPedido = table.Column<int>(type: "int", nullable: false),
                    IdMenu = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Subtotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_DetallePedidos", x => x.IdDetallePedido);
                    table.ForeignKey(
                        name: "FK_T_DetallePedidos_T_Menues_IdMenu",
                        column: x => x.IdMenu,
                        principalTable: "T_Menues",
                        principalColumn: "IdMenu",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_T_DetallePedidos_T_Pedidos_IdPedido",
                        column: x => x.IdPedido,
                        principalTable: "T_Pedidos",
                        principalColumn: "IdPedido",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_T_Clientes_UserId",
                table: "T_Clientes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_T_Comidas_CodComida",
                table: "T_Comidas",
                column: "CodComida");

            migrationBuilder.CreateIndex(
                name: "IX_T_DetalleMenu_IdComida",
                table: "T_DetalleMenu",
                column: "IdComida");

            migrationBuilder.CreateIndex(
                name: "IX_T_DetalleMenu_IdMenu",
                table: "T_DetalleMenu",
                column: "IdMenu");

            migrationBuilder.CreateIndex(
                name: "IX_T_DetallePedidos_IdMenu",
                table: "T_DetallePedidos",
                column: "IdMenu");

            migrationBuilder.CreateIndex(
                name: "IX_T_DetallePedidos_IdPedido",
                table: "T_DetallePedidos",
                column: "IdPedido");

            migrationBuilder.CreateIndex(
                name: "IX_T_Pedidos_IdCliente",
                table: "T_Pedidos",
                column: "IdCliente");

            migrationBuilder.CreateIndex(
                name: "IX_T_Usuarios_IdRol",
                table: "T_Usuarios",
                column: "IdRol");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "T_DetalleMenu");

            migrationBuilder.DropTable(
                name: "T_DetallePedidos");

            migrationBuilder.DropTable(
                name: "T_Comidas");

            migrationBuilder.DropTable(
                name: "T_Menues");

            migrationBuilder.DropTable(
                name: "T_Pedidos");

            migrationBuilder.DropTable(
                name: "FoodCategories");

            migrationBuilder.DropTable(
                name: "T_Clientes");

            migrationBuilder.DropTable(
                name: "T_Usuarios");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
