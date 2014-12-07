using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.ViewModel;
using VKapi;
using VKapi.Audio;
using System.Diagnostics.Contracts;
using VKapi.Models;

namespace Rina.Modules.AudioPlayer.ViewModels
{
    public sealed class LyricsViewModel : NotificationObject
    {
        private readonly VKAudio audio;

        private String lyricsText;
        private Boolean isLoading;

        public Boolean IsLoading
        {
            get { return this.isLoading; }
            private set
            {
                this.isLoading = value;
                RaisePropertyChanged(() => IsLoading);
            }
        }

        public Boolean CanLoadLyrics
        {
            get { return this.audio.LyricsId != 0; }
        }

        public String LyricsText
        {
            get { return this.lyricsText; }
            private set
            {
                this.lyricsText = value;
                RaisePropertyChanged(() => LyricsText);
            }
        }

        public LyricsViewModel(VKAudio audio)
        {
            Contract.Requires(audio != null);

            this.audio = audio;
        }

        public async Task Show()
        {
            try
            {
                IsLoading = true;

                if (String.IsNullOrEmpty(LyricsText))
                {
                    VKLyrics lyrics = await VKApi.Audio.GetLyricsAsync(this.audio.LyricsId);
                    if (lyrics != null)
                    {
                        LyricsText = lyrics.Text;
                    }
                }
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
