using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace CmdStepsCore
{
    public class Step
    {
        public string Id;
        public string Name;
        public string ResultFormatString;
        public string Input;

        public string ServerName;
        public string ServerUser;
        public string ServerPassword;

        public bool CanExecute
        {
            get
            {
                return Executor != null;
            }
        }
        public bool OutputFullResult;
        public bool Executing;

        public DateTime LastExecuted;

        public virtual event EventHandler<StepExecutionArgs> StepExecuting;
        public virtual event EventHandler<StepExecutionArgs> StepExecuted;
        public StepOutputType OutputType;
        private Type _executorType;
        public Type ExecutorType
        {
            get
            {
                return _executorType;
            }
            set
            {
                _executorType = value;
                if(Executor == null)
                {
                    Executor = (IStepExecutor)Activator.CreateInstance(_executorType);
                }
            }
        }
        private Type _serializerType;

        public Type SerializerType
        {
            get
            {
                return _serializerType;
            }
            set
            {
                _serializerType = value;
                if(Serializer == null)
                {
                    Serializer = (IObjectSerializer)Activator.CreateInstance(_serializerType);
                }
            }
        }

        public Dictionary<string, string> Parameters = new Dictionary<string, string>();
        public StepVariableCollection Variables = new StepVariableCollection();
        
        private IObjectSerializer Serializer;
        private IStepExecutor Executor;

        public Step() { }

        public Step(IStepExecutor executor, IObjectSerializer serializer)
        {
            Id = Guid.NewGuid().ToString();
            Executor = executor;
            ExecutorType = executor.GetType();
            Serializer = serializer;
            SerializerType = serializer.GetType();
        }

        public async Task<string> ExecuteAsync()
        {
            Executing = true;

            var input = Input;
            var parameters = Parameters;

            var args = new StepExecutionArgs()
            {
                Input = Input,
                InputParameters = Parameters
            };

            OnStepExecuting(args);
            
            //Do the transform of input & parameters using values scraped out of previous steps
            if(args.InputVariables != null)
            { 
                foreach (var varItem in args.InputVariables)
                {
                    args.Input = args.Input.Replace(string.Format(AppConstants.VariableFormat, varItem.Key), varItem.Value);
                
                    if(args.InputParameters != null)
                    { 
                        foreach(var paramItem in args.InputParameters)
                        {
                            args.InputParameters[paramItem.Key] = paramItem.Value.Replace(string.Format(AppConstants.VariableFormat, varItem.Key), varItem.Value);
                        }
                    }
                }
            }

            args.Output = await Executor.ExecuteAsync(args.Input, args.InputParameters);

            args.OutputVariables = GetOutputVariableResults(args.Output);

            if (OutputFullResult)
                args.Result = args.Output;
            else
                args.Result = GetResultFormatString(args.Output, args.OutputVariables);

            Variables.HasValues = true;
            LastExecuted = DateTime.Now;
            Executing = false;
            OnStepExecuted(args);
            return args.Result;
        }

        protected void OnStepExecuting(StepExecutionArgs args)
        {
            StepExecuting?.Invoke(this, args);
        }
        protected void OnStepExecuted(StepExecutionArgs args)
        {
            StepExecuted?.Invoke(this, args);
        }
        protected virtual Dictionary<string, string> GetOutputVariableResults(string output)
        {
            var ret = new Dictionary<string, string>();
           
            foreach (var variable in Variables)
            {
                var result = "";
                if (OutputType == StepOutputType.String)
                {
                    var reg = new Regex(variable.RegularExpression);
                    result = reg.Match(output).Value;
                }
                else
                {
                    dynamic obj = null;

                    switch (OutputType)
                    {
                        case StepOutputType.Json:
                            obj = Serializer.DeserializeJson(output);
                            break;
                        case StepOutputType.Xml:
                            obj = Serializer.DeserializeXml(output);
                            break;
                    }
                    
                    var reg = new Regex(@"\[(\d+)\]$");
                    foreach (var prop in variable.DataKey.Split('.'))
                    {
                        var i = "";
                        var p = prop;
                        var match = reg.Match(p);
                        if (match.Success)
                        {
                            i = reg.Match(p).Value;
                            p = reg.Replace(p, string.Empty);
                        }
                        obj = obj[p];

                        if (!string.IsNullOrEmpty(i)) obj = obj[i];
                    }

                    result = obj.ToString();
                }
                variable.Value = result;
                ret.Add(variable.Id, result);
            }
            return ret;
        }
        protected virtual string GetResultFormatString(string output, Dictionary<string, string> outputVariables)
        {
            string ret = ResultFormatString;
            foreach(var item in outputVariables)
            {
                ret = ret.Replace(string.Format(AppConstants.VariableFormat, item.Key), item.Value);
            }
            return ret;
        }
    }
    public enum StepOutputType
    {
        String,
        Json,
        Xml
    }

    public class StepExecutionArgs : EventArgs
    {
        public string Input;
        public string Output;
        public string Result;
        public Dictionary<string, string> InputParameters;
        public Dictionary<string, string> InputVariables;
        public Dictionary<string, string> OutputVariables;
    }
}
