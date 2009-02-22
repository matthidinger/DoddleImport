using System;

namespace Doddle.Import
{
    /// <summary>
    /// Represents a single field in an import source or import target
    /// </summary>
    public interface IImportField
    {
        bool IsRequired { get; }
        string Name { get; }
        string DataTypeName { get; }
        Type DataType { get; }
    }

    /// <summary>
    /// Represents a single field in an import source or import target
    /// </summary>
    public class ImportColumn
    {
        public ImportColumn()
        {
        }

        public ImportColumn(string columnName, Type dataType) : this(columnName, dataType, false)
        {}
        
        public ImportColumn(string columnName, Type dataType, bool isRequired)
        {
            Name = columnName;
            DataType = dataType;
            IsRequired = isRequired;
        }


        public bool IsRequired { get; set; }
        public string Name { get; set; }
        //public string DataTypeName { get; set; }
        public Type DataType { get; set; }
    }
}