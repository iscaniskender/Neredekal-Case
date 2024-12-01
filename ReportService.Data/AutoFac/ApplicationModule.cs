using Autofac;
using ReportService.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
