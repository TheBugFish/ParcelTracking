using FluentValidation.Results;
using System;
using System.Collections.Generic;

namespace SKS.ParcelLogistics.BusinessLogic
{
    public class BLException : Exception
    {
        public BLException(string message, Exception innerEx) : base(message, innerEx) { }
        public BLException(IList<ValidationFailure> failures, Exception innerEx) : base(ProcessFailures(failures), innerEx) { }

        private static string ProcessFailures(IList<ValidationFailure> failures)
        {
            string failDesc = "";
            foreach (ValidationFailure fail in failures)
            {
                failDesc += string.Format("ValidationFailure: {0}\n", fail.ErrorMessage);
            }
            return failDesc;
        }
    }
}
