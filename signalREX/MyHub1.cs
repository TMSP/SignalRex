using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace signalREX {
    [HubName("hubTeste")]
    public class MyHub1 : Hub {
        public void NotifyAllClients(List<cliente> msg) {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<MyHub1>();
            context.Clients.All.displayNotification(msg);
        }
    }
}