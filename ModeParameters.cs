using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polosin
{
    public class ModeParameters // Режимные параметры процесса
    {
        public readonly double Vu; // CapSpeed; //Скорость крышки
        public readonly double Tu; // CapTemperature; //Температура крышки

        public ModeParameters(double Vu, double Tu)
        {
            if (Vu <= 0 || Tu <= 0)
                throw new ArgumentException();

            this.Vu = Vu;
            this.Tu = Tu;
        }
    }
}
