using System;

namespace CreditProjectRequestsModels.Models.CreateRequest.Response
{
    /// <summary>
    /// ответ запросу создания заявки на кредит
    /// </summary>
    public class CreateResponse
    {
        public int Id { get; set; }
        public string ApplicationNum { get; set; }
    }
}
