namespace Soundify.Configuration
{
    public static class RabbitMqConfig
    {
        private static class Keys
        {
            private const string GroupName = "RabbitMQ";
            public const string QueueNameKey = GroupName + ":QueueName";
            public const string HostKey = GroupName + ":Host";
            public const string PortKey = GroupName + ":Port";
            public const string UserKey = GroupName + ":User";
            public const string PasswordKey = GroupName + ":Password";
        }

        public static class Values
        {
            public static readonly string QueueName;
            public static readonly string Host;
            public static readonly int Port;
            public static readonly string User;
            public static readonly string Password;

            static Values()
            {
                var configuration = ConfigBase.GetConfiguration();
                
                QueueName = configuration[Keys.QueueNameKey];
                Host = configuration[Keys.HostKey];
                Port = int.Parse(configuration[Keys.PortKey] ?? string.Empty);
                User = configuration[Keys.UserKey];
                Password = configuration[Keys.PasswordKey];
            }
        }
    }
}