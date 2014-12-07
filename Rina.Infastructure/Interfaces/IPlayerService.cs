using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Events;
using Rina.Infastructure.Models;

namespace Rina.Infastructure.Interfaces
{
    public interface IPlayerService
    {
        event EventHandler Starting;
        event EventHandler Started;
        event EventHandler Finished;
        event EventHandler Paused;
        event EventHandler Stoped;
        event EventHandler<PositionChangedEventArgs> PositionChanged;
        event EventHandler<DataEventArgs<Double>> LoadedChanged;
        event EventHandler<DataEventArgs<Double>> VolumeChanged;
        Double Volume { get; }
        Boolean IsDecresingProgress { get; set; }

        void Play(Uri uri);
        void Play();
        void SetVolume(Double vol);
        void SetPosition(Double position);
        void StopPlaying();
        void Pause();
    }
}
