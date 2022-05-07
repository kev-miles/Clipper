using System;
using Host.Infrastructure.Interfaces;

namespace Host.Delivery.Screens.PressStart.Infrastructure
{
    public interface IPressStartView : IApplicationView
    {
        Action OnKeyPressed { get; set; }
    }
}