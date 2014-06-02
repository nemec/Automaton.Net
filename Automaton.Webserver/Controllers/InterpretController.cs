using System;
using System.Linq;
using System.Web.Http;
using Automaton.Net;
using WebApiLongRunningTask;

namespace Automaton.Webserver.Controllers
{
    public class InterpretController : ApiController
    {
        private static readonly Interpreter Interpreter = new Interpreter(new Registrar(), false);

        private static readonly IBackgroundProcessor<string, string> Processor = new TaskBackgroundProcessor<string, string>(data =>
            {
                var result = Interpreter.Interpret(data).FirstOrDefault();
                return result == null ? null : result.Service.Name;
            });

        public WorkerResult<string> PostNewRequest([FromBody]string rawText)
        {
            var workerId = Processor.SubmitForProcessing(rawText);
            return Processor.WaitForRespose(workerId, TimeSpan.FromSeconds(5));
        }

        public WorkerResult<string> Get(string workerId)
        {
            return Processor.WaitForRespose(workerId, TimeSpan.FromSeconds(5));
        }
    }
}