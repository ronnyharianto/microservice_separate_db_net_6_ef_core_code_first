using System.ComponentModel.DataAnnotations;

namespace Falcon.Libraries.DataAccess.Domain
{
    public class ViewModelBase
    {
        /// <summary>
        /// Primary for all view model
        /// </summary>
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Date and Time when data was lastest modified
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// Date and Time when data was lastest modified
        /// </summary>
        public DateTime? Modified { get; set; }

        /// <summary>
        /// 0 -> Active Data
        /// 1 -> Soft Delete Data
        /// </summary>
        public int RowStatus { get; set; } = 0;
    }
}