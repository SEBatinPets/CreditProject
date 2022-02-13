using System;

namespace ServiceCreditRequest.Models.Outcoming.EvaluateRequest.Request
{
    public class CreditRequestEvaluateRequest
    {
        public int Id { get; set; }
        public string ApplicationNum { get; set; }
        public DateTime ApplicationDate { get; set; }
        public string BranchBank { get; set; }
        public string BranchBankAddr { get; set; }
        public int CreditManagerId { get; set; }
        public CreditApplicantEvaluateRequest Applicant { get; set; }
        public CreditContractEvaluateRequest RequestedCredit { get; set; }
    }
}
