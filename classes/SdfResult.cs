namespace _2dGraph
{
     class SdfResult
    {
        public SdfResult(double distance, Circle circle)
        {
            Distance = distance;
            Circle = circle;
        }

        public double Distance{get;set;}
        public Circle Circle {get;set;}
    }
}