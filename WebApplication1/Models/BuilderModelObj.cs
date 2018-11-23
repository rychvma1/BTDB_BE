using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace BTDB_BE.Models
{
    public class BuilderModelObj
    {
       // public Stack<ModelObj> ModelObjStack = new Stack<ModelObj>();
        public List<ModelObj> Objs = new List<ModelObj>();
       // private string type, lastValue;

        public BuilderModelObj(ModelObj value)
        {
            //ModelObjStack.Push(value);
            WriteVal(value);
        }

        public void WriteVal(ModelObj val)
        {
           // ModelObjStack.Push(val);
            Objs.Add(val);
        }

        public void Down(ModelObj val)
        {
            //ModelObjStack.Push(val);
            Objs.Add(val);
            //type = val.Type;
        }

        public void Up()
        {
           // ModelObj temp = ModelObjStack.Pop();

            //ModelObjStack.Peek() = temp;
            ModelObj temp = Objs[Objs.Count - 1];
            
            Objs.RemoveAt(Objs.Count - 1);

            //if (Objs.Count >= 1)
            //{
            //    Objs[Objs.Count - 1].Value.Add(temp);
            //}

            Objs[Objs.Count - 1].Value.Add(temp);
            //type = "";
        }

        public void WriteName(string name)
        {
            Objs[Objs.Count - 1].Name = name;
        }

        public void WriteLastValue(string val)
        {
            Objs[Objs.Count - 1].LastValue = val;
        }


        public void WriteType(string type)
        {
            Objs[Objs.Count - 1].Type = type;
        }

    }
}
