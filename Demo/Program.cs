using SIS.HTTP.Cookies;
using SIS.HTTP.Enums;
using SIS.HTTP.Requests;
using SIS.WebServer;
using SIS.WebServer.Results;
using SIS.WebServer.Routing;
using SIS.WebServer.Routing.Contracts;
using System;

namespace Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            var serverRoutingTable = new ServerRoutingTable();

            var htmlResult = new HtmlResult("<h1>Hello World</h1>", HttpResponseStatusCode.Ok);
            htmlResult.AddCookie(new HttpCookie("lang", "en"));

            serverRoutingTable.Add(HttpRequestMethod.Get, "/", request => htmlResult);

            Server server = new Server(8001, serverRoutingTable);

            server.Run();
        }
    }
}