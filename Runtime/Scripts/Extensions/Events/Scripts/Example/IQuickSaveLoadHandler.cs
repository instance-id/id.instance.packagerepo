using instance.id.Extensions;

namespace Example
{
    public interface IQuickSaveLoadHandler : IEventListener
    {
        void HandleQuickSave();
        void HandleQuickLoad();
    }
}
