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
            var listaCoprimosPhi = new List<int>();
            var aux = 2;
            while (aux < numeroPhi)
            {
                if (coprime(aux, numeroPhi))
                {
                    listaCoprimosPhi.Add(aux);
                }
                aux++;
            }
            aux = 1;
            var Arreglo1 = new long[50];
            var Arreglo2 = new long[50];
            var Arreglo3 = new long[50];
            var Arreglo4 = new long[50];
            Arreglo2[0] = numeroPhi;Arreglo2[1] = listaCoprimosPhi[0];
            Arreglo3[0] = 1;Arreglo3[1] = 0;
            Arreglo4[0] = 0;Arreglo4[1] = 1;

            while (Arreglo2[aux]!=0)
            {
                Arreglo1[aux+1] = Arreglo2[aux -1] / Arreglo2[aux];
                Arreglo2[aux + 1] = Arreglo2[aux - 1] - Arreglo1[aux + 1] * Arreglo2[aux];
                Arreglo3[aux + 1] = Arreglo3[aux - 1] - Arreglo1[aux + 1] * Arreglo3[aux];
                Arreglo4[aux + 1] = Arreglo4[aux - 1] - Arreglo1[aux + 1] * Arreglo4[aux];
                aux++;
            }
            if (Arreglo4[aux-1]<0)
            {
                Arreglo4[aux - 1] = Arreglo4[aux - 1] + numeroPhi;
            }
            var keys = new string[2];
            keys[0] = listaCoprimosPhi[0] + "," + numeroN;
            keys[1] = Arreglo4[aux - 1] + "," + numeroN;

            return keys;
        }
        static long GCM(long a, long b)
        {
            if (a == 0 || b == 0)
                return 0;

            if (a == b)
                return a;

            if (a > b)
                return GCM(a - b, b);

            return GCM(a, b - a);
        }
        static bool coprime(long a, long b)
        {
            if (GCM(a, b) == 1)
                return true;
            else
                return false;
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