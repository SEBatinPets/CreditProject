﻿namespace ServiceCreditRequest.Models.Outcoming.Response
{
    public class CreditContractStatusResponse
    {
        public int CreditType { get; set; }
        public decimal RequestedAmount { get; set; }
        public string RequestedCurrency { get; set; }
    }
}
