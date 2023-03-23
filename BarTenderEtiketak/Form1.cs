
using Seagull.BarTender.Print;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            btFormat.PrintSetup.PrinterName = "Intermec PM43c_406_BACKUP";
            //btFormat.PrintSetup.PrinterName = "Microsoft Print to Pdf";

            // etiketa inprimatu
            btFormat.Print();

            // etiketaren dokumentua itxi
            btFormat.Close(SaveOptions.DoNotSaveChanges);

            // Inpresio motorea gelditu
            btEngine.Stop();
        }
    }
}
