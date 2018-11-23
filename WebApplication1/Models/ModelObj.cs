using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace BTDB_BE.Models
{
    public class ModelObj
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string LastValue { get; set; }
        public List<ModelObj> Value { get; set; }

        //public LinkedList<ModelObj> Value { get; set; }

        public ModelObj() { }

        public ModelObj(string name)
        {
            Name = name;
            Type = null;
            Value = new List<ModelObj>();
            LastValue = null;
        }

        public ModelObj (string type, string name)
        {
            Name = name;
            Type = type;
            Value = new List<ModelObj>();
            LastValue = null;
        }

        //public bool GetValueType(ModelObj<T> node)
        //{
        //    //switch (node.Value.GetType())
        //    //{
        //    //    case typeof(String): return true;
        //    //    case typeof(ModelObj): return false;
        //    //    default:
        //    //        throw new Exception("wrongType");
        //    //}

        //    if (node.Value.GetType() is T)
        //    {
        //        return true;
        //    }
        //    return throw new Exception("wrongType");
        //}

        //public object isvlaueString(object obj)
        //{
        //    //if(obj.GetType() == typeof(string))
        //    //{
        //    //    return new ModelObj();
        //    //}
        //    //else if (obj.GetType() == typeof(ModelObj))
        //    //{
        //    //    return new ModelObj();
        //    //}
        //    //return new Exception("wrongType");

        //   switch (obj.GetType())
        //   {
        //        case typeof(String): r
        //        default: throw new Exception("wrongType");
        //   }
        //}



    }

   


}
