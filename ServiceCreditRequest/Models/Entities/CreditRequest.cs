using System;

namespace ServiceCreditRequest.Models.Entities
{
    public class CreditRequest
    {
        public int Id { get; set; }
        public string ApplicationNum { get; set; }
        public DateTime ApplicationDate { get; set; }
        public string BranchBank { get; set; }
        public string BranchBankAddr { get; set; }
        public int CreditManagerId { get; set; }
        public CreditApplicant Applicant { get; set; }
        public CreditContract RequestedCredit { get; set; }
        public bool? ScoringStatus { get; set; }
        public DateTime? ScoringDate { get; set; }
    }
}
