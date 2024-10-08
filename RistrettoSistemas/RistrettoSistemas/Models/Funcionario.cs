using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RistrettoSistemas.Models
{
    public class Funcionario
    {
        public int FuncionarioId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo CPF é obrigatório.")]
        [MaxLength(11)]
        public string CPF { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Cargo { get; set; }

        [Required]
        public DateTime DataNascimento { get; set; }

        [Required]
        [MaxLength(50)]
        public string Login { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória.")]
        [MinLength(8)]
        public string Senha { get; set; }

        [Required]
        public bool Situacao { get; set; }

        public int EmpresaId { get; set; }

        [JsonIgnore]
        public Empresa? Empresa { get; set; }
    }
}
