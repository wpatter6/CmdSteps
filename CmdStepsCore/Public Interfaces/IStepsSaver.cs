using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CmdStepsCore
{
    public interface IStepsSaver
    {
        void Save(string key, object value, bool encrypt, string appKey);

        T Load<T>(string key, bool encrypt, string appKey);
    }
}