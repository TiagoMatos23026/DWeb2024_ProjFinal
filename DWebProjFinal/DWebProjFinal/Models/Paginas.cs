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


        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O Tutorial precisa de um título")]
        [StringLength(100)]
        [Display(Name = "Título")]
        public string Name { get; set; }

        [Display(Name = "Descrição")]
        public string? Descricao { get; set; } //Preenchimento facultativo

        [Display(Name = "Conteudo")]
        public string Conteudo { get; set; }

        [Required(ErrorMessage = "O Tutorial precisa de uma dificuldade")]
        public int Dificuldade { get; set; }

        [Display(Name = "Thumbnail")] //Define o nome a aparecer no ecrã
        [StringLength(50)]
        public string? Thumbnail { get; set; } //Preenchimento facultativo

        [ForeignKey(nameof(Utente))]
        [Display(Name = "Chave Forasteira do Utente")]
        public int UtenteFK { get; set; }
        public Utentes Utente { get; set; }

        public ICollection<Categorias> ListaCategorias { get; set; }
    }
}
