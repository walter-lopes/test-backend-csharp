using Easynvest.Infohub.Parse.Infra.CrossCutting.Authorization;

using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Web;

namespace Easynvest.Infohub.Parse.Api.Helpers
{
    public class HtmlDecoderFilter : IResourceFilter
    {
        private readonly ILogger<HtmlDecoderFilter> _logger;
        private readonly AuthenticatedUser _authenticatedUser;
       

        public HtmlDecoderFilter(ILogger<HtmlDecoderFilter> logger, AuthenticatedUser authenticatedUser)
        {
            _logger = logger;
            _authenticatedUser = authenticatedUser;
            
        }

        public void OnResourceExecuted(ResourceExecutedContext context) { }

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            try
            {
               //_log.SendLog($"Iniciando o decode da requisição."));
                var dictionary = context.RouteData.Values;
                foreach (var value in dictionary.Values)
                {
                    var dicKey = dictionary.FirstOrDefault(x => x.Value == value);
                    context.RouteData.Values[dicKey.Key] = HttpUtility.UrlDecode(value.ToString());
                }
            }
            catch (Exception ex)
            {
                //_log.SendLog($"Ocorreu um erro no decode da requisição"), ex);
                throw;
            }
        }
    }
}