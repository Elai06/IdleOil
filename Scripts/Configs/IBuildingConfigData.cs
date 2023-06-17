namespace Configs
{
    public interface IBuildingConfigData : ILeveledConfigData
    {
        public float InternalProductionRate { get; }
        public float InternalCost { get; }
        public float InternalCapacity { get; }
    }
}