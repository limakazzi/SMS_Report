using SMS_Report.Models.Domains;
using System.Collections.Generic;
using System.Linq;

namespace SMS_Report.Repositories
{
    internal class ErrorRepository
    {
        public List<Error> GetNotSentErrorsFromDatabase()
        {
            using (var context = new ServiceDbContext())
            {
                var errors = context
                    .Errors
                    .Where(x => x.IsSent == false)
                    .ToList();

                return errors;
            }
        }

        public void UpdateErrorStatus(List<Error> errors)
        {
            using (var context = new ServiceDbContext())
            {
                foreach (var error in errors)
                {
                    var errorsToUpdate = context.Errors.Where(x => x.Id == error.Id).FirstOrDefault();
                    errorsToUpdate.IsSent = true;
                }
                context.SaveChanges();
            }
        }
    }
}
