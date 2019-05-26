using System;
using System.Collections.Generic;
using System.Text;

namespace SKS.ParcelLogistics.DataAccess.SQL
{
    public class DalException: Exception
    {
        public DalException(string message, Exception inner) : base(message, inner)
        {
            
        }
    }
}
