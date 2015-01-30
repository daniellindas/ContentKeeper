using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using ContentKeeperService.Entities;
using Dapper;

namespace ContentKeeperService.Repository.DapperAdapter
{
    public class DapperProvider : IRepository
    {
        public List<ContentEntry> All()
        {
            var contentEntries = ListMatchingEntries(c => c.Id != null);

            return contentEntries;
        }

        public List<ContentEntry> Where(Expression<Func<ContentEntry, bool>> selectionCriteria)
        {
            var contentEntries = ListMatchingEntries(selectionCriteria);

            return contentEntries;
        }

        public ContentEntry FirstOrDefault(Expression<Func<ContentEntry, bool>> selectionCriteria)
        {
            return Where(selectionCriteria).FirstOrDefault();
        }

        public List<User> AllUsers(Expression<Func<User, bool>> selectionCriteria)
        {
            var connectionsString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            var db = new SqlConnection(connectionsString);

            db.Open();

            var sql = GetSqlString<User>(selectionCriteria);

            var allEntries = db.Query<User>(sql).ToList();

            return allEntries;
        }

        private List<ContentEntry> ListMatchingEntries(Expression<Func<ContentEntry, bool>> selectionCriteria)
        {
            var connectionsString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            var db = new SqlConnection(connectionsString);

            db.Open();

            var sql = GetSqlString<ContentEntry>(selectionCriteria);

            var allEntries = db.Query<ContentEntry>(sql).ToList();

            return allEntries;
        }


        public string GetSqlString<T>(Expression expression)
        {
            var translator = new QueryTranslator();
            translator.Translate<T>(expression);

            return translator.SqlQuery;
        }

    }
}