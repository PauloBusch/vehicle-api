using Questor.Vehicle.Domain.Utils.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Vehicle.IntegrationTests.Utils
{
    public class MutationResultTest<TData> where TData : class
    {
        public EStatusCode Status { get; set; }
        public int TotalRows { get; set; }
        public TData Data { get; set; }
        public string Message { get; set; }
    }
}
