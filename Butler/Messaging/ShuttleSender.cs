using Gerard.Messages;
using NLog;
using RosterLib.Interfaces;
using Shuttle.Core.StructureMap;
using Shuttle.Esb;
using StructureMap;
using System;

namespace Butler.Messaging
{
    public class ShuttleSender : ISend
    {
        private readonly Logger _logger;

        public IServiceBus Bus { get; set; }

        public ShuttleSender(Logger logger)
        {
            //  setup the bus see how at InternetScanner.sln
            //  1) need serviceBus config in App.config that
            //     names the queue (gerard-server-work)
            //     and the type of messages sent to it
            _logger = logger;
            Bus = StartTheBus();
        }

        public void Send(ICommand command)
        {
            if (Bus == null)
            {
                Warning("There is no bus for the messages");
                return;
            }
            try
            {
                Info($"Sending {command}");
                //  put a message on the bus
                Bus.Send(command);

                Info($"Sent {command}");
            }
            catch (Exception ex)
            {
                Error(
                    $@"Exception on {
                        command
                        } in ShuttleSender.Send: {
                        ex.Message
                        }",
                    ex);
                throw;
            }
        }

        private void Trace(string msg)
        {
            Console.WriteLine(msg);
            _logger.Trace(msg);
        }

        private void Info(string msg)
        {
            Console.WriteLine(msg);
            _logger.Info(msg);
        }

        private void Warning(string msg)
        {
            Console.WriteLine(msg);
            _logger.Warn(msg);
        }

        private void Error(string msg, Exception ex)
        {
            Console.WriteLine(msg);
            _logger.ErrorException(msg, ex);
        }

        private IServiceBus StartTheBus()
        {
            Info("Starting up the Bus");
            var smRegistry = new Registry();
            var registry = new StructureMapComponentRegistry(
                smRegistry);

            ServiceBus.Register(registry);

            var bus = ServiceBus.Create(
                resolver: new StructureMapComponentResolver(
                                new Container(smRegistry)))
                .Start();
            Info("Bus Started");
            return bus;
        }
    }
}
