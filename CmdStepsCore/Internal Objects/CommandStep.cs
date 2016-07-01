using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CmdStepsCore
{
    //class CommandStep : Step
    //{
    //    protected IStepCommandLineExecutor CommandLineExecutor;
    //    private string FullCommand
    //    {
    //        get
    //        {
    //            return Command;
    //        }
    //    }
    //    public string Command;
    //    public override async Task<string> ExecuteAsync()
    //    {
    //        var args = new StepExecutionArgs()
    //        {
    //            Input = FullCommand
    //        };
    //        OnStepExecuting(args);

    //        if (CommandLineExecutor == null) throw new Exception("No Command Line interface applied.");

    //        args.Output = await CommandLineExecutor.ExecuteAsync(args.Input);

    //        if (OutputFullResult) args.Result = args.Output;
    //        OnStepExecuted(args);
    //        return args.Result;
    //    }
    //}
}
