using System;
using System.Collections.Generic;
using Host.Delivery.ScreenComponents.BaseButton;
using Host.Delivery.Screens.Menu.Infrastructure;
using JetBrains.Annotations;
using TMPro;
using UniRx;
using UnityEngine;

namespace Host.Delivery.Screens.Menu
{
    public class MenuView : MonoBehaviour, IMenuView
    {
        [SerializeField] private GameObject screenContent = default;
        [SerializeField] private TMP_Text gameTitle = default;

        [SerializeField] private BaseButton btnPlay = default;
        [SerializeField] private BaseButton btnSettings = default;
        [SerializeField] private BaseButton btnCredits = default;
        [SerializeField] private BaseButton btnExit = default;
        [SerializeField] private Animator animator = default;
        
        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        private MenuFlow _destination;

        public Action OnIntroFinished { get; set; }
        public Action OnOutroFinished { get; set; }
        public Action OnSettingsOpened { get; set; }
        public Action OnCreditsOpened { get; set; }

        public void EnableInput()
        {
            btnPlay.OnClick().Subscribe().AddTo(_disposable);
            btnSettings.OnClick().Subscribe(_ =>
            {
                Hide();
                _destination = MenuFlow.Settings;
                OnOutroFinished();
            }).AddTo(_disposable);
            btnCredits.OnClick().Subscribe(_ =>
            {
                Hide();
                _destination = MenuFlow.Credits;
                OnOutroFinished();
            }).AddTo(_disposable);
            btnExit.OnClick().Subscribe().AddTo(_disposable);
        }

        public void DisableInput()
        {
            _disposable.Clear();
        }

        [UsedImplicitly] //From Animator
        public void OnIntroAnimationFinish()
        {
        }

        [UsedImplicitly] //From Animator
        public void OnOutroAnimationFinish()
        {
            screenContent.SetActive(false);
            OnOutroFinished();
        }

        public string[] GetTranslaitonIDs()
        {
            return new[]
            {
                btnPlay.GetTranslationID(), btnSettings.GetTranslationID(),
                btnCredits.GetTranslationID(), btnExit.GetTranslationID()
            };
        }

        public void SetTexts(Dictionary<string, string> localizedTIDs)
        {
            btnPlay.SetText(localizedTIDs[btnPlay.GetTranslationID()]);
            btnSettings.SetText(localizedTIDs[btnSettings.GetTranslationID()]);
            btnCredits.SetText(localizedTIDs[btnCredits.GetTranslationID()]);
            btnExit.SetText(localizedTIDs[btnExit.GetTranslationID()]);
        }

        public void Show()
        {
            screenContent.SetActive(true);
            animator.Play($"MenuIntro");
        }

        private void Hide()
        {
            animator.Play($"MenuOutro");
        }

        private void Awake()
        {
            OnOutroFinished += ChooseFlow;
        }

        private void ChooseFlow()
        {
            if (_destination == MenuFlow.Settings)
                OnSettingsOpened();
            else
                OnCreditsOpened();
        }
    }
}