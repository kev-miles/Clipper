using System;
using Host.Infrastructure.Interfaces;

namespace Host.Delivery.Screens.Menu
{
    public interface IMenuView : IApplicationView
    {
        Action OnSettingsOpened { get; set; }
        Action OnCreditsOpened { get; set; }
    }
}