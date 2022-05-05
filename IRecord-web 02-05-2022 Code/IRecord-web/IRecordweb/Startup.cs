using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(IRecordweb.Startup))]
namespace IRecordweb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            // committed by poonam
        }
    }
}
