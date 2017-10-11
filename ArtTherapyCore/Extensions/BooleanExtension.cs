using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Metadata;

namespace ArtTherapyCore.Extensions
{
    public static class BooleanExtension
    {
        public static bool IsPhoneContract(this object value) =>
            ApiInformation.IsApiContractPresent("Windows.Phone.PhoneContract", 1, 0);
    }
}
