using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Sockets;

namespace Prac.MvcWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        Modbus.Device.ModbusIpMaster master;
        public ValuesController()
        {
            //如果做监控 持续拿寄存器值
            var tcpClient = new TcpClient();
            tcpClient.Connect("127.0.0.1", 502);
            //主站访问对象
            master = Modbus.Device.ModbusIpMaster.CreateIp(tcpClient);
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(master.ReadHoldingRegisters(1, 0, 1));
        }

        [HttpPost]
        public IActionResult Post([FromForm] string value)
        {
            var v = ushort.TryParse(value, out var vl) ? vl : (ushort)0;
            master.WriteSingleRegister(1, 1, v);
            return Ok();
        }
    }
}
