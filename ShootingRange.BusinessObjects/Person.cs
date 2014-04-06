using Repository;

namespace ShootingRange.BusinessObjects
{
    public class Person : IEntity
    {
      public int Id { get; set; }
      public string FirstName { get; set; }
      public string LastName { get; set; }
      public override string ToString()
      {
        return string.Format("FirstName: {0}, LastName: {1}", FirstName, LastName);
      }
    }
}
