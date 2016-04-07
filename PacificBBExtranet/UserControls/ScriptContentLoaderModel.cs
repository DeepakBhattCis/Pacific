
using System;

namespace PacificBBExtranet.Web.UserControls
{
    public class ScriptContentLoaderModel
    {
        public string ContainerId { get; set; }

        public string ActionUri { get; set; }

        public string[] FilterControlIDs { get; set; }

        public string LoadingText { get; set; }

        public bool LoadingTargetMini { get; set; }

        public int Delay { get; set; }
    }
}