using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polosin
{
    public class Calculations
    {
        /// <summary>
        /// Коэффициент геометрической формы канала
        /// </summary>
        /// <param name="geometricParameters">Геометрические параметры канала</param>
        /// <returns></returns>
        public static double GetGeometricCoefficient(GeometricParameters geometricParameters)
        {
            double ratio = geometricParameters.H / geometricParameters.W;
            return 0.125 * ratio * ratio - 0.625 * ratio + 1;
        }

        /// <summary>
        /// Объемный расход потока материала в канале
        /// </summary>
        /// <param name="geometricParameters">Геометрические параметры канала</param>
        /// <param name="modeParameters">Режимные параметры процесса</param>
        /// <returns></returns>
        public static double GetVolumetricFlowRate(GeometricParameters geometricParameters, ModeParameters modeParameters)
        {
            return (geometricParameters.H * geometricParameters.W * modeParameters.Vu * GetGeometricCoefficient(geometricParameters)) / 2;
        }

        /// <summary>
        /// Скорость деформации сдвига
        /// </summary>
        /// <param name="Vu">Скорость крышки</param>
        /// <param name="H">Глубина</param>
        /// <returns></returns>
        public static double GetShearStrainRate(double Vu, double H)
        {
            return Vu / H;
        }

        /// <summary>
        /// Удельный тепловой поток
        /// </summary>
        /// <param name="parameters">Геометрические параметры</param>
        /// <param name="empiricalCoefficients">Эмперические коэффициенты математической модели</param>
        /// <param name="Y">Скорость деформации сдвига</param>
        /// <returns></returns>
        public static double GetSpecificHeatFluxQy(GeometricParameters parameters, EmpiricalCoefficients empiricalCoefficients, double Y)
        {
            return parameters.H * parameters.W * empiricalCoefficients.u0 * Math.Pow(Y, empiricalCoefficients.n + 1);
        }

        /// <summary>
        /// Удельный тепловой поток
        /// </summary>
        /// <param name="W">Ширина</param>
        /// <param name="Tu">Температура крышки</param>
        /// <param name="empiricalCoefficients">Эмперические коэффициенты математической модели</param>
        /// <returns></returns>
        public static double GetSpecificHeatFluxQa(double W, double Tu, EmpiricalCoefficients empiricalCoefficients)
        {
            return W * empiricalCoefficients.AlphaU * ((1 / empiricalCoefficients.b) - Tu + empiricalCoefficients.Tr);
        }

        /// <summary>
        /// Температура материала в канале 
        /// </summary>
        /// <param name="empiricalCoefficients">Эмперические коэффициенты математической модели</param>
        /// <param name="materialPropertiesParameters">Параметры свойств материала</param>
        /// <param name="Qa">Удельный тепловой поток</param>
        /// <param name="Qy">Удельный тепловой поток</param>
        /// <param name="QCH">Объемный расход потока материала в канале</param>
        /// <param name="W">Ширина</param>
        /// <param name="z">Шаг</param>
        /// <returns></returns>
        public static double GetMaterialTemperature(EmpiricalCoefficients empiricalCoefficients,
                                                     MaterialPropertiesParameters materialPropertiesParameters,
                                                     double Qa, double Qy, double QCH, double W, double z)
        {
            double c = materialPropertiesParameters.c;
            double p = materialPropertiesParameters.p;
            double T0 = materialPropertiesParameters.T0;
            double b = empiricalCoefficients.b;
            double alphaU = empiricalCoefficients.AlphaU;
            double Tr = empiricalCoefficients.Tr;

            double ratio = (Qa * z) / (p * c * QCH);
            double sublogarithmicExpression = ((b * Qy + W * alphaU) / (b * Qa)) * (1 - Math.Exp(-b * ratio)) + Math.Exp(b * (T0 - Tr - ratio));

            return Tr + (1 / b) * Math.Log(sublogarithmicExpression);
        }

        /// <summary>
        /// Вязкость материала в канале 
        /// </summary>
        /// <param name="empiricalCoefficients">Эмперические коэффициенты математической модели</param>
        /// <param name="T">Температура материала в канале</param>
        /// <param name="Y">Скорость деформации сдвига</param>
        /// <returns></returns>
        public static double GetMaterialViscosity(EmpiricalCoefficients empiricalCoefficients, double T, double Y)
        {
            double u0 = empiricalCoefficients.u0;
            double b = empiricalCoefficients.b;
            double Tr = empiricalCoefficients.Tr;
            double n = empiricalCoefficients.n;

            return u0 * Math.Exp(-b * (T - Tr)) * Math.Pow(Y, n - 1);
        }

        /// <summary>
        /// Производительность канала
        /// </summary>
        /// <param name="QCH">Объемный расход потока материала в канале</param>
        /// <param name="p">Плотность</param>
        /// <returns></returns>
        public static double GetChannelPerformance(double QCH, double p)
        {
            return 3600 * QCH * p;
        }

        public static VisualizedData GetVisualizedData(GeometricParameters geometricParameters, ModeParameters modeParameters,
                                                        EmpiricalCoefficients empiricalCoefficients,
                                                        MaterialPropertiesParameters materialPropertiesParameters, double z)
        {
            double Y = GetShearStrainRate(modeParameters.Vu, geometricParameters.H);
            double Qa = GetSpecificHeatFluxQa(geometricParameters.W, modeParameters.Tu, empiricalCoefficients);
            double Qy = GetSpecificHeatFluxQy(geometricParameters, empiricalCoefficients, Y);
            double QCH = GetVolumetricFlowRate(geometricParameters, modeParameters);

            double l = geometricParameters.L;

            List<Point> temperature = new List<Point>();
            List<Point> viscosity = new List<Point>();

            for (double i = 0; i < l; i += z)
            {
                double T = GetMaterialTemperature(empiricalCoefficients, materialPropertiesParameters, Qa, Qy, QCH, geometricParameters.W, i);
                double Nu = GetMaterialViscosity(empiricalCoefficients, T, Y);
                temperature.Add(new Point(i, T));
                viscosity.Add(new Point(i, Nu));
            }
            double lastT = GetMaterialTemperature(empiricalCoefficients, materialPropertiesParameters, Qa, Qy, QCH, geometricParameters.W, l);
            double lastNu = GetMaterialViscosity(empiricalCoefficients, lastT, Y);
            temperature.Add(new Point(l, lastT));
            viscosity.Add(new Point(l, lastNu));

            return new VisualizedData(temperature, viscosity);
        }
    }
}
