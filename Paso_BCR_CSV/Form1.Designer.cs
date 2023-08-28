namespace Paso_BCR_CSV
{
    partial class frmNuevaRutas
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmNuevaRutas));
            this.lblResul = new System.Windows.Forms.Label();
            this.txtDirectorio = new System.Windows.Forms.TextBox();
            this.btnBuscar = new System.Windows.Forms.Button();
            this.brnGenCSV = new System.Windows.Forms.Button();
            this.btnInsertar = new System.Windows.Forms.Button();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblResul
            // 
            this.lblResul.AutoSize = true;
            this.lblResul.Location = new System.Drawing.Point(28, 155);
            this.lblResul.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblResul.Name = "lblResul";
            this.lblResul.Size = new System.Drawing.Size(237, 16);
            this.lblResul.TabIndex = 0;
            this.lblResul.Text = "Información de directorios y archivos    ";
            // 
            // txtDirectorio
            // 
            this.txtDirectorio.Location = new System.Drawing.Point(16, 58);
            this.txtDirectorio.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtDirectorio.Name = "txtDirectorio";
            this.txtDirectorio.Size = new System.Drawing.Size(1076, 22);
            this.txtDirectorio.TabIndex = 1;
            // 
            // btnBuscar
            // 
            this.btnBuscar.Location = new System.Drawing.Point(1100, 58);
            this.btnBuscar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(191, 23);
            this.btnBuscar.TabIndex = 2;
            this.btnBuscar.Text = "Burcar directorio";
            this.btnBuscar.UseVisualStyleBackColor = true;
            this.btnBuscar.Click += new System.EventHandler(this.btnBuscar_Click);
            // 
            // brnGenCSV
            // 
            this.brnGenCSV.Location = new System.Drawing.Point(1100, 89);
            this.brnGenCSV.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.brnGenCSV.Name = "brnGenCSV";
            this.brnGenCSV.Size = new System.Drawing.Size(191, 27);
            this.brnGenCSV.TabIndex = 3;
            this.brnGenCSV.Text = "Crea Archivos CSV";
            this.brnGenCSV.UseVisualStyleBackColor = true;
            this.brnGenCSV.Click += new System.EventHandler(this.brnGenCSV_Click);
            // 
            // btnInsertar
            // 
            this.btnInsertar.Location = new System.Drawing.Point(1100, 123);
            this.btnInsertar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnInsertar.Name = "btnInsertar";
            this.btnInsertar.Size = new System.Drawing.Size(191, 27);
            this.btnInsertar.TabIndex = 4;
            this.btnInsertar.Text = "Crear Nuevas Rutas";
            this.btnInsertar.UseVisualStyleBackColor = true;
            this.btnInsertar.Click += new System.EventHandler(this.btnInsertar_Click);
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.lblTitulo.ForeColor = System.Drawing.Color.Blue;
            this.lblTitulo.Location = new System.Drawing.Point(13, 9);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(1591, 20);
            this.lblTitulo.TabIndex = 5;
            this.lblTitulo.Text = resources.GetString("lblTitulo.Text");
            // 
            // frmNuevaRutas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1299, 202);
            this.Controls.Add(this.lblTitulo);
            this.Controls.Add(this.btnInsertar);
            this.Controls.Add(this.brnGenCSV);
            this.Controls.Add(this.btnBuscar);
            this.Controls.Add(this.txtDirectorio);
            this.Controls.Add(this.lblResul);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "frmNuevaRutas";
            this.Text = "Creación nuevas rutas";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblResul;
        private System.Windows.Forms.TextBox txtDirectorio;
        private System.Windows.Forms.Button btnBuscar;
        private System.Windows.Forms.Button brnGenCSV;
        private System.Windows.Forms.Button btnInsertar;
        private System.Windows.Forms.Label lblTitulo;
    }
}

