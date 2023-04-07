using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ease_admin_cloud.Models
{
    public class FileModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "Nombre")]
        public string? Name { get; set; }

        [Display(Name = "Tipo de Archivo")]
        public string? FileType { get; set; }

        [Display(Name = "Extension")]
        public string? Extension { get; set; }

        [Display(Name = "Descripción")]
        public string? Description { get; set; }

        [Display(Name = "Usuario")]
        public string? UploadedBy { get; set; }

        [Display(Name = "Fecha")]
        public DateTime? CreatedOn { get; set; }
    }
}