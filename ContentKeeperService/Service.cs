using System.Collections.Generic;
using ContentKeeperService.Entities;
using ContentKeeperService.Repository;
using ContentKeeperService.Repository.DapperAdapter;

namespace ContentKeeperService
{
    public class Service
    {
        private readonly IRepository _repository;

        public Service()
        {
            _repository = new DapperProvider();
        }

        public Service(IRepository repository)
        {
            _repository = repository;
        }

        public List<ContentEntry> ListAllContentEntries()
        {
            return _repository.All();
        }

        public List<User> ListUsers()
        {
            return _repository.AllUsers(u => u.Id != 0);
        }

        public ContentEntry GetContentEntryById(string id)
        {
            return _repository.FirstOrDefault(c => c.Id == id);
        }

    }
}
