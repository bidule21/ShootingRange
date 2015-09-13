using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ShootingRange.BusinessObjects;
using ShootingRange.BusinessObjects.Properties;

namespace ShootingRange.ServiceDesk.ViewModel
{
    public class UiCollectionShooter
    {
        public UiCollectionShooter()
        {
            
        }

        public UiCollectionShooter(Person person, Shooter shooter, int score)
        {
            PersonId = person.PersonId;
            ShooterId = shooter.ShooterId;
            FirstName = person.FirstName;
            LastName = person.LastName;
            ShooterNumber = shooter.ShooterNumber;
            DateOfBirth = person.DateOfBirth;
            CollectionShooterScore = score;
        }

        public DateTime? DateOfBirth { get; set; }

        public string LastName { get; set; }

        public int ShooterNumber { get; set; }

        public string FirstName { get; set; }

        public int ShooterId { get; set; }

        public int PersonId { get; set; }

        public int CollectionShooterScore { get; set; }
    }
}