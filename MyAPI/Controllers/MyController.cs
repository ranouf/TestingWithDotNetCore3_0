using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyAPI.Services;
using MyAPI.Services.Entities;
using System;

namespace MyAPI.Controllers
{
    [ApiController]
    [Route(Constants.API.My.Url)]
    public class MyController
    {
        private readonly IMyService _myService;
        private readonly ILogger<MyController> _logger;

        public MyController(
            IMyService myService,
            ILogger<MyController> logger
        )
        {
            _myService = myService ?? throw new ArgumentNullException(nameof(IMyService));
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return new ObjectResult(_myService.Get());
        }

        [HttpGet]
        [Route("{id:guid}")]
        public IActionResult Get([FromRoute] Guid id)
        {
            return new ObjectResult(_myService.Get(id));
        }

        [HttpPost]
        public IActionResult Create([FromBody] MyEntity myEntity)
        {
            return new ObjectResult(_myService.Create(myEntity.Name));
        }

        [HttpPut]
        [Route("{id:guid}")]
        public IActionResult Update([FromRoute] Guid id, [FromBody] MyEntity myEntity)
        {
            return new ObjectResult(_myService.Update(id, myEntity.Name));
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public IActionResult Delete([FromRoute] Guid id)
        {
            _myService.Delete(id);
            return new OkResult();
        }
    }
}
