using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace JasonWang.Dal.EntityFrameworkDal.Context
{
    /// <summary>
    /// 
    /// </summary>
    public class DefaultContext : DbContext
    {
        private static string contextstr;
        public DefaultContext()
            : base(contextstr)
        {
        }

        public DefaultContext(string context)
        {
            contextstr = context;
        }

        //public DbSet<GenerateNewsLog> GenerateNewsLogs { get; set; }
        //public DbSet<GenerateRule> GenerateRules { get; set; }
        //public DbSet<Material> Materials { get; set; }
        //public DbSet<MaterialType> MaterialTypes { get; set; }
        //public DbSet<News> News { get; set; }
        //public DbSet<Template> Templates { get; set; }
        //public DbSet<TemplateItem> TemplateItems { get; set; }
        //public DbSet<TemplateType> TemplateTypes { get; set; }
        //public DbSet<LHBReason> LHBReasons { get; set; }
        //public DbSet<OptionConfig> OptionConfigs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
