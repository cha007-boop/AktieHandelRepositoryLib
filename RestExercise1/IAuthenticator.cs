using System;
using System.Collections.Generic;
using System.Text;

namespace RestExercise1
{
    public enum Role
    {
        user, admin, superuser
    }
    public interface IAuthenticator
    {
        Role? Authenticate(string username, string password);
    }
}
