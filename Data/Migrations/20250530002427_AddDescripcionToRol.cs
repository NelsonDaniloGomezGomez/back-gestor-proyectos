using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Backend.GestorProyectos.Migrations
{
    /// <inheritdoc />
    public partial class AddDescripcionToRol : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RolId",
                table: "Usuarios",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Descripcion", "Nombre" },
                values: new object[,]
                {
                    { 1, "Control total del sistema: gestión de usuarios, roles y configuración general.", "Administrador" },
                    { 2, "Responsable de crear, asignar y supervisar proyectos y tareas.", "Jefe de Proyecto" },
                    { 3, "Realiza tareas asignadas, actualiza estados y reporta avances.", "Desarrollador" },
                    { 4, "Encargado de probar funcionalidades, reportar errores y validar entregables.", "Tester/QA" },
                    { 5, "Usuario externo con permisos limitados para ver avances y reportes.", "Cliente" },
                    { 6, "Acceso muy limitado o solo lectura a ciertas partes del sistema.", "Invitado" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_RolId",
                table: "Usuarios",
                column: "RolId");

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_Roles_RolId",
                table: "Usuarios",
                column: "RolId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_Roles_RolId",
                table: "Usuarios");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_Usuarios_RolId",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "RolId",
                table: "Usuarios");
        }
    }
}
