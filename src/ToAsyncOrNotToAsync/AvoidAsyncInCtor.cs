using System.Threading.Tasks;

namespace ToAsyncOrNotToAsync
{
    class AvoidAsyncInCtor
    {
        readonly ISomeService someService;
        
        // bad
        public AvoidAsyncInCtor(ISomeServiceFactory serviceFactory)
        {
            someService = serviceFactory.BuildAsync().Result;
        }

        // good
        public AvoidAsyncInCtor(ISomeService someService)
        {
            this.someService = someService;
        }
        public static async Task<AvoidAsyncInCtor> CreateAsync(ISomeServiceFactory serviceFactor)
        {
            return new AvoidAsyncInCtor(await serviceFactor.BuildAsync());
        }
    }

    #region irrelevant
    public interface ISomeService{ };

    public interface ISomeServiceFactory
    {
        Task<ISomeService> BuildAsync();
    }
    #endregion irrelevant
}
