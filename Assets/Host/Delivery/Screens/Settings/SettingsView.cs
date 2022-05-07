using System;
using System.Collections.Generic;
using Host.Delivery.ScreenComponents.BaseButton;
using Host.Delivery.ScreenComponents.SettingsComponents;
using Host.Delivery.Screens.Settings.Infrastructure;
using Host.Infrastructure.SettingsParameters;
using JetBrains.Annotations;
using TMPro;
using UniRx;
using UnityEngine;

namespace Host.Delivery.Screens.Settings
{
    public class SettingsView : MonoBehaviour, ISettingsView
    {
        [SerializeField] private TMP_Text screenLabel = default;
        [SerializeField] private string screenNameTid = default;
        [SerializeField] private ButtonSetting languageSetting = default;
        [SerializeField] private ButtonSetting saveSlotSetting = default;
        [SerializeField] private SliderSetting soundSetting = default;
        [SerializeField] private SliderSetting musicSetting = default;
        [SerializeField] private BaseButton btnBack = default;
        [SerializeField] private GameObject screenContent = default;

        [SerializeField] private Animator animator = default;

        private readonly CompositeDisposable _disposable = new CompositeDisposable();
       
        public Action OnStart { get; set; }
        public Action OnLoad { get; set; }
        public Action OnBackPressed { get; set; }
        public Action OnIntroFinished { get; set; }
        public Action OnOutroFinished { get; set; }
        public Action<(string settingName, object settingValue)> OnSettingUpdated { get; set; }
        
        public string[] GetTranslaitonIDs()
        {
            return new[]
            {
                screenNameTid, languageSetting.GetTranslationID(),
                saveSlotSetting.GetTranslationID(), soundSetting.GetTranslationID(),
                musicSetting.GetTranslationID(), btnBack.GetTranslationID()
            };
        }

        public void SetTexts(Dictionary<string, string> localizedTIDs)
        {
            screenLabel.text = localizedTIDs[screenNameTid];
            languageSetting.SetText(localizedTIDs[languageSetting.GetTranslationID()]);
            saveSlotSetting.SetText(localizedTIDs[saveSlotSetting.GetTranslationID()]);
            soundSetting.SetText(localizedTIDs[soundSetting.GetTranslationID()]);
            musicSetting.SetText(localizedTIDs[musicSetting.GetTranslationID()]);
            btnBack.SetText(localizedTIDs[btnBack.GetTranslationID()]);
        }

        public void SetSelectorValues(Dictionary<string, string[]> settingValues)
        {
            languageSetting.SetValuesToDisplay(settingValues[SettingName.LANGUAGE]);
            saveSlotSetting.SetValuesToDisplay(settingValues[SettingName.SAVE_SLOT]);
        }

        public void SetInitialSelectors(Dictionary<string, int> selectorValues)
        {
            languageSetting.UpdateSelectionValue(selectorValues[SettingName.LANGUAGE]);
            saveSlotSetting.UpdateSelectionValue(selectorValues[SettingName.SAVE_SLOT]);
        }
        
        public void SetInitialSliders(Dictionary<string, float> sliderValues)
        {
            soundSetting.UpdateSliderValue(sliderValues[SettingName.SFX_VOLUME]);
            musicSetting.UpdateSliderValue(sliderValues[SettingName.MUSIC_VOLUME]);
        }

        public void Show()
        {
            screenContent.SetActive(true);
            animator.Play($"SettingsIntro");
            OnLoad();
        }

        private void Hide()
        {
            animator.Play($"SettingsOutro");
        }
        
        [UsedImplicitly] //From Animator
        public void OnIntroAnimationFinish()
        {
            OnIntroFinished();
        }

        [UsedImplicitly] //From Animator
        public void OnOutroAnimationFinish()
        {
            screenContent.SetActive(false);
            OnOutroFinished();
        }

        private void Start()
        {
            ExposeComponentEvents();
        }

        public void EnableInput()
        {
            btnBack.OnClick().Subscribe(_ =>
            {
                OnBackPressed();
                Hide();
            }).AddTo(_disposable);
        }

        public void DisableInput()
        {
            _disposable.Clear();
        }
        
        private void ExposeComponentEvents()
        {
            languageSetting.onSelectorValueUpdated += val => OnSettingUpdated((SettingName.LANGUAGE, val));
            saveSlotSetting.onSelectorValueUpdated += val => OnSettingUpdated((SettingName.SAVE_SLOT, val));
            musicSetting.onSliderValueUpdated  += val => OnSettingUpdated((SettingName.MUSIC_VOLUME, val));
            soundSetting.onSliderValueUpdated += val => OnSettingUpdated((SettingName.SFX_VOLUME, val));
        }
    }
}
