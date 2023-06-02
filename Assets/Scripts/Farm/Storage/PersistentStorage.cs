using System.IO;

public class PersistentStorage
{
    const string FILE_NAME = "saveFile";
    string savePath;

    public PersistentStorage Instance { get => _instance; }
    PersistentStorage _instance = null;
    static readonly object padlock = new object();

    public PersistentStorage(string persistentPath)
    {
        savePath = Path.Combine(persistentPath, FILE_NAME);

        lock (padlock)
        {
            if (_instance == null)
                _instance = this;
        }
    }

    public void Save(IPersistableObject persistableObject)
    {
        using (
            var writer = new BinaryWriter(File.Open(savePath, FileMode.Create))
            )
        {
            persistableObject.Save(new GameDataWriter(writer));
        }
    }

    public void Load(IPersistableObject persistableObject)
    {
        using (
            var reader = new BinaryReader(File.Open(savePath, FileMode.Open))
            )
        {
            persistableObject.Load(new GameDataReader(reader));
        }
    }
}