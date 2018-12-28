using Gerard.Messages;
using RosterLib.Interfaces;

namespace Gerard.Tests.Fakes
{
    public class FakeSender : ISend
    {
        public void Send(ICommand command)
        {
            System.Console.WriteLine($"Command : {command} sent");
        }
    }
}
