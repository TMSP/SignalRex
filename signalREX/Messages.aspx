<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Messages.aspx.cs" Inherits="signalREX.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src='<%=ResolveClientUrl("~/Scripts/jquery-1.6.4.min.js") %>' type="text/javascript"></script>
    <script src='<%=ResolveClientUrl("~/Scripts/jquery.signalR-2.2.1.min.js") %>' type="text/javascript"></script>
    <script src='<%=ResolveClientUrl("~/signalr/hubs") %>' type="text/javascript"></script>
    <script>
        $(function () {
            var notify = $.connection.hubTeste
            
            notify.client.displayNotification = function (msg) {
                console.log("MENSAGEM", msg);
                var st = "<table><tr><th>Email</th><th>Nome</th><th>Telefone</th><th>Pax</th><th>Pager</th><th>Ativo</th></tr>";
                for (var i = 0; i < msg.length ; i++) {                    
                    st += "<tr><td>" + msg[i].emailCli + "</td><td>" + msg[i].nomeCli + "</td><td>" + msg[i].telefoneCli + "</td><td>" + msg[i].lugares + "</td><td>" + msg[i].pager + "</td><td>" + msg[i].ativo+"</td></tr>";
                }
                st += "</table>"
                $("#newData").html(st);
            };

            $.connection.hub.start();
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <span id="newData"></span>
        </div>
    </form>
</body>
</html>
