using System.IO;

public class GameDataReader
{
    BinaryReader reader;

    public GameDataReader(BinaryReader reader)
    {
        this.reader = reader;
    }

    public bool ReadBool()
    {
        return reader.ReadBoolean();
    }

    public int ReadInt()
    {
        return reader.ReadInt32();
    }

    public float ReadFloat()
    {
        return reader.ReadSingle();
    }

    public long ReadLong()
    {
        return reader.ReadInt64();
    }
}
