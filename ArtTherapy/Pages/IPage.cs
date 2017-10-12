using ArtTherapyCore.BaseModels;
using ArtTherapyCore.BaseViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

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

    public interface IPage<T> : IDataContextElement
        where T : IBaseViewModel<BaseModel>
    {
        uint Id { get; set; }
        string Title { get; set; }
        NavigateEventTypes NavigateEventType { get; set; }
        event EventHandler<EventArgs> Initialized;
        T ViewModel { get; }
    }
}
