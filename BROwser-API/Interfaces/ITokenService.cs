using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BROwser_API.Interfaces
{
    /// <summary>
    /// Token Service to create token with the role and other informations
    /// Token refresh option for more secure handling
    /// </summary>
    public interface ITokenService
    {
        Task<string> CreateToken(AppUser user);
        RefreshToken GenerateRefreshToken();
    }
}
