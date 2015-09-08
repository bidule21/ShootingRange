using System.Configuration;

namespace ShootingRange.ConfigurationProvider
{
    [ConfigurationCollection(typeof(ParticipationDescription),
    CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap)]
    public class ParticipationDescriptionCollection : ConfigurationElementCollection
    {
        public ParticipationDescription GetByKey(int key)
        {
            return (ParticipationDescription)base.BaseGet((object)key);
        }

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
            return ((ParticipationDescription) element).ProgramNumber;
        }

        public void Add(ParticipationDescription item)
        {
            base.BaseAdd(item);
        }

        public void Remove(int key)
        {
            base.BaseRemove(key);
        }

        public void Remove(ParticipationDescription particiaptionDescription)
        {
            base.BaseRemove(GetElementKey(particiaptionDescription));
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