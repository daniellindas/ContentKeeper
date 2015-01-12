using System.Collections.Generic;
using ContentKeeperService.Entities;
using Simple.Data;

namespace ContentKeeperService.Repository.SimpleDataAdapter
{
    public class SimpleDataProvider : IRepository
    {
        public List<ContentEntry> ListAllContentEntries()
        {
            using (var db = Database.OpenNamedConnection("DefaultConnection"))
            {
                return db.ContentEntry.All() as List<ContentEntry>;
            }
        }
    }
}
