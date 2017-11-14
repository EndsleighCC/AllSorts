using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TestWebApplicationMVC.Startup))]
namespace TestWebApplicationMVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
