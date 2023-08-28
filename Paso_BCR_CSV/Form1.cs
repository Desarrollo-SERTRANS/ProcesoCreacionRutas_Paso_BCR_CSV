using ComunesRedux;
using System;
using System.IO;
using System.Text;
using System.Windows.Forms;



namespace Paso_BCR_CSV
{
    public partial class frmNuevaRutas : Form
    {

        private BaseDatos basedatos;

        public frmNuevaRutas()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            basedatos = new BaseDatos(BaseDatos.MODO_PRODUCCION);
        }

        public static string DecimalAGradosMinSec(double Valordecimal)
        {
            double absValorDecimal = Math.Abs(Valordecimal);
            int grados = (int)absValorDecimal;
            double tempMinutosSegundos = (absValorDecimal - grados) * 60;
            int minutos = (int)tempMinutosSegundos;
            double segundos = (tempMinutosSegundos - minutos) * 60;

            string direccion = (Valordecimal >= 0) ? "N" : "S"; // "N" para latitud, "S" para longitud

            return $"{grados}° {minutos}' {segundos:F2}\" {direccion}";
        }


        public static double MercatorAGeograficoLongitud(double longitud, double radioterra)
        {
            double lon = (longitud) * (180 / radioterra) * (1 / Math.PI);

            return lon;
        }

        public static double MercatorAGeograficoLatitud(double latitud, double radioterra)
        {
            double lat = (360 / Math.PI) * (Math.Atan(Math.Exp(latitud / radioterra)) - (Math.PI / 4));

            return lat;
        }

        public bool GeneraInserts()
        {

            String line;
            String sql = "";
            String orden = "";     //pos 0
            String longitud = "";  //pos 3
            String latitud = "";   //pos 4
            String paisOri = "";   //pos 0 de decri (pos 5)
            String paisFin = "";   //pos 0 de decri (pos 5)
            String codPosOri = ""; //pos 1 de decri (pos 5)
            String codPosFin = ""; //pos 1 de decri (pos 5)
            String poblaOri = "";  //pos 2 de decri (pos 5)
            String poblaFin = "";  //pos 2 de decri (pos 5) 
            String carretera = ""; //pos 3 de decri (pos 5)
            String temp = "";
            int idRuta = -1;


            //            basedatos = new BaseDatos(BaseDatos.MODO_PRODUCCION);

            String directorio = txtDirectorio.Text + "\\";
            String[] archivos = Directory.GetFiles(directorio);


            foreach (String fichero in archivos)
            {
                StreamReader sr = new StreamReader(fichero, System.Text.Encoding.GetEncoding(28591));

                //Se leen dos porque la primera línea cabecera no se utiliza 
                line = sr.ReadLine();
                line = sr.ReadLine();

                String[] campos = line.Split(';');
                String descripciones = campos[5];
                String[] descripcion = descripciones.Split(',');

                while (line != null)
                {
                    campos = line.Split(';');
                    descripciones = campos[5];
                    descripcion = descripciones.Split(',');

                    if (campos[0] == "1")
                    {
                        descripciones = campos[5];
                        campos = line.Split(';');
                        descripcion = descripciones.Split(',');
                        paisOri = descripcion[0];
                        codPosOri = descripcion[1];
                        poblaOri = descripcion[2];
                    }
                    line = sr.ReadLine();
                }

                lblResul.Text = "Se está insertando el fichero: " + fichero;

                //ultima posición cod pos final
                paisFin = descripcion[0];
                codPosFin = descripcion[1];
                poblaFin = descripcion[2];

                //cerrar fichero
                sr.Close();

                sql = "SELECT DISTINCT ID FROM SERTRANS.dbo.TRANSICSRutasCabecera WHERE poscodorigen = '" + codPosOri + "' AND poscoddestino = '" + codPosFin + "' and activo = 1";

                object res = instSQL.SentenciaSQL_valor(sql, basedatos.sConexionSERTRANS);

                if (res == null)
                {
                    //no hay valor
                    poblaFin = poblaFin;
                }
                else
                {

                    int respuesta = (int)res;

                    //Se updatea la linea antigua para que solo haya una activa
                    sql = "UPDATE SERTRANS.dbo.TRANSICSRutasCabecera SET ACTIVO = 0 WHERE ID = " + respuesta;
                    instSQL.SentenciaSQL(sql, basedatos.sConexionSERTRANS);
                }

                // insert cabecera
                sql = "INSERT SERTRANS.dbo.TRANSICSRutasCabecera( PaisOrigen, NombreOrigen, PosCodOrigen, PaisDestino, NombreDestino, PosCodDestino, Activo, UsuAlta, FechaAlta, UsuMod, FechaMod, RutaTiempo, RutaComp) VALUES (ltrim(rtrim('" +
                              paisOri + "')),ltrim(rtrim('" +
                              poblaOri + "')),ltrim(rtrim('" +
                              codPosOri + "')),ltrim(rtrim('" +
                              paisFin + "')),ltrim(rtrim('" +
                              poblaFin + "')),ltrim(rtrim('" +
                              codPosFin + "')), CONVERT(bit, 'True'), N'sistema', GETDATE() , 'sistema', GETDATE() , " +
                              "null,null)";
                


                idRuta = instSQL.SentenciaSQL(sql, basedatos.sConexionSERTRANS);

                sr = new StreamReader(fichero, System.Text.Encoding.GetEncoding(28591));
                //Se leen dos porque la primera línea cabecera no se utiliza 
                line = sr.ReadLine();
                line = sr.ReadLine();
                //campos = line.Split(';');
                //descripciones = campos[5];
                //descripcion = descripciones.Split(',');




                while (line != null)
                {
                    campos = line.Split(';');
                    descripciones = campos[5];
                    descripcion = descripciones.Split(',');
                    paisOri = descripcion[0];
                    codPosOri = descripcion[1];
                    poblaOri = descripcion[2];
                    carretera = descripcion[3];
                    orden = campos[0];
                    temp = campos[4];
                    latitud = temp.Replace(",",".");
                    temp = campos[3];
                    longitud = temp.Replace(",", ".");


                    //aqui insert
                    sql = "INSERT SERTRANS.dbo.TRANSICSRutasPuntos( idRuta, Descripcion, Orden, Lat, Long, Radio) VALUES (" +
                                    idRuta + ",'" +
                                    paisOri + " " + codPosOri + " " + poblaOri + " " + carretera + "'," +
                                    orden + "," +
                                    latitud + "," +
                                    longitud + ", '" +
                                    "0.00')";

                    instSQL.SentenciaSQL(sql, basedatos.sConexionSERTRANS);

                    line = sr.ReadLine();
                }
                //cerrar fichero
                sr.Close();
            }
            return true;
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            using (var fd = new FolderBrowserDialog())
            {
                if (fd.ShowDialog() == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(fd.SelectedPath))
                {
                    txtDirectorio.Text = fd.SelectedPath;
                    lblResul.Text = "El directorio actual es " + txtDirectorio.Text;
                }
            }
        }

        private void btnInsertar_Click(object sender, EventArgs e)
        {

            bool fin = false;
            try
            {

                bool SiNO = false;
                lblResul.Text = "El directorio actual es " + txtDirectorio.Text;

                var msrs = MessageBox.Show("Se vana a generar los inserts a partir de los archivos 'CVS' del directorio " + txtDirectorio.Text, "Traspaso BCR CSV",
                                        MessageBoxButtons.YesNoCancel,
                                        MessageBoxIcon.Information);

                if (msrs == DialogResult.No)
                {
                    using (var fd = new FolderBrowserDialog())
                    {
                        if (fd.ShowDialog() == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(fd.SelectedPath))
                        {
                            txtDirectorio.Text = fd.SelectedPath;
                            lblResul.Text = "El directorio actual es " + txtDirectorio.Text;
                        }
                    }
                }
                else if (msrs == DialogResult.Yes)
                {
                    fin = GeneraInserts();
                }
                else//cancel
                { }

                //fin = true;
            }

            catch (Exception d)
            {

                MessageBox.Show("Exception: " + d.Message, "Traspaso BCR CSV",
                                 MessageBoxButtons.OK,
                                 MessageBoxIcon.Error);

            }
            finally
            {
                if (fin == true)
                {

                    string directorio = txtDirectorio.Text + "\\cvs\\";

                    txtDirectorio.Text = directorio;

                    lblResul.Text = "El directorio actual es " + directorio;

                    MessageBox.Show("Proceso acabado", "Traspaso BCR CSV",
                                     MessageBoxButtons.OK,
                                     MessageBoxIcon.Information);
                    fin = false;
                }
            }


        }

        private void brnGenCSV_Click(object sender, EventArgs e)
        {
            String line;
            String lineOrigen;
            bool fin = false;
            try
            {
                int i = 0;
                int pos = 0;
                int pos2 = 0;
                String valfin = "";
                int totLine = 0;
                double Rterra = 6371000;
                int temp = 0;
                String lintemp = "";
                double tempFin = 0;
                String Warchivo = "";
                String directorio = txtDirectorio.Text + "\\";
                String Wdirectorio = "";
                if (directorio.Length > 0 && directorio != "\\" && Directory.Exists(directorio))
                {
                    String[] archivos = Directory.GetFiles(directorio);
                    foreach (String fichero in archivos)
                    {
                        lblResul.Text = "Se está leyendo el archivo: " + fichero;

                        //leer fichero 

                        //path del fichero pasarlo por parametro o buscar en pantalla.
                        StreamReader sr = new StreamReader(fichero, System.Text.Encoding.GetEncoding(28591));// ASCII) ;//// System.Text.Encoding.  ASCII); 20284
                        //lee linea
                        lineOrigen = sr.ReadLine();
                        line = lineOrigen.Replace("'","''");
                        while (lineOrigen != null && lineOrigen.Length != 0)
                        {
                            valfin = line.Substring(0, 7);
                            while (valfin != "STATION")
                            {
                                //lee la proxima linea  
                                lineOrigen = sr.ReadLine();
                                line = lineOrigen.Replace("'", "''");
                                valfin = line.Substring(0, 7);
                            }
                            if (valfin == "STATION")
                            {
                                totLine = 0;
                                while (valfin == "STATION" && line != null)
                                {
                                    totLine++;
                                    lineOrigen = sr.ReadLine();
                                    line = lineOrigen.Replace("'", "''");
                                    if (line.Length != 0)
                                    {
                                        valfin = line.Substring(0, 7);
                                    }
                                    else
                                    {
                                        valfin = "";
                                    }
                                }
                            }
                        }

                        String[,] coordArray = new String[totLine, 9];

                        i = 0;

                        while (lineOrigen != null)
                        {
                            if (line == "[COORDINATES]")
                            {
                                lineOrigen = sr.ReadLine();
                                line = lineOrigen.Replace("'", "''");
                                while (line != null && line.Length != 0)
                                {
                                    // orden
                                    pos = 6;//"STATION"
                                    pos2 = line.IndexOf("=", 0) - 1;
                                    valfin = line.Substring(pos + 1, pos2 - pos);
                                    coordArray[i, 0] = valfin;

                                    //longitud
                                    pos = line.IndexOf("=", 0);
                                    pos2 = line.IndexOf(",", 0) - 1;
                                    temp = Int32.Parse(line.Substring(pos + 1, pos2 - pos));
                                    //longitud mercator decimal
                                    coordArray[i, 1] = temp.ToString();
                                    tempFin = MercatorAGeograficoLongitud(temp, Rterra);
                                    //longitud geográfica decimal
                                    coordArray[i, 3] = Convert.ToString(tempFin);
                                    //pase a grados minutos segundos no se pone pero ya lo tenemos
                                    valfin = DecimalAGradosMinSec(tempFin);

                                    //latitud
                                    pos = line.IndexOf(",", 0) + 1;
                                    pos2 = line.Length;
                                    temp = Int32.Parse(line.Substring(pos, pos2 - pos));
                                    //latitud mercator decimal
                                    coordArray[i, 2] = temp.ToString();
                                    tempFin = MercatorAGeograficoLatitud(temp, Rterra);
                                    coordArray[i, 4] = Convert.ToString(tempFin);
                                    //pase a grados minutos segundos no se pone pero ya lo tenemos
                                    valfin = DecimalAGradosMinSec(tempFin);

                                    i++;
                                    lineOrigen = sr.ReadLine();
                                    line = lineOrigen.Replace("'", "''");
                                }
                            }

                            if (line == "[DESCRIPTION]")
                            {
                                lineOrigen = sr.ReadLine();
                                line = lineOrigen.Replace("'", "''");
                                i = 0;
                                while (lineOrigen != null && lineOrigen.Length != 0)
                                {
                                    //PAIS
                                    pos = line.IndexOf("=", 0) + 1;
                                    pos2 = line.IndexOf(" ", 0);
                                    valfin = line.Substring(pos, pos2 - pos);
                                    coordArray[i, 5] = valfin;

                                    //COD POSTAL
                                    pos = pos2 + 1;
                                    pos2 = line.IndexOf(",", 0);
                                    valfin = line.Substring(pos, pos2 - pos);
                                    coordArray[i, 6] = valfin;

                                    //POBLACION
                                    pos = pos2 + 1;
                                    lintemp = line.Substring(pos, (line.Length - pos));
                                    pos2 = lintemp.IndexOf(",", 0);
                                    valfin = lintemp.Substring(0, pos2);
                                    coordArray[i, 7] = valfin;

                                    //CARRETERA
                                    pos = lintemp.IndexOf(",", 0) + 1;
                                    lintemp = lintemp.Substring(pos, lintemp.Length - pos);
                                    pos = 0;
                                    pos2 = lintemp.IndexOf(",", 0);
                                    valfin = lintemp.Substring(pos, pos2 - pos);
                                    coordArray[i, 8] = valfin;

                                    lineOrigen = sr.ReadLine();
                                    line = lineOrigen.Replace("'", "''");
                                    i++;
                                }
                            }

                            lineOrigen = sr.ReadLine();
                            if (lineOrigen != null && lineOrigen.Length != 0)
                            {
                                line = lineOrigen.Replace("'", "''"); 
                            }
                                
                        }

                        //cerrar fichero
                        sr.Close();

                        Wdirectorio = directorio + "cvs\\";

                        if (!Directory.Exists(Wdirectorio))
                        {
                            Directory.CreateDirectory(Wdirectorio);
                        }

                        Warchivo = fichero.Replace(txtDirectorio.Text + "\\", Wdirectorio);

                        Warchivo = Warchivo.Replace(".bcr", ".csv");

                        //StreamWriter sw = new StreamWriter(Warchivo);
                        StreamWriter sw = new StreamWriter(Warchivo, false, Encoding.UTF8);

                        lblResul.Text = "Se está generando el archivo: " + Warchivo;

                        //Se escribe cabecera
                        //sw.WriteLine("Orden;Mercator X;MercatorY;Longitud Decimal X;Latitud Decimal Y;Longitud Gra,Min,Sec;Latitud Gra,Min,Sec;Descripcion(Pais, CodPos, Población, Carretera");
                        sw.WriteLine("Orden;Mercator X;Mercator Y;Longitud Decimal X;Latitud Decimal Y;Descripcion(Pais, CodPos, Poblacion, Carretera");


                        for (int a = 0; a < coordArray.GetLength(0); a++)
                        {
                            sw.WriteLine(coordArray[a, 0] + ";" + coordArray[a, 1] + ";" + coordArray[a, 2] + ";" + coordArray[a, 3] + ";" + coordArray[a, 4] + ";" + coordArray[a, 5] + ", " + coordArray[a, 6] + ", " + coordArray[a, 7] + ", " + coordArray[a, 8]);
                        }


                        //Cierre de fichero
                        sw.Close();

                    }

                    fin = true;
                }
                else
                {
                    MessageBox.Show("El directorio '" + txtDirectorio.Text + "' no existe", "Traspaso BCR CSV",
                                     MessageBoxButtons.OK,
                                     MessageBoxIcon.Error);
                }

            }



            catch (Exception d)
            {

                MessageBox.Show("Exception: " + d.Message, "Traspaso BCR CSV",
                                 MessageBoxButtons.OK,
                                 MessageBoxIcon.Error);

            }
            finally
            {
                if (fin == true)
                {

                    string directorio = txtDirectorio.Text + "\\cvs\\";

                    txtDirectorio.Text = directorio;

                    lblResul.Text = "El directorio actual es " + directorio;

                    MessageBox.Show("Proceso acabado", "Traspaso BCR CSV",
                                     MessageBoxButtons.OK,
                                     MessageBoxIcon.Information);
                    fin = false;
                }
            }

        }
    }
    
}
