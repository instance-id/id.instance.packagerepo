using System;
using instance.id.Extensions;
using UnityEngine;

namespace Example
{
    public class SaveLoadSystem : MonoBehaviour, IQuickSaveLoadHandler
    {
        private void OnEnable()
        {
            EventManager.Subscribe(this);
        }

        private void OnDisable()
        {
            EventManager.Unsubscribe(this);
        }

        public void HandleQuickSave()
        {
            Debug.Log("Quick save");
        }

        public void HandleQuickLoad()
        {
            Debug.Log("Quick load");
        }
    }
}
