using ease_admin_cloud.Areas.Company.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

#nullable disable

namespace ease_admin_cloud.Areas.Address.Models
{
    public partial class tbl_direccion
    {
        [Key]
        public Guid id_direccion { get; set; }

        [Display(Name = "Tipo de Direccion")]
        [Required(ErrorMessage = "Campo Requerido")]
        public string id_tipo_direccion { get; set; } = string.Empty;

        [Display(Name = "Calle")]
        [Required(ErrorMessage = "Campo Requerido")]
        public string calle { get; set; } = string.Empty;

        [Display(Name = "Codigo Postal")]
        [Required(ErrorMessage = "Campo Requerido")]
        public string codigo_postal { get; set; } = string.Empty;

        [Display(Name = "Colonia")]
        [Required(ErrorMessage = "Campo Requerido")]
        public string id_colonia { get; set; } = string.Empty;

        [Display(Name = "Colonia")]
        public string colonia { get; set; } = string.Empty;

        [Display(Name = "Localidad / Municipio")]
        public string localidad_municipio { get; set; } = string.Empty;

        [Display(Name = "Ciudad")]
        public string ciudad { get; set; } = string.Empty;

        [Display(Name = "Estado")]
        public string estado { get; set; } = string.Empty;

        [Display(Name = "Correo Electronico")]
        [Required(ErrorMessage = "Campo Requerido")]
        public string correo_electronico { get; set; } = string.Empty;

        [Display(Name = "Teléfono Movil (10 Digitos)")]
        public string telefono_movil { get; set; } = string.Empty;

        [Display(Name = "Teléfono Fijo (10 Digitos)")]
        public string telefono_fijo { get; set; } = string.Empty;

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
        public tbl_empresa id_empresa { get; set; }
    }
}
