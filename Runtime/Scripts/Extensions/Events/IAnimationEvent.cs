// ----------------------------------------------------------------------------
// -- Project : https://github.com/instance-id/ElementAnimationToolkit       --
// -- instance.id 2020 | http://github.com/instance-id | http://instance.id  --
// ----------------------------------------------------------------------------

using UnityEditor;

// ------------------------------------------------------- IAnimationEvent
namespace instance.id.Extensions
{
    // --------------------------------------------------- IAnimationEvent
    public interface IAnimationEvent : IEventListener
    {
        void Start();
        void Stop();
    }
}
