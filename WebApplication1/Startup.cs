using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BTDB.KVDBLayer;
using BTDB.ODBLayer;
using BTDB_BE.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using VisualisationBTDB;
using WebApplication1.Services;
using static VisualisationBTDB.InFileCol;

namespace WebApplication1
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            IKeyValueDB kvDb = new InMemoryKeyValueDB();
            IObjectDB db = new ObjectDB();
            db.Open(kvDb, false); // false means that dispose of IObjectDB will not dispose IKeyValueDB
            // IObjectDBTransaction tr = db.StartTransaction();   


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSingleton<ITableDataServices, TableDataServices>();
            services.AddSingleton<ICustDataServices, CustDataServices>();
            services.AddSingleton<Func<IObjectDBTransaction, ICustomObjTable>>(initDB(db));
            //services.AddSingleton(initDB2(db));
            services.AddSingleton<IObjectDB>(db);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            // global cors policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());

            app.UseHttpsRedirection();
            app.UseMvc(routes => { routes.MapRoute(name: "default", template: "{controller=Home}/{action=Index}/{id?}"); });
        }

        private static Func<IObjectDBTransaction, ICustomObjTable> initDB(IObjectDB db)
        {
            Func<IObjectDBTransaction, ICustomObjTable> creator;
            using (var tr = db.StartTransaction())
            {
                creator = tr.InitRelation<ICustomObjTable>("CustomObjTable");
                var customObjTable = creator(tr);
                customObjTable.Insert(new CustomObj { Id = 1, Name = "admin", Age = 100, gender = Gender.male });
                customObjTable.Insert(new CustomObj { Id = 2, Name = "Ema", Age = 25, gender = Gender.female });
                customObjTable.Insert(new CustomObj { Id = 3, Name = "Nick", Age = 26, gender = Gender.male });

                var rootName = tr.Singleton<Root>();
                var dict = rootName.Id2User;

                dict.Add(1, new User { Name = "Matus1", Age = 24 });
                dict.Add(2, new User { Name = "Matus2", Age = 24 });
                dict.Add(3, new User { Name = "Matus3", Age = 24 });
                dict.Add(4, new User { Name = "Matus4", Age = 24 });
                dict.Add(5, new User { Name = "Matus5", Age = 24 });

                tr.Commit();
            }



            return creator;


        }

        /*private static void initDB2(IObjectDB db)
        {
            //IObjectDBTransaction tr = db.StartTransaction();
            using (var tr = db.StartTransaction())
            {
                var root = tr.Singleton<Root>();
                var dict = root.Id2User;

                dict.Add(1, new User {Name = "Matus1", Age = 24});
                //dict.Add(2, new User { Name = "Matus2", Age = 24 });
                //dict.Add(3, new User { Name = "Matus3", Age = 24 });
                //dict.Add(4, new User { Name = "Matus4", Age = 24 });
                //dict.Add(5, new User { Name = "Matus5", Age = 24 });

                tr.Commit();
            }
        }*/
    }
}
