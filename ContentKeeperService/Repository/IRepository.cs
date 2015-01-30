using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ContentKeeperService.Entities;

namespace ContentKeeperService.Repository
{
    public interface IRepository
    {
        List<ContentEntry> All();
        List<ContentEntry> Where(Expression<Func<ContentEntry, bool>> selectionCriteria);
        ContentEntry FirstOrDefault(Expression<Func<ContentEntry, bool>> selectionCriteria);

        List<User> AllUsers(Expression<Func<User, bool>> selectionCriteria);
    }
}
