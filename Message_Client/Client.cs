using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ClientSpace
{
    class Client
    {
        public Client(int port) {
            StartClient(port);
        }
        void StartClient(int port)
        {
            try
            {
                IPHostEntry iPHost = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress iPAddr = iPHost.AddressList[0];
                IPEndPoint localEndPoint = new IPEndPoint(iPAddr, port);

                Socket sender = new Socket(iPAddr.AddressFamily,
                    SocketType.Stream, ProtocolType.Tcp);
                // that we are connected
                // We print EndPoint information

                // Creation of message that
                // we will send to Server
                try
                {
                    // Connect Socket to the remote
                    sender.Connect(localEndPoint);
                    Thread Reciever = new Thread(() => MessageReciever(sender));
                    Reciever.Start();
                    MessageLoop(sender);
                }   

                // Manage of Socket's Exceptions
                catch (ArgumentNullException ane)
                {

                    Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                }

                catch (SocketException se)
                {

                    Console.WriteLine("SocketException : {0}", se.ToString());
                }

                catch (Exception e)
                {
                    Console.WriteLine("Unexpected exception : {0}", e.ToString());
                }
            } catch (Exception e) { }
        }
        void MessageLoop(Socket sender) {
            Console.WriteLine("Socket connected to -> {0} ",
            sender.RemoteEndPoint.ToString());
            while(sender.Connected){
                Console.Write(">");
                String msg = Console.ReadLine();
                msg += "<EOF>";
                if(msg == "!exit<EOF>"){
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();
                    return;
                }
                byte[] messageSent = Encoding.ASCII.GetBytes(msg);
                int byteSent = sender.Send(messageSent);
                
            }
        }
        void MessageReciever(Socket sender) {
            while(sender.Connected){
                // Data buffer
                byte[] messageReceived = new byte[1024];
                // We receive the message using
                // the method Receive(). This
                // method returns number of bytes
                // received, that we'll use to
                // convert them to string
                int byteRecv = sender.Receive(messageReceived);
                Console.WriteLine("\nMessage from Server -> {0}",
                      Encoding.ASCII.GetString(messageReceived,
                                                 0, byteRecv));
                Console.Write(">");
            }

        }
    }
}
