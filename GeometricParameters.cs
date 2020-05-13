using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polosin
{
    public class GeometricParameters //Геометрические параметры
    {
        public readonly double W; //Ширина
        public readonly double H; //Глубина
        public readonly double L; //Длина

        public GeometricParameters(double w, double h, double l)
        {
            if (w <= 0 || h <= 0 || l <= 0)
                throw new ArgumentException();

            W = w;
            H = h;
            L = l;
        }
    }
}
