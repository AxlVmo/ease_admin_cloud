using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ease_admin_cloud.Areas.Address.Models

{
    public class cat_codigo_postal
    {
        [Key]
        [Column("id_codigo_postal")]
        [Display(Name = "ID IdCodigo Postal")]
         [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_codigo_postal { get; set; } 

        public string d_codigo { get; set; } = string.Empty; 

        public string d_asenta { get; set; } = string.Empty;

        public string d_tipoAsenta { get; set; } = string.Empty;

        public string d_mnpio { get; set; } = string.Empty;

        public string d_estado { get; set; } = string.Empty;

        public string d_ciudad { get; set; } = string.Empty;

        public string d_cp { get; set; } = string.Empty;

        public string c_estado { get; set; } = string.Empty;

        public string c_oficina { get; set; } = string.Empty;

        public string c_cp { get; set; } = string.Empty;

        public string c_tipoAsenta { get; set; } = string.Empty;

        public string c_mnpio { get; set; } = string.Empty;

        public string id_asenta_cpcons { get; set; } = string.Empty;

        public string d_zona { get; set; } = string.Empty;

        public string c_cveCiudad { get; set; } = string.Empty;


    }
}
