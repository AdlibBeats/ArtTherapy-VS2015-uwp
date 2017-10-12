using System;

using ArtTherapy.Models.ItemsModels;
using ArtTherapy.Pages.PostPages;
using ArtTherapy.Pages.SettingsPages;
using ArtTherapy.Pages.AboutAppPages;

using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System.Diagnostics;
using ArtTherapy.Models.ProfileModels;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Controls;
using ArtTherapy.Pages.ProfilePages;
using ArtTherapy.Pages;
using Windows.UI.Xaml.Navigation;
using ArtTherapy.Pages.PostPages.StoryPages;
using ArtTherapy.Pages.PostPages.PoetryPages;
using ArtTherapy.Pages.PostPages.ArticlePages;
using ArtTherapyCore.BaseViewModels;
using ArtTherapyCore.BaseModels;

namespace ArtTherapy.ViewModels
{
    public class MenuBackButtonClickCommand : ICommand
    {
        #region Public Constructor

        public MenuBackButtonClickCommand(MenuViewModel viewModel)
        {
            ViewModel = viewModel;
        }
        #endregion

        #region Private Members

        private MenuViewModel ViewModel;

        #endregion

        #region ICommand Members

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            ViewModel.GoBack();
        }
        #endregion
    }

    public class MenuPaneButtonClickCommand : ICommand
    {
        #region Public Constructor

        public MenuPaneButtonClickCommand(MenuViewModel viewModel)
        {
            ViewModel = viewModel;
        }
        #endregion

        #region Private Members

        private MenuViewModel ViewModel;

        #endregion

        #region ICommand Members

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            ViewModel.SetMenuPaneOpen(true);
        }
        #endregion
    }

    public class ProfileClickCommand : ICommand
    {
        #region Public Constructor

        public ProfileClickCommand(MenuViewModel viewModel)
        {
            ViewModel = viewModel;
        }
        #endregion

        #region Private Members

        private MenuViewModel ViewModel;

        #endregion

        #region ICommand Members

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            ViewModel.SetMenuSelectedIndex(-1);
            ViewModel.SetProfileChecked(true);

            var currentPage = ViewModel.Frame.Content as IPage<BaseViewModel<BaseModel>>;
            if (currentPage != null && currentPage.Id != 1)
            {
                ViewModel.SetMenuPaneOpen(false);
                ViewModel.FrameNavigate(typeof(ProfilePage));
                Debug.WriteLine(ViewModel.Frame.Content.ToString());
            }
        }
        #endregion
    }

    public class MenuListSelectionChangedCommand : ICommand
    {
        #region Public Constructor

        public MenuListSelectionChangedCommand(MenuViewModel viewModel)
        {
            ViewModel = viewModel;
        }
        #endregion

        #region Private Members

        private MenuViewModel ViewModel;

        #endregion

        #region ICommand Members

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var newItemModel = parameter as CurrentMenuItemModel;
            if (newItemModel != null)
            {
                var currentPage = ViewModel.Frame.Content as IPage<IBaseViewModel<BaseModel>>;
                if (currentPage != null && currentPage.Id != newItemModel.Id)
                {
                    ViewModel.SetProfileChecked(false);
                    ViewModel.SetMenuPaneOpen(false);
                    ViewModel.FrameNavigate(newItemModel.Type);
                    Debug.WriteLine(ViewModel.Frame.Content.ToString());
                }
            }
        }
        #endregion
    }

    public class MenuViewModel : DependencyObject
    {
        public MenuViewModel(Frame frame)
        {
            Frame = frame;

            AddNavigatedEventHandler();

            // fix E80F <- домик, сказки E11D
            MenuItemsModel = new MenuItemsModel()
            {
                Items = new CollectionViewSource()
                {
                    Source = new ObservableCollection<CurrentMenuItemModel>()
                    {
                        new CurrentMenuItemModel() { Id = 2, Icon = "\xE15C", Title = "Стихи", ItemsGroup=ItemsGroup.GroupOne, Type = typeof(PoetryPage) },
                        new CurrentMenuItemModel() { Id = 3, Icon = "\xE12F", Title = "Сказки", ItemsGroup=ItemsGroup.GroupOne, Type = typeof(StoryPage) },
                        new CurrentMenuItemModel() { Id = 4, Icon = "\xE12A", Title = "Статьи", ItemsGroup=ItemsGroup.GroupOne, Type = typeof(ArticlePage) },
                        new CurrentMenuItemModel() { Id = 5, Icon = "\xE11B", Title = "О приложении", ItemsGroup=ItemsGroup.GroupTwo, Type = typeof(AboutAppPage) },
                        new CurrentMenuItemModel() { Id = 6, Icon = "\xE115", Title = "Настройки", ItemsGroup=ItemsGroup.GroupThree, Type = typeof(SettingsPage) }
                    }
                    .GroupBy(i => i.ItemsGroup)
                },
                SelectedIndex = 0,
                IsMenuPaneOpen = false
            };

            ProfileModel = new ProfileModel()
            {
                Id = 1,
                Icon = "\xE13D",
                FirstName = "Юлия",
                LastName = "Свиридова",
                MiddleName = "Психология",
                Avatar = new BitmapImage()
                {
                    CreateOptions = BitmapCreateOptions.IgnoreImageCache,
                    UriSource = new Uri("ms-appx:///Avatar/avatar.jpg", UriKind.RelativeOrAbsolute)
                },
                IsChecked = false
            };

            FrameNavigate(typeof(PoetryPage));

            MenuPaneButtonClickCommand = new MenuPaneButtonClickCommand(this);
            ProfileButtonClickCommand = new ProfileClickCommand(this);
            MenuListSelectionChangedCommand = new MenuListSelectionChangedCommand(this);
            MenuBackButtonClickCommand = new MenuBackButtonClickCommand(this);
        }

        private void AddNavigatedEventHandler()
        {
            this.Frame.Navigated += Frame_Navigated;
        }

        private void RemoveNavigatedEventHandler()
        {
            this.Frame.Navigated -= Frame_Navigated;
        }

        private void Frame_Navigated(object sender, NavigationEventArgs e)
        {
            this.MenuItemsModel.CanGoBack = this.Frame.CanGoBack;
        }

        public void SetProfileChecked(bool value)
        {
            this.ProfileModel.IsChecked = value;
        }

        public void SetMenuPaneOpen(bool value)
        {
            this.MenuItemsModel.IsMenuPaneOpen = value;
        }

        public void FrameNavigate(Type value)
        {
            this.Frame.Navigate(value);

            this.Frame.BackStack.Clear();
            this.Frame.ForwardStack.Clear();

            this.MenuItemsModel.CanGoBack = this.Frame.CanGoBack;
        }

        public void FrameNavigate(Type value, object parameter)
        {
            this.Frame.Navigate(value, parameter);

            this.Frame.BackStack.Clear();
            this.Frame.ForwardStack.Clear();

            this.MenuItemsModel.CanGoBack = this.Frame.CanGoBack;
        }

        public void SetMenuSelectedIndex(int value)
        {
            this.MenuItemsModel.SelectedIndex = value;
        }

        public void GoBack()
        {
            if (this.Frame.CanGoBack)
                this.Frame.GoBack();
        }

        #region Public Dependency Properties

        public Frame Frame
        {
            get { return (Frame)GetValue(FrameProperty); }
            set { SetValue(FrameProperty, value); }
        }

        public static readonly DependencyProperty FrameProperty =
            DependencyProperty.Register("Frame", typeof(Frame), typeof(MenuViewModel), new PropertyMetadata(null));

        public ICommand MenuBackButtonClickCommand
        {
            get { return (ICommand)GetValue(MenuBackButtonClickCommandProperty); }
            set { SetValue(MenuBackButtonClickCommandProperty, value); }
        }

        public static readonly DependencyProperty MenuBackButtonClickCommandProperty =
            DependencyProperty.Register("MenuPaneButtonClickCommand", typeof(ICommand), typeof(MenuViewModel), new PropertyMetadata(null));

        public ICommand MenuPaneButtonClickCommand
        {
            get { return (ICommand)GetValue(MenuPaneButtonClickCommandProperty); }
            set { SetValue(MenuPaneButtonClickCommandProperty, value); }
        }

        public static readonly DependencyProperty MenuPaneButtonClickCommandProperty =
            DependencyProperty.Register("MenuPaneButtonClickCommand", typeof(ICommand), typeof(MenuViewModel), new PropertyMetadata(null));

        public ICommand ProfileButtonClickCommand
        {
            get { return (ICommand)GetValue(ProfileButtonClickCommandProperty); }
            set { SetValue(ProfileButtonClickCommandProperty, value); }
        }
        
        public static readonly DependencyProperty ProfileButtonClickCommandProperty =
            DependencyProperty.Register("ProfileButtonClickCommand", typeof(ICommand), typeof(MenuViewModel), new PropertyMetadata(null));

        public ICommand MenuListSelectionChangedCommand
        {
            get { return (ICommand)GetValue(MenuListSelectionChangedCommandProperty); }
            set { SetValue(MenuListSelectionChangedCommandProperty, value); }
        }

        public static readonly DependencyProperty MenuListSelectionChangedCommandProperty =
            DependencyProperty.Register("MenuListSelectionChangedCommand", typeof(ICommand), typeof(MenuViewModel), new PropertyMetadata(null));

        public ProfileModel ProfileModel
        {
            get { return (ProfileModel)GetValue(ProfileModelProperty); }
            set { SetValue(ProfileModelProperty, value); }
        }

        public static readonly DependencyProperty ProfileModelProperty =
            DependencyProperty.Register("ProfileModel", typeof(ProfileModel), typeof(MenuViewModel), new PropertyMetadata(null));

        public MenuItemsModel MenuItemsModel
        {
            get { return (MenuItemsModel)GetValue(MenuModelProperty); }
            set { SetValue(MenuModelProperty, value); }
        }

        public static readonly DependencyProperty MenuModelProperty =
            DependencyProperty.Register("MenuItemsModel", typeof(MenuItemsModel), typeof(MenuViewModel), new PropertyMetadata(null));

        #endregion
    }
}
