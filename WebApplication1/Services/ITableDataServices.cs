using System.Collections.Generic;
using BTDB_BE.Models;
using VisualisationBTDB;

namespace WebApplication1.Services
{
    public interface ITableDataServices
    {
        //List<string> GetDataFromTable();
        List<CustomObj> GetDataFromTable();
        List<ModelObj> IterateDB();
        List<string> IterateDBStr();
    }
}
