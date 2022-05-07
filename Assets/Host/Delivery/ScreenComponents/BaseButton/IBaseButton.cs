using System;
using UniRx;

namespace Host.Delivery.ScreenComponents.BaseButton
{
    public interface IBaseButton
    {
        IObservable<Unit> OnClick();
    }
}