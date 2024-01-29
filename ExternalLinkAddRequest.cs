using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests
{
    public class ExternalLinkAddRequest
    {

        [Required]
        public int UrlTypeId { get; set; }
        [Required]
        public string Url { get; set; }
        public int EntityId { get; set; }
        [Required]
        public int EntityTypeId { get; set; }
    }
}
