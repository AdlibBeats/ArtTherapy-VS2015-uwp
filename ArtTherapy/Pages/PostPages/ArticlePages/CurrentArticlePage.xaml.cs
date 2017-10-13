﻿using ArtTherapyCore.BaseModels;
using ArtTherapyCore.BaseViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace ArtTherapy.Pages.PostPages.ArticlePages
{
    public sealed partial class CurrentArticlePage : Page, IPage
    {
        public uint Id
        {
            get => _Id;
            set => SetValue(ref _Id, value);
        }
        private uint _Id;

        public string Title
        {
            get => _Title;
            set => SetValue(ref _Title, value);
        }
        private string _Title;

        public NavigateEventTypes NavigateEventType
        {
            get => _NavigateEventType;
            set => SetValue(ref _NavigateEventType, value);
        }
        private NavigateEventTypes _NavigateEventType;

        public BaseViewModel<BaseModel> ViewModel
        {
            get => _ViewModel;
        }
        private BaseViewModel<BaseModel> _ViewModel;

        public event EventHandler<EventArgs> Initialized;

        public CurrentArticlePage()
        {
            this.InitializeComponent();

            Id = 4;
            Title = "Статьи";
            NavigateEventType = NavigateEventTypes.ListBoxSelectionChanged;
            Initialized?.Invoke(this, new EventArgs());
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        public void SetValue<T>(ref T oldValue, T newValue, [CallerMemberName]string propertyName = null)
        {
            oldValue = newValue;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
