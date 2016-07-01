using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CmdStepsCore
{
    public class StepVariable
    {
        private string _value;

        public string RegularExpression;
        public string DataKey;
        public string Id;
        public string Value
        {
            get
            {
                return _value;
            }
            set
            {
                HasValue = true;
                _value = value;
            }
        }

        public bool HasValue = false;

        public StepVariable()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
