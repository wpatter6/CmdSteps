using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CmdStepsCore
{
    public interface IStepExecutor
    {
        Task<string> ExecuteAsync(string input, Dictionary<string, string> parameters);
    }
}
