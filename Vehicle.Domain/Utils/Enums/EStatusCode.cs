using System;
using System.Collections.Generic;
using System.Text;

namespace Questor.Vehicle.Domain.Utils.Enums
{
    public enum EStatusCode
    {
        Success = 200,
        Unauthorized = 401,
        Forbidden = 403,
        NotFound = 404,
        InvalidData = 405,
        Conflict = 409,
        Error = 500
    }
}
