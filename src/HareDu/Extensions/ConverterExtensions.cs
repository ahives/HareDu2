namespace HareDu.Extensions
{
    using System;
    using Model;

    public static class ConverterExtensions
    {
        public static string ConvertTo(this HighAvailabilityModes mode)
        {
            switch (mode)
            {
                case HighAvailabilityModes.All:
                    return "all";
                    
                case HighAvailabilityModes.Exactly:
                    return "exactly";
                    
                case HighAvailabilityModes.Nodes:
                    return "nodes";
                    
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }
        }

        public static string ConvertTo(this AckMode mode)
        {
            switch (mode)
            {
                case AckMode.NoAck:
                    return "no-ack";
                    
                case AckMode.OnConfirm:
                    return "on-confirm";
                    
                case AckMode.OnPublish:
                    return "on-publish";
                    
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }
        }

        public static string ConvertTo(this AckMode? mode)
        {
            switch (mode)
            {
                case AckMode.NoAck:
                    return "no-ack";
                    
                case AckMode.OnConfirm:
                    return "on-confirm";
                    
                case AckMode.OnPublish:
                    return "on-publish";
                    
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }
        }

        public static string ConvertTo(this ShovelProtocolType protocol)
        {
            switch (protocol)
            {
                case ShovelProtocolType.Amqp091:
                    return "amqp091";
                    
                case ShovelProtocolType.Amqp10:
                    return "amqp10";
                    
                default:
                    throw new ArgumentOutOfRangeException(nameof(protocol), protocol, null);
            }
        }

        public static string ConvertTo(this ShovelType shovelType)
        {
            switch (shovelType)
            {
                case ShovelType.Dynamic:
                    return "dynamic";
                    
                case ShovelType.Static:
                    return "static";
                    
                default:
                    throw new ArgumentOutOfRangeException(nameof(shovelType), shovelType, null);
            }
        }

        public static string ConvertTo(this ShovelState shovelState)
        {
            switch (shovelState)
            {
                case ShovelState.Running:
                    return "running";
                    
                case ShovelState.Starting:
                    return "starting";
                    
                case ShovelState.Terminated:
                    return "terminated";
                    
                default:
                    throw new ArgumentOutOfRangeException(nameof(shovelState), shovelState, null);
            }
        }

        public static string ConvertTo(this DeleteShovelMode mode)
        {
            switch (mode)
            {
                case DeleteShovelMode.Never:
                    return "never";
                
                case DeleteShovelMode.QueueLength:
                    return "queue-length";
                    
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }
        }

        public static HighAvailabilityModes ConvertTo(this string mode)
        {
            switch (mode.ToLower())
            {
                case "all":
                    return HighAvailabilityModes.All;
                    
                case "exactly":
                    return HighAvailabilityModes.Exactly;
                    
                case "nodes":
                    return HighAvailabilityModes.Nodes;
                    
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }
        }

        public static string ConvertTo(this HighAvailabilitySyncModes mode)
        {
            switch (mode)
            {
                case HighAvailabilitySyncModes.Manual:
                    return "manual";
                    
                case HighAvailabilitySyncModes.Automatic:
                    return "automatic";
                    
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }
        }
    }
}