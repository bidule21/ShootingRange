using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository;
using ShootingRange.BusinessObjects;
using ShootingRange.Entities;

namespace ShootingRange.Repository.Repositories
{
  public class ParticipationTypeDataStore : IParticipationTypeDataStore
  {
    private SqlRepository<t_participationdescription> _sqlRepository;
    private Func<t_participationdescription, ParticipationType> _selector;

    public ParticipationTypeDataStore(DbContext context)
    {
      _sqlRepository = new SqlRepository<t_participationdescription>(context);
      _selector = e => new ParticipationType
      {
        ParticipationTypeId = e.ParticipationDescriptionId,
        ParticipationTypeName = e.Description
      };
    }

    public void Revert()
    {
      _sqlRepository.Revert();
    }

    public void Create(ParticipationType entity)
    {
      throw new NotImplementedException();
    }

    public ParticipationType FindById(int id)
    {
      return _sqlRepository.Find(_ => _.ParticipationDescriptionId == id).Select(_selector).Single();
    }

    public IEnumerable<ParticipationType> GetAll()
    {
      return _sqlRepository.GetAll().ToList().Select(_selector);
    }

    public void Update(ParticipationType entity)
    {
      throw new NotImplementedException();
    }

    public void Delete(ParticipationType entity)
    {
      throw new NotImplementedException();
    }
  }
}
