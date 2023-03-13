using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace ease_admin_cloud.Areas.Catalogs.Models
{
    public partial class cat_area
    {
        [Key]
        
        public int id_area { get; set; }

        [Display(Name = "Descripción")]
        [DataType(DataType.Text)]
        
        public string area_desc { get; set; } = string.Empty;

        [Display(Name = "Usuario Modifico")]
        public Guid id_usuario_modifico { get; set; }

        [Column("fecha_registro")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha Registro")]
        public DateTime fecha_registro { get; set; }

        [Display(Name = "Estatus")]
        
        public int id_estatus_registro { get; set; }
    }
}