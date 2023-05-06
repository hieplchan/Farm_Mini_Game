using System.IO;

public class GameDataWriter
{
    BinaryWriter writer;

    public GameDataWriter(BinaryWriter writer)
    {
        this.writer = writer;
    }

    public void Write(bool value)
    {
        writer.Write(value);
    }

    public void Write(int value)
    {
        writer.Write(value);
    }

    public void Write(float value)
    {
        writer.Write(value);
    }

    public void Write(long value)
    {
        writer.Write(value);
    }
}
