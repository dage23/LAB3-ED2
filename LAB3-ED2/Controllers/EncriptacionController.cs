using System;
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
        public ActionResult CifradoZigZag()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CifradoZigZag(HttpPostedFileBase ArchivoImportado,int CantidadNiveles,string Opcion)
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
            var ExtensionNuevoArchivo = ".cif";
            var NombreArchivo = Path.GetFileNameWithoutExtension(ArchivoImportado.FileName);
            var ExtensionArchivo = Path.GetExtension(ArchivoImportado.FileName);
            if (ArchivoImportado != null)
            {

                var DiccionarioCifrado = new Dictionary<char, char>();
                var Cesar = new EncriptacionModel();
                DiccionarioCifrado = Cesar.DiccionarioCesar(clave/*, OpcionDeCifrado*/);
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
                //Danger("El archivo es nulo.", true);
            }
            return View();      
        }        
    }
}
