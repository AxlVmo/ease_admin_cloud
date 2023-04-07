using System;
using System.ComponentModel.DataAnnotations;

namespace ease_admin_cloud.Areas.Users.Models
{
    public class tbl_usuario_control
    {
        [Key]
        public Guid id_usuario_control { get; set; }

        [Display(Name = "Nombres")]
        public string? nombres { get; set; }

        [Display(Name = "Apellido Paterno")]
        public string? apellido_paterno { get; set; }

        [Display(Name = "ApellidoMaterno")]
        public string? apellido_materno { get; set; }

        [Display(Name = "Nombre Usuario")]
        public string nombre_usuario { get; set; } = string.Empty;

        [Display(Name = "Area")]
        public int id_area { get; set; }

        [Display(Name = "Genero")]
        public int? id_genero { get; set; }

        [Display(Name = "Perfil")]
        public int id_perfil { get; set; }

        [Display(Name = "Rol")]
        public int id_rol { get; set; }
        public bool terminos_uso { get; set; }

        [DataType(DataType.Date)]
        public DateTime? fecha_nacimiento { get; set; }

        [Display(Name = "Correo de Acceso")]
        public string correo_acceso { get; set; } = string.Empty;

        [Display(Name = "Rol")]
        public byte[]? profile_picture { get; set; }

        [Display(Name = "Usuario Modifico")]
        public Guid? id_usuario_modifico { get; set; }

        [Display(Name = "Fecha de Registro")]
        [DataType(DataType.Date)]
        public DateTime fecha_registro { get; set; }

        [Display(Name = "Fecha de Actualización")]
        [DataType(DataType.Date)]
        public DateTime fecha_actualizacion { get; set; }

        [Display(Name = "Estatus")]
        public int id_estatus_registro { get; set; }
    }
}
