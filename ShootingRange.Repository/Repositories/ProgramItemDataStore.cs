using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Repository;
using ShootingRange.BusinessObjects;
using ShootingRange.Entities;
using ShootingRange.Repository.Mapper;
using ShootingRange.Repository.RepositoryInterfaces;

namespace ShootingRange.Repository.Repositories
{
  public class ProgramItemDataStore : IProgramItemDataStore
  {
    private SqlRepository<t_programitem> _sqlRepository;
    private Func<t_programitem, ProgramItem> _selector;

    public ProgramItemDataStore(DbContext context)
    {
      _sqlRepository = new SqlRepository<t_programitem>(context);
      _selector = programItem => new ProgramItem
      {
        ProgramItemId = programItem.ProgramItemId,
        ProgramName = programItem.ItemName,
        ProgramNumber = programItem.ProgramNumber
      };
    }

    public void Revert()
    {
      _sqlRepository.Revert();
    }

    public void Create(ProgramItem programItem)
    {
      t_programitem entity = new t_programitem();
      entity.UpdateEntity(programItem);
      _sqlRepository.Insert(entity);
      programItem.ProgramItemId = entity.ProgramItemId;
    }

    public ProgramItem FindById(int id)
    {
      return _sqlRepository.Find(_ => _.ProgramItemId == id).Select(_selector).Single();
    }

    public IEnumerable<ProgramItem> GetAll()
    {
      return _sqlRepository.GetAll().ToList().Select(_selector);
    }

    public void Update(ProgramItem programItem)
    {
      t_programitem entity = _sqlRepository.Find(_ => _.ProgramItemId == programItem.ProgramItemId).Single();
      entity.UpdateEntity(programItem);
      _sqlRepository.Commit();
    }

    public void Delete(ProgramItem programItem)
    {
      t_programitem entity = _sqlRepository.Find(_ => _.ProgramItemId == programItem.ProgramItemId).Single();
      _sqlRepository.Delete(entity);
    }

    public ProgramItem FindByProgramNumber(int programNumber)
    {
      return GetAll().SingleOrDefault(_ => _.ProgramNumber == programNumber);
    }
  }
}
