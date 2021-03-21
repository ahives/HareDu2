namespace HareDu.IntegrationTesting.Diagnostics.Observers
{
    using System;
    using Core.Extensions;
    using Extensions;
    using HareDu.Diagnostics;
    using Serialization;

    public class DefaultDiagnosticConsoleLogger :
        IObserver<ProbeContext>
    {
        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            Console.WriteLine(error.Message);
        }

        public void OnNext(ProbeContext value)
        {
            Console.WriteLine("Probe Id: {0}", value.Result.Id);
            Console.WriteLine("Probe: {0}", value.Result);
            Console.WriteLine("Timestamp: {0}", value.Timestamp.ToString());
            Console.WriteLine("Component Id: {0}", value.Result.ComponentId);
            Console.WriteLine("Component Type: {0}", value.Result.ComponentType);
            Console.WriteLine("Status: {0}", value.Result.Status);
            Console.WriteLine("Data => {0}", value.Result.Data.ToJsonString(Deserializer.Options));

            if (value.Result.Status == ProbeResultStatus.Unhealthy)
            {
                Console.WriteLine("Reason: {0}", value.Result.KB.Reason);
                Console.WriteLine("Remediation: {0}", value.Result.KB.Remediation);
            }

            Console.WriteLine();
        }
    }
}