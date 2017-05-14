using Hangfire;
using System;
using System.IO;
using System.Web.Http;

namespace HangFire.Controller
{
    public class ValuesController : ApiController
    {
        public string Get() => User.Identity.Name;

        [HttpPost]
        public void Set()
        {

            BackgroundJob.Enqueue(() => CreateText(User.Identity.Name, DateTime.Now ));
            BackgroundJob.Schedule(() => CreateText("Delayed", DateTime.Now), TimeSpan.FromDays(1));
            RecurringJob.AddOrUpdate(() => CreateText("Daily Job", DateTime.Now), Cron.Daily);

            var id = BackgroundJob.Enqueue(() => CreateText("Hello, ", DateTime.Now));
            BackgroundJob.ContinueWith(id, () => CreateText("world!", DateTime.Now));
        }


        [NonAction]
        public static void CreateText(string text, DateTime now)
        {
            var path = $@"G:\DEV\{Guid.NewGuid()}.txt";

            if (!File.Exists(path))
            {
                File.Create(path).Dispose();
                using (TextWriter tw = new StreamWriter(path))
                {
                    tw.WriteLine($"Data de solicitação: {string.Format("{0:G}", now) }");
                    tw.WriteLine(text);
                    tw.WriteLine($"Data de conclusão: {string.Format("{0:G}", DateTime.Now) }");
                    tw.Close();
                }

            }

            else if (File.Exists(path))
            {
                using (TextWriter tw = new StreamWriter(path))
                {
                    tw.WriteLine($"Data de solicitação: {string.Format("{0:G}", now) }");
                    tw.WriteLine(text);
                    tw.WriteLine($"Data de conclusão: {string.Format("{0:G}",  DateTime.Now) }");
                    tw.Close();
                }
            }

        }
    }
}