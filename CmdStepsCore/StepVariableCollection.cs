using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CmdStepsCore
{
    public class StepVariableCollection : ICollection<StepVariable>
    {
        public bool HasValues = false;

        public StepVariable this[int i]
        {
            get
            {
                return StepVariableList[i];
            }
            set
            {
                StepVariableList[i] = value;
            }
        }

        public StepVariable this[string id]
        {
            get
            {
                return StepVariableList.Where(x => x.Id == id).FirstOrDefault();
            }
            set
            {
                var obj = this[id];
                if (obj != null)
                {
                    StepVariableList.Insert(StepVariableList.IndexOf(obj), value);
                    StepVariableList.Remove(obj);
                }
                else
                {
                    StepVariableList.Add(value);
                }
            }
        }

        private List<StepVariable> StepVariableList = new List<StepVariable>();
        
        public int Count
        {
            get
            {
                return StepVariableList.Count();
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public void Add(StepVariable item)
        {
            StepVariableList.Add(item);
        }

        public void AddRange(IEnumerable<StepVariable> items)
        {
            StepVariableList.AddRange(items);
        }

        public void Clear()
        {
            StepVariableList.Clear();
        }

        public bool Contains(StepVariable item)
        {
            return StepVariableList.Contains(item);
        }

        public void CopyTo(StepVariable[] array, int arrayIndex)
        {
            StepVariableList.CopyTo(array, arrayIndex);
        }

        public Dictionary<string, string> ToDictionary()
        {
            var ret = new Dictionary<string, string>();
            foreach(var item in StepVariableList)
            {
                ret.Add(item.Id, item.Value);
            }
            return ret;
        }

        public IEnumerator<StepVariable> GetEnumerator()
        {
            return StepVariableList.GetEnumerator();
        }

        public bool Remove(StepVariable item)
        {
            return StepVariableList.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return StepVariableList.GetEnumerator();
        }
    }
}
