using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CmdStepsCore
{
    public class StepCollection : ICollection<Step>
    {
        public string Id;
        public bool Encrypt = true;
        public bool CanSave
        {
            get
            {
                return Saver != null;
            }
        }
        public Step this[int i]
        {
            get
            {
                return StepList[i];
            }
            set
            {
                StepList[i] = value;
            }
        }
        public Step this[string id]
        {
            get
            {
                return StepList.Where(x => x.Id == id).FirstOrDefault();
            }
            set
            {
                var obj = this[id];
                if (obj != null)
                {
                    StepList.Insert(StepList.IndexOf(obj), value);
                    StepList.Remove(obj);
                }
                else
                {
                    StepList.Add(value);
                }
            }
        }
        public int Count
        {
            get
            {
                return StepList.Count();
            }
        }
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }
        public StepVariableCollection PopulatedVariables = new StepVariableCollection();
        
        public virtual event EventHandler<StepExecutionArgs> StepExecuting;
        public virtual event EventHandler<StepExecutionArgs> StepExecuted;

        private List<Step> StepList;

        private IStepsSaver Saver;
        private string SaveFileName
        {
            get
            {
                return string.Format("{0}.{1}", Id, AppConstants.ListFileExtension);
            }
        }

        public StepCollection()
        {
            Id = Guid.NewGuid().ToString();
            StepList = new List<Step>();
        }

        public StepCollection(IStepsSaver saver) : this()
        {
            Saver = saver;
        }
        
        public void Load(string listId)
        {
            if (!CanSave) throw new Exception("No IStepsSaver present on StepCollection object.");
            Id = listId;
            StepList = Saver.Load<List<Step>>(SaveFileName, Encrypt, AppConstants.AppKey);
        }

        public void Save()
        {
            if (!CanSave) throw new Exception("No IStepsSaver present on StepCollection object.");
            Saver.Save(SaveFileName, StepList, Encrypt, AppConstants.AppKey);
        }

        public void Add(Step item)
        {
            item.StepExecuting += Item_StepExecuting;
            item.StepExecuted += Item_StepExecuted;
            StepList.Add(item);
        }

        private void Item_StepExecuting(object sender, StepExecutionArgs e)
        {
            e.InputVariables = PopulatedVariables.ToDictionary();
            OnStepExecuting(e);
        }

        private void Item_StepExecuted(object sender, StepExecutionArgs e)
        {
            PopulatedVariables.AddRange(((Step)sender).Variables);
            OnStepExecuted(e);
        }

        protected void OnStepExecuting(StepExecutionArgs args)
        {
            StepExecuting?.Invoke(this, args);
        }
        protected void OnStepExecuted(StepExecutionArgs args)
        {
            StepExecuted?.Invoke(this, args);
        }

        public void Clear()
        {
            StepList.Clear();
        }

        public bool Contains(Step item)
        {
            return StepList.Contains(item);
        }

        public void CopyTo(Step[] array, int arrayIndex)
        {
            StepList.CopyTo(array, arrayIndex);
        }

        public bool Remove(Step item)
        {
            return StepList.Remove(item);
        }

        public IEnumerator<Step> GetEnumerator()
        {
            return StepList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return StepList.GetEnumerator();
        }
    }
}