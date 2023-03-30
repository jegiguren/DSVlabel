using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BarTenderEtiketak
{
    public class XmlIrakurtzaile
    {
        private XmlDocument xmlDoc;

        public XmlIrakurtzaile(string filePath)
        {
            xmlDoc = new XmlDocument();
            xmlDoc.Load(filePath);
        }

        public void ReadXml()
        {
            XmlNode root = xmlDoc.SelectSingleNode("DatosEntrada");
            string proyecto = root.SelectSingleNode("Proyecto").InnerText;
            string codigoArticulo = root.SelectSingleNode("Codigo_Articulo").InnerText;
            string anyo = root.SelectSingleNode("Anyo").InnerText;
            string mes = root.SelectSingleNode("Mes").InnerText;
            string dia = root.SelectSingleNode("Dia").InnerText;
            string cantidad = root.SelectSingleNode("Cantidad").InnerText;
            string lote = root.SelectSingleNode("Lote").InnerText;
            string usuario = root.SelectSingleNode("Usuario").InnerText;
            string etiquetas = root.SelectSingleNode("Etiquetas").InnerText;
            string codBarras = root.SelectSingleNode("CodBarras").InnerText;
            string hojaGarantia = root.SelectSingleNode("HojaGarantia").InnerText;

            Console.WriteLine("Proyecto: " + proyecto);
            Console.WriteLine("Código de Artículo: " + codigoArticulo);
            Console.WriteLine("Año: " + anyo);
            Console.WriteLine("Mes: " + mes);
            Console.WriteLine("Día: " + dia);
            Console.WriteLine("Cantidad: " + cantidad);
            Console.WriteLine("Lote: " + lote);
            Console.WriteLine("Usuario: " + usuario);
            Console.WriteLine("Etiquetas: " + etiquetas);
            Console.WriteLine("Código de Barras: " + codBarras);
            Console.WriteLine("Hoja de Garantía: " + hojaGarantia);

            XmlNode numerosSerieNode = root.SelectSingleNode("Numeros_Serie");
            if (numerosSerieNode != null)
            {
                foreach (XmlNode serieNode in numerosSerieNode.SelectNodes("Serie"))
                {
                    string serie = serieNode.InnerText;
                    Console.WriteLine("Número de Serie: " + serie);
                }
            }
        }
    }
}
