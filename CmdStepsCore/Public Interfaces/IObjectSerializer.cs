using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CmdStepsCore
{
    public interface IObjectSerializer
    {
        //dynamic Deserialize(string str, StepOutputType type);
        //string Serialize(object obj, StepOutputType type);
        string SerializeJson(object obj);
        dynamic DeserializeJson(string str);
        dynamic DeserializeXml(string str);
    }
}