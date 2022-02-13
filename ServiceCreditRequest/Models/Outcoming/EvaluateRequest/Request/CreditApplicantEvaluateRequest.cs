using System;

namespace ServiceCreditRequest.Models.Outcoming.EvaluateRequest.Request
{
    public class CreditApplicantEvaluateRequest
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime DateBirth { get; set; }
        public string CityBirth { get; set; }
        public string AddressBirth { get; set; }
        public string AddressCurrent { get; set; }
        public int INN { get; set; }
        public int SNILS { get; set; }
        public string PassportNum { get; set; }
    }
}
