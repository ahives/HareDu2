namespace HareDu.Extensions
{
    using System;

    public static class EnumConversionExtensions
    {
        public static string Convert(this DeleteShovelMode mode)
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

        public static string Convert(this HighAvailabilityModes mode)
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

        public static HighAvailabilityModes Convert(this string mode)
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

        public static string Convert(this HighAvailabilitySyncMode mode)
        {
            switch (mode)
            {
                case HighAvailabilitySyncMode.Manual:
                    return "manual";
                    
                case HighAvailabilitySyncMode.Automatic:
                    return "automatic";
                    
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }
        }

        public static string Convert(this QueueMode mode)
        {
            switch (mode)
            {
                case QueueMode.Default:
                    return "default";
                
                case QueueMode.Lazy:
                    return "lazy";
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }
        }

        public static string Convert(this QueuePromotionFailureMode mode)
        {
            switch (mode)
            {
                case QueuePromotionFailureMode.Always:
                    return "always";
                
                case QueuePromotionFailureMode.WhenSynced:
                    return "when-synced";
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }
        }

        public static string Convert(this QueuePromotionShutdownMode mode)
        {
            switch (mode)
            {
                case QueuePromotionShutdownMode.Always:
                    return "always";
                
                case QueuePromotionShutdownMode.WhenSynced:
                    return "when-synced";
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }
        }
    }
}