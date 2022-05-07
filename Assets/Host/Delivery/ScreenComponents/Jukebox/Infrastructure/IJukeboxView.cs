using System;
using ScreenComponents.Jukebox;
using UnityEngine;

namespace Host.Delivery.ScreenComponents.Jukebox.Infrastructure
{
    public interface IJukeboxView
    {
        Action OnStart { get; set; }
        Action OnUnload { get; set; }
        void PlayIntroSong(AudioClip song);
        void PlaySong(Song song);
        void UpdateNowPlayingText(string newText);
        void UpdateAudioSourceSettings(float volume);
    }
}