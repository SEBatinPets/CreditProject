using System;

namespace CreditProjectRequestsModels.Models.CreateRequest.Request
{
    /// <summary>
    /// запрос создания заявки на кредит
    /// </summary>
    public class CreditRequestCreateRequest
    {
        public string ApplicationNum { get; set; }
        public DateTime ApplicationDate { get; set; }
        public string BranchBank { get; set; }
        public string BranchBankAddr { get; set; }
        public int CreditManagerId { get; set; }
        public CreditApplicantCreateRequest Applicant { get; set; }
        public CreditContractCreateRequest RequestedCredit { get; set; }
    }
}
