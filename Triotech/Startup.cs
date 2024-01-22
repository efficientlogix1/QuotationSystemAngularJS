using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Triotech.Startup))]
namespace Triotech
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
