using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacificBBExtranet.Web.Helpers
{
    public class WebConstants
    {
        public static CultureInfo culture = new CultureInfo("en-US");
        public class Azure
        {
            public static readonly string connectionString = "DefaultEndpointsProtocol=https;AccountName=pacificbbextranet;AccountKey=DM9r1ZgLEb3ie8Qcy1DaroQcCkmAHwUYOGLRji+v1dJoGwCrta/XQTOEKRaq1LlRrCnUCvaaq3FBZ/Qgj2PXOg==";
            public static readonly string ResortsContainer = "resorts";
        }

       public static string DefaultPhotoUrl = "https://pacificbbextranet.blob.core.windows.net/resorts/DefaultPhoto.png";
       public static string DefaultLogoUrl = "https://pacificbbextranet.blob.core.windows.net/resorts/DefaultLogo.png";

    }
}
