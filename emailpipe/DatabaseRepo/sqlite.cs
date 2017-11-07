using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using MimeKit;

namespace emailpipe.DatabaseRepo
{
    public class sqlite : IDatabase
    {
        private SQLiteConnection _sqlConnection;
        public sqlite()
        {
            CreateIfNotExists();
            OpenConnection();
        }

        private void CreateIfNotExists()
        {
            //TODO ADD LOGIC
        }

        private void OpenConnection()
        {
            try
            {
                _sqlConnection = new SQLiteConnection();
                _sqlConnection.Open();
            }
            catch (SQLiteException ex)
            {
                //TODO ADD ERROR HANDLING AND PROBOLBLY BETTER Exception more specific!
            }
        }

        public void AddMail(MimeMessage eml)
        {
            if (!IsConnectionValid())
                return;

            var ident = GetIdentification(eml);

            if (CheckIfMailExists(ident))
                return;

            using (SQLiteCommand cmd = _sqlConnection.CreateCommand())
            {
                cmd.CommandText = "INSERT INTO mail (id, ) VALUES()";
                cmd.ExecuteNonQuery();
            }
        }

        public bool CheckIfMailExists(long ident)
        {
            if (!IsConnectionValid())
                return false;

            using (SQLiteCommand cmd = _sqlConnection.CreateCommand())
            {
                cmd.CommandText = "SELECT id FROM mail WHERE ident = "+ ident + "";
                var id = (int)cmd.ExecuteScalar();

                if (id > 0)
                    return true;
            }

            return false;
        }

        public void RemoveMail(MimeMessage eml)
        {
            throw new NotImplementedException();
        }

        private bool IsConnectionValid()
        {
            if (_sqlConnection == null ||
                (_sqlConnection != null && _sqlConnection.State == System.Data.ConnectionState.Closed || _sqlConnection.State == System.Data.ConnectionState.Broken
                || _sqlConnection.State == System.Data.ConnectionState.Connecting))
                return false;

            return true;
        }

        private long GetIdentification(MimeMessage eml)
        {
            var dateHash = eml.Date.GetHashCode();
            var subjectHash = eml.Subject.GetHashCode();
            var bodyHash = eml.TextBody.Trim().GetHashCode();

            return subjectHash + bodyHash + dateHash;
        }  

        public void Dispose()
        {
            if (_sqlConnection.State == System.Data.ConnectionState.Open)
                _sqlConnection.Clone();

            _sqlConnection.Dispose();
        }
    }
}
