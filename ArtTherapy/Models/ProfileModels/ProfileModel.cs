using ArtTherapyCore.BaseModels;
using Windows.UI.Xaml.Media;

namespace ArtTherapy.Models.ProfileModels
{
    public class ProfileModel : BaseModel
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

        public bool IsChecked
        {
            get => _IsChecked;
            set => SetValue(ref _IsChecked, value);
        }
        private bool _IsChecked;

        public ImageSource Avatar
        {
            get => _Avatar;
            set => SetValue(ref _Avatar, value);
        }
        private ImageSource _Avatar;

        public string FirstName
        {
            get => _FirstName;
            set => SetValue(ref _FirstName, value);
        }
        private string _FirstName;

        public string LastName
        {
            get => _LastName;
            set => SetValue(ref _LastName, value);
        }
        private string _LastName;

        public string MiddleName
        {
            get => _MiddleName;
            set => SetValue(ref _MiddleName, value);
        }
        private string _MiddleName;

        public string Email
        {
            get => _Email;
            set => SetValue(ref _Email, value);
        }
        private string _Email;

        public string Phone
        {
            get => _Phone;
            set => SetValue(ref _Phone, value);
        }
        private string _Phone;

        public string Company
        {
            get => _Company;
            set => SetValue(ref _Company, value);
        }
        private string _Company;

        public string Job
        {
            get => _Job;
            set => SetValue(ref _Job, value);
        }
        private string _Job;
    }
}
