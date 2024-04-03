using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DWebProjFinal.Models
{
    public class Paginas
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Descricao { get; set; }

        public int Dificuldade { get; set; }

        public string[] Media { get; set; }

        [ForeignKey(nameof(Utentes))]
        public int UtenteFK { get; set; }

        public ICollection<Categorias> ListaCategorias { get; set; }
    }
}
