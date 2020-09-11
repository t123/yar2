using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace Yar.Data
{
    public class SessionFactoryBuilder
    {
        public static ISessionFactory BuildSessionFactory(string connectionString, bool create = false, bool update = false)
        {
            return Fluently
                .Configure()
                .Database(SQLiteConfiguration.Standard.ConnectionString(connectionString))
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<UserMap>())
                .ExposeConfiguration(config => {
                    //config.SetInterceptor(new SqlStatementInterceptor());
                    BuildSchema(config, create, update);
                    })
                .BuildSessionFactory()
                ;
        }

        private static void BuildSchema(Configuration config, bool create = false, bool update = false)
        {
            if (create)
            {
                new SchemaExport(config).Create(false, true);
            }
            else
            {
                new SchemaUpdate(config).Execute(false, update);
            }
        }
    }
}
