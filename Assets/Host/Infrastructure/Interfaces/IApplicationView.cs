using System;
using System.Collections.Generic;

namespace Host.Infrastructure.Interfaces
{
    public interface IApplicationView
    {
        Action OnIntroFinished { get; set; }
        Action OnOutroFinished { get; set; }
        void Show();
        void EnableInput();
        void DisableInput();
        void OnIntroAnimationFinish();
        void OnOutroAnimationFinish();
        public string[] GetTranslaitonIDs();
        public void SetTexts(Dictionary<string, string> localizedTIDs);
    }
}