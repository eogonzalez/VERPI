using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(VERPI.Startup))]
namespace VERPI
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
