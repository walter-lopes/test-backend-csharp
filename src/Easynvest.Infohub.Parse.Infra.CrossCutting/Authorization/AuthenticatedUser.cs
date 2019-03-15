using Microsoft.AspNetCore.Http;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Claims;

namespace Easynvest.Infohub.Parse.Infra.CrossCutting.Authorization
{

    public class AuthenticatedUser
    {
        private const string accountIdClaimType = "acc";
        private readonly IHttpContextAccessor _accessor;

        /// <summary>
        /// Cria um novo usuário autenticado no sistema
        /// </summary>
        /// <param name="accessor">Interface de acesso ao contexto HTTP do request</param>
        public AuthenticatedUser(IHttpContextAccessor accessor)
        {
            _accessor = accessor;

            if (_accessor != null && _accessor.HttpContext != null)
            {
                FillAuthenticatedUserByHttpContext(_accessor.HttpContext);
            }
        }

        /// <summary>
        /// Id do usuário autenticado
        /// </summary>
        public string Username { get; private set; }

        /// <summary>
        /// Nome do usuário autenticado
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Email do usuário autenticado
        /// </summary>
        public string Email { get; private set; }

        /// <summary>
        /// Número da Conta do usuário autenticado
        /// </summary>
        public string AccountNumber { get; private set; }

        /// <summary>
        /// Verifica se o usuário está autenticado
        /// </summary>
        /// <returns></returns>
        public bool IsAuthenticated { get; private set; }

        /// <summary>
        /// Retorna JWT do usuário
        /// </summary>
        public string Token { get; private set; }

        /// <summary>
        /// Retorna o Agent User utilizado
        /// </summary>
        public string UserAgent { get; private set; }

        /// <summary>
        /// Seta o token do usuário
        /// </summary>
        /// <param name="token">token</param>
        public void SetToken(string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                var index = "bearer ".Length;
                Token = token.Substring(index, token.Length - index);
            }
        }

        /// <summary>
        /// Retorna Id da requisição obtido do header
        /// </summary>
        public string RequestId { private set; get; }

        /// <summary>
        /// Obtém as claims do usuário contidas no token JWT
        /// </summary>
        /// <returns></returns>
        public string GetClaimsIdentity(string claim)
        {
            if (_accessor != null && _accessor.HttpContext != null)
            {
                return _accessor.HttpContext.User.Claims.FirstOrDefault(a => a.Type == claim)?.Value;
            }

            return string.Empty;
        }

        /// <summary>
        /// Seta o usuário que não é referente a API(não veio pelo HTTPCONTEXT)
        /// </summary>
        /// <param name="requestId">Is da requisição para log</param>
        /// <param name="userName">Usuário da requisição</param>
        /// <param name="accountId">Número da Conta do usuário autenticado</param>
        public void SetAuthenticatedUser(string requestId, string userName, string accountId)
        {
            RequestId = requestId;
            Username = userName;
            AccountNumber = accountId;
        }

        /// <summary>
        /// Preenche o usuaário autenticado pela chamada da API
        /// </summary>
        /// <param name="httpContext">HttpContext da API</param>
        private void FillAuthenticatedUserByHttpContext(HttpContext httpContext)
        {
            var requestIdHeader = _accessor.HttpContext.Request.Headers["Request-Id"];
            RequestId = string.IsNullOrEmpty(requestIdHeader) ? Guid.NewGuid().ToString() : requestIdHeader.ToString();
            AccountNumber = GetClaimsIdentity(accountIdClaimType);
            Name = GetClaimsIdentity(ClaimTypes.GivenName);
            Email = GetClaimsIdentity(ClaimTypes.Email);
            IsAuthenticated = _accessor.HttpContext.User.Identity.IsAuthenticated;
            Username = _accessor.HttpContext.User.Identity.Name;
            SetToken(_accessor.HttpContext.Request.Headers["Authorization"].ToString());
        }
    }
}

