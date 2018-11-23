using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BTDB.KVDBLayer;
using BTDB.ODBLayer;
using BTDB_BE.Models;

namespace BTDB_BE.Services
{
    public class CustDataServices : ICustDataServices
    {
        private IObjectDB TempDb { get; set; }

        public CustDataServices(IObjectDB db)
        {
            TempDb = db;
        }

        public List<ModelObj> IterateDB()
        {
            List<ModelObj> objs = new List<ModelObj>();
            using (var tr = TempDb.StartTransaction())
            {
                var visitor = new ToDTOVisitor();
                var iterator = new ODBIterator(tr, visitor);
                iterator.Iterate();
                objs = visitor.Builder.Objs;

            }
            return objs;
        }
        
        internal class ToDTOVisitor : IODBVisitor
        {
            public BuilderModelObj Builder = new BuilderModelObj(new ModelObj("baseObj", "baseObj"));
            
            public bool VisitSingleton(uint tableId, string tableName, ulong oid)
            {
                Builder.Down(new ModelObj("singleton", tableName));
                return true;
            }

            public bool StartObject(ulong oid, uint tableId, string tableName, uint version)
            {
                Builder.Down(new ModelObj("startObject", tableName));
                return true;
            }

            public bool StartField(string name)
            {
                Builder.Down(new ModelObj(name));
                return true;
            }

            public bool NeedScalarAsObject()
            {
                return true;
            }

            public void ScalarAsObject(object content)
            {
                Builder.WriteType(string.Format(CultureInfo.InvariantCulture, "{0}", content.GetType()));
            }

            public bool NeedScalarAsText()
            {
                return true;
            }

            public void ScalarAsText(string content)
            {
                Builder.WriteLastValue(content);
            }

            public void OidReference(ulong oid)
            {
                
            }

            public bool StartInlineObject(uint tableId, string tableName, uint version)
            {
                Builder.Down(new ModelObj("inlineObject", tableName));
                return true;
            }

            public void EndInlineObject()
            {
                Builder.Up();
            }

            public bool StartList()
            {
                Builder.Down(new ModelObj("lsit", ""));
                return true;
            }

            public bool StartItem()
            {
                Builder.Down(new ModelObj("lsitItem", ""));
                return true;
            }

            public void EndItem()
            {
                Builder.Up();
            }

            public void EndList()
            {
                Builder.Up();
            }

            public bool StartDictionary()
            {
                Builder.WriteType("dictionary");
                return true;
            }

            public bool StartDictKey()
            {
                Builder.Down(new ModelObj("dictKey",""));
                return true;
            }

            public void EndDictKey()
            {
                Builder.Up();
            }

            public bool StartDictValue()
            {
                Builder.Down(new ModelObj("dictValue", ""));
                return true;
            }

            public void EndDictValue()
            {
                Builder.Up();
            }

            public void EndDictionary()
            {
                Builder.Up();
            }

            public void EndField()
            {
                Builder.Up();
            }

            public void EndObject()
            {
                Builder.Up();
            }

            public bool StartRelation(string relationName)
            {
                Builder.Down(new ModelObj("relation", relationName));
                
                return true;
            }

            public bool StartRelationKey()
            {
                Builder.Down(new ModelObj("relKey", ""));
                return true;
            }

            public void EndRelationKey()
            {
                Builder.Up();
            }

            public bool StartRelationValue()
            {
                Builder.Down(new ModelObj("relValue", ""));
                return true;
            }

            public void EndRelationValue()
            {
                Builder.Up();
            }

            public void EndRelation()
            {
               Builder.Up();
            }

            public void InlineBackRef(int iid)
            {
               
            }

            public void InlineRef(int iid)
            {
               
            }

            public void MarkCurrentKeyAsUsed(IKeyValueDBTransaction tr)
            {
                
            }
        }
    }
}

