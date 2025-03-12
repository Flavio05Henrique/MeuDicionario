using MeuDicionariov2.Infra.Data;

namespace MeuDicionarioV2.Infra.Startup
{
    public static class DependencyInjectionConfigExtensions
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            services.AddAutoMapper(AssemblyReference.Assembly);

            services.AddMediatR(c => c.RegisterServicesFromAssemblies(AssemblyReference.Assembly));

            services.AddDbContext<MyDictionaryDbContex>();
        }
    }
}
