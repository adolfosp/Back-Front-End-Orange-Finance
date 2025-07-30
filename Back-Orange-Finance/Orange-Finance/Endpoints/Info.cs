using Serilog;
using System.Net;
using System.Net.NetworkInformation;

namespace OrangeFinance.Endpoints;

public static class Info
{
    public static void RegisterInfoEndpoints(this IEndpointRouteBuilder routes)
    {
        var farms = routes.MapGroup("/info");

        farms.MapPost("/log-seq", (ILogger<string> logger) =>
        {
            Log.Error("Testando Api Seq");

        }).MapToApiVersion(1).MapToApiVersion(2).WithOpenApi();

        farms.MapGet("", async () =>
        {
            var machineName = Environment.MachineName;
            var osVersion = Environment.OSVersion.ToString();
            var ipAddress = GetLocalIPAddress();
            var macAddress = GetMacAddress();
            var processId = Environment.ProcessId;

            return (new
            {
                MachineName = machineName,
                OSVersion = osVersion,
                IPAddress = ipAddress,
                MacAddress = macAddress,
                ProcessId = processId,
                Timestamp = DateTime.UtcNow
            });

            string GetLocalIPAddress()
            {
                string localIP = "N/A";
                var host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        localIP = ip.ToString();
                        break;
                    }
                }
                return localIP;
            }

            string GetMacAddress()
            {
                return NetworkInterface.GetAllNetworkInterfaces()
                    .Where(nic => nic.OperationalStatus == OperationalStatus.Up)
                    .Select(nic => nic.GetPhysicalAddress().ToString())
                    .FirstOrDefault() ?? "N/A";
            }

        }).Produces(statusCode: 200)
          .MapToApiVersion(1)
          .WithOpenApi();


    }

}
