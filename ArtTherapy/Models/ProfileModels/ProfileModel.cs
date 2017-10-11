using ArtTherapyCore.BaseModels;
using Windows.UI.Xaml.Media;

namespace ArtTherapy.Models.ProfileModels
{
    public class ProfileModel : BaseModel
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

        public bool IsChecked
        {
            get { return _IsChecked; }
            set
            {
                _IsChecked = value;
                OnPropertyChanged(nameof(IsChecked));
            }
        }
        private bool _IsChecked;

        public ImageSource Avatar
        {
            get { return _Avatar; }
            set
            {
                _Avatar = value;
                OnPropertyChanged(nameof(Avatar));
            }
        }
        private ImageSource _Avatar;

        public string FirstName
        {
            get { return _FirstName; }
            set
            {
                _FirstName = value;
                OnPropertyChanged(nameof(FirstName));
            }
        }
        private string _FirstName;

        public string LastName
        {
            get { return _LastName; }
            set
            {
                _LastName = value;
                OnPropertyChanged(nameof(LastName));
            }
        }
        private string _LastName;

        public string MiddleName
        {
            get { return _MiddleName; }
            set
            {
                _MiddleName = value;
                OnPropertyChanged(nameof(MiddleName));
            }
        }
        private string _MiddleName;

        public string Email
        {
            get { return _Email; }
            set
            {
                _Email = value;
                OnPropertyChanged(nameof(Email));
            }
        }
        private string _Email;

        public string Phone
        {
            get { return _Phone; }
            set
            {
                _Phone = value;
                OnPropertyChanged(nameof(Phone));
            }
        }
        private string _Phone;

        public string Company
        {
            get { return _Company; }
            set
            {
                _Company = value;
                OnPropertyChanged(nameof(Company));
            }
        }
        private string _Company;

        public string Job
        {
            get { return _Job; }
            set
            {
                _Job = value;
                OnPropertyChanged(nameof(Job));
            }
        }
        private string _Job;
    }
}
