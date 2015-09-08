using System.Configuration;
using ShootingRange.ConfigurationProvider;

namespace ShootingRange.ServiceDesk.Configuration
{
    public sealed class ServiceDeskConfiguration : ConfigurationSection, IServiceDeskConfiguration
    {
        [ConfigurationProperty("DefaultParticipation",
            DefaultValue = "Wurst & Brot",
            IsRequired = true,
            IsKey = true)]
        public string DefaultParticipation
        {
            get { return (string)this["DefaultParticipation"]; }
            set { this["DefaultParticipation"] = value; }
        }

        [ConfigurationProperty("ParticipationDescriptions", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof (ParticipationDescriptionCollection),CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap)]
        public ParticipationDescriptionCollection ParticipationDescriptions
        {
            get { return (ParticipationDescriptionCollection) base["ParticipationDescriptions"]; }
        }
    }
}