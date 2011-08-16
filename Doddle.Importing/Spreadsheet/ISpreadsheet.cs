using System;

namespace Doddle.Importing
{
    public interface ISpreadsheet : IImportSource, IDisposable
    {
        string[] LoadWorksheets();
    }
}