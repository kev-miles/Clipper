using System;
using System.Collections.Generic;
using Host.Delivery.Screens.PressStart.Infrastructure;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

namespace Host.Delivery.Screens.PressStart
{
    public class PressStartView : MonoBehaviour, IPressStartView
    {
        [SerializeField] private GameObject screenContent = default;
        [SerializeField] private Animator animator = default;
        [SerializeField] private TMP_Text pressStartLabel = default;
        private string pressStartTID = "tid_press_start";

        private bool _inputActive = false;
    
        public Action OnKeyPressed { get; set; }
        public Action OnIntroFinished { get; set; }
        public Action OnOutroFinished { get; set; }
    
        public void Show()
        {
            screenContent.SetActive(true);
        }

        public void EnableInput()
        {
            _inputActive = true;
        }

        public void DisableInput()
        {
            _inputActive = false;
        }

        [UsedImplicitly] //From Animator
        public void OnIntroAnimationFinish()
        {
            OnIntroFinished();
        }

        [UsedImplicitly] //From Animator
        public void OnOutroAnimationFinish()
        {
            OnOutroFinished();
            screenContent.SetActive(false);
        }

        public string[] GetTranslaitonIDs()
        {
            return new[] {pressStartTID};
        }

        public void SetTexts(Dictionary<string, string> localizedTIDs)
        {
            pressStartLabel.text = localizedTIDs[pressStartTID];
        }

        private void Update()
        {
            if (_inputActive && Input.anyKey)
            {
                OnKeyPressed();
                Hide();
            }
        }

        private void Hide()
        {
            animator.Play("PressStartOutro");
        }
    }
}
