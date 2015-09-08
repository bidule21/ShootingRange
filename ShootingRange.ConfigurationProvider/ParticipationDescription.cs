using System.Configuration;

namespace ShootingRange.ConfigurationProvider
{
    public class ParticipationDescription : ConfigurationElement
    {
        [ConfigurationProperty("ProgramNumber", IsKey = true, IsRequired = true)]
        public int ProgramNumber
        {
            get { return (int)this["ProgramNumber"]; }
            set { this["ProgramNumber"] = value; }
        }

        [ConfigurationProperty("ProgramName", IsKey = false, IsRequired = true)]
        public string ProgramName
        {
            get { return (string) this["ProgramName"]; }
            set { this["ProgramName"] = value; }
        }

        [ConfigurationProperty("AllowShooterCollectionParticipation", IsKey = false, IsRequired = true)]
        public bool AllowShooterCollectionParticipation
        {
            get { return (bool) this["AllowShooterCollectionParticipation"]; }
            set { this["AllowShooterCollectionParticipation"] = value; }
        }

        [ConfigurationProperty("AllowShooterParticipation", IsKey = false, IsRequired = true)]
        public bool AllowShooterParticipation
        {
            get { return (bool) this["AllowShooterParticipation"]; }
            set { this["AllowShooterParticipation"] = value; }
        }
    }
}