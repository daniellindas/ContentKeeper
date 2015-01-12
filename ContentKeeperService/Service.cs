using System.Collections.Generic;
using ContentKeeperService.Entities;
using ContentKeeperService.Repository;
using ContentKeeperService.Repository.SimpleDataAdapter;

namespace ContentKeeperService
{
    public class Service
    {
        private readonly IRepository _repository;

        public Service()
        {
            _repository = new SimpleDataProvider();
        }

        public Service(IRepository repository)
        {
            _repository = repository;
        }

        public List<ContentEntry> ListAllContentEntries()
        {
            return _repository.ListAllContentEntries();
        }

        public ContentEntry GetContentEntryById(string id)
        {
            return new ContentEntry();
        }

    }
}
