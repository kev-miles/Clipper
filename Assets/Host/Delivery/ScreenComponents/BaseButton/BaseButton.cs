using System;
using Host.Delivery.GenericInterfaces;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Host.Delivery.ScreenComponents.BaseButton
{
    public class BaseButton : MonoBehaviour, IBaseButton, ILocalizable
    {
        // Start is called before the first frame update
        [SerializeField] private Button button = default;
        [SerializeField] private TMP_Text buttonLabel = default;
        [SerializeField] private string tid = default;

        public IObservable<Unit> OnClick() => button.OnClickAsObservable();

        public string GetTranslationID() => tid;

        public void SetText(string newText)
        {
            buttonLabel.text = newText;
        }
    }
}
