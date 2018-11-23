using System.Collections.Generic;
using BTDB.ODBLayer;

namespace VisualisationBTDB
{
    public enum Gender
    {
        male,
        female
    }
    
    public class CustomObj
    {
        //radi sa podla klucov
        [PrimaryKey(1)]
        public ulong Id { get; set; }
        [SecondaryKey("Name")]
        public string Name { get; set; }
        // 1 ku N
        [SecondaryKey("Age")]
        public int Age { get; set; }
        public Gender gender;

    }


    //public interface ICustomObjTable
    //umoznuje pouzitie foreach IReadOnlyCollection<T>
    public interface ICustomObjTable: IReadOnlyCollection<CustomObj>
    {
        //vsetko generuje IL code az ked to bezi
        void Insert(CustomObj customObj);
        bool RemoveById(ulong id);
        CustomObj FindById(ulong id);
        void Update(CustomObj customObj);
        bool Upsert(CustomObj customObj);
        bool Contains(ulong id);
        CustomObj FindByAgeOrDefault(int age);
        IEnumerator<CustomObj> FindByAge(int age);
        IEnumerator<CustomObj> ListByAge(AdvancedEnumeratorParam<int> param);

        //umoznuje iterovat interval 
        //zlozite vraj sa pouziva skor ten interface

        IOrderedDictionaryEnumerator<ulong, CustomObj> ListById(AdvancedEnumeratorParam<ulong> param);
    }

    public class User
    {
        public string Name { get; set; }
        public ulong Age { get; set; }
    }

    public class Root
    {
        public IOrderedDictionary<ulong, User> Id2User { get; set; }
    }
}