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

        [Display(Name = "Telemóvel")]
        [StringLength(19)]
        [RegularExpression("([+]|00)?[0-9]{6,17}", ErrorMessage = "o {0} só pode conter digitos. No mínimo 6.")]
        public string Telemovel { get; set; }

        [Display(Name = "Data de Nascimento")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateOnly dataNasc { get; set; }

        public string? Biografia { get; set; }

        public ICollection<Paginas> ListaPaginas { get; set; }

        /// <summary>
        /// atributo para funcionar como FK entre a tabela dos Utilizadores
        /// e a tabela da Autenticação
        /// </summary>
        public string UserID { get; set; }



    }
}
