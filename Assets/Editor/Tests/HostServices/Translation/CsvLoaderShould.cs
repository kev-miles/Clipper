using Host.Scripts.Services.Translation;
using NUnit.Framework;
using UniRx;

namespace Editor.Tests.HostServices.Translation
{
    public class CsvLoaderShould
    {
        [TestCase("en", new [] {
            "tid_accept","tid_cancel","tid_exit_game","tid_press_start","tid_sfx_volume"})]
        [TestCase("en", new [] {
            "tid_accept","tid_cancel","tid_exit_game","tid_press_start","tid_sfx_volume"})]
        
        public void ReturnLocalizationKeysForRequiredLanguage(string languageKey, string[] keysToCheck)
        {
            //Given
            var loader = new CsvLoader();
            var localizationKeys = loader.GetLocalizationKeysForLanguage();

            loader.LoadFiles()
                .DoOnCompleted(() =>
                {
                    KeysAreRequested();
                    LocalizationKeysAreReturned();
                })
                .Subscribe();
            
            //When
            void KeysAreRequested()
            {
                localizationKeys = loader.GetLocalizationKeysForLanguage(languageKey);
            }
            
            //Then
            void LocalizationKeysAreReturned()
            {
                foreach (var key in keysToCheck)
                    Assert.IsTrue(localizationKeys.ContainsKey(key));
            }
        }
    }
}