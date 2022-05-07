using System;
using System.Collections.Generic;
using Host.Infrastructure.Interfaces;

namespace Host.Delivery.Screens.Settings.Infrastructure
{
    public interface ISettingsView : IApplicationView
    {
        Action OnStart { get; set; }
        Action OnLoad { get; set; }
        Action OnBackPressed { get; set; }
        
        Action<(string settingName, object settingValue)> OnSettingUpdated { get; set; }
        void SetSelectorValues(Dictionary<string, string[]> settingValues);
        void SetInitialSelectors(Dictionary<string, int> selectorValues);
        void SetInitialSliders(Dictionary<string, float> sliderValues);
    }
}