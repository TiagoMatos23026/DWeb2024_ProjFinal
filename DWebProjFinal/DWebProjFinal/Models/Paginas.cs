using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DWebProjFinal.Models
{
    public class Paginas
    {
        public Paginas()
        {
            ListaCategorias = new HashSet<Categorias>();
        }

        /// <summary>
        /// Id da página, é automaticamente criado
        /// </summary>
        [Key]
        public int Id { get; set; }


        /// <summary>
        /// Titulo da página
        /// </summary>
        [Required(ErrorMessage = "A página precisa de um título")]
        [StringLength(100)]
        [Display(Name = "Título")]
        public string Name { get; set; }

        /// <summary>
        /// Descrição da página, preenchimento facultativo
        /// </summary>

        [Display(Name = "Descrição")]
        public string? Descricao { get; set; }

        /// <summary>
        /// Conteúdo da página, ou seja, o tutorial
        /// </summary>
        [Display(Name = "Conteúdo")]
        public string Conteudo { get; set; }

        /// <summary>
        /// Dificuldade, de 1-5 da página
        /// </summary>
        [Required(ErrorMessage = "O Tutorial precisa de uma dificuldade")]
        public int Dificuldade { get; set; }

        /// <summary>
        /// Imagem da página. Caso nenhuma seja provida pelo utente, é usada uma imagem default
        /// </summary>
        [Display(Name = "Thumbnail")]
        [StringLength(50)]
        public string? Thumbnail { get; set; }

        /// <summary>
        /// Id do utente que criou a página
        /// </summary>
        [ForeignKey(nameof(Utente))]
        [Display(Name = "Chave Forasteira do Utente")]
        public int? UtenteFK { get; set; }
        /// <summary>
        /// Utente que criou a página
        /// </summary>
        public Utentes? Utente { get; set; }

        /// <summary>
        /// Lista de categorias que a página contém
        /// </summary>
        public ICollection<Categorias> ListaCategorias { get; set; }
    }
}
