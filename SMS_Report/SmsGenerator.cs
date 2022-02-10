using SMS_Report.Models.Domains;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SMS_Report
{
    internal class SmsGenerator
    {
        public string GenerateSms(List<Error> errors, int intervalInMinutes)
        {
            if (errors == null)
                throw new ArgumentNullException(nameof(errors));

            string message = $"Błędy z ostatnich {intervalInMinutes} minut:";

            foreach (var error in errors)
            {
                message += $"\n{error.Date} || {error.Name}: {error.Description}";
            }

            return message;
        }
    }
}
