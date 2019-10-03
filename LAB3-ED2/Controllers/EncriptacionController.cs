﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using LAB3_ED2.Models;
namespace LAB3_ED2.Controllers
{
    public class EncriptacionController : Controller
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
            var ExtensionNuevoArchivo = string.Empty;
            var NombreArchivo = Path.GetFileNameWithoutExtension(ArchivoImportado.FileName);
            var ExtensionArchivo = Path.GetExtension(ArchivoImportado.FileName);
            if (ArchivoImportado != null)
            {
                using (var Lectura = new StreamReader(ArchivoImportado.InputStream))
                {
                    var TextoArchivo = Lectura.ReadToEnd();
                    if (Opcion == "Descifrar" && ExtensionArchivo == ".cif")
                    {
                        ExtensionNuevoArchivo = ".txt";
                        var TextoDescifrado = new EncriptacionModel().DecryptZZ(TextoArchivo, CantidadNiveles);
                        using (var writeStream = new FileStream(Server.MapPath(@"~/App_Data/" + NombreArchivo + ExtensionNuevoArchivo), FileMode.OpenOrCreate))
                        {
                            using (var writer = new StreamWriter(writeStream))
                            {
                                writer.Write(TextoDescifrado);
                            }
                        }
                    }
                    if (Opcion == "Cifrar" && ExtensionArchivo == ".txt")
                    {
                        ExtensionNuevoArchivo = ".cif";
                        var TextoCifrado = new EncriptacionModel().EncryptionZigZag(TextoArchivo, CantidadNiveles);
                        using (var writeStream = new FileStream(Server.MapPath(@"~/App_Data/" + NombreArchivo + ExtensionNuevoArchivo), FileMode.OpenOrCreate))
                        {
                            using (var writer = new StreamWriter(writeStream))
                            {
                                writer.Write(TextoCifrado);
                            }
                        }
                    }
                }

            }
            var FileVirtualPath = @"~/App_Data/" + NombreArchivo + ExtensionNuevoArchivo;
            return File(FileVirtualPath, "application / force - download", Path.GetFileName(FileVirtualPath));
        }
        public ActionResult CifradoCesar()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CifradoCesar(HttpPostedFileBase ArchivoImportado, string clave, string Opcion)
        {
            var OpcionDeCifrado = true;
            if (Opcion == "Descifrar")
            {
                OpcionDeCifrado = false;
            }
            var ExtensionNuevoArchivo = string.Empty;
            var NombreArchivo = Path.GetFileNameWithoutExtension(ArchivoImportado.FileName);
            var ExtensionArchivo = Path.GetExtension(ArchivoImportado.FileName);
            if (ArchivoImportado != null)
            {

                var DiccionarioCifrado = new Dictionary<char, char>();
                var Cesar = new EncriptacionModel();
                DiccionarioCifrado = Cesar.DiccionarioCesar(clave, OpcionDeCifrado);

                if (!OpcionDeCifrado && ExtensionArchivo == ".cif")
                {
                    ExtensionNuevoArchivo = ".txt";
                }
                if (OpcionDeCifrado && ExtensionArchivo == ".txt")
                {
                    ExtensionNuevoArchivo = ".cif";
                }
                if (ExtensionNuevoArchivo != null)
                {
                    using (var Lectura = new BinaryReader(ArchivoImportado.InputStream))
                    {
                        using (var writeStream = new FileStream(Server.MapPath(@"~/App_Data/" + NombreArchivo + ExtensionNuevoArchivo), FileMode.OpenOrCreate))
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
                else
                {
                    //Danger("El archivo tiene un formato erroneo.", true);
                }

            }
            else
            {
                //Danger("El archivo es nulo.", true);
            }
            var FileVirtualPath = @"~/App_Data/" + NombreArchivo + ExtensionNuevoArchivo;
            return File(FileVirtualPath, "application / force - download", Path.GetFileName(FileVirtualPath));
        }



        public ActionResult CifradoEspiral()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CifradoEspiral(HttpPostedFileBase ArchivoImportado, int Ancho, string Opcion, string Direccion)
        {
            var OpcionDeCifrado = true;
            if (Opcion == "Descifrar")
            {
                OpcionDeCifrado = false;
            }

            Directory.CreateDirectory(Server.MapPath(@"~/App_Data/"));
            var ExtensionNuevoArchivo = string.Empty;
            var NombreArchivo = Path.GetFileNameWithoutExtension(ArchivoImportado.FileName);
            var ExtensionArchivo = Path.GetExtension(ArchivoImportado.FileName);
            if (ArchivoImportado != null)
            {
                if (!OpcionDeCifrado && ExtensionArchivo == ".cif")
                {
                    ExtensionNuevoArchivo = ".txt";
                }
                if (OpcionDeCifrado && ExtensionArchivo == ".txt")
                {
                    ExtensionNuevoArchivo = ".cif";
                }
                if (ExtensionNuevoArchivo != null)
                {
                    var TextoCifrado = string.Empty;
                    using (var Lectura = new StreamReader(ArchivoImportado.InputStream))
                    {
                        var TextoArchivo = Lectura.ReadToEnd();
                        if (ExtensionArchivo == ".txt")
                        {
                            if (Direccion == "Derecha")
                            {
                                TextoCifrado = new EncriptacionModel().EspiralDerecha(Ancho, OpcionDeCifrado, TextoArchivo);
                            }
                            else
                            {
                                TextoCifrado = new EncriptacionModel().EspiralAbajo(Ancho, OpcionDeCifrado, TextoArchivo);
                            }
                            using (var writeStream = new FileStream(Server.MapPath(@"~/App_Data/" + NombreArchivo + ExtensionNuevoArchivo), FileMode.OpenOrCreate))
                            {
                                using (var writer = new StreamWriter(writeStream))
                                {
                                    writer.Write(TextoCifrado);
                                }
                            }
                        }
                        else
                        {
                            if (Direccion == "Derecha")
                            {
                                TextoCifrado = new EncriptacionModel().EspiralDerecha(Ancho, OpcionDeCifrado, TextoArchivo);
                            }
                            else
                            {
                                TextoCifrado = new EncriptacionModel().EspiralAbajo(Ancho, OpcionDeCifrado, TextoArchivo);
                            }
                            using (var writeStream = new FileStream(Server.MapPath(@"~/App_Data/" + NombreArchivo + ExtensionNuevoArchivo), FileMode.OpenOrCreate))
                            {
                                using (var writer = new StreamWriter(writeStream))
                                {
                                    writer.Write(TextoCifrado);
                                }
                            }
                        }
                    }
                }

            }
            var FileVirtualPath = @"~/App_Data/" + NombreArchivo + ExtensionNuevoArchivo;
            return File(FileVirtualPath, "application / force - download", Path.GetFileName(FileVirtualPath));
        }
        
    }
}
