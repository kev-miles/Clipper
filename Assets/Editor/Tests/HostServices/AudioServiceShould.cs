using System.Globalization;
using Host.Infrastructure.ApplicationEvents;
using Host.Infrastructure.Interfaces;
using Host.Infrastructure.SettingsParameters;
using Host.Scripts.Services.Audio;
using NSubstitute;
using NUnit.Framework;
using UniRx;

namespace Editor.Tests.HostServices
{
    public class AudioServiceShould
    {
        [Test]
        public void GetCurrentAudioSettingsWhenInitialized()
        {
            //Given
            var eventService = Substitute.For<IEventService>();
            var settingsService = Substitute.For<ISettingsService>();
            var audioService = new AudioService(eventService, settingsService);
            var soundVolumeSetting = 3f;
            var musicVolumeSetting = 6f;
            var expectedSoundVolume = 0.3f;
            var expectedMusicVolume = 0.6f;
            settingsService.GetSetting(SettingName.SFX_VOLUME).Returns(soundVolumeSetting.ToString(CultureInfo.InvariantCulture));
            settingsService.GetSetting(SettingName.MUSIC_VOLUME).Returns(musicVolumeSetting.ToString(CultureInfo.InvariantCulture));
            
            
            //When
            audioService.Initialize()
                .DoOnCompleted(VolumesAreSet)
                .Subscribe();
            
            //Then
            void VolumesAreSet()
            {
                Assert.AreEqual(expectedSoundVolume, audioService.SoundVolume);
                Assert.AreEqual(expectedMusicVolume, audioService.MusicVolume);
            }
        }
        
        [Test]
        public void SendEventAfterUpdatingSoundVolume()
        {
            //Given
            var eventService = Substitute.For<IEventService>();
            var settingsService = Substitute.For<ISettingsService>();
            var audioService = new AudioService(eventService, settingsService);
            var volume = 0.3f;
            
            //When
            audioService.UpdateSoundVolume(volume);
            
            //Then
            Assert.AreEqual(volume, audioService.SoundVolume);
            eventService.Received(1).SendEvent(Arg.Any<AudioSettingsChangedEvent>());
        }
        
        [Test]
        public void SendEventAfterUpdatingMusicVolume()
        {
            //Given
            var eventService = Substitute.For<IEventService>();
            var settingsService = Substitute.For<ISettingsService>();
            var audioService = new AudioService(eventService, settingsService);
            var volume = 0.3f;
            
            //When
            audioService.UpdateMusicVolume(volume);
            
            //Then
            Assert.AreEqual(volume, audioService.MusicVolume);
            eventService.Received(1).SendEvent(Arg.Any<AudioSettingsChangedEvent>());
        }
    }
}