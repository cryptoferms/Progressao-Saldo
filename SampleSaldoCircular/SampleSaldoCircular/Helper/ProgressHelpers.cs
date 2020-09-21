using System;
using System.Collections.Generic;
using System.Text;

namespace SampleSaldoCircular.Helper
{
    /// <summary>
    /// classe criada para auxiliar a criação da barra de progressão circular, para mais detalhes leia o artigo
    /// https://medium.com/@bertuzzi/xamarin-rocket-8-indicador-de-progresso-fe512e8812da
    /// </summary>
    public class ProgressHelpers
    {
        // Valores de Referencia (Padrão Pixel 1 por Dispositivo)
        private const float refHeight = 1080; //1677;
        private const float refWidth = 632; //940

        //Valores derivados proporcionais

        private float deviceHeight = 1; //Initializing to 1
        private float deviceWidth = 1; //Initializing to 1

        public ProgressHelpers() { }

        public void SetDevice(int deviceHeight, int deviceWidth)
        {
            this.deviceHeight = deviceHeight;
            this.deviceWidth = deviceWidth;
        }

        public float GetFactoredValue(int value)
        {
            float refRatio = refHeight / refWidth;
            float devRatio = deviceHeight / deviceWidth;

            float factoredValue = value * (refRatio / devRatio);

            return factoredValue;
        }

        public float GetFactoredHeight(int value)
        {
            return (float)((value / refHeight) * deviceHeight);
        }

        public float GetFactoredWidth(int value)
        {
            return (float)((value / refWidth) * deviceWidth);
        }

        public int GetSweepAngle(int goal, int achieved)
        {
            int SweepAngle = 260;
            float factor = (float)achieved / goal;
            return (int)(SweepAngle * factor);
        }
    }
}
