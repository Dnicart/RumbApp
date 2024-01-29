using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Domain.ExternalLinks
{
    public class ExternalLink
    {
        public int Id { get; set; }
        public BaseUser CreatedBy { get; set; }
        public LookUp UrlTypeId { get; set; }
        public string Url { get; set; }
        public int EntityId { get; set; }
        public LookUp EntityTypeId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
    }
}
