using System.ComponentModel.DataAnnotations;

namespace RistrettoSistemas.Models
{
    public class Empresa
    {
        public int EmpresaId { get; set; }

        [Required]
        [MaxLength(100)]
        public string NomeEmpresarial { get; set; }

        [Required]
        [Phone]
        [StringLength(14)]
        public string Telefone { get; set; }

        [Required]
        [Url]
        public string Url { get; set; }

        public ICollection<Funcionario> Funcionarios { get; set; }
    }

}
