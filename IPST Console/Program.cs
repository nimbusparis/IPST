using System;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using IPST_Engine;
using IPST_Engine.Mapping;
using IPST_Engine.Repository;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace IPST_Console
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome in the Ingress Portal Submission Tracker console");
            //Configuration nhibernateConfiguration = new Configuration().Configure("hibernate.linux.cfg.xml");


            var sessionFactory = Fluently.Configure()
                
                .Database(SQLiteConfiguration.Standard.UsingFile("IPSTData.db"))
            .Mappings(m => m.FluentMappings.Add<PortalSubmissionMapping>())
            .ExposeConfiguration(c=>new SchemaUpdate(c).Execute(false, true))
            .BuildSessionFactory();

            var session = sessionFactory.OpenSession();
            var parser = new PortalSubmissionParser();
            var clientSecretStream = typeof(Program).Assembly.GetManifestResourceStream("IPST_Console.client_secret.json");
            var repository = new PortalSubmissionRepository(session);
            var gmailEngine = new IPSTEngine(repository, parser);
            Console.WriteLine("This utility will parse your gmail account to search portal submissions/validations. It will only search emails from ingress-support@google.com, parse it and store in local database. No data will be uploaded to anybody.");
            gmailEngine.ConnectAsync(clientSecretStream).Wait();
            gmailEngine.CheckSubmissions(new Progress<SubmissionProgress>(DoProgress)).Wait();
            Console.WriteLine("Parsing complete, the local database is up to date.");
            Console.WriteLine("Hit any key to close this window");
            Console.ReadLine();
        }

        private static void DoProgress(SubmissionProgress obj)
        {
            drawTextProgressBar(obj.Current, obj.Maximum);
        }

        private static void drawTextProgressBar(int progress, int total)
        {
            //draw empty progress bar
            Console.CursorLeft = 0;
            Console.Write("["); //start
            Console.CursorLeft = 32;
            Console.Write("]"); //end
            Console.CursorLeft = 1;
            float onechunk = 30.0f / total;

            //draw filled part
            int position = 1;
            for (int i = 0; i < onechunk * progress; i++)
            {
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.CursorLeft = position++;
                Console.Write(" ");
            }

            //draw unfilled part
            for (int i = position; i <= 31; i++)
            {
                Console.BackgroundColor = ConsoleColor.Green;
                Console.CursorLeft = position++;
                Console.Write(" ");
            }

            //draw totals
            Console.CursorLeft = 35;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write(progress.ToString() + " of " + total.ToString() + "    "); //blanks at the end remove any excess
        }
    }
}
