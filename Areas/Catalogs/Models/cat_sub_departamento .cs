using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace ease_admin_cloud.Areas.Catalogs.Models
{
    public partial class cat_sub_departamento
    {
        [Key]
        [Display(Name = "Id Sub Departamento")]
        public int id_sub_departamento { get; set; }

        [Display(Name = "Descripción")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Campo Requrido")]
        public string sub_departamento_desc { get; set; } = string.Empty;

        [Display(Name = "Id Departamento")]
        [Required(ErrorMessage = "Campo Requrido")]
        public int id_departamento { get; set; }

        [NotMapped]
        [Display(Name = "Departamento Descripción")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Campo Requrido")]
        public string departamento_desc { get; set; } = string.Empty;

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
        [Required(ErrorMessage = "Campo Requrido")]
        public int id_estatus_registro { get; set; }
    }
}
