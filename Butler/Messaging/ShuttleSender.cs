using Gerard.Messages;
using RosterLib.Interfaces;
using Shuttle.Core.StructureMap;
using Shuttle.Esb;
using StructureMap;
using System;

namespace Butler.Messaging
{
    public class ShuttleSender : ISend
    {
        public IServiceBus Bus { get; set; }

        public ShuttleSender()
        {
            //  setup the bus see how at InternetScanner.sln
            //  1) need serviceBus config in App.config that
            //     names the queue (gerard-server-work)
            //     and the type of messages sent to it
            //Logger = logger;
            Bus = StartTheBus();
        }

        public void Send(ICommand command)
        {
            if (Bus == null)
            {
                //TODO: Inject logging latter
                //Logger.Warning(
                Console.WriteLine(
                    "There is no bus for the messages");
                return;
            }
            try
            {
                //  put a message on the bus
                Bus.Send(command);

                Console.WriteLine(
                    $"Sent {command}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"Exception on {command}");
                //Logger.Error(
                Console.WriteLine(
                    $"ShuttleSender.Send: {ex.Message}");
                throw;
            }
        }

        private static IServiceBus StartTheBus()
        {
            var smRegistry = new Registry();
            var registry = new StructureMapComponentRegistry(
                smRegistry);

            ServiceBus.Register(registry);

            var bus = ServiceBus.Create(
                resolver: new StructureMapComponentResolver(
                                new Container(smRegistry)))
                .Start();
            return bus;
        }
    }
}
