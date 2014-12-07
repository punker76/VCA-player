using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rina.Infastructure.Models;
using System.Net;
using Microsoft.Practices.Prism.ViewModel;
using System.IO;
using System.Collections.ObjectModel;
using Rina.Modules;
using Rina.Infastructure.Interfaces;
using System.ComponentModel.Composition;
using Rina.Modules.DownloadManager.Properties;

namespace Rina.Modules.DownloadManager.Services
{
    [Export(typeof(IDownloadManagerService))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public sealed class DownloadManagerService : NotificationObject, IDownloadManagerService
    {
        private readonly Queue<DownloadModel> downloadQueue = new Queue<DownloadModel>();

        private String folder;
        private Int32 downloadCount;
        private Boolean downloading = false;

        public String Folder
        {
            get { return this.folder; }
            set
            {
                if (String.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Folder must be non-empty");
                }
                this.folder = Settings.Default.DownloadPath = value;
                Settings.Default.Save();
                RaisePropertyChanged(() => Folder);
            }
        }

        public Int32 DownloadCount
        {
            get { return this.downloadCount; }
            private set
            {
                this.downloadCount = value;
                RaisePropertyChanged(() => DownloadCount);
            }
        }

        public ObservableCollection<DownloadModel> Items { get; private set; }

        public void RemoveCompletedItems()
        {
            foreach (var item in Items.Where(v => v.DownloadComplete).ToArray())
            {
                Items.Remove(item);
            }
        }

        public DownloadManagerService()
        {
            Items = new ObservableCollection<DownloadModel>();
            Folder = String.IsNullOrWhiteSpace(Settings.Default.DownloadPath) ? AppDomain.CurrentDomain.BaseDirectory : Settings.Default.DownloadPath;
        }

        public void Start(String url, String title, DownloadType type)
        {
            DownloadModel model = new DownloadModel(new Uri(url), String.Empty, title, type);
            Items.Add(model);
            this.downloadQueue.Enqueue(model);
            DownloadCount++;

            Download();
        }

        private void Download()
        {
            if (this.downloadQueue.Any() && !this.downloading)
            {
                this.downloading = true;
                var model = this.downloadQueue.Dequeue();
                if (model.DownloadComplete) return;
                model.FileName = GetUniqueFileName(Folder, model.Title);

                WebClient client = new WebClient();
                EventHandler modelCanceled = (s, e) =>
                {
                    if (client.IsBusy) client.CancelAsync();
                };

                model.CancelRequested += modelCanceled;
                client.DownloadFileCompleted += (s, e) =>
                {
                    DownloadCount--;
                    this.downloading = false;
                    model.CancelRequested -= modelCanceled;
                    model.DownloadComplete = true;
                    Download();
                };
                client.DownloadProgressChanged += (s, e) =>
                {
                    model.DownloadProgress = e.ProgressPercentage;
                };
                client.DownloadFileAsync(model.Url, model.FileName);
            }
        }

        private String GetUniqueFileName(String directory, String fileName)
        {
            var path = Path.Combine(directory, CleanFileName(fileName));
            Int32 attempt = 1;

            while (File.Exists(path))
            {
                path = String.Format("{0}{1}.{2}", Path.GetFileNameWithoutExtension(path), attempt.ToString(),
                    Path.GetExtension(path));
            }
            path = Path.ChangeExtension(path, "mp3");

            return path;
        }

        public static String CleanFileName(String filename)
        {
            String file = filename;
            file = String.Concat(file.Split(System.IO.Path.GetInvalidFileNameChars(), StringSplitOptions.RemoveEmptyEntries));

            if (file.Length > 250)
            {
                file = file.Substring(0, 250);
            }
            return file;
        }

    }
}
