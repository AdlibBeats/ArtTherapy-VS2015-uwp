using ArtTherapy.Pages;
using System;
using Windows.UI.Xaml;

namespace ArtTherapy.Models.ItemsModels
{
    public enum ItemsGroup
    {
        GroupOne,
        GroupTwo,
        GroupThree,
        GroupFour,
        GroupFive,
    }
    public class CurrentMenuItemModel : BaseModel
    {
        public uint Id
        {
            get { return _Id; }
            set
            {
                _Id = value;
                OnPropertyChanged(nameof(Id));
            }
        }
        private uint _Id;

        public string Icon
        {
            get { return _Icon; }
            set
            {
                _Icon = value;
                OnPropertyChanged(nameof(Icon));
            }
        }
        private string _Icon;

        public ItemsGroup ItemsGroup
        {
            get { return _ItemsGroup; }
            set
            {
                _ItemsGroup = value;
                OnPropertyChanged(nameof(ItemsGroup));
            }
        }
        private ItemsGroup _ItemsGroup;

        public string Title
        {
            get { return _Title; }
            set
            {
                _Title = value;
                OnPropertyChanged(nameof(Title));
            }
        }
        private string _Title;

        public string Description
        {
            get { return _Description; }
            set
            {
                _Description = value;
                OnPropertyChanged(nameof(Description));
            }
        }
        private string _Description;

        public Type Type
        {
            get { return _Type; }
            set
            {
                _Type = value;
                OnPropertyChanged(nameof(Type));
            }
        }
        private Type _Type;
    }
}
