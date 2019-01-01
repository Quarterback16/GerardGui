using Gerard.Messages;
using RosterLib.Interfaces;

namespace Butler.Messaging
{
    public class ShuttleSender : ISend
    {
        public ShuttleSender()
        {
            //  setup the bus see how at InternetScanner.sln
            //  1) need serviceBus config in App.config that
            //     names the queue (gerard-server-work)
            //     and the type of messages sent to it
        }
        public void Send(ICommand command)
        {
            //  put a message on the bus
            System.Console.WriteLine($"Sent {command}");
        }
    }
}
