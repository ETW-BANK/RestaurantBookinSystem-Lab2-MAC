using Microsoft.Extensions.DependencyInjection;

namespace ServiceRegisterExtension
{
  public interface IServicesRegisterExtension
    {
       
            void RegisterServices(IServiceCollection services);
        
    }
}
