namespace HareDu.Core.Configuration.Internal
{
    using System;
    using YamlDotNet.Serialization;

    class BrokerConfigYaml
    {
        [YamlMember(Alias = "url")]
        public string BrokerUrl { get; set; }
        
        [YamlMember(Alias = "timeout")]
        public TimeSpan Timeout { get; set; }
        
        [YamlMember(Alias = "username")]
        public string Username { get; set; }
        
        [YamlMember(Alias = "password")]
        public string Password { get; set; }
    }
}