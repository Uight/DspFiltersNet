namespace DspFiltersNet.Filter;

public class MovingAverageFilterDefinition : IFilterDefinition
{
    public int Width { get; }

    public MovingAverageFilterDefinition(int movingAverageWidth)
    {
        Width = movingAverageWidth;
    }
}