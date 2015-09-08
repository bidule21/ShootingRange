using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace ShootingRange.ConfigurationProvider
{
    public class ServiceDeskConfiguration : ConfigurationSection
    {
        [ConfigurationProperty("ParticipationDescriptions", IsDefaultCollection = true)]
        [ConfigurationCollection(typeof(ParticipationDescriptionCollection), AddItemName = "add", ClearItemsName = "clear", RemoveItemName = "remove")]
        public ParticipationDescriptionCollection ParticipationDescriptions
        {
            get { return (ParticipationDescriptionCollection) this["ParticipationDescriptions"]; }
        }
    }

    [ConfigurationCollection(typeof(ParticipationDescription), CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap)]
    public class ParticipationDescriptionCollection : ConfigurationElementCollection
    {
        public ParticipationDescription this[int index]
        {
            get { return (ParticipationDescription) base.BaseGet(index); }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        public IEnumerable<ParticipationDescription> GetAll()
        {
            return from string key in BaseGetAllKeys() select this[key];
        }

        public ParticipationDescription this[string key]
        {
            get { return (ParticipationDescription) base.BaseGet(key); }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new ParticipationDescription();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ParticipationDescription) element).ProgramNumber;
        }
    }

    public class ParticipationDescription : ConfigurationElement
    {
        [ConfigurationProperty("ProgramNumber", IsRequired = true, IsKey = true)]
        public string ProgramNumber
        {
            get { return (string)this["ProgramNumber"]; }
            set { this["ProgramNumber"] = value; }
        }

        [ConfigurationProperty("ProgramName", IsRequired = true, IsKey = false)]
        public string ProgramName
        {
            get { return (string) this["ProgramName"]; }
            set { this["ProgramName"] = value; }
        }

        [ConfigurationProperty("AllowShooterCollectionParticipation", IsRequired = true, IsKey = false)]
        public bool AllowShooterCollectionParticipation
        {
            get { return (bool) this["AllowShooterCollectionParticipation"]; }
            set { this["AllowShooterCollectionParticipation"] = value; }
        }

        [ConfigurationProperty("AllowShooterParticipation", IsRequired = true, IsKey = false)]
        public bool AllowShooterParticipation
        {
            get { return (bool) this["AllowShooterParticipation"]; }
            set { this["AllowShooterParticipation"] = value; }
        }
    }
}