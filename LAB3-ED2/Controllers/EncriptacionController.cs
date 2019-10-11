using System;
using System.Web;
using System.Web.Mvc;
using System.IO;
using LAB3_ED2.Models;
namespace LAB3_ED2.Controllers
{
    public class EncriptacionController : Controller
    {
        const int bufferLength = 100;
        public ActionResult SerieI()
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
                    while (lectura.BaseStream.Position!=lectura.BaseStream.Length)
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
            if (Direccion!="Abajo")
            {
                direccion = false;
            }
            Directory.CreateDirectory(Server.MapPath(@"~/App_Data/"));
            var extensionNuevoArchivo = string.Empty;
            var nombreArchivo = Path.GetFileNameWithoutExtension(ArchivoImportado.FileName);
            var extensionArchivo = Path.GetExtension(ArchivoImportado.FileName);
            var DireccionArchivos=Server.MapPath(@"~/App_Data/");
            if (ArchivoImportado != null)
            {
                var textoArchivo = new byte[ArchivoImportado.InputStream.Length];
                var i = 0;
                using (var lectura=new BinaryReader(ArchivoImportado.InputStream))
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
        public ActionResult SerieII()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult SerieII(HttpPostedFileBase ArchivoImportado, string clave, string Opcion)
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
                if (!opcionDeCifrado && extensionArchivo == ".cif")
                {
                    extensionNuevoArchivo = ".txt";
                }
                if (opcionDeCifrado && extensionArchivo == ".txt")
                {
                    extensionNuevoArchivo = ".cif";
                }
                int NumeroClave = Int32.Parse(clave);
                if (NumeroClave<0||NumeroClave>1023)
                {
                    throw new FormatException("La clave no cumple con el formato establecido.");
                }

            }
            else
            {
                throw new FileLoadException();
            }
            var FileVirtualPath = @"~/App_Data/" + nombreArchivo + extensionNuevoArchivo;
            return File(FileVirtualPath, "application / force - download", Path.GetFileName(FileVirtualPath));
        }

    }
}
