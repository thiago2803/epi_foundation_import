using EPiServer.ServiceLocation;
using Mediachase.Commerce.BackgroundTasks;
using Mediachase.Commerce.Catalog.ImportExport;
using Mediachase.Commerce.Extensions;
using Mediachase.Commerce.Shared;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Foundation.Commerce.Import
{
    [ServiceConfiguration(ServiceType = typeof(ICatalogImporter))]
    public class CatalogImporter : ICatalogImporter
    {
        public void ImportCatalog(string path, Action<string> statusChange)
        {
            IProgressMessenger _progressMessenger = new WebProgressMessenger();

            var importJob = new ImportJob(path, "Catalog.xml", true);

            Action importCatalog = () =>
            {
                _progressMessenger.AddProgressMessageText("Importing Catalog content...", false, 20);
                Action<IBackgroundTaskMessage> addMessage = msg =>
                {
                    if (msg.MessageType == BackgroundTaskMessageType.Warning && msg.Message.Contains("Overwriting"))
                    {
                        return;
                    }
                    var isError = msg.MessageType == BackgroundTaskMessageType.Error;
                    var percent = (int)Math.Round(msg.GetOverallProgress() * 100);
                    var message = msg.Exception == null
                        ? msg.Message
                        : $"{msg.Message} {msg.ExceptionMessage}";
                    statusChange($"Message: {message} | Has Error: {isError} | Percent: {percent}");
                    _progressMessenger.AddProgressMessageText(message, isError, percent);
                };
                importJob.Execute(addMessage, CancellationToken.None);
                _progressMessenger.AddProgressMessageText("Done importing Catalog content", false, 60);
            };

            importCatalog();
        }
    }
}
