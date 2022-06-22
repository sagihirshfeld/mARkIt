using Microsoft.Azure.Mobile.Server.Config;
using Owin;
using System.Data.Entity.Migrations;
using System.Web.Http;

namespace Backend
{
    public partial class Startup
    {
        public static void ConfigureMobileApp(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();

            new MobileAppConfiguration()
                .UseDefaultConfiguration()
                .MapApiControllers() 
                .ApplyTo(config);

            // Automatic Code First Migrations
            var migrator = new DbMigrator(new Migrations.Configuration());
            migrator.Update();

            app.UseWebApi(config);
        }
    }
}

