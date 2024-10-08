using Microsoft.AspNetCore.Authentication;
using QYQ.Base.Common.Models;
using System.Security.Claims;

namespace _2DSupplyStation.AuthenticationExtend
{
    /// <summary>
    /// 完全自定义的凭证格式和解析方式
    /// </summary>
    public class UrlTokenAuthenticationHandler(ILogger<UrlTokenAuthenticationHandler> logger,IConfiguration configuration) : IAuthenticationHandler, IAuthenticationSignInHandler, IAuthenticationSignOutHandler
    {
        private AuthenticationScheme? _AuthenticationScheme = null;//"UrltokenScheme"
        private HttpContext? _HttpContext = null;

        /// <summary>
        /// 初始化，Provider传递进来的
        /// 像方法注入
        /// </summary>
        /// <param name="scheme"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task InitializeAsync(AuthenticationScheme scheme, HttpContext context)
        {
            logger.LogInformation($"This is {nameof(UrlTokenAuthenticationHandler)}.InitializeAsync");
            _AuthenticationScheme = scheme;
            _HttpContext = context;
            return Task.CompletedTask;
        }

        /// <summary>
        /// 核心鉴权处理方法,解析用户信息
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<AuthenticateResult> AuthenticateAsync()
        {

            string? auth = _HttpContext?.Request.Query[UrlTokenAuthenticationDefaults.Query];//信息从哪里读
            if (string.IsNullOrEmpty(auth))
            {
                return Task.FromResult(AuthenticateResult.Fail($"Auth is wrong: {auth}"));//ForbidAsync
            }
            else if (auth == configuration.GetSection("Auth").Get<string>())
            {
                var claimIdentity = new ClaimsIdentity("Custom");
                claimIdentity.AddClaim(new Claim(ClaimTypes.Authentication, "auth"));
                ClaimsPrincipal claimsPrincipal = new(claimIdentity);//信息拼装和传递
                return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal, null, _AuthenticationScheme!.Name)));
            }
            else
            {
                return Task.FromResult(AuthenticateResult.Fail($"Auth is wrong: {auth}"));//ForbidAsync
            }
        }

        /// <summary>
        /// 未登录
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        public Task ChallengeAsync(AuthenticationProperties? properties)
        {
            string redirectUri = "/Error";
            _HttpContext?.Response.Redirect(redirectUri);
            return Task.CompletedTask;
        }

        /// <summary>
        /// 未授权，无权限
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        public Task ForbidAsync(AuthenticationProperties? properties)
        {
            logger.LogInformation($"This is {nameof(UrlTokenAuthenticationHandler)}.ForbidAsync");
            if (_HttpContext != null)
            {
                _HttpContext.Response.StatusCode = 403;
            }
            return Task.CompletedTask;
        }


        /// <summary>
        /// SignInAsync和SignOutAsync使用了独立的定义接口，
        /// 因为在现代架构中，通常会提供一个统一的认证中心，负责证书的颁发及销毁（登入和登出），
        /// 而其它服务只用来验证证书，并用不到SingIn/SingOut。
        /// </summary>
        /// <param name="user"></param>
        /// <param name="properties"></param>
        /// <returns></returns>
        public Task SignInAsync(ClaimsPrincipal user, AuthenticationProperties? properties)
        {
            var ticket = new AuthenticationTicket(user, properties, _AuthenticationScheme?.Name ?? "");
            _HttpContext?.Response.Cookies.Append("UrlTokenCookie", Newtonsoft.Json.JsonConvert.SerializeObject(ticket.Principal.Claims));
            //把一些信息再写入到前端cookie，客户端请求时，从coookie读取UrlTokenCookie信息，放到url上
            return Task.CompletedTask;
        }

        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        public Task SignOutAsync(AuthenticationProperties? properties)
        {
            _HttpContext?.Response.Cookies.Delete("UrlTokenCookie");
            return Task.CompletedTask;
        }

    }

    /// <summary>
    /// 提供个固定值
    /// </summary>
    public class UrlTokenAuthenticationDefaults
    {
        /// <summary>
        /// 提供固定名称
        /// </summary>
        public const string AuthenticationScheme = "UrlTokenScheme";

        public const string Query = "auth";
    }
}
