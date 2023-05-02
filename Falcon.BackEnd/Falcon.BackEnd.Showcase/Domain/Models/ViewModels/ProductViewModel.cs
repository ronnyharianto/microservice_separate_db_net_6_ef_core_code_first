using Falcon.Libraries.DataAccess.Domain;

namespace Falcon.BackEnd.Showcases.Domain.Models.ViewModels
{
    public class ProductViewModel : ViewModelBase
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Remark { get; set; }
    }
}
