using System.Configuration;

namespace ShootingRange.ServiceDesk.Configuration
{
    public class ParticipationDescription : ConfigurationElement
    {
        /// <summary>
        /// Gets the Type setting.
        /// </summary>
        [ConfigurationProperty("Identifier", IsKey = true, IsRequired = true)]
        public int Identifier
        {
            get { return (int)this["Identifier"]; }
            set { this["Identifier"] = value; }
        }
    }
}