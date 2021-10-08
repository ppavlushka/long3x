namespace long3x.Common.ConfigurationModels
{
    public class SshConnectionSettings
    {
        public bool UseSshConnection { get; set; }

        public string HostName { get; set; }

        public string UserName { get; set; }

        public string SshKeyFile { get; set; }
    }
}
