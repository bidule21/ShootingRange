using Repository;

namespace ShootingRange.BusinessObjects
{
  public class Person
  {
    public int PersonId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public override string ToString()
    {
      return string.Format("LastName: {0}, FirstName: {1}", LastName, FirstName);
    }
  }
}
