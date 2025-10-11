namespace Infrastructure.Configurations
{
    public class KafkaConfiguration
    {
        public string BootstrapServers { get; set; } = null!;
        public string GroupId { get; set; } = null!;
        public string Topic { get; set; } = null!;

    }
}
