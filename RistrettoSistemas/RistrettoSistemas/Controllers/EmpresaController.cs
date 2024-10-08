using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RistrettoSistemas.Data;
using RistrettoSistemas.Models;
using System.Text.RegularExpressions;

namespace RistrettoSistemas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmpresaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EmpresaController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Empresa>>> GetEmpresas()
        {
            return await _context.Empresas.Include(e => e.Funcionarios).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Empresa>> GetEmpresa(int id)
        {
            var empresa = await _context.Empresas.Include(e => e.Funcionarios)
                                                 .FirstOrDefaultAsync(e => e.EmpresaId == id);

            if (empresa == null)
            {
                return NotFound();
            }

            return empresa;
        }

        [HttpPost]
        public IActionResult CriarEmpresa([FromBody] Empresa empresa)
        {
            if (!Regex.IsMatch(empresa.Telefone, @"^\(\d{2}\) \d{8,9}$"))
            {
                return BadRequest("Telefone inválido. O formato deve ser (xx) xxxxxxxx ou (xx) xxxxxxxxx.");
            }

            var empresaExistente = _context.Empresas
                .FirstOrDefault(e => e.NomeEmpresarial == empresa.NomeEmpresarial);

            if (empresaExistente != null)
            {
                return BadRequest("Já existe uma empresa cadastrada com este nome empresarial.");
            }

            try
            {
                _context.Empresas.Add(empresa);
                _context.SaveChanges();

                return Ok($"A empresa {empresa.NomeEmpresarial} foi criada com sucesso e possui o ID: {empresa.EmpresaId}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao criar a empresa: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public IActionResult AtualizarEmpresa(int id, [FromBody] Empresa empresaAtualizada)
        {
            try
            {
                if (!Regex.IsMatch(empresaAtualizada.Telefone, @"^\(\d{2}\) \d{8,9}$"))
                {
                    return BadRequest("Telefone inválido. O formato deve ser (xx) xxxxxxxx ou (xx) xxxxxxxxx.");
                }

                var empresaExistente = _context.Empresas.Find(id);
                if (empresaExistente == null)
                {
                    return NotFound("Empresa não encontrada.");
                }

                empresaExistente.NomeEmpresarial = empresaAtualizada.NomeEmpresarial;
                empresaExistente.Telefone = empresaAtualizada.Telefone;
                empresaExistente.Url = empresaAtualizada.Url;

                _context.SaveChanges();

                return Ok($"Empresa atualizada com sucesso!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocorreu um erro ao atualizar a empresa: {ex.Message}");
            }
           
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmpresa(int id)
        {
            try
            {
                var empresa = await _context.Empresas.FindAsync(id);
                if (empresa == null)
                {
                    return NotFound();
                }

                _context.Empresas.Remove(empresa);
                await _context.SaveChangesAsync();

                return Ok($"A empresa {empresa.NomeEmpresarial} foi excluída com sucesso!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocorreu um erro ao deletar a empresa: {ex.Message}");
            }
        }
    }

}
