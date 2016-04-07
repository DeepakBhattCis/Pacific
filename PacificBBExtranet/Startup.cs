using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PacificBBExtranet.Web.Startup))]
namespace PacificBBExtranet.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
