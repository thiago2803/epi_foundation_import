using System;

namespace Foundation.Commerce.Import
{
    public interface ICatalogImporter
    {
        void ImportCatalog(string path, Action<string> statusChange);
    }
}