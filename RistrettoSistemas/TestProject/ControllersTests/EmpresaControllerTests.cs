using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RistrettoSistemas.Controllers;
using RistrettoSistemas.Models;
using RistrettoSistemas.Data;
using Xunit;
using Moq;

namespace TestProject.ControllersTests
{
    public class EmpresaControllerTests
    {

        [Fact]
        public void CriaEmpresa()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            using (var context = new AppDbContext(options))
            {
                var controller = new EmpresaController(context);

                var novaEmpresa = new Empresa
                {
                    NomeEmpresarial = "Nova Empresa",
                    Telefone = "(11) 987654321",
                    Url = "http://novaempresa.com"
                };

                var result = controller.CriarEmpresa(novaEmpresa);

                Assert.IsType<OkObjectResult>(result);
                var okResult = result as OkObjectResult;
                Assert.Equal($"A empresa {novaEmpresa.NomeEmpresarial} foi criada com sucesso e possui o ID: {novaEmpresa.EmpresaId}", okResult.Value);
            }
        }


        [Fact]
        public void CriarEmpresaTelefoneErrado()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
                .Options;

            using (var context = new AppDbContext(options))
            {
                var controller = new EmpresaController(context);

                var novaEmpresa = new Empresa
                {
                    NomeEmpresarial = "Empresa Inválida",
                    Telefone = "12345678",
                    Url = "http://empresainvalida.com"
                };

                var result = controller.CriarEmpresa(novaEmpresa);

                Assert.IsType<BadRequestObjectResult>(result);
                var badRequestResult = result as BadRequestObjectResult;
                Assert.Equal("Telefone inválido. O formato deve ser (xx) xxxxxxxx ou (xx) xxxxxxxxx.", badRequestResult.Value);
            }
        }

        [Fact]
        public void AtualizaEmpresa()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new AppDbContext(options))
            {
                context.Empresas.Add(new Empresa
                {
                    EmpresaId = 1,
                    NomeEmpresarial = "Empresa Teste",
                    Telefone = "(11) 12345678",
                    Url = "http://empresateste.com"
                });
                context.SaveChanges();

                var controller = new EmpresaController(context);

                var empresaAtualizada = new Empresa
                {
                    NomeEmpresarial = "Empresa Atualizada",
                    Telefone = "(11) 987654321",
                    Url = "http://empresateste.com"
                };

                var result = controller.AtualizarEmpresa(1, empresaAtualizada);

                Assert.IsType<OkObjectResult>(result);
                var okResult = result as OkObjectResult;
                Assert.Equal("Empresa atualizada com sucesso!", okResult.Value);
            }
        }

        [Fact]
        public async Task BuscaEmpresaPorId()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
                .Options;

            using (var context = new AppDbContext(options))
            {
                var empresa = new Empresa
                {
                    EmpresaId = 1,
                    NomeEmpresarial = "Empresa Teste",
                    Telefone = "(11) 12345678",
                    Url = "http://empresateste.com"
                };

                context.Empresas.Add(empresa);
                context.SaveChanges();

                var controller = new EmpresaController(context);

                var result = await controller.GetEmpresa(1);

                Assert.IsType<ActionResult<Empresa>>(result);
                var empresaResult = result.Value;
                Assert.Equal(1, empresaResult.EmpresaId);
                Assert.Equal("Empresa Teste", empresaResult.NomeEmpresarial);
            }
        }

        [Fact]
        public async Task BuscaEmpresaIdErrado()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            using (var context = new AppDbContext(options))
            {
                var controller = new EmpresaController(context);

                var result = await controller.GetEmpresa(100);

                Assert.IsType<NotFoundResult>(result.Result);
            }
        }

        [Fact]
        public async Task ExcluiEmpresa()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            using (var context = new AppDbContext(options))
            {
                var empresa = new Empresa
                {
                    EmpresaId = 1,
                    NomeEmpresarial = "Empresa a ser deletada",
                    Telefone = "(11) 12345678",
                    Url = "http://empresaadeletar.com"
                };

                context.Empresas.Add(empresa);
                context.SaveChanges();

                var controller = new EmpresaController(context);

                var result = await controller.DeleteEmpresa(1);

                Assert.IsType<OkObjectResult>(result);
                var okResult = result as OkObjectResult;
                Assert.Equal($"A empresa {empresa.NomeEmpresarial} foi excluída com sucesso!", okResult.Value);
            }
        }

    }
}
