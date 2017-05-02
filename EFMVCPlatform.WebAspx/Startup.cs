using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EFMVCPlatform.WebAspx.Startup))]
namespace EFMVCPlatform.WebAspx
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
