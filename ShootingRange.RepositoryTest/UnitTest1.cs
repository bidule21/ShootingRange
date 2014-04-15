using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShootingRange.BusinessObjects;
using ShootingRange.Persistence;
using ShootingRange.Repository;

namespace ShootingRange.RepositoryTest
{
  [TestClass]
  public class UnitTest1
  {
    [TestMethod]
    public void TestMethod1()
    {
      IPersonDataStore store = new PersonDataStore(new ShootingRangeEntities());
      Person person = store.FindByFirstName("Dan").First();
      person.FirstName = "Faronel";
      store.Update(person);
    }
  }
}
