using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Code.Services
{
    /// <summary>
    /// Simple service locator
    /// </summary>
    public class ServiceLocator : MonoBehaviour
    {
        private static readonly List<IService> s_Services = new List<IService>();


        public static void Register<T>(T system) where T : IService
        {
            if (!s_Services.Contains(system))
            {
                s_Services.Add(system);
                Debug.Log($"Register service: {system}");
            }
        }

        public static void Unregister<T>(T system) where T : IService
        {
            if (s_Services.Remove(system))
            {
                Debug.Log($"Unregister service: {system}");
            }
        }

        public static T Get<T>()
        {
            var system = s_Services.Find(x => x is T);
            return system == null ? default : (T)system;
        }

        public static T Get<T>(System.Func<T, bool> match)
        {
            var system = s_Services.OfType<T>().Where(match).FirstOrDefault();
            return system == null ? default : (T)system;
        }

        public static T[] GetAll<T>()
        {
            return s_Services.OfType<T>().ToArray<T>();
        }

        public static void ForEach<T>(System.Action<T> forEach)
        {
            foreach (var service in s_Services)
            {
                if (service is T typeService)
                {
                    forEach.Invoke(typeService);
                }
            }
        }

        /*public void Update()
        {
            foreach (var service in s_Services)
            {
                if (service is IUpdateble updateble) updateble.OnUpdate();
            }
        }*/
    }
}