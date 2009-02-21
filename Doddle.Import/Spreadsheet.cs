using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Data;

namespace Doddle.Import
{
    //public class Spreadsheet
    //{
    //    public Spreadsheet(Stream spreadsheet)
    //    {
            
    //    }

    //    public Spreadsheet(string path)
    //    {
            
    //    }

    //    public Spreadsheet()
    //    {
    //        Rows = new SpreadsheetRowCollection(this);
    //        Columns = new SpreadsheetColumnCollection(this);
    //    }

    //    public SpreadsheetRowCollection Rows { get; private set; }
    //    public SpreadsheetColumnCollection Columns { get; private set; }

    //}

    public class Spreadsheet : IDisposable
    {
        #region Private Fields

        private const string ENUM_WORKSHEET_EXCEPTION = "Could not enumerate worksheets '{0}'.";
        private const string OPEN_SPREADSHEET_EXCEPTION = "Could not open spreadsheet '{0}'.";
        private const string CACHE_SPREADSHEET_EXCEPTION = "Could not cache spreadsheet '{0}'.";
        private const string EXCEL_CONNECTION_STRING = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties=""Excel 8.0;HDR=YES;IMEX=1;""";

        #endregion

        #region Private Fields

        private string path;
        private string[] worksheets;
        private DbConnection connection;
        private Stream stream;

        #endregion

        #region Constructor

        public Spreadsheet(Stream spreadsheet)
        {
            path = CacheSpreadsheet(spreadsheet);
        }

        public Spreadsheet(string path)
        {
            this.path = path;
        }

        #endregion

        #region Public Properties

        public string Path
        { get { return path; } }

        public string[] Worksheets
        {
            get
            {
                if (worksheets == null)
                { worksheets = LoadWorksheets(); }
                return worksheets;
            }
        }

        #endregion

        #region Public Methods

        public DbCommand CreateCommand(string query)
        {
            DbCommand command = Connection.CreateCommand();
            command.CommandText = query;
            command.CommandType = CommandType.Text;
            command.Connection = Connection;
            return command;
        }

        public DbDataReader ExecuteReader(DbCommand command)
        {
            try
            {
                command.Connection = Connection;
                return command.ExecuteReader();
            }
            catch (Exception exception)
            {
                throw new Exception(string.Format(ENUM_WORKSHEET_EXCEPTION, path), exception);
            }
        }

        private SpreadsheetRowCollection _rows;
        public SpreadsheetRowCollection Rows
        {
            get
            {
                if (_rows == null)
                {
                    DbCommand command = CreateCommand(string.Format("SELECT * FROM [{0}]", this.Worksheets[0]));

                    using (DbDataReader reader = ExecuteReader(command))
                    {

                        _rows = SpreadsheetRowLoader.LoadRows(this, reader); // new SpreadsheetRowCollection(this, reader);
                    }
                }

                return _rows;
            }
        }


        private SpreadsheetColumnCollection _columns;
        public SpreadsheetColumnCollection Columns
        {
            get
            {
                if (_columns == null)
                {
                    // TODO: Change this to only select the TOP row
                    DbCommand command = this.CreateCommand(string.Format("SELECT * FROM [{0}]", this.Worksheets[0]));

                    using (DbDataReader reader = this.ExecuteReader(command))
                    {
                        _columns = new SpreadsheetColumnCollection(this, reader);
                    }
                }

                return _columns;
            }
        }



        public void Close(bool delete)
        {
            try
            {
                Dispose(false);

                if (delete && File.Exists(path))
                    File.Delete(path);
            }
            catch { }
        }

        #endregion

        #region Private Properties

        private DbConnection Connection
        {
            get
            {
                try
                {
                    if (connection == null)
                    {
                        connection = DbProviderFactories.GetFactory("System.Data.OleDb").CreateConnection();
                        connection.ConnectionString = string.Format(EXCEL_CONNECTION_STRING, path);
                        connection.Open();
                    }
                    return connection;
                }
                catch (Exception exception)
                {
                    throw new Exception(string.Format(OPEN_SPREADSHEET_EXCEPTION, path), exception);
                }
            }
        }

        #endregion

        #region Private Methods

        private string[] LoadWorksheets()
        {
            List<string> worksheetList = new List<string>();
            try
            {
                DataTable tables = Connection.GetSchema("Tables");

                foreach (DataRow row in tables.Rows)
                {
                    worksheetList.Add((string)row["TABLE_NAME"]);
                }
            }
            catch (Exception exception)
            {
                throw new Exception(string.Format(ENUM_WORKSHEET_EXCEPTION, path), exception);
            }
            finally
            {
            }
            return worksheetList.ToArray();
        }

        private string CacheSpreadsheet(Stream spreadsheet)
        {
            int bufferLength = 102400;
            string path = System.IO.Path.GetTempFileName();
            try
            {
                using (BinaryWriter writer = new BinaryWriter(new FileStream(path, FileMode.Append)))
                {
                    byte[] buffer = new byte[bufferLength];
                    int bytesRead = 0;

                    do
                    {
                        bytesRead = spreadsheet.Read(buffer, 0, bufferLength);
                        if (bytesRead > 0)
                            writer.Write(buffer, 0, bytesRead);
                    } while (bytesRead > 0);
                    writer.Flush();
                }
            }
            catch (Exception exception)
            {
                throw new Exception(string.Format(CACHE_SPREADSHEET_EXCEPTION, path), exception);
            }
            finally
            {
                spreadsheet.Close();
            }
            return path;
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(false);
        }

        protected void Dispose(bool disposing)
        {
            if (!disposing)
                GC.SuppressFinalize(this);

            if (connection != null && connection.State == ConnectionState.Open)
            {
                //connection.Close();
            }
        }

        ~Spreadsheet()
        {
            Dispose(true);
        }

        #endregion
    }
}