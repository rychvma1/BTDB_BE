using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{

    [EnableCors]
    public class DataTableController : ControllerBase
    {
        private readonly ITableDataServices _services;

        public DataTableController(ITableDataServices services)
        {
            _services = services;
        }

        
        public ActionResult<object> GetData()
        {
            var tableData = _services.GetDataFromTable();

            if (tableData == null)
            {
                return NotFound();
            }
            Request.HttpContext.Response.Headers.Add("Access-Control-Allow-Origin:", " *");
            return Ok(tableData);
        }

        public ActionResult<object> GetDataFromDB()
        {
            var tableData = _services.IterateDB();

            if (tableData == null)
            {
                return NotFound();
            }
            Request.HttpContext.Response.Headers.Add("Access-Control-Allow-Origin:", " *");
            return Ok(tableData);
        }


        public ActionResult<object> GetDataFromDB2()
        {
            var tableData = _services.IterateDBStr();

            if (tableData == null)
            {
                return NotFound();
            }
            Request.HttpContext.Response.Headers.Add("Access-Control-Allow-Origin:", " *");
            return Ok(tableData);
        }
    }
}