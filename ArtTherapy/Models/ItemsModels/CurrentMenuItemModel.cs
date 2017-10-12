using ArtTherapy.Pages;
using ArtTherapyCore.BaseModels;
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
            get => _Id;
            set => SetValue(ref _Id, value);
        }
        private uint _Id;

        public string Icon
        {
            get => _Icon;
            set => SetValue(ref _Icon, value);
        }
        private string _Icon;

        public ItemsGroup ItemsGroup
        {
            get => _ItemsGroup;
            set => SetValue(ref _ItemsGroup, value);
        }
        private ItemsGroup _ItemsGroup;

        public string Title
        {
            get => _Title;
            set => SetValue(ref _Title, value);
        }
        private string _Title;

        public string Description
        {
            get => _Description;
            set => SetValue(ref _Description, value);
        }
        private string _Description;

        public Type Type
        {
            get => _Type;
            set => SetValue(ref _Type, value);
        }
        private Type _Type;
    }
}
