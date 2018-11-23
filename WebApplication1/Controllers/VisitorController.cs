using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BTDB_BE.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BTDB_BE.Controllers
{
    [EnableCors]
    public class VisitorController : ControllerBase
    {
        private readonly ICustDataServices _services;

        public VisitorController(ICustDataServices services)
        {
            _services = services;
        }

        public ActionResult<object> GetData()
        {
            var tableData = _services.IterateDB();

            if (tableData == null)
            {
                return NotFound();
            }
            Request.HttpContext.Response.Headers.Add("Access-Control-Allow-Origin:", " *");
            return Ok(tableData[0]);
        }
    }
}