using Falcon.Libraries.DataAccess.Domain;

namespace Falcon.BackEnd.Showcases.Domain.Models.ViewModels
{
    public class ProductVariantViewModel : ViewModelBase
    {
        public Guid ProductId { get; set; }
        public string VariantName { get; set; } = string.Empty;
    }
}
