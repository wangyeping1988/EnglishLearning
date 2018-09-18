using System.Data.Entity;

namespace JasonWang.Dal.EntityFrameworkDal.Context
{
    /// <summary>
    /// config文件中使用
    /// </summary>
    public class ContextInitializer : CreateDatabaseIfNotExists<DefaultContext>
    {
        protected override void Seed(DefaultContext context)
        {
            //context.Database.ExecuteSqlCommand("create unique index ix_userinfo_username on userinfo(username)");    //添加唯一约束
            //context.SaveChanges();
            base.Seed(context);
        }
    }
}
