using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DBBrokerSite.Startup))]
namespace DBBrokerSite
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
