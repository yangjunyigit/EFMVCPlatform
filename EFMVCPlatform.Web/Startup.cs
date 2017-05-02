using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EFMVCPlatform.Web.Startup))]
namespace EFMVCPlatform.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
