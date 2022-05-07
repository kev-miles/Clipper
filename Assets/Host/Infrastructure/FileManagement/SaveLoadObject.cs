using System.Collections.Generic;

namespace Host.Infrastructure.FileManagement
{
    public class SaveLoadObject
    {
        public int SlotId;
        public Dictionary<string, string> SaveData;

        public static SaveLoadObject Create(
            int slotId = 0,
            Dictionary<string, string> data = default
        ) => new SaveLoadObject()
        {
            SlotId = slotId,
            SaveData = data
        };
        
        public SerializableSaveLoadObject ToSerializable()
        {
            return SerializableSaveLoadObject.Create(
                this.SlotId,
                this.SaveData[SaveDataKeys.UserName]
            );
        }
    }

    public class SerializableSaveLoadObject
    {
        public int SlotId;
        public string UserName;
        public string CurrentLevel;

        public static SerializableSaveLoadObject Create(
            int slotId = 0,
            string userName = "",
            string currentLevel = ""
        ) => new SerializableSaveLoadObject()
        {
            SlotId = slotId,
            UserName = userName,
            CurrentLevel = currentLevel
        };

        public SaveLoadObject ToSaveLoadObject()
        {
            return SaveLoadObject.Create(
                this.SlotId,
                new Dictionary<string, string>()
                {
                    {SaveDataKeys.UserName, this.UserName},
                    {SaveDataKeys.CurrentLevel, this.CurrentLevel}
                }
            );
        }
    }
}