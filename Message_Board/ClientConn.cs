using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

public delegate void Notify();

namespace Messaging
{
public class ClientConn {

    Socket conn;
    public EventHandler<string> msgSend;
    public ClientConn(Socket client){
            this.conn = client;
           
        }

    public void recieveMessages(){
        byte[] buff = new byte[1024];
        while(true)
        {
            string data = null;
            Array.Clear(buff, 0, buff.Length);
            int numByte = conn.Receive(buff);
            data += Encoding.ASCII.GetString(buff, 0, numByte);
            if (data.IndexOf("<EOF>") > -1 )
            {
                data = data.Substring(0, data.Length - 5);
            }
            NotifyClients(data);
            Console.WriteLine("Message: {0}", data);
        }
    }

    public void sendMsg(string msg){
        conn.Send(Encoding.ASCII.GetBytes(msg));
    }

    public void NotifyClients(string msg){
        msgSend?.Invoke(this, msg);
    }

    public Socket getConn(){
        return conn;
    }
}
}
