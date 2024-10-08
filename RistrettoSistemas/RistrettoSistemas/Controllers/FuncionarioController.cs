using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RistrettoSistemas.Data;
using RistrettoSistemas.Models;
using System.Security.Cryptography;

namespace RistrettoSistemas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FuncionarioController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FuncionarioController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Funcionario>>> GetFuncionarios()
        {
            return await _context.Funcionarios.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Funcionario>> GetFuncionario(int id)
        {
            var funcionario = await _context.Funcionarios.FindAsync(id);

            if (funcionario == null)
            {
                return NotFound();
            }

            return funcionario;
        }

        [HttpPost]
        public IActionResult AdicionarFuncionario([FromBody] Funcionario funcionario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var empresa = _context.Empresas.FirstOrDefault(e => e.EmpresaId == funcionario.EmpresaId);
            if (empresa == null)
            {
                return BadRequest("A empresa associada ao funcionário não foi encontrada.");
            }

            var funcionarioExistente = _context.Funcionarios.FirstOrDefault(f => f.CPF == funcionario.CPF);
            if (funcionarioExistente != null)
            {
                return BadRequest("Já existe um funcionário com este CPF.");
            }

            if (funcionario.Situacao == false)
            {
                return BadRequest("Não é possível adicionar um funcionário com 'situacao' = false.");
            }

            funcionario.Senha = HashSenha(funcionario.Senha);

            funcionario.Situacao = true;

            try
            {
                _context.Funcionarios.Add(funcionario);
                _context.SaveChanges();

                return Ok($"O funcionário {funcionario.Nome} foi adicionado com sucesso e possui o ID: {funcionario.FuncionarioId}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocorreu um erro ao adicionar o funcionário: {ex.Message}");
            }
        }

        private string HashSenha(string senha)
        {
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: senha,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return hashed;
        }


        [HttpPut("{id}")]
        public IActionResult AlterarFuncionario(int id, [FromBody] Funcionario funcionarioAtualizado)
        {
            try
            {
                var funcionario = _context.Funcionarios.Find(id);
                if (funcionario == null)
                {
                    return NotFound();
                }

                funcionario.Situacao = funcionarioAtualizado.Situacao;
                _context.SaveChanges();
                return Ok("Funcionário atualizado com sucesso!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocorreu um erro ao atualizar o funcionário: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFuncionario(int id)
        {

            var funcionario = await _context.Funcionarios.FindAsync(id);
            if (funcionario == null)
            {
                return NotFound();
            }

            _context.Funcionarios.Remove(funcionario);
            await _context.SaveChangesAsync();

            return Ok($"O funcionário {funcionario.Nome} foi excluído com sucesso!");

        }
    }

}
