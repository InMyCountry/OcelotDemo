using Microsoft.AspNetCore.Mvc;

namespace OcelotApiA.Controllers
{
    [Route("api/[controller]"), ApiController]
    public class HealthController : ControllerBase
    {
        /// <summary>
        /// 健康检查
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }
    }
}