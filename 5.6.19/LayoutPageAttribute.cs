using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;


namespace Data
{
    public class LayoutPageAttribute : ActionFilterAttribute
    {
        private string _conn;

        public LayoutPageAttribute(IConfiguration configuration)
        {
            _conn = configuration.GetConnectionString("ConStr");
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var controller = (Controller)context.Controller;
            var repository = new CTRepository(_conn);
            controller.ViewBag.Pending = repository.GetPendingAmount();
            controller.ViewBag.Confirmed = repository.GetConfirmedAmount();
            controller.ViewBag.Declined = repository.GetDeclinedAmount();
            base.OnActionExecuted(context);
        }
    }
}
