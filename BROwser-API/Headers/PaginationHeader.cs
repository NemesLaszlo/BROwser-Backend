using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace BROwser_API.Headers
{
    /// <summary>
    /// Pagination http header extension
    /// </summary>
    public static class PaginationHeader
    {
        /// <summary>
        /// Pagination header initializer
        /// </summary>
        /// <param name="response">Response result value</param>
        /// <param name="currentPage">Actual page</param>
        /// <param name="itemsPerPage">Entities per page</param>
        /// <param name="totalItems">Number of all items</param>
        /// <param name="totalPages">Number of all pages</param>
        public static void AddPaginationHeader(this HttpResponse response, int currentPage, int itemsPerPage, int totalItems, int totalPages)
        {
            var paginationHeader = new
            {
                currentPage,
                itemsPerPage,
                totalItems,
                totalPages
            };
            response.Headers.Add("Pagination", JsonSerializer.Serialize(paginationHeader));
            response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
        }
    }
}
