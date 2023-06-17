namespace Configs
{
    public interface ILeveledConfigData : IConfigData
    {
        public int InternalLevel { get; }
    }
}