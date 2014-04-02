using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TblAdmin.Startup))]
namespace TblAdmin
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
