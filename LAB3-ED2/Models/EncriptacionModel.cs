using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LAB3_ED2.Models
{
    public class EncriptacionModel
    {
        string TextoCryptedEspiral = "";
        public string EncryptionZigZag(string TextoOriginal, int CantidadNiveles)
        {
            //Se crea un matriz vacia y se rellena con ~
            var MatrizCifrado = new char[CantidadNiveles, TextoOriginal.Length];
            for (int i = 0; i < CantidadNiveles; i++)
            {
                for (int j = 0; j < TextoOriginal.Length; j++)
                {
                    MatrizCifrado[i, j] = '~';
                }
            }
            //Se hace el recorrido estilo zig zag
            var RecoridoBaja = false; var Fila = 0; var Columna = 0;
            for (int i = 0; i < TextoOriginal.Length; i++)
            {
                if (Fila == 0 || Fila == CantidadNiveles - 1)
                {
                    RecoridoBaja = !RecoridoBaja;
                }
                MatrizCifrado[Fila, Columna++] = TextoOriginal[i];
                if (RecoridoBaja)
                {
                    Fila++;
                }
                else
                {
                    Fila--;
                }
            }
            //Se crea el string encriptado
            var TextoEncriptado = string.Empty;
            for (int i = 0; i < CantidadNiveles; i++)
            {
                for (int j = 0; j < TextoOriginal.Length; j++)
                {
                    if (MatrizCifrado[i, j] != '~')
                    {
                        TextoEncriptado += MatrizCifrado[i, j];
                    }
                }
            }
            return TextoEncriptado;
        }
        public string DecryptZZ(string TextoEncriptado, int CantidadNiveles)
        {
            //Creacion y llenado de matriz
            var MatrizCifrada = new char[CantidadNiveles, TextoEncriptado.Length];
            for (int i = 0; i < CantidadNiveles; i++)
            {
                for (int j = 0; j < TextoEncriptado.Length; j++)
                {
                    MatrizCifrada[i, j] = '~';
                }
            }
            //Hacer el recorrido en zig zag
            var HaciaAbajo = false;
            var Fila = 0; var Columna = 0;
            for (int i = 0; i < TextoEncriptado.Length; i++)
            {
                if (Fila == 0)
                {
                    HaciaAbajo = true;
                }
                if (Fila == CantidadNiveles - 1)
                {
                    HaciaAbajo = false;
                }
                MatrizCifrada[Fila, Columna++] = '$';
                if (HaciaAbajo)
                {
                    Fila++;
                }
                else
                {
                    Fila--;
                }
            }
            //Colocar los caracteres encriptados en la matriz
            var PosicionActual = 0;
            for (int i = 0; i < CantidadNiveles; i++)
            {
                for (int j = 0; j < TextoEncriptado.Length; j++)
                {
                    if (MatrizCifrada[i, j] == '$' && PosicionActual < TextoEncriptado.Length)
                    {
                        MatrizCifrada[i, j] = TextoEncriptado[PosicionActual++];
                    }
                }
            }
            //Desencriptar el texto
            var TextoDescifrado = string.Empty;
            Fila = 0; Columna = 0;
            for (int i = 0; i < TextoEncriptado.Length; i++)
            {
                if (Fila == 0)
                {
                    HaciaAbajo = true;
                }
                if (Fila == CantidadNiveles - 1)
                {
                    HaciaAbajo = false;
                }
                if (MatrizCifrada[Fila, Columna] != '$')
                {
                    TextoDescifrado += (MatrizCifrada[Fila, Columna++]);
                }
                if (HaciaAbajo)
                {
                    Fila++;
                }
                else
                {
                    Fila--;
                }
            }
            return TextoDescifrado;
        }
        public Dictionary<char, char> DiccionarioCesar(string clave, bool Opcion)
        {
            var DiccionarioCifrado = new Dictionary<char, char>();
            var Clave = clave.ToCharArray();
            var ContadorAbecedario = 65; //Empieza en 'A' (65) y termina en 'z' (122) sin el rango [91-96]            
            foreach (var item in Clave)
            {
                if (!(ContadorAbecedario >= 91 && ContadorAbecedario <= 96))
                {
                    if (!DiccionarioCifrado.ContainsValue(item))
                    {
                        DiccionarioCifrado.Add(Convert.ToChar(ContadorAbecedario), item);
                        ContadorAbecedario++;
                    }
                }
            }
            for (int i = 65; i < 123; i++)
            {
                if (!(ContadorAbecedario >= 91 && ContadorAbecedario <= 96))
                {
                    if (!DiccionarioCifrado.ContainsValue(Convert.ToChar(i)) && !(i >= 91 && i <= 96))
                    {
                        DiccionarioCifrado.Add(Convert.ToChar(ContadorAbecedario), Convert.ToChar(i));
                        ContadorAbecedario++;
                    }
                }
                else
                {
                    i--;
                    ContadorAbecedario++;
                }
            }
            if (!Opcion)
            {
                DiccionarioCifrado = DiccionarioCifrado.ToDictionary(kp => kp.Value, kp => kp.Key);
            }
            return DiccionarioCifrado;
        }
        public string EncryptionSpiral(string TextoOriginal, int Ancho)
        {
            var DivisionAncho = Math.Ceiling(Convert.ToDecimal(TextoOriginal.Length) / Convert.ToDecimal(Ancho));
            var Altura = Convert.ToInt32(DivisionAncho);
            var Matriz = new char[Ancho, Altura];
            for (int i = 0; i < Ancho; i++)
            {
                for (int j = 0; j < Altura; j++)
                {
                    Matriz[i, j] = '@';
                }
            }
            var Fila = 0; var Columna = 0;
            for (int i = 0; i < TextoOriginal.Length; i++)
            {
                if (Fila == Ancho)
                {
                    Columna++;
                    Fila = 0;
                }
                Matriz[Fila, Columna] = TextoOriginal[i];
                Fila++;
            }
            CapaSuperior(Matriz, 0, 0, Ancho - 1, Altura - 1);          
            return TextoCryptedEspiral;
        }
        public string DecryptionSpiral(string TextoEncripcion, int Ancho) { return null; }
        void CapaSuperior(char[,] matriz, int InicioPosX, int InicioPosY,int FinPosX, int FinPosY)
        {
            var i = 0;var j = 0;
            for (i = InicioPosX; i <= FinPosX; i++)
            {
                TextoCryptedEspiral += matriz[i,InicioPosY];
            }
            for (j = InicioPosY+1; j <= FinPosY; j++)
            {
                TextoCryptedEspiral += matriz[FinPosX,j];
            }
            if (FinPosX-InicioPosX>0)
            {
                CapaInferior(matriz, InicioPosX, InicioPosY+1, FinPosX - 1, FinPosY);
            }
        }
        void CapaInferior(char[,] matriz, int InicioPosX, int InicioPosY, int FinPosX, int FinPosY)
        {
            var i = 0; var j = 0;
            for (i = FinPosX; i >= InicioPosX; i--)
            {
                TextoCryptedEspiral += matriz[i,FinPosY];
            }
            for (j = FinPosY -1; j >= FinPosY; j--)
            {
                TextoCryptedEspiral += matriz[InicioPosX,j];
            }
            if (FinPosX - InicioPosX > 0)
            {
                CapaSuperior(matriz, InicioPosX+1, InicioPosY, FinPosX, FinPosY-1);
            }
        }
    }
    
}