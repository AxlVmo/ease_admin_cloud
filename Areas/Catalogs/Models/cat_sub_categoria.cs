using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ease_admin_cloud.Areas.Catalogs.Models
{
    public class cat_sub_categoria
    {
        [Key]
        public int id_sub_categoria { get; set; }

        [Display(Name = "Descripción")]
        [DataType(DataType.Text)]
        public string sub_categoria_desc { get; set; } = string.Empty;

        [Display(Name = "Id Categoria")]
        [Required(ErrorMessage = "Campo Requrido")]
        public int id_categoria { get; set; }

        [NotMapped]
        [Display(Name = "Categoria")]
        [DataType(DataType.Text)]
        public string categoria_desc { get; set; } = string.Empty;

        [Display(Name = "Usuario Modifico")]
        public Guid id_usuario_modifico { get; set; }

        [NotMapped]
        [Display(Name = "Usuario Modifico")]
        public string usuario_modifico_desc { get; set; } = string.Empty;

        [Column("fecha_registro")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha Registro")]
        public DateTime fecha_registro { get; set; }

        [Display(Name = "Fecha de Actualización")]
        [DataType(DataType.Date)]
        public DateTime fecha_actualizacion { get; set; }

        [Display(Name = "Estatus")]
        public int id_estatus_registro { get; set; }
    }
}
