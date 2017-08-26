using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Panchayat.Startup))]
namespace Panchayat
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
