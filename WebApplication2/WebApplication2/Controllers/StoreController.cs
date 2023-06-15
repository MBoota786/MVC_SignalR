using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class StoreController : ApiController
    {
        userManagementEntities context = new userManagementEntities();       
        public IHttpActionResult Get()
        {
            var list = context.products.ToList();
            if(list.Count > 0)
            {
                return Ok(list);
            }
            return NotFound(); //200 status 44  
        }
    }
}
