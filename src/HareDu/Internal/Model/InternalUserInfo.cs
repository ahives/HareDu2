namespace HareDu.Internal.Model
{
    using HareDu.Model;

    class InternalUserInfo :
        UserInfo
    {
        public InternalUserInfo(UserInfoImpl obj)
        {
            Username = obj.Username;
            PasswordHash = obj.PasswordHash;
            HashingAlgorithm = obj.HashingAlgorithm;
            Tags = obj.Tags;
        }

        public string Username { get; }
        public string PasswordHash { get; }
        public string HashingAlgorithm { get; }
        public string Tags { get; }
    }
}