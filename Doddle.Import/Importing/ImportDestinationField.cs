using System;

namespace Doddle.Import
{
    /// <summary>
    /// Represents a single field in an Import Destination
    /// </summary>
    public abstract class IImportDestinationField
    {
        public abstract bool IsRequired { get; }
        public abstract string Name { get; }
        public abstract string DataTypeName { get; }
        public abstract Type DataType { get; }
    }
}