using ArtTherapyCore.BaseModels;
using System;

namespace ArtTherapy.Pages
{
    public enum NavigateEventTypes
    {
        ListBoxSelectionChanged,
        FrameNavigated
    }

    public interface IDataContextElement : IBaseModel
    {
        object DataContext { get; set; }
    }

    public interface IPage : IDataContextElement
    {
        uint Id { get; set; }
        string Title { get; set; }
        NavigateEventTypes NavigateEventType { get; set; }
        event EventHandler<EventArgs> Initialized;
    }
}
