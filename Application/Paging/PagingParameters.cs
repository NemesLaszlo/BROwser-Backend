using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Paging
{
    /// <summary>
    /// Pagination configuration helper class
    /// </summary>
    public class PagingParameters
    {
        private const int MaxPageSize = 50; // Maximum page size
        private int _pageSize = 10; // Default usage for a page size
        public int PageNumber { get; set; } = 1;

        /// <summary>
        /// Page size settings
        /// </summary>
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }
    }
}
