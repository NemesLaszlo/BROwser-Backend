using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    /// <summary>
    /// Current logged in user access
    /// </summary>
    public interface IUserAccessor
    {
        string GetEmail();
        public string GetUsername();
    }
}
