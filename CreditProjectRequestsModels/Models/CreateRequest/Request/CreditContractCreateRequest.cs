namespace CreditProjectRequestsModels.Models.CreateRequest.Request
{
    public class CreditContractCreateRequest
    {
        public int CreditType { get; set; }
        public decimal RequestedAmount { get; set; }
        public string RequestedCurrency { get; set; }
        public decimal AnnualSalary { get; set; }
        public decimal MonthlySalary { get; set; }
        public string CompanyName { get; set; }
        public string Comment { get; set; }
    }
}
