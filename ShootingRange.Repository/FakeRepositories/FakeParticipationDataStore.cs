using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository;
using ShootingRange.BusinessObjects;
using ShootingRange.Repository.Repositories;
using ShootingRange.Repository.RepositoryInterfaces;

namespace ShootingRange.Repository.FakeRepositories
{
  public class FakeParticipationDataStore : IParticipationDataStore
  {
    private FakeRepository<Participation> _repository;

    public FakeParticipationDataStore()
    {
      _repository = new FakeRepository<Participation>();

      Participation[] groups = new[]
      {
        new Participation {ParticipationId = 1, ParticipationName = "Eichenlaub"}
      };

      foreach (var g in groups)
      {
        _repository.Insert(g);
      }
    }

    public void Revert()
    {
      throw new NotImplementedException();
    }

    public void Create(Participation group)
    {
      _repository.Insert(group);
    }

    public Participation FindById(int id)
    {
      return _repository.Find(_ => _.ParticipationId == id).Single();
    }

    public IEnumerable<Participation> GetAll()
    {
      return _repository.GetAll();
    }

    public void Update(Participation group)
    {
      Participation entity = FindById(group.ParticipationId);
      entity.ParticipationName = group.ParticipationName;
      _repository.Commit();
    }

    public void Delete(Participation group)
    {
      _repository.Delete(group);
    }

    public IEnumerable<Participation> FindByParticipationName(string participationName)
    {
      return
        _repository.Find(g => string.Compare(g.ParticipationName, participationName, StringComparison.OrdinalIgnoreCase) == 0);
    }
  }
}
