public interface IPersistableObject
{
    public void Save(GameDataWriter writer);

    public void Load(GameDataReader reader);
}
