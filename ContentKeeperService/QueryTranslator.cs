using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using ContentKeeperService.Attributes;

namespace ContentKeeperService
{
    public class QueryTranslator : ExpressionVisitor
    {
        private string _selectClause = string.Empty;
        private string _whereClause = string.Empty;
        private readonly StringBuilder _sqlBuilder = new StringBuilder();

        public string SqlQuery
        {
            get { return _selectClause + _whereClause; }
        }

        public void Translate<T>(Expression expression)
        {
            _selectClause = CreateSelectFromType(typeof(T));

            Visit(expression);
            _whereClause = " WHERE " + _sqlBuilder;

        }

        private string CreateSelectFromType(Type targetType)
        {
            var sql = "SELECT ";

            if (targetType.GetCustomAttribute(typeof (SelectAllProperties)) != null)
                sql += "*";
            else
            {
                var props = targetType.GetProperties()
                    .Where(p => p.GetCustomAttribute(typeof (SelectThisProperty)) != null)
                    .Select(propertyInfo => propertyInfo.Name)
                    .ToArray();

                sql += string.Join(", ", props);
            }

            sql += " FROM [" + targetType.Name + "]";

            return sql;
        }

        private static Expression StripQuotes(Expression e)
        {
            while (e.NodeType == ExpressionType.Quote)
            {
                e = ((UnaryExpression)e).Operand;
            }
            return e;
        }

        protected override Expression VisitMethodCall(MethodCallExpression m)
        {
            if (m.Method.DeclaringType != typeof (Queryable) || m.Method.Name != "Where")
                throw new NotSupportedException(string.Format("The method '{0}' is not supported", m.Method.Name));


            Visit(m.Arguments[0]);
            var lambda = (LambdaExpression)StripQuotes(m.Arguments[1]);
            Visit(lambda.Body);
            return m;
        }

        protected override Expression VisitUnary(UnaryExpression u)
        {
            switch (u.NodeType)
            {
                case ExpressionType.Not:
                    _sqlBuilder.Append(" NOT ");
                    Visit(u.Operand);
                    break;
                case ExpressionType.Convert:
                    Visit(u.Operand);
                    break;
                default:
                    throw new NotSupportedException(string.Format("The unary operator '{0}' is not supported", u.NodeType));
            }
            return u;
        }


        protected override Expression VisitBinary(BinaryExpression expression)
        {
            _sqlBuilder.Append("(");
            Visit(expression.Left);

            switch (expression.NodeType)
            {
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                    _sqlBuilder.Append(" AND ");
                    break;

                case ExpressionType.Or:
                case ExpressionType.OrElse:
                    _sqlBuilder.Append(" OR ");
                    break;

                case ExpressionType.Equal:
                    _sqlBuilder.Append(IsNullConstant(expression.Right) ? " IS " : " = ");
                    break;

                case ExpressionType.NotEqual:
                    _sqlBuilder.Append(IsNullConstant(expression.Right) ? " IS NOT " : " <> ");
                    break;

                case ExpressionType.LessThan:
                    _sqlBuilder.Append(" < ");
                    break;

                case ExpressionType.LessThanOrEqual:
                    _sqlBuilder.Append(" <= ");
                    break;

                case ExpressionType.GreaterThan:
                    _sqlBuilder.Append(" > ");
                    break;

                case ExpressionType.GreaterThanOrEqual:
                    _sqlBuilder.Append(" >= ");
                    break;

                default:
                    throw new NotSupportedException(string.Format("The binary operator '{0}' is not supported", expression.NodeType));

            }

            Visit(expression.Right);
            _sqlBuilder.Append(")");
            return expression;
        }

        protected override Expression VisitConstant(ConstantExpression c)
        {
            var q = c.Value as IQueryable;

            if (q == null && c.Value == null)
            {
                _sqlBuilder.Append("NULL");
            }
            else if (q == null)
            {
                switch (Type.GetTypeCode(c.Value.GetType()))
                {
                    case TypeCode.Boolean:
                        _sqlBuilder.Append(((bool)c.Value) ? 1 : 0);
                        break;

                    case TypeCode.String:
                        _sqlBuilder.Append("'");
                        _sqlBuilder.Append(c.Value);
                        _sqlBuilder.Append("'");
                        break;

                    case TypeCode.DateTime:
                        _sqlBuilder.Append("'");
                        _sqlBuilder.Append(c.Value);
                        _sqlBuilder.Append("'");
                        break;

                    case TypeCode.Object:
                        throw new NotSupportedException(string.Format("The constant for '{0}' is not supported", c.Value));

                    default:
                        _sqlBuilder.Append(c.Value);
                        break;
                }
            }

            return c;
        }

        protected override Expression VisitMember(MemberExpression m)
        {
            if (m.Expression == null || m.Expression.NodeType != ExpressionType.Parameter)
                throw new NotSupportedException(string.Format("The member '{0}' is not supported", m.Member.Name));

            _sqlBuilder.Append(m.Member.Name);
            return m;
        }

        protected bool IsNullConstant(Expression exp)
        {
            return (exp.NodeType == ExpressionType.Constant && ((ConstantExpression)exp).Value == null);
        }

    }
}
