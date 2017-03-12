using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(i2Nova.Web.Startup))]
namespace i2Nova.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
