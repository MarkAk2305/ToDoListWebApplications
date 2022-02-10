using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ToDoListWebApplications.Startup))]
namespace ToDoListWebApplications
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
