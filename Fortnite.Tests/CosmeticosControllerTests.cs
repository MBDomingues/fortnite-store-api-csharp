using Xunit;
using Microsoft.EntityFrameworkCore;
using Fortinite_Project.Web.Controllers;
using Fortinite_Project.Web.Data;
using Fortinite_Project.Web.Models;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using Fortinite_Project.Web.DTOs;

namespace Fortnite.Tests
{
    public class CosmeticosControllerTests
    {
        private AppDbContext GetInMemoryDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new AppDbContext(options);
        }

        [Fact]
        public async Task GetLojaDiaria_DeveRetornarApenasItensAVenda()
        {
            // Arrange
            var context = GetInMemoryDatabaseContext();
            context.Cosmeticos.AddRange(new List<Cosmetico>
            {
                new Cosmetico 
                { 
                    Id = "1", 
                    Nome = "Skin A", 
                    isForSale = true, 
                    Raridade = "Lendária",
                    Descricao = "Descrição Teste A",
                    Tipo = "Outfit",               
                    UrlImagem = "http://imagem.com" 
                },
                new Cosmetico 
                { 
                    Id = "2", 
                    Nome = "Skin B", 
                    isForSale = false, 
                    Raridade = "Comum",
                    Descricao = "Descrição Teste B",
                    Tipo = "Pickaxe",               
                    UrlImagem = "http://imagem.com"
                }
            });
            await context.SaveChangesAsync();

            var controller = new CosmeticosController(context);

            var result = await controller.GetLojaDiaria();

            var okResult = result.Result as OkObjectResult;
            var response = okResult.Value as BaseResponse_DTO<IEnumerable<CosmeticoApi_DTO>>;
            
            response.Data.Should().HaveCount(1);
            response.Data.First().Nome.Should().Be("Skin A");
        }
    }
}