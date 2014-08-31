using ShootingRange.BusinessObjects;

namespace ShootingRange.Repository.RepositoryInterfaces
{
  public interface IProgramItemDataStore : IDataStore<ProgramItem, int>
  {
    ProgramItem FindByProgramNumber(int programNumber);
  }
}
