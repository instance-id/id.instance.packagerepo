using instance.id.Extensions;
using UnityEngine;

namespace Example
{
    public class InputSystem : MonoBehaviour
    {
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                EventManager.RaiseEvent<IQuickSaveLoadHandler>(h => h.HandleQuickSave());
            }
            
            if (Input.GetKeyDown(KeyCode.L))
            {
                EventManager.RaiseEvent<IQuickSaveLoadHandler>(h => h.HandleQuickLoad());
            }
        }
    }
}
