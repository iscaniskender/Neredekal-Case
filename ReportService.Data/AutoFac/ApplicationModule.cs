using Autofac;
using ReportService.Data.Repository;

namespace ReportService.Data.AutoFac
{
    public class ApplicationModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ReportRepository>().As<IReportRepository>().InstancePerLifetimeScope();
        }
    }
}
