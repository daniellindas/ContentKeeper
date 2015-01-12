using System.Collections.Generic;
using ContentKeeperService.Entities;

namespace ContentKeeperService.Repository
{
    public interface IRepository
    {
        List<ContentEntry> ListAllContentEntries();
    }
}
