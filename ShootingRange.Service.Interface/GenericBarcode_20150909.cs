using System;
using System.Collections.Generic;

namespace ShootingRange.Service.Interface
{
    public class GenericBarcode_20150909
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Barcode { get; set; }
        public List<Tuple<string, string>> ParticipationTypeToCollectionName { get; set; }

        public List<string> Participations { get; set; }
    }
}
