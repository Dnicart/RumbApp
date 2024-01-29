using Sabio.Models.Domain.ExternalLinks;
using Sabio.Models.Requests;
using System.Collections.Generic;

namespace Sabio.Services.Interfaces
{
    public interface IExternalLinkService
    {
        int Add(ExternalLinkAddRequest model, int userId);
        void Delete(int id);
        List<ExternalLink> GetSelectByCreatedBy(int userId);
        void Update(ExternalLinkUpdateRequest model, int userId);
    }
}