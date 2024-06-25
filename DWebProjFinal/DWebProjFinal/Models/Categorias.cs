using System.ComponentModel.DataAnnotations;

namespace DWebProjFinal.Models
{
    public class Categorias
    {
        public Categorias()
        {
            ListaPaginas = new HashSet<Paginas>();
        }

        /// <summary>
        /// Id da categoria
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Nome da categoria
        /// </summary>
        public string Nome { get; set;}


        /// <summary>
        /// Lista de páginas que a categoria contém (Páginas que estão catalogadas com a categoria)
        /// </summary>
        public ICollection<Paginas> ListaPaginas { get; set; }
    }
}
