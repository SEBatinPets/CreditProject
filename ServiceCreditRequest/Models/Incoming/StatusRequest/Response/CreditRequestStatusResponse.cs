using System;

namespace ServiceCreditRequest.Models.Outcoming.Response
{
    public class CreditRequestStatusResponse
    {
        public int Id { get; set; }
        public string ApplicationNum { get; set; }
        public DateTime ApplicationDate { get; set; }
        public string BranchBank { get; set; }
        public string BranchBankAddr { get; set; }
        public int CreditManagerId { get; set; }
        public CreditApplicantStatusResponse Applicant { get; set; }
        public CreditContractStatusResponse RequestedCredit { get; set; }
        public bool? ScoringStatus { get; set; }
        public DateTime? ScoringDate { get; set; }
    }
}
