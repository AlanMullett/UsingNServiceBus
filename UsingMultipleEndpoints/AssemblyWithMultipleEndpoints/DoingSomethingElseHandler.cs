using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyWithMultipleEndpoints
{
    using System.Diagnostics;
    using EndpointConfiguration;
    using Messages;
    using NServiceBus;

    public class DoingSomethingElseHandler : IHandleMessages<DoSomethingElse>
    {
        public IBus Bus { get; set; }

        public void Handle(DoSomethingElse message)
        {
            Debug.WriteLine("Handling the DoSomethingElse message");

            Bus.CurrentMessageContext.Headers[Headers.ReplyToAddress] = SecondHandlerInSagaEndpoint.EndpointName;
            Bus.Reply<SomethingElseComplete>(r => r.ProcessId = message.ProcessId);
        }
    }
}
