﻿using System;
using System.Web;
using System.Web.Mvc;
using System.IO;
using LAB3_ED2.Models;
using System.IO.Compression;
using System.Web.UI;
using System.Web.UI.WebControls;



namespace LAB3_ED2.Controllers
{
    public class EncriptacionController : BaseController
    {
        const int bufferLength = 100;
        public ActionResult Menu()
        {
            return View();
        }
        public ActionResult MenuEspiral()
        { return View(); }
        public ActionResult CifradoZigZag()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CifradoZigZag(HttpPostedFileBase ArchivoImportado, int CantidadNiveles, string Opcion)
        {
            Directory.CreateDirectory(Server.MapPath(@"~/App_Data/"));
            var extensionNuevoArchivo = string.Empty;
            var nombreArchivo = Path.GetFileNameWithoutExtension(ArchivoImportado.FileName);
            var extensionArchivo = Path.GetExtension(ArchivoImportado.FileName);
            if (ArchivoImportado != null)
            {
                using (var lectura = new BinaryReader(ArchivoImportado.InputStream))
                {
                    var textoArchivo = new byte[ArchivoImportado.InputStream.Length];
                    var i = 0;
                    while (lectura.BaseStream.Position != lectura.BaseStream.Length)
                    {
                        textoArchivo[i] = lectura.ReadByte();
                        i++;
                    }
                    if (Opcion == "Descifrar" && extensionArchivo == ".cif")
                    {
                        extensionNuevoArchivo = ".txt";
                        var textoDescifrado = EncriptacionModel.DecryptZZ(textoArchivo, CantidadNiveles);
                        using (var writeStream = new FileStream(Server.MapPath(@"~/App_Data/" + nombreArchivo + extensionNuevoArchivo), FileMode.OpenOrCreate))
                        {
                            using (var writer = new BinaryWriter(writeStream))
                            {
                                writer.Write(textoDescifrado);
                            }
                        }
                    }
                    if (Opcion == "Cifrar" && extensionArchivo == ".txt")
                    {
                        extensionNuevoArchivo = ".cif";
                        var TextoCifrado = EncriptacionModel.EncryptionZigZag(textoArchivo, CantidadNiveles);
                        using (var writeStream = new FileStream(Server.MapPath(@"~/App_Data/" + nombreArchivo + extensionNuevoArchivo), FileMode.OpenOrCreate))
                        {
                            using (var writer = new BinaryWriter(writeStream))
                            {
                                writer.Write(TextoCifrado);
                            }
                        }
                    }
                }

            }
            else
            {
                throw new FileLoadException();
            }
            var fileVirtualPath = @"~/App_Data/" + nombreArchivo + extensionNuevoArchivo;
            return File(fileVirtualPath, "application / force - download", Path.GetFileName(fileVirtualPath));
        }
        public ActionResult CifradoCesar()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CifradoCesar(HttpPostedFileBase ArchivoImportado, string clave, string Opcion)
        {
            var opcionDeCifrado = true;
            if (Opcion == "Descifrar")
            {
                opcionDeCifrado = false;
            }
            var extensionNuevoArchivo = string.Empty;
            var nombreArchivo = Path.GetFileNameWithoutExtension(ArchivoImportado.FileName);
            var extensionArchivo = Path.GetExtension(ArchivoImportado.FileName);
            if (ArchivoImportado != null)
            {

                var DiccionarioCifrado = EncriptacionModel.DiccionarioCesar(clave, opcionDeCifrado);

                if (!opcionDeCifrado && extensionArchivo == ".cif")
                {
                    extensionNuevoArchivo = ".txt";
                }
                if (opcionDeCifrado && extensionArchivo == ".txt")
                {
                    extensionNuevoArchivo = ".cif";
                }
                if (extensionNuevoArchivo != null)
                {
                    using (var Lectura = new BinaryReader(ArchivoImportado.InputStream))
                    {
                        using (var writeStream = new FileStream(Server.MapPath(@"~/App_Data/" + nombreArchivo + extensionNuevoArchivo), FileMode.OpenOrCreate))
                        {
                            using (var writer = new BinaryWriter(writeStream))
                            {
                                var byteBuffer = new byte[bufferLength];
                                while (Lectura.BaseStream.Position != Lectura.BaseStream.Length)
                                {
                                    byteBuffer = Lectura.ReadBytes(bufferLength);
                                    foreach (var item in byteBuffer)
                                    {
                                        if (DiccionarioCifrado.ContainsKey(Convert.ToChar(item)))
                                        {
                                            var ByteEscrito = DiccionarioCifrado[Convert.ToChar(item)];
                                            writer.Write(ByteEscrito);
                                        }
                                        else
                                        {
                                            writer.Write(item);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                throw new FileLoadException();
            }
            var FileVirtualPath = @"~/App_Data/" + nombreArchivo + extensionNuevoArchivo;
            return File(FileVirtualPath, "application / force - download", Path.GetFileName(FileVirtualPath));
        }
        public ActionResult CifradoEspiral()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CifradoEspiral(HttpPostedFileBase ArchivoImportado, int Ancho, string Opcion, string Direccion)
        {
            var opcionDeCifrado = true;
            if (Opcion == "Descifrar")
            {
                opcionDeCifrado = false;
            }
            var direccion = true;
            if (Direccion != "Abajo")
            {
                direccion = false;
            }
            Directory.CreateDirectory(Server.MapPath(@"~/App_Data/"));
            var extensionNuevoArchivo = string.Empty;
            var nombreArchivo = Path.GetFileNameWithoutExtension(ArchivoImportado.FileName);
            var extensionArchivo = Path.GetExtension(ArchivoImportado.FileName);
            var DireccionArchivos = Server.MapPath(@"~/App_Data/");
            if (ArchivoImportado != null)
            {
                var textoArchivo = new byte[ArchivoImportado.InputStream.Length];
                var i = 0;
                using (var lectura = new BinaryReader(ArchivoImportado.InputStream))
                {
                    while (lectura.BaseStream.Position != lectura.BaseStream.Length)
                    {
                        textoArchivo[i] = lectura.ReadByte();
                        i++;
                    }
                }

                if (!opcionDeCifrado && extensionArchivo == ".cif")
                {
                    extensionNuevoArchivo = ".txt";
                }
                if (opcionDeCifrado && extensionArchivo == ".txt")
                {
                    extensionNuevoArchivo = ".cif";
                }
                if (extensionNuevoArchivo != null)
                {
                    var textoResultante = new byte[1];
                    if (opcionDeCifrado)
                    {
                        var auxTextoCifrado = EncriptacionModel.CifradoEspiral(Ancho, direccion, textoArchivo);
                        textoResultante = new byte[auxTextoCifrado.Length];
                        textoResultante = auxTextoCifrado;
                    }
                    else
                    {
                        var auxTextoDescifrado = EncriptacionModel.DescifradoEspiral(Ancho, direccion, textoArchivo);
                        textoResultante = new byte[auxTextoDescifrado.Length];
                        textoResultante = auxTextoDescifrado;
                    }
                    using (var writeStream = new FileStream(Server.MapPath(@"~/App_Data/" + nombreArchivo + extensionNuevoArchivo), FileMode.OpenOrCreate))
                    {
                        using (var writer = new BinaryWriter(writeStream))
                        {
                            writer.Write(textoResultante);
                        }
                    }
                }

            }
            else
            {
                throw new FileLoadException();
            }
            var fileVirtualPath = @"~/App_Data/" + nombreArchivo + extensionNuevoArchivo;
            return File(fileVirtualPath, "application / force - download", Path.GetFileName(fileVirtualPath));
        }
        public ActionResult CifradoSDES()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CifradoSDES(HttpPostedFileBase ArchivoImportado, string clave, string Opcion)
        {
            Directory.CreateDirectory(Server.MapPath(@"~/App_Data/"));
            var extensionNuevoArchivo = string.Empty;
            var nombreArchivo = Path.GetFileNameWithoutExtension(ArchivoImportado.FileName);
            var extensionArchivo = Path.GetExtension(ArchivoImportado.FileName);
            var DireccionArchivos = Server.MapPath(@"~/Others/");
            if (ArchivoImportado != null)
            {
                int NumeroClave = int.Parse(clave);
                if (NumeroClave < 0 || NumeroClave > 1023)
                {
                    throw new FormatException("La clave no cumple con el formato establecido.");
                }
                bool esPosible = false;
                var Keys = EncriptacionModel.ObtenerKeys(NumeroClave, DireccionArchivos);
                if (Opcion == "Descifrar" && extensionArchivo == ".cif")
                {
                    extensionNuevoArchivo = ".txt";
                    Array.Reverse(Keys);
                    esPosible = true;
                }
                if (Opcion == "Cifrar" && extensionArchivo == ".txt")
                {
                    extensionNuevoArchivo = ".cif";
                    esPosible = true;
                }
                if (esPosible)
                {
                    using (var lectura = new BinaryReader(ArchivoImportado.InputStream))
                    {
                        var byteBuffer = new byte[bufferLength];
                        while (lectura.BaseStream.Position != lectura.BaseStream.Length)
                        {
                            byteBuffer = lectura.ReadBytes(bufferLength);
                            var textoEncriptado = EncriptacionModel.SDES(DireccionArchivos, byteBuffer, Keys[0], Keys[1]);
                            using (var writeStream = new FileStream(Server.MapPath(@"~/App_Data/" + nombreArchivo + extensionNuevoArchivo), FileMode.OpenOrCreate))
                            {
                                using (var writer = new BinaryWriter(writeStream))
                                {
                                    writer.Write(textoEncriptado);
                                }
                            }
                        }
                    }

                }
                else
                {
                    Danger("Formato de entrada erronea, por favor, revisalo", true);
                    return View();
                }
            }
            else
            {
                Danger("Archivo nulo, por favor, revisalo", true);
                return View();
            }
            var FileVirtualPath = @"~/App_Data/" + nombreArchivo + extensionNuevoArchivo;
            Success("Archivo procesado exitosamente", true);
            return File(FileVirtualPath, "application / force - download", Path.GetFileName(FileVirtualPath));
        }

        public ActionResult CifradoRSA()
        {
            return View();
        }

        public ActionResult GenerarKeysRSA()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GenerarKeysRSA(string NumberP, string NumberQ)
        {
            //Key 0 = public
            //Key 1 = private
            var Keys = new string[2];
            Keys = RSAEncription.GenerateKeys(NumberP, NumberQ);
            using (var memoryStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    var file1 = archive.CreateEntry("public.key");
                    using (var streamWriter = new StreamWriter(file1.Open()))
                    {
                        streamWriter.Write(Keys[0]);
                    }

                    var file2 = archive.CreateEntry("private.key");
                    using (var streamWriter = new StreamWriter(file2.Open()))
                    {
                        streamWriter.Write(Keys[1]);
                    }
                }
                Success("Llaves generadas exitosamente!", true);
                return File(memoryStream.ToArray(), "application/zip", "Keys.zip");
            }
        }

        public ActionResult CifrarRSA()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CifrarRSA(HttpPostedFileBase ArchivoImportado, HttpPostedFileBase Llave)
        {
            if (ArchivoImportado != null && Llave != null)
            {
                Directory.CreateDirectory(Server.MapPath(@"~/App_Data/"));
                var archivoLlave = Path.GetFileName(Llave.FileName);
                var extensionArchivo = Path.GetExtension(ArchivoImportado.FileName);
                var extensionLlave = Path.GetExtension(Llave.FileName);
                var nombreArchivo = Path.GetFileNameWithoutExtension(ArchivoImportado.FileName);
                var DireccionArchivos = Server.MapPath(@"~/Others/");
                if (extensionLlave == ".key" && extensionArchivo == ".txt")
                {
                    string[] llave;
                    using (var lectura = new StreamReader(Llave.InputStream))
                    {
                        llave = lectura.ReadLine().Split(',');
                    }//llave[0]=d y llave[1]=N 
                    using (var lectura = new BinaryReader(ArchivoImportado.InputStream))
                    {
                        var byteBuffer = new byte[bufferLength];
                        byte[] textoEncriptado = null;

                        while (lectura.BaseStream.Position != lectura.BaseStream.Length)
                        {
                            using (var writeStream = new FileStream(Server.MapPath(@"~/App_Data/" + nombreArchivo + ".rsacif"), FileMode.OpenOrCreate))
                            {
                                using (var writer = new BinaryWriter(writeStream))
                                {
                                    byteBuffer = lectura.ReadBytes(bufferLength);
                                    textoEncriptado = RSAEncription.Cifrado(byteBuffer, llave);
                                    writer.Write(textoEncriptado);
                                }
                            }
                        }
                    }
                    Success("Archivo comprimido correctamente", true);
                    var FileVirtualPath = @"~/App_Data/" + nombreArchivo + ".rsacif";
                    return File(FileVirtualPath, "application / force - download", Path.GetFileName(FileVirtualPath));
                }
                else
                {
                    Danger("Formato de entrada incorrecto", true);
                    return View();
                }
            }
            else
            {
                Danger("Archivo nulo", true);
                return View();
            }
        }
    

        public ActionResult DescifrarRSA()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DescifrarRSA(HttpPostedFileBase ArchivoImportado, HttpPostedFileBase Llave)
        {           
            if (ArchivoImportado != null && Llave != null)
            {
                Directory.CreateDirectory(Server.MapPath(@"~/App_Data/"));
                var archivoLlave = Path.GetFileName(Llave.FileName);
                var extensionArchivo = Path.GetExtension(ArchivoImportado.FileName);
                var extensionLlave = Path.GetExtension(Llave.FileName);
                var nombreArchivo = Path.GetFileNameWithoutExtension(ArchivoImportado.FileName);
                var DireccionArchivos = Server.MapPath(@"~/Others/");
                if (extensionLlave == ".key" && extensionArchivo == ".rsacif")
                {
                    string[] llave;
                    using (var lectura = new StreamReader(Llave.InputStream))
                    {
                        llave = lectura.ReadLine().Split(',');
                    }//llave[0]=d y llave[1]=N 
                    using (var lectura = new BinaryReader(ArchivoImportado.InputStream))
                    {
                        var byteBuffer = new byte[bufferLength];
                        byte[] textoEncriptado = null;

                        while (lectura.BaseStream.Position != lectura.BaseStream.Length)
                        {
                            using (var writeStream = new FileStream(Server.MapPath(@"~/App_Data/" + nombreArchivo + ".txt"), FileMode.OpenOrCreate))
                            {
                                using (var writer = new BinaryWriter(writeStream))
                                {
                                    byteBuffer = lectura.ReadBytes(bufferLength);
                                    textoEncriptado = RSAEncription.Descifrado(byteBuffer, llave);
                                    writer.Write(textoEncriptado);
                                }
                            }
                        }
                    }
                    Success("Archivo comprimido correctamente", true);
                    var FileVirtualPath = @"~/App_Data/" + nombreArchivo + ".txt";
                    return File(FileVirtualPath, "application / force - download", Path.GetFileName(FileVirtualPath));
                }
                else
                {
                    Danger("Formato de entrada incorrecto", true);
                    return View();
                }
            }
            else
            {
                Danger("Archivo nulo", true);
                return View();
            }
        }
    }
}
