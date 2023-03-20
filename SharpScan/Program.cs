using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace PortScanner
{
    class Program
    {
        static async Task Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: PortScanner <hostsFile> <portsFile>");
                return;
            }

            string hostsFile = args[0];
            string portsFile = args[1];

            if (!File.Exists(hostsFile) || !File.Exists(portsFile))
            {
                Console.WriteLine("Error: One or both files do not exist.");
                return;
            }

            string[] hosts = File.ReadAllLines(hostsFile);
            string[] portLines = File.ReadAllLines(portsFile);
            int[] ports = Array.ConvertAll(portLines, int.Parse);

            Console.WriteLine("Starting port scan...");

            foreach (string host in hosts)
            {
                Console.WriteLine($"Scanning host: {host}");
                foreach (int port in ports)
                {
                    bool isOpen = await IsPortOpenAsync(host, port);
                    Console.WriteLine($"Port {port}: {(isOpen ? "Open" : "Closed")}");
                }
            }

            Console.WriteLine("Port scan complete.");
        }

        private static async Task<bool> IsPortOpenAsync(string host, int port)
        {
            using (TcpClient tcpClient = new TcpClient())
            {
                try
                {
                    await tcpClient.ConnectAsync(host, port);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
    }
}
