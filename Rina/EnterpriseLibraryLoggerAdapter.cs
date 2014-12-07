using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.Diagnostics;

namespace Rina
{
    // todo: Are we still using this?
    public class EnterpriseLibraryLoggerAdapter : ILoggerFacade
    {
        #region ILoggerFacade Members

        public EnterpriseLibraryLoggerAdapter()
        {
        }

        public void Log(string message, Category category, Priority priority)
        {
            //Logger.Write(message, category.ToString(), (int)priority);
            Trace.Write(message);
        }

        #endregion
    }
}
