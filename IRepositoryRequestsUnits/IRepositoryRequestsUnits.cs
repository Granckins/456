using System;
using Warehouse.Model;

namespace IRepositoryRequestsUnits
{
    public interface IRepositoryRequestsUnits :  IDisposable
    {
        UserIdentity GetUserIdentityByName(string username, string password);
    }
}
