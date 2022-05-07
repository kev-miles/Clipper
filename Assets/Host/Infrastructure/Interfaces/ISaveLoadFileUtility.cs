using System;
using System.Collections.Generic;
using Host.Infrastructure.FileManagement;
using UniRx;

namespace Host.Infrastructure.Interfaces
{
    public interface ISaveLoadFileUtility
    {
        public Dictionary<int, Dictionary<string, string>> SavedDataDictionary();
        public IObservable<Unit> WriteFile(SaveLoadObject saveObject);
        public IObservable<Unit> ReadFile(int slotId);
        public IObservable<Unit> ReadFiles();
        public IObservable<Unit> DeleteFile(int slotId);
    }
}