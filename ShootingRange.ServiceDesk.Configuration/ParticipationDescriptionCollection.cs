using System.Configuration;

namespace ShootingRange.ServiceDesk.Configuration
{
    [ConfigurationCollection(typeof(ParticipationDescription),
    CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap)]
    public class ParticipationDescriptionCollection : ConfigurationElementCollection
    {

        public ParticipationDescription this[int index]
        {
            get
            {
                return base.BaseGet(index) as ParticipationDescription;
            }
            set
            {
                if (base.BaseGet(index) != null)
                {
                    base.BaseRemoveAt(index);
                }
                this.BaseAdd(index, value);
            }
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.AddRemoveClearMap; }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new ParticipationDescription();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ParticipationDescription) element).Identifier;
        }

        public void Add(ParticipationDescription thing)
        {
            base.BaseAdd(thing);
        }

        public void Remove(string name)
        {
            base.BaseRemove(name);
        }

        public void Remove(ParticipationDescription thing)
        {
            base.BaseRemove(GetElementKey(thing));
        }

        public void Clear()
        {
            base.BaseClear();
        }

        public void RemoveAt(int index)
        {
            base.BaseRemoveAt(index);
        }

        public string GetKey(int index)
        {
            return (string)base.BaseGetKey(index);
        }
    }
}