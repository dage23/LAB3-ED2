using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LAB3_ED2.Models
{
    public class EncriptacionModel
    {
        public static byte[] EncryptionZigZag(byte[] TextoOriginal, int CantidadNiveles)
        {
            if (CantidadNiveles<=1)
            {
                return TextoOriginal;
            }
            //Se crea un matriz vacia y se rellena con ~
            var MatrizCifrado = new byte[CantidadNiveles, TextoOriginal.Length];
            for (int i = 0; i < CantidadNiveles; i++)
            {
                for (int j = 0; j < TextoOriginal.Length; j++)
                {
                    MatrizCifrado[i, j] = 0;
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
            var TextoEncriptado = new byte[TextoOriginal.Length];
            var h = 0;
            for (int i = 0; i < CantidadNiveles; i++)
            {
                for (int j = 0; j < TextoOriginal.Length; j++)
                {
                    if (MatrizCifrado[i, j] != 0)
                    {
                        TextoEncriptado[h] = MatrizCifrado[i, j];
                        h++;
                    }
                }
            }
            return TextoEncriptado;
        }
        public static byte[] DecryptZZ(byte[] TextoEncriptado, int CantidadNiveles)
        {
            if (CantidadNiveles<=1)
            {
                return TextoEncriptado;
            }
            //Creacion y llenado de matriz
            var MatrizCifrada = new byte[CantidadNiveles, TextoEncriptado.Length];
            for (int i = 0; i < CantidadNiveles; i++)
            {
                for (int j = 0; j < TextoEncriptado.Length; j++)
                {
                    MatrizCifrada[i, j] = 0;
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
                MatrizCifrada[Fila, Columna++] = 1;
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
                    if (MatrizCifrada[i, j] == 1 && PosicionActual < TextoEncriptado.Length)
                    {
                        MatrizCifrada[i, j] = TextoEncriptado[PosicionActual++];
                    }
                }
            }
            //Desencriptar el texto
            var TextoDescifrado = new byte[TextoEncriptado.Length];
            var h = 0;
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
                if (MatrizCifrada[Fila, Columna] != 1)
                {
                    TextoDescifrado[h] = (MatrizCifrada[Fila, Columna++]);
                    h++;
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
        public static Dictionary<char, char> DiccionarioCesar(string clave, bool Opcion)
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
        public static byte[] CifradoEspiral(int Ancho, bool Abajo, byte[] TextoEncripcion)
        {
            var DivisionAncho = Math.Ceiling(Convert.ToDecimal(TextoEncripcion.Length) / Convert.ToDecimal(Ancho));
            var Altura = Convert.ToInt32(DivisionAncho);
            var DCircularMatriz = new byte[Ancho, Altura];

            var PosicionTexto = 0;
            for (int i = 0; i < Altura; i++)
            {
                for (int j = 0; j < Ancho; j++)
                {
                    if (PosicionTexto < TextoEncripcion.Length)
                    {
                        DCircularMatriz[j, i] = TextoEncripcion[PosicionTexto];
                        PosicionTexto++;
                    }
                    else
                    {
                        DCircularMatriz[j, i] = 0;
                    }
                }
            }
            var REGRESA = new byte[Ancho*Altura];
            var CantidadIteraciones = Ancho < Altura ? Ancho / 2 : Altura / 2;
            var AnchoAux = Ancho;
            var AltoAux = Altura;
            var contador = 0;
            if (Abajo)
            {
                for (int i = 0; i < CantidadIteraciones; i++)
                {
                    for (int j = i; j < AltoAux + i; j++)
                    {
                        REGRESA[contador] =DCircularMatriz[i, j];
                        contador++;
                    }
                    for (int j = i + 1; j < AnchoAux + i; j++)
                    {
                        REGRESA[contador] = DCircularMatriz[j, AltoAux - 1 + i];
                        contador++;
                    }
                    for (int j = AltoAux - 2 + i; j >= i; j--)
                    {
                        REGRESA[contador] = DCircularMatriz[AnchoAux - 1 + i, j];
                        contador++;
                    }
                    for (int j = AnchoAux - 2 + i; j > i; j--)
                    {
                        REGRESA[contador] = DCircularMatriz[j, i];
                        contador++;
                    }
                    AnchoAux = AnchoAux - 2;
                    AltoAux = AltoAux - 2;
                }
                if (AnchoAux == 1)
                {
                    for (int i = CantidadIteraciones; i < AltoAux + CantidadIteraciones; i++)
                    {
                        REGRESA[contador] = DCircularMatriz[CantidadIteraciones, i];
                        contador++;
                    }
                    AnchoAux = 0;
                    AltoAux = 0;
                }
                else if (AltoAux == 1)
                {
                    for (int i = CantidadIteraciones; i < AnchoAux + CantidadIteraciones; i++)
                    {
                        REGRESA[contador] = DCircularMatriz[i, CantidadIteraciones];
                        contador++;
                    }
                    AnchoAux = 0;
                    AltoAux = 0;
                }
            }
            else
            {
                for (int i = 0; i < CantidadIteraciones; i++)
                {
                    for (int j = i; j < AnchoAux + i; j++)
                    {
                        REGRESA[contador] = DCircularMatriz[j, i];
                        contador ++;
                    }
                    for (int j = i + 1; j < AltoAux + i; j++)
                    {
                        REGRESA[contador] = DCircularMatriz[AnchoAux - 1 + i, j];
                        contador++;
                    }
                    for (int j = AnchoAux - 2 + i; j >= i; j--)
                    {
                        REGRESA[contador] = DCircularMatriz[j, AltoAux - 1 + i];
                        contador++;
                    }
                    for (int j = AltoAux - 2 + i; j > i; j--)
                    {
                        REGRESA[contador] = DCircularMatriz[i, j];
                        contador++;
                    }
                    AnchoAux = AnchoAux - 2;
                    AltoAux = AltoAux - 2;
                }
                if (AnchoAux == 1)
                {
                    for (int i = CantidadIteraciones; i < AltoAux + CantidadIteraciones; i++)
                    {
                        REGRESA[contador] = DCircularMatriz[CantidadIteraciones, i];
                        contador++;
                    }
                    AnchoAux = 0;
                    AltoAux = 0;
                }
                else if (AltoAux == 1)
                {
                    for (int i = CantidadIteraciones; i < AnchoAux + CantidadIteraciones; i++)
                    {
                        REGRESA[contador] = DCircularMatriz[i, CantidadIteraciones];
                        contador++;
                    }
                    AnchoAux = 0;
                    AltoAux = 0;
                }
            }
            return REGRESA;
        }
        public static byte[] DescifradoEspiral(int Ancho, bool Abajo, byte[] TextoEncripcion)
        {
            var DivisionAncho = Math.Ceiling(Convert.ToDecimal(TextoEncripcion.Length) / Convert.ToDecimal(Ancho));
            var Altura = Convert.ToInt32(DivisionAncho);
            var DCircularMatriz = new byte[Ancho, Altura];
            var PosicionTexto = 0;
            var AnchoAux = Ancho;
            var AltoAux = Altura;
            var CantidadIteraciones = Ancho < Altura ? Ancho / 2 : Altura / 2;
            var contador = 0;
            if(TextoEncripcion.Length<(Ancho*Altura))
            {
                for (int i = TextoEncripcion.Length; i <=Ancho*Altura; i++)
                {
                    TextoEncripcion[i] = 0;
                }
            }
            if (Abajo)
            {
                for (int i = 0; i < CantidadIteraciones; i++)
                {
                    for (int j = i; j < AltoAux + i; j++)
                    {
                        DCircularMatriz[i, j] = TextoEncripcion[PosicionTexto];
                        PosicionTexto++;
                    }
                    for (int j = i + 1; j < AnchoAux + i; j++)
                    {
                        DCircularMatriz[j, AltoAux - 1 + i] = TextoEncripcion[PosicionTexto];
                        PosicionTexto++;
                    }
                    for (int j = AltoAux - 2 + i; j >= i; j--)
                    {
                        DCircularMatriz[AnchoAux - 1 + i, j] = TextoEncripcion[PosicionTexto];
                        PosicionTexto++;
                    }
                    for (int j = AnchoAux - 2 + i; j > i; j--)
                    {
                        DCircularMatriz[j, i] = TextoEncripcion[PosicionTexto];
                        PosicionTexto++;
                    }
                    AnchoAux = AnchoAux - 2;
                    AltoAux = AltoAux - 2;
                }
                if (AnchoAux == 1)
                {
                    for (int i = CantidadIteraciones; i < AltoAux + CantidadIteraciones; i++)
                    {
                        DCircularMatriz[CantidadIteraciones, i] = TextoEncripcion[PosicionTexto];
                        PosicionTexto++;
                    }
                    AnchoAux = 0;
                    AltoAux = 0;
                }
                else if (AltoAux == 1)
                {
                    for (int i = CantidadIteraciones; i < AnchoAux + CantidadIteraciones; i++)
                    {
                        DCircularMatriz[i, CantidadIteraciones] = TextoEncripcion[PosicionTexto];
                        PosicionTexto++;
                    }
                    AnchoAux = 0;
                    AltoAux = 0;
                }
            }
            else
            {
                for (int i = 0; i < CantidadIteraciones; i++)
                {
                    for (int j = i; j < AnchoAux + i; j++)
                    {
                        DCircularMatriz[j,i] = TextoEncripcion[PosicionTexto];
                        PosicionTexto++;
                    }
                    for (int j = i + 1; j < AltoAux + i; j++)
                    {
                        DCircularMatriz[AnchoAux - 1 + i, j] = TextoEncripcion[PosicionTexto];
                        PosicionTexto++;
                    }
                    for (int j = AnchoAux - 2 + i; j >= i; j--)
                    {
                        DCircularMatriz[j, AltoAux - 1 + i] = TextoEncripcion[PosicionTexto];
                        PosicionTexto++;
                    }
                    for (int j = AltoAux - 2 + i; j > i; j--)
                    {
                        DCircularMatriz[i,j] = TextoEncripcion[PosicionTexto];
                        PosicionTexto++;
                    }
                    AnchoAux = AnchoAux - 2;
                    AltoAux = AltoAux - 2;
                }
                if (AnchoAux == 1)
                {
                    for (int i = CantidadIteraciones; i < AltoAux + CantidadIteraciones; i++)
                    {
                        DCircularMatriz[CantidadIteraciones, i] = TextoEncripcion[PosicionTexto];
                        PosicionTexto++;
                    }
                    AnchoAux = 0;
                    AltoAux = 0;
                }
                else if (AltoAux == 1)
                {
                    for (int i = CantidadIteraciones; i < AnchoAux + CantidadIteraciones; i++)
                    {
                        DCircularMatriz[i,CantidadIteraciones] = TextoEncripcion[PosicionTexto];
                        PosicionTexto++;
                    }
                    AnchoAux = 0;
                    AltoAux = 0;
                }
            }

            var REGRESA = new byte[Ancho*Altura];
            for (int i = 0; i < Altura; i++)
            {
                for (int j = 0; j < Ancho; j++)
                {
                    REGRESA[contador]=DCircularMatriz[j, i];
                    contador++;
                }
            }
            return REGRESA;
        }
    }

}