using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polosin
{
    public class MaterialPropertiesParameters //Параметры свойств материала
    {
        public readonly double p; // Density; Плотность
        public readonly double c; // SpecificHeat; Средняя удельная теплоёмкость
        public readonly double T0; // MeltingTemp; Температура плавления

        public MaterialPropertiesParameters(double p, double c, double t0)
        {
            if (p <= 0 || c <= 0 || t0 <= 0)
                throw new ArgumentException();

            this.p = p;
            this.c = c;
            T0 = t0;
        }
    }
}
