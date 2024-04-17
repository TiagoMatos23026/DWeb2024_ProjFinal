using System.ComponentModel.DataAnnotations;

namespace DWebProjFinal.Models
{
    public class Utentes
    {
        public Utentes()
        {
            ListaPaginas = new HashSet<Paginas>();
        }


        [Key]
        public int Id { get; set; }

        [Display(Name = "Nome de Utilizador")]
        [Required(ErrorMessage = "O Nome de Utilizador é de preenchimento obrigatório")]
        [StringLength(100)]
        public string Nome { get; set; }

        [Display(Name = "Imagem de Perfil")]
        [StringLength(50)]
        public string? Icon { get; set; }

        [Required(ErrorMessage = "O Email é de preenchimento obrigatório")]
        [StringLength(100)]
        public string Email { get; set; }

        [Display(Name = "Número de Telemóvel (+351)")]
        [Required(ErrorMessage = "O Número de Telemóvel é de preenchimento obrigatório")]
        [StringLength(100)]
        public string Telemovel { get; set; }

        [Display(Name = "Data de Nascimento (dd-mm-aaaa)")]
        [Required(ErrorMessage = "A Data de Nascimento é de preenchimento obrigatório")]
        public DateOnly dataNasc { get; set; }

        public string? Biografia { get; set; }

        public ICollection<Paginas> ListaPaginas { get; set; }





    }
}
