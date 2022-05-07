using System;
using System.Collections.Generic;
using System.Linq;
using Host.Delivery.ScreenComponents.BaseButton;
using Host.Delivery.Screens.Credits.Infrastructure;
using JetBrains.Annotations;
using TMPro;
using UniRx;
using UnityEngine;

namespace Host.Delivery.Screens.Credits
{
    public class CreditsView : MonoBehaviour, ICreditsView
    {
        [SerializeField] private GameObject screenContent = default;
        
        [SerializeField] private TMP_Text screenTitle = default;
        [SerializeField] private TMP_Text gdLabel = default;
        [SerializeField] private TMP_Text artLabel = default;
        [SerializeField] private TMP_Text audioLabel = default;
        
        [SerializeField] private BaseButton btnBack = default;
        [SerializeField] private Animator animator = default;
        
        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        
        private string[] _tids = new string[]
            {"tid_credits","tid_gd_credits","tid_art_credits","tid_audio_credits", "tid_back"};
        public Action OnBackPressed { get; set; }
        public Action OnIntroFinished { get; set; }
        public Action OnOutroFinished { get; set; }
        public void Show()
        {
            screenContent.SetActive(true);
            animator.Play($"CreditsIntro");
        }

        private void Hide()
        {
            animator.Play($"CreditsOutro");
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

        public string[] GetTranslaitonIDs()
        {
            return _tids;
        }

        public void SetTexts(Dictionary<string, string> localizedTIDs)
        {
            screenTitle.text = localizedTIDs["tid_credits"];
            gdLabel.text = localizedTIDs["tid_gd_credits"];
            artLabel.text = localizedTIDs["tid_art_credits"];
            audioLabel.text = localizedTIDs["tid_audio_credits"];
            btnBack.SetText(localizedTIDs["tid_back"]);
        }
    }
}
