﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
            aux = 0;
            var numeroE = new List<long>();
            while (aux < listaCoprimosPhi.Count)
            {
                if (coprime(listaCoprimosPhi[aux], numeroN))
                {
                    numeroE.Add(listaCoprimosPhi[aux]);
                }
                aux++;
            }
            var diccionario = new List<long>();
            aux = 1;
            while (diccionario.Count < 2)
            {

                if ((aux * numeroE[0]) % numeroPhi == 1)
                {
                    diccionario.Add(aux);
                }
                aux++;
            }           

            var keys = new string[2];
            keys[0] = numeroE[0] + "," + numeroN;
            keys[1] = diccionario[1] + "," + numeroN;

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
        public static char[] Cifrado(byte[] buffer, string []llaves)
        {
            int cantTotalDeCaracteres = 100;//PREGUNTAR ???
            var e = Convert.ToInt16(llaves[0]);
            var N = Convert.ToInt16(llaves[1]);
            char[] regresa = null;
            var numMax = ((cantTotalDeCaracteres ^ e) % N)/ cantTotalDeCaracteres;
            if (numMax == 0)
            {
                regresa = new char[buffer.Length];
                for (int i = 0; i < buffer.Length; i++)
                {
                    regresa[i] = Convert.ToChar((buffer[i] ^ e) % N);
                }
            }
            else
            {
                var binarioNumVueltas = Convert.ToString(numMax, 2);
            }
            
            return regresa;
        }
        public static char[] Descifrado(byte[] buffer, string[] llaves)
        {
            var regresa = new char[2];
            return regresa;
        }
    }
}