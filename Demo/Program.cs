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

            serverRoutingTable.Add(HttpRequestMethod.Get, "/", request => new HtmlResult("<h1>Hello World</h1>", HttpResponseStatusCode.Ok));

            Server server = new Server(8000, serverRoutingTable);

            server.Run();
        }
    }
}