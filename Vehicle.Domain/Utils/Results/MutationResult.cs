using Newtonsoft.Json;
using Questor.Vehicle.Domain.Utils.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Questor.Vehicle.Domain.Utils.Results
{
    public class MutationResult
    {
        [JsonIgnore] public EStatusCode Status { get; private set; }
        public int TotalRows { get; private set; }
        public dynamic Data { get; private set; }
        public string Message { get; private set; }

        public MutationResult (int totalRows)
        {
            this.Status = EStatusCode.Success;
            this.TotalRows = totalRows;
        }

        public MutationResult(
            EStatusCode status,
            string message = null,
            dynamic data = null
        ) {
            this.Data = data;
            this.Status = status;
            this.Message = message;
        }
    }
}
