using Autofac;

namespace Telligent.Consumer.Identity.Application.IoC;

public static class AutofacExtension
{
    public static void RegisterUnitOfWork(this ContainerBuilder builder)
    {
        builder.RegisterType<UnitOfWork>().AsSelf();
    }
}