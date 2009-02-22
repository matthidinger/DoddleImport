using System.Collections.Generic;
using System;

namespace Doddle.Import
{
    /// <summary>
    /// Represents the destination of a data import from an Excel spreadsheet
    /// </summary>
    public interface IImportDestination
    {
        /// <summary>
        /// Checks whether a field exists on the current Import Destination
        /// </summary>
        /// <param name="name">The name of the field</param>
        bool FieldExists(string name);

        /// <summary>
        /// The fields that the import target expects
        /// </summary>
        IEnumerable<IImportField> Fields { get; }

        /// <summary>
        /// Determines whether or not the Import Destination has the ability to create new fields at runtime
        /// </summary>
        bool SupportsFieldCreation { get; }

        /// <summary>
        /// Create a new field in the Import Destination
        /// </summary>
        /// <param name="fieldName">The name of the field</param>
        /// <param name="dataType">The type of data this column contains</param>
        void CreateField(string fieldName, Type dataType);

        /// <summary>
        /// Import a Spreadsheet row into the Import Destination
        /// </summary>
        /// <param name="row">The spreadsheet row to import</param>
        void ImportRow(ImportRow row);
    }
}