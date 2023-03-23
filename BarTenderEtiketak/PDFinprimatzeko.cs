using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarTenderEtiketak
{
    internal class PDFinprimatzeko
    {
        public void Raw_impresioa_pdf(ref Aldagai_orokorrak aldagaiak, Seagull.BarTender.Print.Engine BTENGINE, Seagull.BarTender.Print.LabelFormatDocument BTFORMAT)
        {
            // Establece el nombre de la impresora en la que se va a imprimir en PDFCreator
            BTFORMAT.PrintSetup.PrinterName = "PDFCreator";

            // Divide la propiedad Katea del objeto aldagaiak en un array usando '|' como separador
            string[] subkatea;
            subkatea = aldagaiak.Katea.Split('|');

            // Asigna el valor del tercer elemento del array subkatea a la variable serieZenbakia
            string serieZenbakia = subkatea[3];

            // Inicializa la variable i en 3
            int i = 3;

            // Recorre los elementos del array subkatea desde el cuarto elemento hasta el penúltimo
            for (i = i; i <= subkatea.GetUpperBound(0) - 1; i++)
            {
                try
                {
                    // Intenta establecer el valor correspondiente en la variable de texto VAR del objeto BTFORMAT
                    BTFORMAT.SubStrings.SetSubString("VAR" + (i - 2), subkatea[i]);
                }
                catch (Exception ex)
                {
                }
            }

            try
            {
                // Imprime la etiqueta interna del subkatea[3] usando BTFORMAT
                BTFORMAT.Print(subkatea[3] + "_INTERNAL_Label");

                // Espera hasta que BTENGINE haya terminado de procesar comandos o imprimir
                while (BTENGINE.IsProcessingCommandLines || BTENGINE.IsPrinting)
                {
                    System.Threading.Thread.Sleep(50);
                }
            }
            catch (Exception ex)
            {
                throw new System.Exception("ERROR1.Kontuz.Arazoak Aluminiozko imprimagailuarekin\n" + ex.Message);
            }
        }
    }
}
