using ease_admin_cloud.Areas.Address.Models;

namespace ease_admin_cloud.Areas.Company.Models.ViewModels
{
    public class EmpresaViewModel
    {
        public tbl_empresa? tbl_empresas { get; set; }
        public List<tbl_direccion>? tbl_direcciones { get; set; }

    }
}
