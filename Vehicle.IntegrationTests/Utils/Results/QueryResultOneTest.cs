using Questor.Vehicle.Domain.Utils.Enums;
using Questor.Vehicle.Domain.Utils.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Vehicle.IntegrationTests.Utils.Results
{
    public class QueryResultOneTest<T> where T : IViewModel
    {
        public EStatusCode Status { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }
}
