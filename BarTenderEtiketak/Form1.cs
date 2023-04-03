
using Seagull.BarTender.Print;
using Seagull.BarTender.PrintServer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace BarTenderEtiketak
{
    public partial class Form1 : Form
    {
        string directoryPath = @"C:\bt\XML";//ERP-ak xml-ak uzten dituen karpeta
        private AutoResetEvent fileCreatedEvent = new AutoResetEvent(false);
        string xmlFilePath;//ERP-ak sortzen duen xml-aren ruta osoa (karpeta+fitxategi izena)
        XmlDocument xmlDoc, xmlWebService, xmlosoa;
        LabelFormatDocument etiketa;
        FileSystemWatcher watcher = null;


        public Form1()
        {
            InitializeComponent();
        }

        private async void Xml_print_Click(object sender, EventArgs e)
        {
            KoloreaAldatu();

            // Inpresio motorearen instantzia sortu
            Engine btEngine = new Engine();

            // Inpresio zerbitzariarekin konektatu
            btEngine.Start();

            // Etiketaren fitxategia ireki eta etiketa aldagaian gorde
            etiketa = btEngine.Documents.Open(@"C:\bt\FrogakYoko\EtiketaFrogaXml.btw");

            //XmlDocument klaseko objetuak sortu
            xmlDoc = new XmlDocument(); //ERP-ak sortuko duen xml-a
            xmlWebService = new XmlDocument(); //Web zerbitzutik jasoko dugun xml-a
            xmlosoa = new XmlDocument(); //aurreko 2 xml-ak juntatuta lortzen dugun xml-a


            begiratuKarpeta(directoryPath);
    
        }

        private async Task begiratuKarpeta(string filePath)
        {
            // Crear un objeto FileSystemWatcher
            watcher = new FileSystemWatcher();
            watcher.Path = filePath;

            // Vigilar solo los archivos con extensión .xml
            watcher.Filter = "*.xml";

            // Suscribirse al evento cuando se detecte un cambio en la carpeta
            watcher.Created += OnChanged;

            // Iniciar la vigilancia
            watcher.EnableRaisingEvents = true;

            // Esperar a que se detecte un archivo
            Console.WriteLine("XML karpeta zaintzen...", filePath);
            Console.ReadLine();

            while (true)
            {
                // Esperar a que se cree un archivo en la carpeta
                fileCreatedEvent.WaitOne();

                // Leer el archivo XML
                Console.WriteLine(xmlFilePath + " fitxategia aurkitua da");
                //xmlIzena = Path.GetFileName(xmlFilePath); //fitxategiaren izena lortu

                //aldagaian kargatu xml-a
                xmlDoc.Load(xmlFilePath);

                //xml-tik kodigo artikulua atera gero Ws-ari bidaltzeko
                string codigoArticulo = KodigoaAtera(xmlDoc);

                //WsReader klaseko objetua sortu
                WsReader wsreader = new WsReader();
                //web zerbitzua kontsumitu parametro bezala kodigoa bidaliz eta emaitza xml batean gorde
                xmlWebService = await wsreader.WsKontsumitu(codigoArticulo);

                //ERP-aren xml-a eta Web zerbitzuaren xml- juntatu
                xmlosoa = JuntatuXmlak(xmlWebService, xmlDoc);
                Console.WriteLine(xmlosoa.OuterXml);

                //root nodoa (aurrena) aldagai batean gorde
                XmlNode rootNode = xmlosoa.DocumentElement;

                BaloreakAsignatu(rootNode, etiketa);

                //crea un objeto de la clase XmlDeleter
                XmlDeleter deleter = new XmlDeleter();

                //borra el archivo de la carpeta "XML" y guarda una copia en la carpeta "Xml kopiak"
                Thread.Sleep(500);
                deleter.ezabatuXml();
            }

        }


        private void OnChanged(object source, FileSystemEventArgs e)
        {
            // Leer el archivo XML
            xmlFilePath = e.FullPath;

            // Señalizar el evento de que se ha creado un archivo en la carpeta
            fileCreatedEvent.Set();

            
        }

        private static void BaloreakAsignatu(XmlNode nodoXml, LabelFormatDocument etiketa)
        {
            string nodoIzena = "";
            string nodoBalorea = "";
            SubStrings aldagaiak = null;

            foreach (XmlNode nodo in nodoXml.ChildNodes)
            {
                if (nodo.Name != "Numeros_Serie")
                {

                    // Obtener el nombre del nodo
                    nodoIzena = nodo.Name;

                    // Obtener el valor del nodo
                    nodoBalorea = nodo.InnerText;

                    // Obtener las variables de la etiqueta
                    aldagaiak = etiketa.SubStrings;

                    // Recorrer las variables de la etiqueta
                    foreach (SubString aldagaia in aldagaiak)
                    {
                        // Comparar el nombre de la variable con el nombre de la variable a asignar
                        if (aldagaia.Name == nodoIzena)
                        {
                            // Asignar el valor de la variable a la variable de la etiqueta
                            aldagaia.Value = nodoBalorea;
                        }
                    }
                }

                //XML dokumentoan "Numeros_Serie" aurkitzen duenean egingo duena
                else
                {
                    foreach (XmlNode nodoSerial in nodo.ChildNodes)
                    {
                        foreach (SubString aldagaia in aldagaiak)
                        {
                            // Comparar el nombre de la variable con el nombre de la variable a asignar
                            if (aldagaia.Name == "Serie")
                            {
                                //nodoaren balore aldagaia batean gorde
                                nodoBalorea = nodoSerial.InnerText;

                                // Asignar el valor de la variable a la variable de la etiqueta
                                aldagaia.Value = nodoBalorea;

                                Inprimatu(etiketa);
                            }
                        }
                    }
                }
            }
        }

        private string KodigoaAtera(XmlDocument ErpXml)
        {

            // Obtener el nodo "Codigo_Articulo"
            XmlNode codigoArticuloNode = ErpXml.SelectSingleNode("//Codigo_Articulo");

            // Obtener el valor del nodo y asignarlo a una variable
            string codigoArticulo = null;
            try
            {
                codigoArticulo = codigoArticuloNode.InnerText;
                return codigoArticulo;
            }
            catch (NullReferenceException ex)
            {
                // Manejar la excepción si el nodo no se encuentra
                Console.WriteLine("No se encontró el nodo 'Codigo_Articulo'");
                return null;
            }
        }


        private static void Inprimatu(LabelFormatDocument etiketa)
        {
            Engine btEngine = new Engine();

            // Inpresio zerbitzariarekin konektatu
            btEngine.Start();

            // Inpresora konfiguratu
            //etiketa.PrintSetup.PrinterName = "Intermec PM43c_406_BACKUP";
            etiketa.PrintSetup.PrinterName = "Microsoft Print to Pdf";

            // etiketa inprimatu
            etiketa.Print();
            Thread.Sleep(1000);

            // etiketaren dokumentua itxi
            //etiketa.Close(SaveOptions.DoNotSaveChanges);

            btEngine.Stop();

        }

        private async void btn_WsKontsumitu_Click(object sender, EventArgs e)
        {
            XmlDocument xmlWebService = new XmlDocument();
            WsReader wsreader = new WsReader();
            xmlWebService = await wsreader.WsKontsumitu("5846005");
            Console.WriteLine(xmlWebService.OuterXml);

        }


        public XmlDocument JuntatuXmlak(XmlDocument xmlDoc1, XmlDocument xmlDoc2)
        {
            XmlDocument xmlDoc = new XmlDocument();

            // Crear el nodo raíz
            XmlNode rootNode = xmlDoc.CreateElement("root");
            xmlDoc.AppendChild(rootNode);

            // Importar los nodos hijos del primer documento
            foreach (XmlNode node in xmlDoc1.DocumentElement.ChildNodes)
            {
                XmlNode importedNode = xmlDoc.ImportNode(node, true);
                rootNode.AppendChild(importedNode);
            }

            // Importar los nodos hijos del segundo documento
            foreach (XmlNode node in xmlDoc2.DocumentElement.ChildNodes)
            {
                XmlNode importedNode = xmlDoc.ImportNode(node, true);
                rootNode.AppendChild(importedNode);
            }

            return xmlDoc;
        }

        private void KoloreaAldatu()
        {
            if (Xml_print.BackColor != Color.LightGreen)
            {
                // Cambiar el color del botón a verde si no lo está
                Xml_print.BackColor = Color.LightGreen;
            }
            else
            {
                // Cambiar el color del botón a su color original si ya está en verde
                Xml_print.BackColor = DefaultBackColor;
            }
        }

        private void btn_Gelditu_Click(object sender, EventArgs e)
        {
            if (watcher != null)
            {
                watcher.Created -= OnChanged;
                watcher.EnableRaisingEvents = false;
            }
        }
    }
}