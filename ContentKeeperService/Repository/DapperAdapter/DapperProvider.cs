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

        private List<ContentEntry> ListMatchingEntries(Expression<Func<ContentEntry, bool>> selectionCriteria)
        {
            var connectionsString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            var db = new SqlConnection(connectionsString);

            db.Open();

            var sql = GetSqlString(selectionCriteria);

            var allEntries = db.Query<ContentEntry>(sql).ToList();

            return allEntries;
        }


        public string GetSqlString(Expression<Func<ContentEntry, bool>> expression)
        {
            var translator = new QueryTranslator();
            translator.Translate(expression);

            return translator.SqlQuery;
        }

    }
}