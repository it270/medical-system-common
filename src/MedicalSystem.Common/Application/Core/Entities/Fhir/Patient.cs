using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace It270.MedicalSystem.Common.Application.Core.Entities.Fhir
{
    public class Patient
    {
        #region PatientData
        public int? Id { get; set; }
        public int patientTypeDocumentId { get; set; }
        public string IdVersion { get; set; }
        public string LastUpdated { get; set; }
        public string NameGiven { get; set; }
        public string Namefamily { get; set; }
        public DateTime BirthDate { get; set; }
        public string CommunicationCode { get; set; }
        public string IdentifierValue { get; set; }
        public string IdentifierCode { get; set; }
        public string TelecomPhoneValue { get; set; }
        public string TelecomSmsValue { get; set; }
        public string TelecomEmailValue { get; set; }
        public string Gender { get; set; }
        public string patientDayDeceased { get; set; }
        public string patientBloodType { get; set; }
        public string MaritalStatus { get; set; }
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
        public string PatientCallAs { get; set; }
        public string patientClassifyAs { get; set; }
        public string patientAddressComplement { get; set; }
        public bool? Active { get; set; }
        public bool Simple { get; set; } = false;
        #endregion

        #region geograpihc patiente
        public string adressCountry { get; set; }
        public string AdressCityResident { get; set; }
        public string patientNationality { get; set; }
        public string BornCity { get; set; }
        public string patientNeighborhood { get; set; }
        #endregion

        public List<Disability> patientTypeDisability { get; set; }


    }
    public class Disability
    {
        public string TypeDisability { get; set; }
        public string IdTypeDisability { get; set; }
        public string GradeDisability { get; set; }
        public string IdGradeDisability { get; set; }
        public string NameDisability { get; set; }
        public string Aftermath { get; set; }
        public string InitDate { get; set; }
        public string EndDate { get; set; }

    }
}
