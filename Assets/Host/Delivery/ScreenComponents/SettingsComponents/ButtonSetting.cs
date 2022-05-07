using System;
using Host.Delivery.GenericInterfaces;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Host.Delivery.ScreenComponents.SettingsComponents
{
    public class ButtonSetting : MonoBehaviour, IValueSelector, ILocalizable
    {
        public  string settingName = default;
        
        [SerializeField] private Button btnIncrement = default;
        [SerializeField] private Button btnDecrement = default;
        [SerializeField] private TMP_Text settingLabel = default;
        [SerializeField] private TMP_Text selectionLabel = default;
        [SerializeField] private string tid = default;

        public Action<int> onSelectorValueUpdated = i => { };
        
        private CompositeDisposable _disposable = new CompositeDisposable();
        private int _selectionIndex = 0;
        private string[] _values;

        public string GetTranslationID() => tid;

        public void SetText(string newText)
        {
            settingLabel.text = newText;
        }
    
        public void SetValuesToDisplay(string[] values)
        {
            _values = values;
        }

        public void UpdateSelectionValue(int value)
        {
            _selectionIndex = value;
            DisplaySelectionValue();
        }

        private void OnEnable()
        {
            btnIncrement.OnClickAsObservable().Subscribe(_ =>
            {
                IncreaseSelection();
            }).AddTo(_disposable);
            btnDecrement.OnClickAsObservable().Subscribe(_ =>
            {
                DecreaseSelection();
            }).AddTo(_disposable);
        }

        private void IncreaseSelection()
        {
            IncreaseIndex();
            DisplaySelectionValue();
            onSelectorValueUpdated(_selectionIndex);
        }
    
        private void DecreaseSelection()
        {
            DecreaseIndex();
            DisplaySelectionValue();
            onSelectorValueUpdated(_selectionIndex);
        }

        private void DisplaySelectionValue()
        {
            var labelText = _values.Length > 0
                ? _values[_selectionIndex]
                : "tid_error";
            selectionLabel.text = labelText;
        }

        private void IncreaseIndex() => _selectionIndex = (_selectionIndex + 1) % _values.Length;
    
        private void DecreaseIndex() => _selectionIndex = (_selectionIndex - 1 + _values.Length) % _values.Length;

        private void OnDisable()
        {
            _disposable.Clear();
        }

        private void OnDestroy()
        {
            _disposable.Clear();
        }
    }
}
