namespace Gameplay.Tilemaps.Buildings
{
    public class RangeData
    {
        private readonly float _startRange;
        private readonly float _endRange;

        public RangeData(float startRange, float endRange)
        {
            _startRange = startRange;
            _endRange = endRange;
        }

        public bool InRange(float value) => 
            value >= _startRange && value < _endRange;
    }
}