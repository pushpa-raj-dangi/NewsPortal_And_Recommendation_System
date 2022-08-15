using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NewsWebApp.Hubs
{
    public class UrlHub : Hub
    {
            List<string> urls = new List<string>();
        
        public override Task OnConnectedAsync()
        {
           
            base.OnConnectedAsync();
           
            var httpcontext = Context.GetHttpContext();
            var pname = httpcontext.Request.Query["pagename"];
            var from = httpcontext.Request.Headers["Origin"];
            urls.Add(from + pname);
            Clients.All.SendAsync("getUrl",urls);

            return Task.CompletedTask;
        }
        public override Task OnDisconnectedAsync(Exception exception)
        {
          
            base.OnDisconnectedAsync(exception);
            urls.Clear();
            Clients.All.SendAsync("getUrl", urls);
            return Task.CompletedTask;
        }

        
    }



}
