using System;
using BTDB.Buffer;
using BTDB.KVDBLayer;
using BTDB.ODBLayer;

namespace VisualisationBTDB
{
    public class InFileCol
    {   
        

        public static void Init()
        {
            
            string directory = @"D:\BcBackend\NewDirectory1";
            var kv = new KeyValueDB(new OnDiskFileCollection(directory));
            var db = new ObjectDB();
            db.Open(kv, false); 
            
            IObjectDBTransaction tr = db.StartTransaction();
            
            var root = tr.Singleton<Root>();
            var dict = root.Id2User;
            
            dict.Add(1, new User{Name = "Matus1", Age = 24});
            dict.Add(2, new User{Name = "Matus2", Age = 24});
            dict.Add(3, new User{Name = "Matus3", Age = 24});
            dict.Add(4, new User{Name = "Matus4", Age = 24});
            dict.Add(5, new User{Name = "Matus5", Age = 24});

            foreach (var user in dict)
            {
                Console.WriteLine("Id: " + user.Key +", Name: " +user.Value.Name+ ", Age: " + user.Value.Age);
            }
            
            var enumerator = dict.GetAdvancedEnumerator(new AdvancedEnumeratorParam<ulong>());
            Console.WriteLine("Total count of users: " + enumerator.Count);
            
            dict.RemoveRange(2, true, 5, false);
            dict.Add(3, new User{Name = "Matus3", Age = 24});

            foreach (var user in dict)
            {
                Console.WriteLine("Id: " + user.Key +", Name: " +user.Value.Name+ ", Age: " + user.Value.Age);
            }
            
            tr.Dispose();
            db.Dispose();
        }
    }
}