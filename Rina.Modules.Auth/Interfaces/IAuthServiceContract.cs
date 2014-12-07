using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.Contracts;

namespace Rina.Modules.Auth.Interfaces
{
    [ContractClassFor(typeof(IAuthService))]
    internal abstract class IAuthServiceContract : IAuthService
    {
        Uri IAuthService.AuthUri
        {
            get { return default(Uri); }
        }

        Boolean IAuthService.IsAuth(Uri uri)
        {
            Contract.Requires(uri != null);

            return default(Boolean);
        }
    }
}
