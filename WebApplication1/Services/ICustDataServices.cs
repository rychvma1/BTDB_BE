using System.Collections.Generic;
using BTDB_BE.Models;

namespace BTDB_BE.Services
{
    public interface ICustDataServices
    {
        List<ModelObj> IterateDB();
    }
}