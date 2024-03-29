﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace ease_admin_cloud.Areas.Company.Models
{
    public partial class tbl_filial
    {
        [Key]
        public Guid id_filial { get; set; }

        [Display(Name = "Tipo de Filial")]
        [Required(ErrorMessage = "Campo Requerido")]
        public int id_tipo_filial { get; set; }

        [Display(Name = "Nombre de Filial")]
        [Required(ErrorMessage = "Campo Requerido")]
        public string nombre_filial { get; set; }

        [Display(Name = "Calle")]
        public string calle { get; set; } = string.Empty;

        [Display(Name = "Codigo Postal")]
        public string codigo_postal { get; set; } = string.Empty;

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

        [Display(Name = "Telefono")]
        public string telefono { get; set; } = string.Empty;

        [Display(Name = "Corporativo")]
        [Required(ErrorMessage = "Campo Requerido")]
        public Guid id_corporativo { get; set; }

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
