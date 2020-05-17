﻿using System;
using System.ComponentModel;

namespace Questor.Vehicle.Domain.Utils.Extensions
{
    public static class EnumExtensions
    {
        public static string ToDescriptionString(this Enum value)
        {
            DescriptionAttribute[] attributes = (DescriptionAttribute[])value
               .GetType()
               .GetField(value.ToString())
               .GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }
    }
}
