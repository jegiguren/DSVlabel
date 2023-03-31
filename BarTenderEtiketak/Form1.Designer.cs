namespace BarTenderEtiketak
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.etiketakBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.etiketa_DBDataSet = new BarTenderEtiketak.Etiketa_DBDataSet();
            this.etiketakTableAdapter = new BarTenderEtiketak.Etiketa_DBDataSetTableAdapters.etiketakTableAdapter();
            this.Xml_print = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.btn_Gelditu = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.etiketakBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.etiketa_DBDataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // etiketakBindingSource
            // 
            this.etiketakBindingSource.DataMember = "etiketak";
            this.etiketakBindingSource.DataSource = this.etiketa_DBDataSet;
            // 
            // etiketa_DBDataSet
            // 
            this.etiketa_DBDataSet.DataSetName = "Etiketa_DBDataSet";
            this.etiketa_DBDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // etiketakTableAdapter
            // 
            this.etiketakTableAdapter.ClearBeforeFill = true;
            // 
            // Xml_print
            // 
            this.Xml_print.Location = new System.Drawing.Point(52, 33);
            this.Xml_print.Name = "Xml_print";
            this.Xml_print.Size = new System.Drawing.Size(230, 54);
            this.Xml_print.TabIndex = 9;
            this.Xml_print.Text = "MARTXAN JARRI";
            this.Xml_print.UseVisualStyleBackColor = true;
            this.Xml_print.Click += new System.EventHandler(this.Xml_print_Click);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(52, 187);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(230, 407);
            this.listBox1.TabIndex = 11;
            // 
            // btn_Gelditu
            // 
            this.btn_Gelditu.Location = new System.Drawing.Point(309, 33);
            this.btn_Gelditu.Name = "btn_Gelditu";
            this.btn_Gelditu.Size = new System.Drawing.Size(99, 54);
            this.btn_Gelditu.TabIndex = 12;
            this.btn_Gelditu.Text = "GELDITU";
            this.btn_Gelditu.UseVisualStyleBackColor = true;
            this.btn_Gelditu.Click += new System.EventHandler(this.btn_Gelditu_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(690, 644);
            this.Controls.Add(this.btn_Gelditu);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.Xml_print);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.etiketakBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.etiketa_DBDataSet)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Etiketa_DBDataSet etiketa_DBDataSet;
        private System.Windows.Forms.BindingSource etiketakBindingSource;
        private Etiketa_DBDataSetTableAdapters.etiketakTableAdapter etiketakTableAdapter;
        private System.Windows.Forms.Button Xml_print;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button btn_Gelditu;
    }
}

