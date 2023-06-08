using ClientSpace;

namespace Messaging
{

    class Program 
    { 
    public static void Main(String[] args)
        {
            try{
            Client client = new Client(Int32.Parse(args[0]));
            } catch(Exception e){
                System.Console.WriteLine("Invalid Port: {0}", args[0]);
            }
        }
    }
}

