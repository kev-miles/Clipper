using UnityEngine;

namespace Host.Delivery.Screens.ViewProviders
{
    public interface IViewProvider
    {
        public T GetView<T>() where T : MonoBehaviour;
    }
}