using ease_admin_cloud.Areas.Address.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace ease_admin_cloud.Areas.Company.Models
{
    public partial class tbl_empresa
    {
        [Key]
        public Guid id_empresa { get; set; }

        [Display(Name = "Nombre Empresa")]
        [Required(ErrorMessage = "Campo Requerido")]
        public string nombre_empresa { get; set; }

        [Display(Name = "RFC")]
        public string rfc { get; set; }

        [Display(Name = "Giro Comercial")]
        public string giro_comercial { get; set; } = string.Empty;

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

        public ICollection<tbl_direccion> tbl_direcciones { get; set; }

    }
}
