using System.Collections.Generic;
using System.Web.Http;
using ContentKeeperService;
using ContentKeeperService.Entities;

namespace ContentKeeper.Controllers
{
    public class GetContentController : ApiController
    {
        private readonly Service _service = new Service();

        public List<ContentEntry> ListAll()
        {
            return _service.ListAllContentEntries();
        }
    }
}
