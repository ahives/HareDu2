namespace HareDu.Diagnostics.Formatting
{
    using System.Text;

    public static class FormatterExtensions
    {
        public static string Format(this DiagnosticReport report)
        {
            var builder = new StringBuilder();

            builder.AppendLine($"Report Identifier: {report.Identifier.ToString()}");
            builder.AppendLine($"Timestamp: {report.Timestamp.ToString()}");
            builder.AppendLine();

            builder.AppendLine("Results");
            
            foreach (var result in report.Results)
            {
                Format(result, ref builder);
            }

            return builder.ToString();
        }
        
        static void Format(DiagnosticSensorData data, ref StringBuilder builder) =>
            builder.AppendLine($"\t\t{data?.PropertyName} => {data?.PropertyValue}");

        static void Format(this DiagnosticResult result, ref StringBuilder builder)
        {
            builder.AppendLine($"\tTimestamp: {result.Timestamp.ToString()}");
            builder.AppendLine($"\tComponent Identifier: {result.ComponentIdentifier}");
            builder.AppendLine($"\tComponent Type: {result.ComponentType.ToString()}");
            builder.AppendLine($"\tSensor: {result.SensorIdentifier}");
            builder.AppendLine($"\tStatus: {result.Status.ToString()}");
            builder.AppendLine("\tSensor Data");
            
            foreach (var data in result.SensorData)
            {
                Format(data, ref builder);
            }

            builder.AppendLine($"\tReason: {result.Reason}");
            builder.AppendLine($"\tRemediation: {result.Remediation}");
            builder.AppendLine();
        }
    }
}