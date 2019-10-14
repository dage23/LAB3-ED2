using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
namespace LAB3_ED2.Models
{
    public class EncriptacionModel
    {
        public static byte[] EncryptionZigZag(byte[] TextoOriginal, int CantidadNiveles)
        {
            if (CantidadNiveles <= 1)
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
            if (CantidadNiveles <= 1)
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
            var REGRESA = new byte[Ancho * Altura];
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
                        REGRESA[contador] = DCircularMatriz[i, j];
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
                        contador++;
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
            if (TextoEncripcion.Length < (Ancho * Altura))
            {
                for (int i = TextoEncripcion.Length; i <= Ancho * Altura; i++)
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
                        DCircularMatriz[j, i] = TextoEncripcion[PosicionTexto];
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
                        DCircularMatriz[i, j] = TextoEncripcion[PosicionTexto];
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

            var REGRESA = new byte[Ancho * Altura];
            for (int i = 0; i < Altura; i++)
            {
                for (int j = 0; j < Ancho; j++)
                {
                    REGRESA[contador] = DCircularMatriz[j, i];
                    contador++;
                }
            }
            return REGRESA;
        }
        public static string[] ObtenerKeys(int OriginalKey, string DireccionArchivos)
        {
            var GeneratedKeys = new string[2];
            var KeyBinario = Convert.ToString(OriginalKey, 2);
            if (KeyBinario.Length < 10)
            {
                KeyBinario = KeyBinario.PadLeft(10, '0');
            }
            var KeyAfterP10 = PermutacionX(KeyBinario, DireccionArchivos, 10);
            var KeyBinariaSeparada = new string[2];
            for (int i = 0; i < KeyAfterP10.Length; i++)
            {
                if (i < 5)
                {
                    KeyBinariaSeparada[0] += KeyAfterP10[i];
                }
                else
                {
                    KeyBinariaSeparada[1] += KeyAfterP10[i];
                }
            }
            var BlockOneAfterLS1 = LeftShiftOne(KeyBinariaSeparada[0]);
            var BlockTwoAfterLS1 = LeftShiftOne(KeyBinariaSeparada[1]);
            var KeyOne = PermutacionX(BlockOneAfterLS1 + BlockTwoAfterLS1, DireccionArchivos, 8);

            var BlockOneAfterLS2 = LeftShiftTwo(BlockOneAfterLS1);
            var BlockTwoAfterLS2 = LeftShiftTwo(BlockTwoAfterLS1);
            var KeyTwo = PermutacionX(BlockOneAfterLS2 + BlockTwoAfterLS2, DireccionArchivos, 8);
            GeneratedKeys[0] = KeyOne;
            GeneratedKeys[1] = KeyTwo;
            return GeneratedKeys;
        }
        public static byte SDES(string DireccionArchivos,byte ByteOriginal,string Key1, string Key2)
        {
            var ByteBinario = Convert.ToString(ByteOriginal, 2);
            var BinaryAfterIP = IP(DireccionArchivos,ByteBinario);
            var ByteSeparadoAfterIP = new string[2];
            for (int i = 0; i < BinaryAfterIP.Length; i++)
            {
                if (i < 3)
                {
                    ByteSeparadoAfterIP[0] += BinaryAfterIP[i];
                }
                else
                {
                    ByteSeparadoAfterIP[1] += BinaryAfterIP[i];
                }
            }
            //Mandar Bloque2
            var step2Round1=CifradoGeneralSDES(ByteSeparadoAfterIP[1], Key1);
            var step3Round1 = string.Empty;
            for (int i = 0; i < 4; i++)
            {
                for (int i = 0; i < 8; i++)
                {
                    step3Round1 += step2Round1[i] == ByteSeparadoAfterIP[0][i] ? "0" : "1";
                }
            }
            ////NO ES NECESARIO PERO POR SEGUIR LOS PASOS
            //var step4Round1 = step3Round1 + ByteSeparadoAfterIP[1];
            ////swap
            //var step1Round2 = ByteSeparadoAfterIP[1] + step3Round1;
            var step2Round2= CifradoGeneralSDES(step3Round1, Key2);
            var step3Round2 = step2Round2 + step3Round1;
            var step4Round2 = IPNegativa(DireccionArchivos, step3Round2);
            return step4Round2;            
        }
        //Metodos SDES-Keys
        public static string LeftShiftOne(string KeyAfterP10)
        {
            var Result = string.Empty;
            for (int i = 1; i < KeyAfterP10.Length; i++)
            {
                Result += KeyAfterP10[i];
            }
            Result += KeyAfterP10[0];
            return Result;
        }
        public static string LeftShiftTwo(string KeyAfterP10)
        {
            var Result = string.Empty;
            for (int i = 2; i < KeyAfterP10.Length; i++)
            {
                Result += KeyAfterP10[i];
            }
            Result += (KeyAfterP10[0]);
            Result += KeyAfterP10[1];
            return Result;
        }
        public static string PermutacionX(string OriginalKey, string DireccionArchivo, int Tipo)
        {
            var ArregloPosiciones = new int[Tipo];
            var ArregloNuevasPosiciones = string.Empty;
            var contador = 0;
            using (var Archivo = new FileStream((DireccionArchivo + "P" + Tipo + ".txt"), FileMode.OpenOrCreate))
            {
                using (var Lectura = new BinaryReader(Archivo))
                {
                    while (Lectura.BaseStream.Position != Lectura.BaseStream.Length)
                    {
                        ArregloPosiciones[contador] = Int32.Parse(Convert.ToString(Convert.ToChar(Lectura.ReadByte())));
                        contador++;
                    }
                }
            }
            for (int i = 0; i < ArregloPosiciones.Length; i++)
            {
                ArregloNuevasPosiciones += (OriginalKey[ArregloPosiciones[i]]);
            }
            return ArregloNuevasPosiciones;
        }
        //Metodos SDES-Byte
        public static string IP(string DireccionArchivos,string ByteBinarioOriginal)
        {
            var ArregloPosiciones = new int[8];
            var ArregloNuevasPosiciones = string.Empty;
            var contador = 0;
            using (var Archivo = new FileStream((DireccionArchivos + "IP.txt"), FileMode.OpenOrCreate))
            {
                using (var Lectura = new BinaryReader(Archivo))
                {
                    while (Lectura.BaseStream.Position != Lectura.BaseStream.Length)
                    {
                        ArregloPosiciones[contador] = Int32.Parse(Convert.ToString(Convert.ToChar(Lectura.ReadByte())));
                        contador++;
                    }
                }
            }
            for (int i = 0; i < ArregloPosiciones.Length; i++)
            {
                ArregloNuevasPosiciones += (ByteBinarioOriginal[ArregloPosiciones[i]]);
            }
            return ArregloNuevasPosiciones;
        }
        public static string SBoxes(string ByteBinarioOriginal)
        {
            var sBox0 = new string[4, 4];
            var sBox1 = new string[4, 4];
            sBox1[0, 0] = "00";
            sBox1[1, 1] = sBox1[2, 1] = sBox1[3, 2] = sBox1[2, 3] = sBox0[2, 0] = sBox0[0, 1] = sBox0[1, 3] = sBox0[2, 3] = sBox1[0, 0];
            sBox0[0, 0] = "01";
            sBox0[3, 1] = sBox0[0, 0] = sBox0[1, 2] = sBox0[2, 2] = sBox1[0, 1] = sBox1[3, 1] = sBox1[1, 2] = sBox1[2, 2] = sBox0[0, 0];
            sBox0[1, 0] = "11";
            sBox0[3, 0] = sBox0[0, 2] = sBox0[3, 2] = sBox0[2, 3] = sBox1[2, 0] = sBox1[0, 3] = sBox1[1, 3] = sBox1[3, 3] = sBox0[1, 0];
            sBox0[1, 1] = "10";
            sBox0[2, 1] = sBox0[0, 3] = sBox0[3, 3] = sBox1[1, 0]= sBox1[3, 0]= sBox1[0, 2] = sBox0[1, 1];
            var positions = { ByteBinarioOriginal[0] + ByteBinarioOriginal[3], ByteBinarioOriginal[1] + ByteBinarioOriginal[2], ByteBinarioOriginal[4] + ByteBinarioOriginal[7], ByteBinarioOriginal[5] + ByteBinarioOriginal[6] };
            var postionsDecimal = new int[4];
            for (int i = 0; i < 4; i++)
            {
                if (positions[i]=="00")
                {
                    positionsDecimal[i] = 0;
                }
                else if (positions[i]="01")
                {
                    positionsDecimal[i] = 1;
                }
                else if (positions[i] = "10")
                {
                    positionsDecimal[i] = 2;
                }
                else if (positions[i] = "11")
                {
                    positionsDecimal[i] = 3;
                }
            }
            return (sBox0[postionsDecimal[0], postionsDecimal[1]] + sBox1[postionsDecimal[2], postionsDecimal[3]]);
            
        }
        public static string EP(string DireccionArchivos, string ByteBinarioOriginal)
        {
            var ArregloPosiciones = new int[8];
            var ArregloNuevasPosiciones = string.Empty;
            var contador = 0;
            using (var Archivo = new FileStream((DireccionArchivos + "EP.txt"), FileMode.OpenOrCreate))
            {
                using (var Lectura = new BinaryReader(Archivo))
                {
                    while (Lectura.BaseStream.Position != Lectura.BaseStream.Length)
                    {
                        ArregloPosiciones[contador] = Int32.Parse(Convert.ToString(Convert.ToChar(Lectura.ReadByte())));
                        contador++;
                    }
                }
            }
            for (int i = 0; i < ArregloPosiciones.Length; i++)
            {
                ArregloNuevasPosiciones += (ByteBinarioOriginal[ArregloPosiciones[i]]);
            }
            return ArregloNuevasPosiciones;
        }
        public static string IPNegativa(string DireccionArchivos, string ByteBinarioOriginal)
        {
            var ArregloPosiciones = new int[8];
            var ArregloNuevasPosiciones = string.Empty;
            var contador = 0;
            using (var Archivo = new FileStream((DireccionArchivos + "IP-1.txt"), FileMode.OpenOrCreate))
            {
                using (var Lectura = new BinaryReader(Archivo))
                {
                    while (Lectura.BaseStream.Position != Lectura.BaseStream.Length)
                    {
                        ArregloPosiciones[contador] = Int32.Parse(Convert.ToString(Convert.ToChar(Lectura.ReadByte())));
                        contador++;
                    }
                }
            }
            for (int i = 0; i < ArregloPosiciones.Length; i++)
            {
                ArregloNuevasPosiciones += (ByteBinarioOriginal[ArregloPosiciones[i]]);
            }
            return ArregloNuevasPosiciones;
        }
        //Metodos que se hacen mas de una vez

        public static string CifradoGeneralSDES(string Bloque, string Key, string DireccionArchivos)
        {
            var step1EP = EP(DireccionArchivos, Bloque);
            //Xor with k1
            var step2Xor = string.Empty;
            for (int i = 0; i < 8; i++)
            {
                step2Xor += step1EP[i] == Key[i] ? "0" : "1";
            }
            var step3SBoxes = SBoxes(step2Xor);
            var step4P4 = PermutacionX(step3SBoxes, DireccionArchivos, 4);

            return step4P4;
        }
    }
}