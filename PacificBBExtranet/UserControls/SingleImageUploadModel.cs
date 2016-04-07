using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacificBBExtranet.Web.UserControls
{
    public class SingleImageUploadModel
    {

        public SingleImageUploadModel() {
        }
        public string UploadActionUrl { get; set; }
        public string CurrentPhotoUrl { get; set; }
        public string DefaultImageUrl { get; set; }
        public string ContainerAddClasses { get; set; }
        public bool HideInput { get; set; }
    }
}
