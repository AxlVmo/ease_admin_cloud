using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ease_admin_cloud.Areas.Catalogs.Models
{
    public class cat_marca
    {
        [Key]
        public int id_marca { get; set; }

        [Display(Name = "Descripción")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Campo Requrido")]
        public string marca_desc { get; set; } = string.Empty;

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
