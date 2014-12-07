using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using Microsoft.Practices.Prism.ViewModel;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;

namespace Rina.Infastructure.Models
{
    public enum DownloadType
    {
        Music,
        Document,
        Video
    }

    public class DownloadModel : NotificationObject
    {
        private Int32 downloadProgress;
        private String fileName;
        private Boolean downloadComplete;

        public Uri Url { get; private set; }

        public String FileName
        {
            get { return this.fileName; }
            set
            {
                this.fileName = value;
                RaisePropertyChanged(() => FileName);
            }
        }
        public String Title { get; private set; }
        public DownloadType Type { get; private set; }

        public Boolean DownloadComplete
        {
            get { return this.downloadComplete; }
            set
            {
                this.downloadComplete = value;
                RaisePropertyChanged(() => DownloadComplete);
            }
        }
        public Int32 DownloadProgress
        {
            get { return this.downloadProgress; }
            set
            {
                this.downloadProgress = value;
                RaisePropertyChanged(() => DownloadProgress);
            }
        }

        public event EventHandler CancelRequested;

        public ICommand CancelCommand { get; private set; }

        public DownloadModel(Uri url, String fileName, String title, DownloadType type)
        {
            DownloadProgress = 0;
            Url = url;
            FileName = fileName;
            Title = title;
            Type = type;

            CancelCommand = new DelegateCommand(() => RaiseEvent(ref CancelRequested));
        }

        private void RaiseEvent(ref EventHandler handler)
        {
            var handlerSafe = Volatile.Read(ref handler);
            if (handlerSafe != null)
            {
                handlerSafe(this, EventArgs.Empty);
            }
        }
    }
}
