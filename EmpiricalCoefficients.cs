using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polosin
{
    public class EmpiricalCoefficients //Эмперические коэффициенты математической модели
    {
        public readonly double u0; // ConsistencyCoeff; //Коэффициент консистенции материала при температуре приведения
        public readonly double b;  // ViscosityCoeff; //Температурный коэффициент вязкости материала
        public readonly double Tr; // CaseTemp; //Температура приведения
        public readonly double n;  // MaterialFlowIndex; //Индекс течения материала
        public readonly double AlphaU; // HeatTransferCoeff; //Коэффициент теплоотдачи от крышки канала к материалу

        public EmpiricalCoefficients(double u0, double b, double Tr, double n, double alphaU)
        {
            if (u0 <= 0 || b <= 0 || Tr <= 0 || n <= 0 || alphaU <= 0)
                throw new ArgumentException();

            this.u0 = u0;
            this.b = b;
            this.Tr = Tr;
            this.n = n;
            AlphaU = alphaU;
        }
    }
}
