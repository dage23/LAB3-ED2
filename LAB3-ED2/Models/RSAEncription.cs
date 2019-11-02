using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Web;
using System.Security.Cryptography;

namespace LAB3_ED2.Models
{
    public class RSAEncription
    {
        public static string[] GenerateKeys(string NumberP, string NumberQ)
        {
            var numeroP = long.Parse(NumberP);
            var numeroQ = long.Parse(NumberQ);
            var numeroN = numeroP * numeroQ;
            var numeroPhi = (numeroP - 1) * (numeroQ - 1);
            var numeroE = 0;
            List<int> A = new List<int>();
            for (int i = 2; i <= numeroN; i++)
            {
                if (numeroPhi >= i && numeroPhi % i == 0)
                {
                    A.Add(i);
                }
                if (numeroN % i == 0)
                {
                    A.Add(i);
                }
            }
            var encontrado = true;
            for (int i = Convert.ToInt32(numeroPhi); i > 0; i--)
            {
                encontrado = true;
                foreach (var item in A)
                {
                    if (i % item == 0)
                    {
                        encontrado = false;
                    }
                }
                if (encontrado)
                {
                    numeroE = i;
                    i = 0;
                }
            }
            var numeroD = FindingD(numeroE, Convert.ToInt32(numeroPhi));
            var keys = new string[2];
            var phis = numeroPhi;
            keys[0] = numeroE + "," + numeroN;
            keys[1] = numeroD + "," + numeroN;

            return keys;
        }
        public static string FindingD(int e, int phi)
        {
            var A = phi;
            var B = e;
            var C = A % B;
            var D = A / B;
            List<int> AValues = new List<int>();
            List<int> BValues = new List<int>();
            List<int> CValues = new List<int>();
            List<int> DValues = new List<int>();
            AValues.Add(A);
            BValues.Add(B);
            CValues.Add(C);
            DValues.Add(D);
            while (C != 0)
            {
                A = B;
                B = C;
                C = A % B;
                if (C != 0)
                {
                    AValues.Add(A);
                    BValues.Add(B);
                    CValues.Add(C);
                    DValues.Add(D);
                }

            }
            AValues.Reverse();
            BValues.Reverse();
            CValues.Reverse();
            DValues.Reverse();
            var charAValues = AValues.ToArray();
            var charBValues = BValues.ToArray();
            var charCValues = CValues.ToArray();
            var charDValues = DValues.ToArray();
            //inverso
            int cont = 0;
            var E = charAValues[cont];
            var F = 1;
            var G = charBValues[cont];
            var H = charDValues[cont];
            cont++;
            while (E != phi)
            {
                var aux = F;
                F = H;
                H = (charCValues[cont] * H) + aux;
                G = E;
                E = charAValues[cont];
            }
            var d = 0;
            if ((H * e) % phi == 1)
            {
                d = H;
            }
            else
            {
                d = phi - H;
            }
            return d.ToString();
        }

        public static byte[] Cifrado(byte[] buffer, string[] llaves)
        {
            int cantTotalDeCaracteres = 255;//PREGUNTAR ???
            var e = Convert.ToInt16(llaves[0]);
            var N = Convert.ToInt16(llaves[1]);
            byte[] regresa;
            if (N >= cantTotalDeCaracteres)
            {
                var numVueltas = (N - 1) / cantTotalDeCaracteres;
                var binarioNumVueltas = Convert.ToString(numVueltas, 2);
                regresa = new byte[buffer.Length * 2];
                for (int i = 0; i < buffer.Length; i += 2)
                {
                    BigInteger vueltas = BigInteger.ModPow(buffer[i], e, N) / cantTotalDeCaracteres;
                    regresa[i] = Convert.ToByte(long.Parse(vueltas.ToString()));
                    BigInteger caracter = (BigInteger.ModPow(buffer[i], e, N) % cantTotalDeCaracteres);
                    regresa[i++] = Convert.ToByte(long.Parse(caracter.ToString()));
                }
            }
            else
            {
                regresa = new byte[buffer.Length];
                for (int i = 0; i < buffer.Length; i++)
                {
                    BigInteger operacion = BigInteger.ModPow(buffer[i], e, N);
                    regresa[i] = Convert.ToByte(long.Parse(operacion.ToString()));
                }
            }
            return regresa;
        }
        public static byte[] Descifrado(byte[] buffer, string[] llaves)
        {
            int cantTotalDeCaracteres = 255;//PREGUNTAR ???
            var d = Convert.ToInt16(llaves[0]);
            var N = Convert.ToInt16(llaves[1]);
            byte[] regresa;
            if (N >= cantTotalDeCaracteres)
            {
                regresa = new byte[buffer.Length / 2];
                for (int i = 0; i < buffer.Length; i += 2)
                {
                    var operacion = (buffer[i] * cantTotalDeCaracteres) + buffer[i++];
                    BigInteger caracter = BigInteger.ModPow(operacion, d, N);
                    regresa[i++] = Convert.ToByte(long.Parse(caracter.ToString()));
                }
            }
            else
            {
                regresa = new byte[buffer.Length];
                for (int i = 0; i < buffer.Length; i++)
                {
                    BigInteger operacion = BigInteger.ModPow(buffer[i], d, N);
                    regresa[i] = Convert.ToByte(long.Parse(operacion.ToString()));
                }
            }

            return regresa;
        }
    }
}