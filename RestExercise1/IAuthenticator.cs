using System;
using System.Collections.Generic;
using System.Text;

namespace AktieHandelRepositoryLib
{
    public interface IAuthenticator
    {
        bool Authenticate(string username, string password);
    }
}
