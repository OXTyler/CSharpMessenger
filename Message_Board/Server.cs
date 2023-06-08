using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Messaging;
namespace Messaging
{
    internal class Server
    {
        static List<ClientConn> clientList = new List<ClientConn>();
        IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
        IPAddress ipAddr;
        IPEndPoint localEndPoint;
        TcpListener listener;
        public Server() 
        {


            StartServer();   
        }
        public static void TellClients(object sender, string msg){
            foreach(ClientConn client in clientList){
                client.sendMsg(msg);
            }
        }
        public void StartServer()
        {
            ipHost = Dns.GetHostEntry(Dns.GetHostName());
            ipAddr = ipHost.AddressList[0];
            
            //for local testing
            int port = 11111;
            try
            {
                localEndPoint = new IPEndPoint(ipAddr, port);
                port++;
                listener = new TcpListener(localEndPoint);
                Console.WriteLine("Trying to Connect...");
                listener.Start();
                while (true)
                {
                clientList.Add(new Messaging.ClientConn(listener.AcceptSocket()));
                if(clientList.Last().getConn().Connected){
                    localEndPoint = new IPEndPoint(ipAddr, port);
                    port++;
                    listener = new TcpListener(localEndPoint);
                    listener.Start();
                    clientList.Last().msgSend += TellClients;
                    Thread clientThread = new Thread(() => clientList.Last().recieveMessages());
                    clientThread.Start();
                }
                }
            } catch(Exception ex)
            {
                Console.WriteLine("Shit happens: {0}", ex);
            }
        }

        
    }
}
