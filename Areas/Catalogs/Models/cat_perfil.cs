using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace ease_admin_cloud.Areas.Catalogs.Models
{
    public partial class cat_perfil
    {
        [Key]
        
        public int id_perfil { get; set; }

        [Display(Name = "Descripción")]
        [DataType(DataType.Text)]
        
        public string perfil_desc { get; set; } = string.Empty;

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