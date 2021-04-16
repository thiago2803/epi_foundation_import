using Mediachase.Commerce.Shared;
using System;

namespace Foundation.Commerce.Import
{
    public class WebProgressMessenger : IProgressMessenger
    {
        public string Progress { get; private set; }
        public int CurrentPercentage { get; private set; }

        public void AddProgressMessageText(string message, bool error, int percent)
        {
            var timeStamp = string.Format("{0:T}", DateTime.Now);
            CurrentPercentage = percent > 0 ? percent : CurrentPercentage;
            Progress = timeStamp + " : " + message + "&lt;br /&gt;" + Progress;
        }
    }
}
