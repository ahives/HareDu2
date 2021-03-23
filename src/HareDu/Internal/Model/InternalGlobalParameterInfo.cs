namespace HareDu.Internal.Model
{
    using HareDu.Model;

    class InternalGlobalParameterInfo :
        GlobalParameterInfo
    {
        public InternalGlobalParameterInfo(GlobalParameterInfoImpl obj)
        {
            Name = obj.Name;
            Value = obj.Value;
        }

        public string Name { get; }
        public object Value { get; }
    }
}