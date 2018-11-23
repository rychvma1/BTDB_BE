using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using BTDB.Buffer;
using BTDB.KVDBLayer;
using BTDB.ODBLayer;
using BTDB_BE.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using VisualisationBTDB;

namespace WebApplication1.Services
{
    public class TableDataServices : ITableDataServices
    {
        private IObjectDB TempDb { get; set; }
        private Func<IObjectDBTransaction, ICustomObjTable> _creator;

        public TableDataServices(IObjectDB tempDb, Func<IObjectDBTransaction, ICustomObjTable> creator)
        {
            TempDb = tempDb;
            _creator = creator;
        }

        public List<CustomObj> GetDataFromTable()
        {
            List<CustomObj> jsonResult = new List<CustomObj>();
            using (var tr = TempDb.StartReadOnlyTransaction())
            {

                var customObjTable = _creator(tr);

                foreach (var customObj in customObjTable)
                {
                   // string userJson = JsonConvert.SerializeObject(customObj,
                   //     new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });

                   //jsonResult.Add(userJson);
                    jsonResult.Add(customObj);
                }

            }
            TempDb.Dispose();
            return jsonResult;
        }

        //public List<string> GetDataFromTable()
        //{
        //    List<string> jsonResult = new List<string>();
        //    using (var tr = TempDb.StartReadOnlyTransaction())
        //    {

        //        var customObjTable = _creator(tr);

        //        foreach (var customObj in customObjTable)
        //        {
        //            string userJson = JsonConvert.SerializeObject(customObj,
        //                new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });

        //            jsonResult.Add(userJson);
        //            //jsonResult.Add(customObj);
        //        }

        //    }
        //    TempDb.Dispose();
        //    return jsonResult;
        //}

        public List<ModelObj> IterateDB()
        {
            List<ModelObj> objs = new List<ModelObj>();
            using (var tr = TempDb.StartTransaction())
            {
              //  var fastVisitor = new ToStringFastVisitor();
                var visitor = new ToStringVisitor();
                //var iterator = new ODBIterator(tr, fastVisitor);
                //var iterator = new ODBIterator(tr, visitor);
                //iterator.Iterate();
                var iterator = new ODBIterator(tr, visitor);
                iterator.Iterate();
                var text = visitor.ToString();
                objs = ParseText(text);
                
                //string userJson = JsonConvert.SerializeObject(text,
                //    new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
                //jsonResult.Add(userJson);

                
            }
            return objs;
        }

        public List<string> IterateDBStr()
        {
            List<string> objs = new List<string>();
            using (var tr = TempDb.StartTransaction())
            {
                //  var fastVisitor = new ToStringFastVisitor();
                var visitor = new ToStringVisitor();
                //var iterator = new ODBIterator(tr, fastVisitor);
                //var iterator = new ODBIterator(tr, visitor);
                //iterator.Iterate();
                var iterator = new ODBIterator(tr, visitor);
                iterator.Iterate();
                var text = visitor.ToString();
                objs.Add(text);
                //string userJson = JsonConvert.SerializeObject(text,
                //    new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
                //jsonResult.Add(userJson);


            }
            return objs;
        }

        public List<ModelObj> ParseText(string text)
        {
            var objs = new List<ModelObj>();
            string[] parts = text.Split("\r\n");
            var temp = new ModelObj();

            temp.Type = parts[0];
            temp.Name = parts[1];
            //temp.Value = new ModelObj { Name = "id", Type = parts[2], Value = parts[3].ToString()};

            foreach (var obj in objs)
            {
                Console.WriteLine(obj.Name + " " + obj.Type + " " +obj.LastValue);
            }

            return objs;
        }
    }

    internal class ToStringFastVisitor : IODBFastVisitor
    {
        protected readonly StringBuilder Builder = new StringBuilder();
        //public ByteBuffer Keys = ByteBuffer.NewEmpty();

        public override string ToString()
        {
            return Builder.ToString();
        }

        public void MarkCurrentKeyAsUsed(IKeyValueDBTransaction tr)
        {
          
        }

    }

    internal class ToStringVisitor : ToStringFastVisitor, IODBVisitor
    {
        public bool VisitSingleton(uint tableId, string tableName, ulong oid)
        {
            //Builder.AppendFormat("Singleton {0}-{1} oid:{2}", tableId, tableName ?? "?Unknown?", oid);
            //Builder.AppendFormat("Singleton tableId-tableName: {0}-{1}", tableId, tableName ?? "?Unknown?");
            //Builder.AppendFormat("{0}-{1}", tableId, tableName ?? "?Unknown?");
            //Builder.AppendLine("singleton");
            Builder.AppendFormat("singletonName:{0} ", tableName ?? "?Unknown?");
            return true;
        }

        public bool StartObject(ulong oid, uint tableId, string tableName, uint version)
        {
            Builder.AppendFormat("objectStartName:{0} ", tableName ?? "?Unknown?");
            
            return true;
        }

        public bool StartField(string name)
        {
            //Builder.AppendLine($"StartField");
            Builder.AppendFormat("StartField:{0}", name);
            return true;
        }

        public bool NeedScalarAsObject()
        {
            return true;
        }

        public void ScalarAsObject(object content)
        {
            Builder.AppendLine(string.Format(CultureInfo.InvariantCulture, "{0}", content.GetType()));
        }

        public bool NeedScalarAsText()
        {
            return true;
        }

        public void ScalarAsText(string content)
        {
            Builder.AppendLine($"{content}");
        }

        public void OidReference(ulong oid)
        {
            Builder.AppendLine($"OidReference {oid}");
        }

        public bool StartInlineObject(uint tableId, string tableName, uint version)
        {
            //Builder.AppendLine($"StartInlineObject {tableId}-{tableName}-{version}");
            Builder.AppendLine($"objectInline:{tableName}");
            return true;
        }

        public void EndInlineObject()
        {
            Builder.AppendLine("EndInlineObject");
        }

        public bool StartList()
        {
            Builder.AppendLine("StartList");
            return true;
        }

        public bool StartItem()
        {
            Builder.AppendLine("StartItem");
            return true;
        }

        public void EndItem()
        {
            Builder.AppendLine("EndItem");
        }

        public void EndList()
        {
            Builder.AppendLine("EndList");
        }

        public bool StartDictionary()
        {
            Builder.AppendLine("StartDictionary");
            return true;
        }

        public bool StartDictKey()
        {
            Builder.AppendLine("StartDictKey");
            return true;
        }

        public void EndDictKey()
        {
            Builder.AppendLine("EndDictKey");
        }

        public bool StartDictValue()
        {
            Builder.AppendLine("StartDictValue");
            return true;
        }

        public void EndDictValue()
        {
            Builder.AppendLine("EndDictValue");
        }

        public void EndDictionary()
        {
            Builder.AppendLine("EndDictionary");
        }

        public void EndField()
        {
            Builder.AppendLine("EndField");
        }

        public void EndObject()
        {
            Builder.AppendLine("EndObject");
        }

        public bool StartRelation(string relationName)
        {
            Builder.AppendLine("relation");
            Builder.AppendLine($"{relationName}");
            return true;
        }

        public bool StartRelationKey()
        {
            Builder.AppendLine("BeginKey");
            return true;
        }

        public void EndRelationKey()
        {
            Builder.AppendLine("EndKey");
        }

        public bool StartRelationValue()
        {
            Builder.AppendLine("BeginValue");
            return true;
        }

        public void EndRelationValue()
        {
            Builder.AppendLine("EndValue");
        }

        public void EndRelation()
        {
            Builder.AppendLine("EndRelation");
        }

        public void InlineBackRef(int iid)
        {
            Builder.AppendLine($"Inline back ref {iid}");
        }

        public void InlineRef(int iid)
        {
            Builder.AppendLine($"Inline ref {iid}");
        }
    }

   
}
