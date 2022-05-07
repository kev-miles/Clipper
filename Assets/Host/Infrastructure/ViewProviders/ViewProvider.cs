using Host.Infrastructure.Interfaces;
using UnityEngine;

namespace Host.Delivery.Screens.ViewProviders
{
    public class ViewProvider : MonoBehaviour, IViewProvider
    {
        [SerializeField] private MonoBehaviour[] _views;
        
        public T GetView<T>() where T : MonoBehaviour
        {
            foreach (var view in _views)
            {
                if (view.GetType() == typeof(T))
                {
                    return (T)view;
                }
            }

            throw new System.Exception("View Type not found in collection");
        }
    }
}