using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSVlabel
{
    internal class PdFDeleter
    {
        string origen = @"C:\ZEBRA etiketak\DSV\PDFinput";
        string destino = @"C:\ZEBRA etiketak\DSV\PDFoutput";
        public PdFDeleter() { }

        public void ezabatuPdf()
        {
            string[] fitxategiak = Directory.GetFiles(origen);

            foreach (string fitxategia in fitxategiak)
            {

                if (File.Exists(fitxategia))
                {
                    // Mover el archivo a la carpeta de destino
                    File.Move(fitxategia, Path.Combine(destino, Path.GetFileName(fitxategia)));
                    Console.WriteLine("Fitxategia ongi mugitu da PDFoutput karpetara");
                    Console.WriteLine("PDFinput karpeta zaintzen...");
                }
                else
                {
                    Console.WriteLine("Fitxategia ez da ongi mugitu karpetara");
                }
            }
        }
    }
}
