using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Project_sem3.InterFace;
using Project_sem3.Models;
using Project_sem3.SqlTableDependencies;
using System.Diagnostics;

namespace Project_sem3.Hubs
{
    
    public sealed class DemoHubs : Hub
    {
       
       

        public override async Task OnConnectedAsync()
        {
           
            await Clients.All.SendAsync("ReceiveMessage" , "Reload");
        }
        public  async Task Test()
        {
            await Clients.All.SendAsync("ReceiveMessage", $" da ket noi");
        }
    }
}
