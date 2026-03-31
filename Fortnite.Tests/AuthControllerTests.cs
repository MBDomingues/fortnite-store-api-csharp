using Xunit;
using Moq;
using Microsoft.EntityFrameworkCore;
using Fortinite_Project.Web.Controllers;
using Fortinite_Project.Web.Data;
using Fortinite_Project.Web.Models;
using Fortinite_Project.Web.DTOs;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;

namespace Fortnite.Tests
{
    public class AuthControllerTests
    {
        private AppDbContext GetInMemoryDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new AppDbContext(options);
        }

        [Fact]
        public async Task Registrar_DeveRetornarStatus201_QuandoDadosSaoValidos()
        {
            var context = GetInMemoryDatabaseContext();
            var controller = new AuthController(context);
            var novoUsuario = new RegistroUsuarioDTO 
            { 
                Nome = "Mateus", 
                Email = "mateus@teste.com", 
                Senha = "123" 
            };

            var result = await controller.Registrar(novoUsuario);

            var objectResult = result.Result as ObjectResult;
            objectResult.StatusCode.Should().Be(201);
            
            var response = objectResult.Value as BaseResponse_DTO<UsuarioRespostaDTO>;
            response.Message.Should().Be("Usuário cadastrado com sucesso.");
            response.Data.Email.Should().Be("mateus@teste.com");
        }

        [Fact]
        public async Task Login_DeveRetornarUnauthorized_QuandoSenhaIncorreta()
        {
            var context = GetInMemoryDatabaseContext();
            context.Usuarios.Add(new Usuario { Email = "user@test.com", Senha = "correta", Nome = "User" });
            await context.SaveChangesAsync();

            var controller = new AuthController(context);
            var loginInvalido = new LoginDTO { Email = "user@test.com", Senha = "errada" };

            var result = await controller.Login(loginInvalido);

            var unauthorizedResult = result.Result as UnauthorizedObjectResult;
            unauthorizedResult.StatusCode.Should().Be(401);
        }
    }
}