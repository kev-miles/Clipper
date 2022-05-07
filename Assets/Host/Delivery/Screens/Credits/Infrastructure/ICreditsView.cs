using System;
using Host.Infrastructure.Interfaces;

namespace Host.Delivery.Screens.Credits.Infrastructure
{
    public interface ICreditsView : IApplicationView
    {
        Action OnBackPressed { get; set; }
    }
}