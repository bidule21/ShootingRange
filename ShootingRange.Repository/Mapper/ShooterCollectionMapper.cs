using ShootingRange.BusinessObjects;
using ShootingRange.Entities;

namespace ShootingRange.Repository.Mapper
{
    internal static class ShooterCollectionMapper
    {
        public static t_shootercollection UpdateEntity(this t_shootercollection entity, ShooterCollection shooterCollection)
        {
            entity.CollectionName = shooterCollection.CollectionName;
            entity.ProgramNumber = shooterCollection.ProgramNumber;
            return entity;
        }
    }
}
