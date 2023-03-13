using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace ease_admin_cloud.Areas.Catalogs.Models
{
    public partial class cat_categoria
    {
        [Key]
        
        public int IdCategoria { get; set; }

        [Display(Name = "Categoria")]
        [DataType(DataType.Text)]
        
        public string CategoriaDesc { get; set; } = string.Empty;

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