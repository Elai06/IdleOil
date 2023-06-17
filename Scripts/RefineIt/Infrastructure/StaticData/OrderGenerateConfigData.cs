using System;

namespace Infrastructure.StaticData
{
    [Serializable]
    public class OrderGenerateConfigData
    {
    public float MinProductQuantity;
    public float MaxProductQuantity;
    public int MinSlotQuantity;
    public int MaxSlotQuantity;
    public float RewardRatio;
    public int OrderCount;
    public int TimeToFailed;
    public int MinDelayNextOrder;
    public int MaxDelayNextOrder;
    public int TimeToAds;
    }
}