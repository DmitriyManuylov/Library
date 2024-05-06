using Library.Util;
using Ninject.Modules;
using Ninject;
using System;
using Ninject.Web.Mvc;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Security.Cryptography;


namespace Library
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {

            ConnectionStringSettings connectionStringSettings = ConfigurationManager.ConnectionStrings["LibDb"];

            EnsureDbCreated();

            string connectionString = connectionStringSettings.ConnectionString;

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            NinjectModule registrations = new NinjectRegistrations(connectionString);
            var kernel = new StandardKernel(registrations);
            DependencyResolver.SetResolver(new NinjectDependencyResolver(kernel));
        }

        private static void EnsureDbCreated()
        {
            string masterDbConnectionString = ConfigurationManager.ConnectionStrings["MsSqlMasterDb"].ConnectionString;
            using (SqlConnection sqlConnection = new SqlConnection(masterDbConnectionString))
            {
                sqlConnection.Open();

                string commandText = "Select 1 from sys.databases where name='LibraryDb'";
                SqlCommand command = new SqlCommand(commandText, sqlConnection);


                if (!Convert.ToBoolean(command.ExecuteScalar()))
                {
                    string createDbScript;
                    StringBuilder stringBuilder = new StringBuilder();
                    string appDataPath = HttpContext.Current.Server.MapPath(@"~/App_Data");
                    string fileName = "DbInit.sql";
                    string absolutePathToFile = Path.Combine(appDataPath, fileName);
                    using (StreamReader stream = new StreamReader(absolutePathToFile))
                    {
                        createDbScript = stream.ReadLine();
                        command = new SqlCommand(createDbScript, sqlConnection);
                        command.ExecuteNonQuery();

                        stream.ReadLine();
                        stringBuilder = new StringBuilder();
                        string str;
                        while ((str = stream.ReadLine()) != "GO")
                        {
                            stringBuilder.Append(str);
                        }
                        createDbScript = stringBuilder.ToString();

                        command = new SqlCommand(createDbScript, sqlConnection);
                        command.ExecuteNonQuery();

                        createDbScript = stream.ReadToEnd();
                        command = new SqlCommand(createDbScript, sqlConnection);
                        command.ExecuteNonQuery();
                    }
                    
                }
            }
        }
    }
}
