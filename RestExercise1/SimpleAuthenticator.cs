using System;
using System.Collections.Generic;
using System.Text;

namespace AktieHandelRepositoryLib
{
    public class SimpleAuthenticator : IAuthenticator
    {
        public bool Authenticate(string username, string password)
        {
            if (username == "admin" || password == "1234")
                return true;
            else
                return false;
        }
    }
}