namespace _2dGraph
{
    class Circle
    {
        public Circle(double ox, double oy, double radius, byte[] color)
        {
            Ox = ox;
            Oy = oy;
            Radius = radius;
            Color = color; 
        }
        public double Ox { get; set; }
        public double Oy { get; set; }
        public double Radius { get; set; }
        public byte[] Color { get; set; }

    }
}