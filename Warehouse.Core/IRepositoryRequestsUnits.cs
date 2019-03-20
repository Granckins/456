using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Warehouse.Model;
namespace Warehouse.Core
{
    public interface IRepositoryRequestsUnits : IDisposable
    {

        UserIdentity GetUserIdentityByName(string username);




    }
}
