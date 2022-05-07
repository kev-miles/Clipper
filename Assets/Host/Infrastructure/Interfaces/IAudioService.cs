using System;
using System.Collections.Generic;
using UniRx;

namespace Host.Infrastructure.Interfaces
{
    public interface IAudioService
    {
        Dictionary<string, float> GetAudioVolumes();
        public IObservable<Unit> Initialize();
        public void UpdateMusicVolume(float volume);
        public void UpdateSoundVolume(float volume);
    }
}