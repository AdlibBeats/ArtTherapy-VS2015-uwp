using Windows.Foundation.Metadata;

namespace ArtTherapyCore.Extensions
{
    public static class BooleanExtension
    {
        public static bool IsPhoneContract(this object value) =>
            ApiInformation.IsApiContractPresent("Windows.Phone.PhoneContract", 1, 0);
    }
}
