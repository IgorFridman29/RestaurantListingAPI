using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestaurantListingAPI.IoC
{

    [Route("api/[controller]")]
    [ApiController]
    public class OperationsController : ControllerBase
    {
        private readonly ITransientService _transientService1;
        private readonly ITransientService _transientService2;

        private readonly IScopedService _scopedService1;
        private readonly IScopedService _scopedService2;

        private readonly ISingletonService _singletonService1;
        private readonly ISingletonService _singletonService2;

        public OperationsController(ITransientService transientService1, ITransientService transientService2, IScopedService scopedService1, IScopedService scopedService2, ISingletonService singletonService1, ISingletonService singletonService2)
        {
            _transientService1 = transientService1;
            _transientService2 = transientService2;
            _scopedService1 = scopedService1;
            _scopedService2 = scopedService2;
            _singletonService1 = singletonService1;
            _singletonService2 = singletonService2;
        }

        [HttpGet]
        public async Task<IActionResult> GetOperation()
        {
            var dict = new Dictionary<string, string>
            {
                { "Transient1", _transientService1.GetOperation().ToString() },
                { "Transient2", _transientService2.GetOperation().ToString() },
                { "Scoped1", _scopedService1.GetOperation().ToString() },
                { "Scoped2", _scopedService2.GetOperation().ToString() },
                { "Singleton1", _singletonService1.GetOperation().ToString() },
                { "Singleton2", _singletonService2.GetOperation().ToString() }
            };
            return Ok(dict);
        }
    }

}
