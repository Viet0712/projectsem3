
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Project_sem3.Hubs;
using Project_sem3.Models;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Project_sem3.Repositories
{
    public class MiddleCheck : IMiddleware
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;
        private readonly IHubContext<DemoHubs> _hubContext;
        private int cnt = 0 ;
        public MiddleCheck(IServiceProvider IServiceProvider , IConfiguration configuration, IHubContext<DemoHubs> hubContext)
        {
            _serviceProvider = IServiceProvider;
            _configuration = configuration;
            _hubContext = hubContext;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            string path = context.Request.Path.ToString().Trim();

        
            string[] segments = path.Split('/');

            if(context.Request.Path.ToString() == "/Demo-hub")
            {

            }
        
            bool containsFE = false;
            if (segments[2].Contains("FE") || segments[2].Contains("Mb"))
            {
                containsFE = true;
            }
            Debug.WriteLine(segments[2]);
            if (context.Request.Path == "/api/Auth/CheckLogin" || context.Request.Path.ToString().Trim().StartsWith("/api/SeedData/")|| context.Request.Path.ToString().Trim().StartsWith("/api/Admin/LogOut")|| context.Request.Path.ToString().Trim().StartsWith("/api/Admin/ForgotPassword/")||  context.Request.Path == "/PropertiesImage/" || context.Request.Path == "/api/Demo-hub/negotiate"|| context.Request.Path == "/api/Demo-hub" || containsFE )
            {

                await next(context);
                if (context.Request.Path.ToString().Trim().StartsWith("/api/CartFE/Create"))
                {
                    await _hubContext.Clients.All.SendAsync("ReceiveMessage", $"AddCart{cnt++}");
                }
                if (context.Request.Path.ToString().Trim().StartsWith("/api/InteractFE/CreateQuestion"))
                {
                    await _hubContext.Clients.All.SendAsync("ReceiveMessage", $"CreateQuestion{cnt++}");
                }
                if (context.Request.Path.ToString().Trim().StartsWith("/api/InteractFE/CreateRate"))
                {
                    await _hubContext.Clients.All.SendAsync("ReceiveMessage", $"CreateRate{cnt++}");
                }
                if (context.Request.Path.ToString().Trim().StartsWith("/api/OrderFE/Create")&& context.Request.Method == "POST")
                {
                    await _hubContext.Clients.All.SendAsync("ReceiveMessage", $"CreateOrder{cnt++}");
                }
                if (context.Request.Path.ToString().Trim().StartsWith("/api/CartFE/Delete") && context.Request.Method == "POST")
                {
                    await _hubContext.Clients.All.SendAsync("ReceiveMessage", $"DeleteCart{cnt++}");
                }
            }
            else
            {
               
                Debug.WriteLine($"Request for {context.Request.Path} received.");
                string token = context.Request.Headers["Authorization"];
                token = token.Substring("Bearer ".Length).Trim();
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var handler = new JwtSecurityTokenHandler();
                var tokenBytes = Encoding.UTF8.GetBytes(token);
              
                // Giải mã token
                var Decodetoken = handler.ReadJwtToken(token);

                // Lấy các claims từ token
                var claims = Decodetoken.Claims;
                var emailSendRequest = claims.SingleOrDefault(e => e.Type == "Email").Value;
                var Id = claims.SingleOrDefault(e => e.Type == "Id").Value;
             
                using (var scope = _serviceProvider.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<dataContext>();
                    var checkEmailValid = await dbContext.Admins.SingleOrDefaultAsync(e => e.Id ==int.Parse(Id));
                    var Permission = await dbContext.Permissions.SingleOrDefaultAsync(e => e.AdminId == int.Parse(Id));
                    if(checkEmailValid.Status) {

                        if (context.Request.Path.ToString().Trim().StartsWith("/api/Properties"))
                        {
                            if (Permission.AddProperties == true)
                                await next(context);
                            else
                            {
                                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                                await context.Response.WriteAsync($"Invalid email");
                            }
                            
                        }

                        else if (context.Request.Path.ToString().Trim().StartsWith("/api/Good"))
                        {
                            if (Permission.AddGoods == true)
                                await next(context);
                            else
                            {
                                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                                await context.Response.WriteAsync($"Invalid email");
                            }
                        }

                        else if (context.Request.Path.ToString().Trim().StartsWith("/api/Event"))
                        {
                            if (Permission.SetEven == true)
                                await next(context);
                            else
                            {
                                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                                await context.Response.WriteAsync($"Invalid email");
                            }
                        }

                        else
                        {
                            await next(context);
                            if (context.Request.Path.ToString().Trim().StartsWith("/api/Properties/ChangeStatusSAdmin"))
                            {
                                await _hubContext.Clients.All.SendAsync("ReceiveMessage", $"Connected{cnt++}");
                            }
                            if (context.Request.Path.ToString().Trim().StartsWith("/api/Rate") && (context.Request.Method == "POST" || context.Request.Method == "PUT"))
                            {
                                await _hubContext.Clients.All.SendAsync("ReceiveMessage", $"RepQuestion{cnt++}");
                            }
                            if (context.Request.Path.ToString().Trim().StartsWith("/api/Question") && (context.Request.Method == "POST" || context.Request.Method == "PUT"))
                            {
                                await _hubContext.Clients.All.SendAsync("ReceiveMessage", $"RepRate{cnt++}");
                            }
                            if (context.Request.Path.ToString().Trim().StartsWith("/api/Permission/UpdatePermission"))
                            {
                                await _hubContext.Clients.All.SendAsync("ReceiveMessage", $"UpdatePermission{cnt++}");
                            }
                        }
                      
                       


                    }
                    else
                    {
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        await context.Response.WriteAsync($"Invalid email");
                    }
                }
            }
           
         

        }
    }
}
