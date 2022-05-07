using System;
using Host.Delivery.GenericInterfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Host.Delivery.ScreenComponents.SettingsComponents
{
    public class SliderSetting : MonoBehaviour, ISliderSetting, ILocalizable
    {
        public  string settingName = default;
    
        [SerializeField] private TMP_Text sliderLabel = default;
        [SerializeField] private string tid = default;
        [SerializeField] private Slider slider = default;

        public Action<float> onSliderValueUpdated { get; set; }

        public string GetTranslationID() => tid;

        public void SetText(string newText)
        {
            sliderLabel.text = newText;
        }
    
        public void UpdateSliderValue(float value)
        {
            slider.value = value;
            onSliderValueUpdated(value);
        }
    }
}
