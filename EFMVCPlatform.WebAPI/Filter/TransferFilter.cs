using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace WebApiServer.Filter
{ 
    /// <summary>
    /// 处理响应数据格式的过滤器
    /// </summary>
    public class TransferFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            Type returnType = actionExecutedContext.ActionContext.ActionDescriptor.ReturnType;
            try
            {
                if(actionExecutedContext.Exception!=null)
                {
                    throw actionExecutedContext.Exception;
                }
                HttpContent content = actionExecutedContext.Response.Content;
                var actionResult = content.ReadAsAsync(returnType).Result;
                MsgResult result = new MsgResult();
                result.State = "成功！";
                result.Data = actionResult;
                MediaTypeFormatter formatter = new JsonMediaTypeFormatter();

                content = new ObjectContent<MsgResult>(result, formatter);

                actionExecutedContext.Response.Content = content;
            }
            catch (Exception ex)
            {
                MsgResult result = new MsgResult();
                result.State = "失败！";
                result.Data = null;

                actionExecutedContext.Response = new HttpResponseMessage();
                MediaTypeFormatter formatter = new JsonMediaTypeFormatter();

                actionExecutedContext.Response.Content = new ObjectContent<MsgResult>(result, formatter);
                
            }

            base.OnActionExecuted(actionExecutedContext);
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            base.OnActionExecuting(actionContext);
        }
    }
    public class MsgResult
    {
        public string State { get; set; }
        public dynamic Data { get; set; }
    }
}