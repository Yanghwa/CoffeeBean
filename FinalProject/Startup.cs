using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CoffeeBean.Startup))]
namespace CoffeeBean
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
