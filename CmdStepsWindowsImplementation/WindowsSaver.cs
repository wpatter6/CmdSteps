using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CmdStepsCore;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace CmdStepsWindowsImplementation
{
    public class WindowsSaver : IStepsSaver
    {
        private string AppDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\" + AppConstants.AppName;
        public T Load<T>(string key, bool encrypt, string appKey)
        {
            string text;
            string path = AppDataFolder + @"\" + key;
            if (encrypt)
                text = WindowsProtectedData.UnprotectString(File.ReadAllBytes(path), appKey);
            else
                text = File.ReadAllText(path);

            return JsonConvert.DeserializeObject<T>(text);
        }

        public void Save(string key, object value, bool encrypt, string appKey)
        {
            string path = AppDataFolder + @"\" + key;
            string text = JsonConvert.SerializeObject(value);
            byte[] bytes;

            if (encrypt)
                bytes = WindowsProtectedData.ProtectString(text, appKey);
            else
                bytes = Encoding.ASCII.GetBytes(text);

            if (!Directory.Exists(AppDataFolder)) Directory.CreateDirectory(AppDataFolder);

            File.WriteAllBytes(path, bytes);
        }
    }
}
