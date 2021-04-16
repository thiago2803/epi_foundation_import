using EPiServer.PlugIn;
using EPiServer.Scheduler;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace Foundation.Commerce.Import
{
    [ScheduledPlugIn(DisplayName = "Import Job",
        Description = "Import file import",
        GUID = "ce18f418-adce-498c-b217-e48abcc801f1",
        DefaultEnabled = true,
        Restartable = false,
        SortIndex = 1)]
    public class ImportCatalog : ScheduledJobBase
    {
        private const string CATEGORYBASEPATH = @"App_Data\Catalog\Categories\";
        private const string ZIPFILEPATH = CATEGORYBASEPATH + @"Catalog.zip";
        private ICatalogImporter _catalogImporter;
        private bool _stopSignaled;

        public ImportCatalog(ICatalogImporter catalogImporter)
        {
            _catalogImporter = catalogImporter;
            IsStoppable = true;
        }

        /// <summary>
        /// Called when a user clicks on Stop for a manually started job, or when ASP.NET shuts down.
        /// </summary>
        public override void Stop()
        {
            _stopSignaled = true;
        }

        /// <summary>
        /// Called when a scheduled job executes
        /// </summary>
        /// <returns>A status message to be stored in the database log and visible from admin mode</returns>
        public override string Execute()
        {
            //Call OnStatusChanged to periodically notify progress of job for manually started jobs
            OnStatusChanged(string.Format("Starting execution of {0}", this.GetType()));

            Import();

            //For long running jobs periodically check if stop is signaled and if so stop execution
            if (_stopSignaled)
            {
                return "Stop of job was called";
            }

            return "Change to message that describes outcome of execution";
        }

        private void Import()
        {
            var zipFile = CombinePath(ZIPFILEPATH);
            if (!File.Exists(zipFile))
            {
                return;
            }

            try
            {

                _catalogImporter.ImportCatalog(CombinePath(ZIPFILEPATH), OnStatusChanged);
            }
            finally
            {
                //temp
            }
        }

        private string CombinePath(string path)
        {
            return Path.Combine(HostingEnvironment.ApplicationPhysicalPath, path);
        }
    }
}
