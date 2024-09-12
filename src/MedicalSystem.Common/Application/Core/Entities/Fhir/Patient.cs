using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace It270.MedicalSystem.Common.Application.Core.Entities.Fhir
{

    public class Patient
    {
        public int total { get; set; }
        public Listsearch[] listSearch { get; set; }
    }

    public class Listsearch
    {
        public int id { get; set; }
        public int patientTypeDocumentId { get; set; }
        public string idVersion { get; set; }
        public string lastUpdated { get; set; }
        public string nameGiven { get; set; }
        public string namefamily { get; set; }
        public DateTime birthDate { get; set; }
        public string communicationCode { get; set; }
        public string identifierValue { get; set; }
        public string identifierCode { get; set; }
        public string telecomPhoneValue { get; set; }
        public string telecomSmsValue { get; set; }
        public string telecomEmailValue { get; set; }
        public string gender { get; set; }
        public string patientDayDeceased { get; set; }
        public string patientBloodType { get; set; }
        public string maritalStatus { get; set; }
        public string patientBirthGender { get; set; }
        public string patientGender { get; set; }
        public string patientReligion { get; set; }
        public string patientEducationalLevel { get; set; }
        public string patientOccupation { get; set; }
        public string patientEthnicGroup { get; set; }
        public string patientZoneType { get; set; }
        public string patientResidenceAddress { get; set; }
        public string patientAuthorizeEmail { get; set; }
        public string patientAuthorizeSms { get; set; }
        public string patientAdvanceWill { get; set; }
        public string patientOrganDonor { get; set; }
        public string patientSpecialPopulation { get; set; }
        public string patientCallAs { get; set; }
        public string patientClassifyAs { get; set; }
        public string patientAddressComplement { get; set; }
        public bool active { get; set; }
        public bool simple { get; set; }
        public string adressCountry { get; set; }
        public string adressCityResident { get; set; }
        public string patientNationality { get; set; }
        public string bornCity { get; set; }
        public string patientNeighborhood { get; set; }
        public Patienttypedisability[] patientTypeDisability { get; set; }
    }

    public class Patienttypedisability
    {
        public string typeDisability { get; set; }
        public string idTypeDisability { get; set; }
        public string gradeDisability { get; set; }
        public string idGradeDisability { get; set; }
        public string nameDisability { get; set; }
        public string aftermath { get; set; }
        public string initDate { get; set; }
        public object endDate { get; set; }
    }

}
