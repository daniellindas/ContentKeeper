using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ContentKeeper.Startup))]
namespace ContentKeeper
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
