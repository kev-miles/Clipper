using System.Collections.Generic;
using Host.Delivery.ScreenComponents.Jukebox;
using Host.Delivery.ScreenComponents.Jukebox.Infrastructure;
using Host.Infrastructure.ApplicationEvents;
using Host.Infrastructure.Interfaces;
using Host.Infrastructure.SettingsParameters;
using Host.Scripts.Services.Events;
using NSubstitute;
using NUnit.Framework;
using ScreenComponents.Jukebox;
using UnityEngine;

namespace Editor.Tests.HostDelivery
{
    public class JukeboxPresenterShould
    {
        private IEventService _eventService;

        private Dictionary<string, float> _volumes = new Dictionary<string, float>()
        {
            {SettingName.SFX_VOLUME, 0.8f},
            {SettingName.MUSIC_VOLUME, 0.5f}
        };

        [SetUp]
        public void Before()
        {
            _eventService = new EventService();
        }
        
        [Test]
        public void SetInitialVolumesAtStart()
        {
            //Given
            var translator = Substitute.For<ITranslationService>();
            var view = Substitute.For<IJukeboxView>();
            
            
            //When
            var presenter = new JukeboxPresenter(_eventService, translator, view, _volumes);
            
            //Then
            view.UpdateAudioSourceSettings(_volumes[SettingName.MUSIC_VOLUME]);
        }

        [Test]
        public void PlayTrackWhenStarButtonIsPressed()
        {
            //Given
            var translator = Substitute.For<ITranslationService>();
            var view = Substitute.For<IJukeboxView>();
            var presenter = new JukeboxPresenter(_eventService, translator, view, _volumes);
            
            //When
            _eventService.SendEvent(new PressStartEvent());
            
            //Then
            view.PlaySong(Arg.Any<Song>());
        }
        
        [Test]
        public void LocalizeTextsOnStart()
        {
            //Given
            var translator = Substitute.For<ITranslationService>();
            var view = Substitute.For<IJukeboxView>();
            var presenter = new JukeboxPresenter(_eventService, translator, view, _volumes);

            //When
            view.OnStart();

            //Then
            view.Received(1).UpdateNowPlayingText(Arg.Any<string>());
        }
        
        [Test]
        public void LocalizeTextsOnLanguageChanged()
        {
            //Given
            var translator = Substitute.For<ITranslationService>();
            var view = Substitute.For<IJukeboxView>();
            var presenter = new JukeboxPresenter(_eventService, translator, view, _volumes);
            view.OnStart();

            //When
            _eventService.SendEvent(new LanguageChangedEvent());
            
            //Then
            view.Received(2).UpdateNowPlayingText(Arg.Any<string>());
        }
        
        [Test]
        public void UpdateAudioSourceSettingsWhenGameSettingsAreChanged()
        {
            //Given
            var translator = Substitute.For<ITranslationService>();
            var view = Substitute.For<IJukeboxView>();
            var volume = 0.4f;
            var presenter = new JukeboxPresenter(_eventService, translator, view, _volumes);

            //When
            _eventService.SendEvent(new AudioSettingsChangedEvent()
                .WithParameters(new Dictionary<string, float>(){{SettingName.MUSIC_VOLUME, volume}}));
            
            //Then
            view.Received(1).UpdateAudioSourceSettings(volume);
        }
        
        [Test]
        public void PlayIntroSongOnInitializationCompleted()
        {
            //Given
            var translator = Substitute.For<ITranslationService>();
            var view = Substitute.For<IJukeboxView>();
            var presenter = new JukeboxPresenter(_eventService, translator, view, _volumes);

            //When
            _eventService.SendEvent(new InitializationDoneEvent());
            
            //Then
            view.Received(1).PlayIntroSong(Arg.Any<AudioClip>());
        }
    }
}