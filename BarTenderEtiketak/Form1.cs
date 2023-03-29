
using Seagull.BarTender.Print;
using Seagull.BarTender.PrintServer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace BarTenderEtiketak
{
    public partial class Form1 : Form
    {

        SqlConnection conn = new SqlConnection("Data Source = itd2682303; Initial Catalog = Etiketa_DB; User ID = sa; Password=2023SQLServer2019");

        
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: esta línea de código carga datos en la tabla 'etiketa_DBDataSet.etiketak' Puede moverla o quitarla según sea necesario.
            this.etiketakTableAdapter.Fill(this.etiketa_DBDataSet.etiketak);

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string query = "SELECT mota, pisua, kolorea FROM etiketak WHERE id = @id";

            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }

            using (SqlCommand command = new SqlCommand(query, conn))
            {
                // al parametro id se le asigna el valor seleccionado en el combobox
                command.Parameters.AddWithValue("@id", comboBox1.SelectedValue);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // Llena los TextBox con los valores correspondientes
                        textBox1.Text = reader["mota"].ToString();
                        textBox2.Text = reader["pisua"].ToString();
                        textBox3.Text = reader["kolorea"].ToString();
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Inpresio motorearen instantzia sortu
            Engine btEngine = new Engine();

            // Inpresio zerbitzariarekin konektatu
            btEngine.Start();

            // Etiketaren fitxategia ireki
            //LabelFormatDocument btFormat = btEngine.Documents.Open(@"C:\bt\Cycle.btw");
            LabelFormatDocument btFormat = btEngine.Documents.Open(@"C:\bt\FrogakYoko\EtiketaFroga2.btw");
           

            // Etiketaren datuak aktualizatu
            btFormat.SubStrings["VAR1"].Value = textBox1.Text;
            btFormat.SubStrings["VAR2"].Value = textBox2.Text;
            btFormat.SubStrings["VAR3"].Value = textBox3.Text;


            // Inpresora konfiguratu
            //btFormat.PrintSetup.PrinterName = "Intermec PM43c_406_BACKUP";
            btFormat.PrintSetup.PrinterName = "Microsoft Print to Pdf";

            // etiketa inprimatu
            btFormat.Print();

            // etiketaren dokumentua itxi
            btFormat.Close(SaveOptions.DoNotSaveChanges);

            // Inpresio motorea gelditu
            btEngine.Stop();
        }

        private void Xml_print_Click(object sender, EventArgs e)
        {
            // Inpresio motorearen instantzia sortu
            Engine btEngine = new Engine();

            // Inpresio zerbitzariarekin konektatu
            btEngine.Start();
           
            // Etiketaren fitxategia ireki eta etiketa aldagaian gorde
            LabelFormatDocument etiketa = btEngine.Documents.Open(@"C:\bt\FrogakYoko\EtiketaFrogaXml.btw");

            //XmlDocument klaseko objetua sortu
            XmlDocument xmlDoc = new XmlDocument();

            // xml dokumentuaren ruta jaso
            string ruta = @"C:\bt\XML\entrada20180904104808804.xml";

            //xml-a kargatu aldagaian
            xmlDoc.Load(ruta);

            //root nodoa(aurrena) aldagai batean gorde
            XmlNode rootNode = xmlDoc.DocumentElement;

            BaloreakAsignatu(rootNode, etiketa);
           
        }


        private static void BaloreakAsignatu(XmlNode nodoXml, LabelFormatDocument etiketa)
        {
            string nodoIzena = "";
            string nodoBalorea = "";
            SubStrings aldagaiak = null;

            foreach (XmlNode nodo in nodoXml.ChildNodes)
            {
                if(nodo.Name != "Numeros_Serie") {

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
                                nodoBalorea=nodoSerial.InnerText;

                                // Asignar el valor de la variable a la variable de la etiqueta
                                aldagaia.Value = nodoBalorea;

                                Inprimatu(etiketa);
                            }
                        }
                    }
                }
            }
        }


        private static void Inprimatu (LabelFormatDocument etiketa)
        {
            Engine btEngine = new Engine();

            // Inpresio zerbitzariarekin konektatu
            btEngine.Start();

            // Inpresora konfiguratu
            //etiketa.PrintSetup.PrinterName = "Intermec PM43c_406_BACKUP";
            etiketa.PrintSetup.PrinterName = "Microsoft Print to Pdf";

            // etiketa inprimatu
            etiketa.Print();
            Thread.Sleep(5000);

            // etiketaren dokumentua itxi
            //etiketa.Close(SaveOptions.DoNotSaveChanges);

            btEngine.Stop();

        }



    }
}
