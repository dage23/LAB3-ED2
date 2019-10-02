using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LAB3_ED2.Models
{
    public class EncriptacionModel
    {
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
    }
}