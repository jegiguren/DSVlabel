using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarTenderEtiketak
{
    internal class XmlDeleter
    {
        string origen = @"C:\bt\XML\";
        string destino = @"C:\bt\XML kopiak\";
        public XmlDeleter() { }

        public void ezabatuXml()
        {
            string[] fitxategiak = Directory.GetFiles(origen);

            foreach (string fitxategia in fitxategiak)
            {

                if (File.Exists(fitxategia))
                {
                    // Mover el archivo a la carpeta de destino
                    File.Move(fitxategia, Path.Combine(destino, Path.GetFileName(fitxategia)));
                    Console.WriteLine("Fitxategia ongi mugitu da Xml kopiak karpetara");
                }
                else
                {
                    Console.WriteLine("Fitxategia ez da ongi mugitu karpetara");
                }
            }
        }
    }
}
