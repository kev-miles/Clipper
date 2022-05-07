using System;
using Host.Delivery.ScreenComponents.Jukebox.Infrastructure;
using ScreenComponents.Jukebox;
using TMPro;
using UnityEngine;

namespace Host.Delivery.ScreenComponents.Jukebox
{
    public class JukeboxView : MonoBehaviour, IJukeboxView
    {
        [SerializeField] private Animator animator = default;
        [SerializeField] private AudioSource audioSource = default;
        [SerializeField] private TMP_Text nowPlayingText = default;
        [SerializeField] private TMP_Text songName = default;
        [SerializeField] private TMP_Text artistName = default;
        public Action OnStart { get; set; }
        public Action OnUnload { get; set; }

        public void PlayIntroSong(AudioClip song)
        {
            audioSource.clip = song;
            audioSource.Play();
        }

        public void PlaySong(Song song)
        {
            songName.text = song.name;
            artistName.text = song.artist;
            audioSource.clip = song.clip;
            audioSource.Play();
            if(audioSource.volume > 0.1f)
                animator.Play($"WidgetIntro");
        }

        public void UpdateNowPlayingText(string newText)
        {
            nowPlayingText.text = newText;
        }
    
        public void UpdateAudioSourceSettings(float volume)
        {
            audioSource.volume = volume;
        }

        private void Start()
        {
            OnStart();
        }

        private void OnDestroy()
        {
            //OnUnload();
        }
    }
}
