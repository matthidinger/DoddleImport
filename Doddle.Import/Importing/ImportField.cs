using System;

namespace Doddle.Import
{
    /// <summary>
    /// Represents a single field in an import source or import target
    /// </summary>
    public class ImportField
    {
        public ImportField()
        {
        }

        public ImportField(string fieldName, Type dataType) : this(fieldName, dataType, false)
        {}
        
        public ImportField(string fieldName, Type dataType, bool isRequired)
        {
            Name = fieldName;
            DataType = dataType;
            IsRequired = isRequired;
        }


        public bool IsRequired { get; set; }
        public string Name { get; set; }
        //public string DataTypeName { get; set; }
        public Type DataType { get; set; }
    }
}