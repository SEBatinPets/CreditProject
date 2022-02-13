using System;

namespace ServiceCreditRequest.Models.ScoringResult.Request
{
    public class ScoringResultRequest
    {
        public int Id { get; set; }
        public string ApplicationNum { get; set; }
        public bool ScoringStatus { get; set; }
        public DateTime ScoringDate { get; set; }
    }
}
