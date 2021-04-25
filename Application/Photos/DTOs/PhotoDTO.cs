using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Photos.DTOs
{
    public class PhotoDTO
    {
        public string Id { get; set; }
        public string Url { get; set; }
        public bool IsMain { get; set; }
        public string DisplayName { get; set; }
        public string MainImage { get; set; }
    }
}
