using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CreatureClass2.Startup))]
namespace CreatureClass2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
