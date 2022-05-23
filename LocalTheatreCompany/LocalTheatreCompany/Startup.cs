using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LocalTheatreCompany.Startup))]
namespace LocalTheatreCompany
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
