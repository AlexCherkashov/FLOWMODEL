using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polosin
{
    public class VisualizedData
    {
        public readonly List<Point> Temperature;
        public readonly List<Point> Viscosity;

        public VisualizedData(List<Point> temperature, List<Point> viscosity)
        {
            Temperature = temperature;
            Viscosity = viscosity;
        }
    }
}
