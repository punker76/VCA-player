using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Diagnostics.Contracts;

namespace Rina.Modules.Auth.Interfaces
{
    [ContractClass(typeof(IAuthServiceContract))]
    public interface IAuthService
    {
        Uri AuthUri { get; }
        Boolean IsAuth(Uri uri);
    }
}
