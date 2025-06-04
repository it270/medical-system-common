using It270.MedicalSystem.Common.Application.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace It270.MedicalSystem.Common.Application.Core.Entities.PDFinfo
{
    public class PDFinfo
    {
        public int CenterId { get; set; }
        public string CenterName { get; set; }
        public string CenterLogo { get; set; }
        public string CenterNit { get; set; }
        public string CenterCity { get; set; }
        public string CenterPresidentName { get; set; }
        public string Protocol { get; set; }
        public List<string> Description { get; set; }
        public string StudyPhase { get; set; }
        public List<Subject> Subjects { get; set; }
        public string SigningInvestigatorName { get; set; }
        public string FilingNumber { get; set; }
        public string CenterAddress { get; set; }
    }

    public class Subject
    {
        public int Id { get; set; }
        public int CiomsId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string Observacion { get; set; }
        public string TrackingNumber { get; set; }
        public string SubjectNumber { get; set; }
        public string Event { get; set; }
        public int CountryId { get; set; }
        public DateTime EventDate { get; set; }
        public DateTime ReceiptDate { get; set; }
        public int CiomsReportCausalityId { get; set; }
        public int CiomsReportTypeId { get; set; }
    }

}
