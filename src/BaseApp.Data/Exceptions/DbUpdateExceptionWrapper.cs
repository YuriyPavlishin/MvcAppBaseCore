using System;
using System.Globalization;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace BaseApp.Data.Exceptions
{
    /// <summary>
    /// //used to analyze exception reason
    /// </summary>
    public class DbUpdateExceptionWrapper : Exception
    {
        private const int USER_SQL_EXCEPTION_NUMBER = 50000;
        private const char USER_SQL_EXCEPTION_INNER_NUMBER_SEPARATOR = '/';

        private SqlException SqlException { get; set; }

        internal DbUpdateExceptionWrapper(DbUpdateException exception)
            : base(exception.Message, exception)
        {
            SqlException = FindSqlException(exception);
            AnalyzeUsersDataOfSQLException();
        }

        internal DbUpdateExceptionWrapper(SqlException exception)
            : base(exception.Message, exception)
        {
            SqlException = exception;
            AnalyzeUsersDataOfSQLException();
        }

        public bool IsUserSQLException { get; private set; }
        public int? UserSQLExceptionNumber { get; private set; }
        public string UserSQLExceptionMessage { get; private set; }

        public bool IsForeingKeyException
        {
            get
            {
                if (SqlException == null)
                    return false;
                return (SqlException.Number == 547);
            }
        }

        public bool IsUniqueIndexException
        {
            get
            {
                if (SqlException == null)
                    return false;
                return (((SqlException.Number == 2601 && SqlException.State == 3)
                    || (SqlException.Number == 2627 && SqlException.State == 2))
                    && (SqlException.Message.ToLower().IndexOf("unique", StringComparison.OrdinalIgnoreCase) >= 0));
            }
        }

        private SqlException FindSqlException(Exception ex)
        {
            if (ex is SqlException)
                return (SqlException)ex;
            if (ex.InnerException != null)
                return FindSqlException(ex.InnerException);
            return null;
        }

        /// <summary>
        /// Analyze SqlException. If it is custom user exception - parse it.
        ///		SqlException message usage:
        ///			'[/code/]message'
        ///		Example DB SQL throw code:
        ///			RAISERROR ('/000001/You can''t merge patients because one of merget patient was deleted', 16, 1) 
        /// </summary>
        private void AnalyzeUsersDataOfSQLException()
        {
            if (SqlException == null)
                return;

            if (SqlException.Number != USER_SQL_EXCEPTION_NUMBER)
            {
                IsUserSQLException = false;
            }
            else
            {
                string s = SqlException.Message.Trim();

                int i = -1;
                if (s[0] == USER_SQL_EXCEPTION_INNER_NUMBER_SEPARATOR)
                {
                    i = s.IndexOf(USER_SQL_EXCEPTION_INNER_NUMBER_SEPARATOR, 1);
                    if (i > -1)
                    {
                        double a;
                        if (double.TryParse(s.Substring(1, i - 1), NumberStyles.Integer, null, out a))
                        {
                            UserSQLExceptionNumber = (int)a;
                        }
                    }
                }

                UserSQLExceptionMessage = s.Substring(i + 1);

                IsUserSQLException = true;
            }
        }
    }
}
