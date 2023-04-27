
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Rectangle = iTextSharp.text.Rectangle;

namespace DSVlabel
{
    public partial class Form1 : Form
    {
        string pdfInputPath = @"C:\ZEBRA etiketak\DSV\PDFinput";//ERP-ak pdf-ak uzten dituen karpeta
        string pdfOutputPath = @"C:\ZEBRA etiketak\DSV\PDFoutput";//dimensioa aldatutako pdf-ak nun gordeko diren
        string pdfFilePath, pdfFilePathdestino;
        private AutoResetEvent fileCreatedEvent = new AutoResetEvent(false);
        FileSystemWatcher watcher = null;
        private Thread begiraleThread;
        private bool begira = false;
        List<string> fileNames = new List<string>();
        string intermec = "Intermec PM43c_406_BACKUP";
        string pdf = "Microsoft Print to Pdf";
        string konica = "KONICA MINOLTA Admin";


        public Form1()
        {
            InitializeComponent();
            btn_Gelditu.Enabled = false;
        }

        private async void Xml_print_Click(object sender, EventArgs e)
        {
            KoloreaAldatu();

            //begiralea martxan jarri hari batean
            begiraleThread = new Thread(() => begiratuKarpeta(pdfInputPath));
            begiraleThread.Start();

            Xml_print.Enabled = false;
            Xml_print.Text = "BEGIRALEA MARTXAN DAGO...";
            btn_Gelditu.Enabled = true;
            begira = true;
    
        }

        private async Task begiratuKarpeta(string filePath)
        {
            // Crear un objeto FileSystemWatcher
            watcher = new FileSystemWatcher();
            watcher.Path = filePath;

            // Vigilar solo los archivos con extensión .xml
            watcher.Filter = "*.pdf";

            // Suscribirse al evento cuando se detecte un cambio en la carpeta
            watcher.Created += OnChanged;

            // Iniciar la vigilancia
            watcher.EnableRaisingEvents = true;

            // Esperar a que se detecte un archivo
            Console.WriteLine("PDF karpeta zaintzen...", filePath);

            while (true)
            {
                // Esperar a que se cree un archivo en la carpeta
                fileCreatedEvent.WaitOne();
                
                Console.WriteLine(pdfFilePath + " fitxategia aurkitua da");


                EskalatuPDF(pdfFilePath, Path.Combine(pdfOutputPath, Path.GetFileName(pdfFilePath)));

                //crea un objeto de la clase PdfDeleter
                //PdFDeleter deleter = new PdFDeleter();

                //borra el archivo de la carpeta "PDFinput" y guarda una copia en la carpeta "PDFoutput"
                //Thread.Sleep(500);
                //deleter.ezabatuPdf();

                //Limpiar el listbox despues de imprimir
                Invoke(new Action(() =>
                {
                    listBox1.Items.Clear();
                }));
            }
        }

        private void OnChanged(object source, FileSystemEventArgs e)
        {
            // Leer el archivo PDF
            pdfFilePath = e.FullPath;

            // Señalizar el evento de que se ha creado un archivo en la carpeta
            fileCreatedEvent.Set();

            // Agregar el nombre del archivo a la lista
            fileNames.Add(e.Name);

            // Actualizar el contenido del ListBox con los nombres de los archivos
            listBox1.Invoke(new Action(() => {
                listBox1.Items.Clear();
                listBox1.Items.AddRange(fileNames.ToArray());
            }));
        }

        public void EskalatuPDF(string pdfin, string pdfout)
        {
            // Abrir el archivo PDF de origen
            PdfReader reader = new PdfReader(pdfin);

            // Obtener el número total de páginas del PDF
            int numPaginas = reader.NumberOfPages;

            // Crear un objeto Document para escribir en el PDF de salida
            Document doc = new Document(new Rectangle(295f, 479f));

            // Crear un objeto PdfWriter para escribir en el PDF de salida
            PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(pdfout, FileMode.Create));

            // Abrir el objeto Document para comenzar a escribir en él
            doc.Open();

            // Iterar sobre cada página del PDF de origen
            for (int pagina = 1; pagina <= numPaginas; pagina++)
            {
                // Obtener las dimensiones de la página actual
                Rectangle pageSize = reader.GetPageSize(pagina);

                // Crear una nueva plantilla con las dimensiones deseadas
                PdfTemplate template = writer.GetImportedPage(reader, pagina).CreateTemplate(295f, 479f);

                // Agregar la página original a la plantilla
                template.AddTemplate(writer.GetImportedPage(reader, pagina), 295f / pageSize.Width, 0, 0, 479f / pageSize.Height, 0, 0);

                // Agregar la plantilla a una nueva página
                doc.NewPage();
                PdfContentByte cb = writer.DirectContent;
                cb.AddTemplate(template, 0, 0);

            }

            // Cerrar el objeto Document y el objeto PdfReader
            doc.Close();
            reader.Close();
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
            btn_Gelditu.Enabled = false;

            watcher.EnableRaisingEvents = false; // Desactivar la generación de eventos del objeto FileSystemWatcher
            watcher.Created -= OnChanged; // Desuscribirse del evento cuando se detecte un cambio en la carpeta
            watcher.Dispose(); // Liberar los recursos del objeto FileSystemWatcher
            fileCreatedEvent.Reset(); // Resetear el AutoResetEvent utilizado para esperar la creación de archivos en la carpeta

            Xml_print.Enabled = true;
            KoloreaAldatu();
            Xml_print.Text = "BEGIRALEA MARTXAN JARRI";
            

            if (watcher != null)
            {
                watcher.Created -= OnChanged;
                watcher.EnableRaisingEvents = false;
            }
        }
    }
}