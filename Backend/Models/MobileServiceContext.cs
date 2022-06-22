using mARkIt.Backend.DataObjects;
using Microsoft.Azure.Mobile.Server.Tables;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;

namespace Backend.Models
{
    public class MobileServiceContext : DbContext
    {
        private const string connectionStringName = "Name=MS_TableConnectionString";

        public MobileServiceContext() : base(connectionStringName)
        {
            Database.Log = LogAction;
        }

        // System.Diagnostics.Debug is removed from the context when DEBUG is not defined, so we can't just use it directly
        public void LogAction(string msg)
        {
            System.Diagnostics.Debug.WriteLine(msg);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Mark> Marks { get; set; }
        public DbSet<UserMarkExperience> UserMarkExperiences { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Add(
                new AttributeToColumnAnnotationConvention<TableColumnAttribute, string>(
                    "ServiceTableColumn", (property, attributes) => attributes.Single().ColumnType.ToString()));
        }
    }
}
