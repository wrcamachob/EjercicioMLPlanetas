using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.Models;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        #region Variables
        Thread th; //Hilo que se encarga de manejar los planetas.
        Graphics g;
        Graphics fg;
        Bitmap btm;

        int CantDias = 3650; //Cantidad de dias por los 10 años.
        int periodoLluvia = 0;
        double valorMaximo = 0;
        string clima = string.Empty;
        int diasMaximo = 0;        
        #endregion

        /// <summary>
        /// Constructor de la clase.
        /// </summary>
        public Form1()
        {
            InitializeComponent();
        }
        
        /// <summary>
        /// Cargar el formulario con una imagen de fondo.
        /// </summary>
        /// <param name="sender">Objeto Sender.</param>        
        private void Form1_Load(object sender, EventArgs e)
        {
            btm = new Bitmap(500, 500);            
            g = Graphics.FromImage(btm);            
            fg = CreateGraphics();

            th = new Thread(Draw);
            th.IsBackground = true;
            th.Start();
        }      

        /// <summary>
        /// Se encarga de dibujar o pintar los planetas.
        /// </summary>
        public void Draw()
        {
            float angleFerengi = 1f;
            float angleBetas = 3f;
            float angleVulcan = -5f;
            float angleSol = 0f;

            int dias = 0, contLinea = 0, contLinea1 = 0, contTrianLluvia = 0;

            Point pointFerengi = new Point(250, 250);
            Point pointBetas = new Point(250, 250);
            Point pointVulcan = new Point(250, 250);
            Point pointSol = new Point(250, 250);

            //Para la prueba realizada se le elimino un 0 para ver los planetas y la simulacion.
            float radiusFerengi = 500;
            float radiusBetas = 2000;
            float radiusVulcan = 1000;
            float radiusSol = 0;

            PointF PuntoSol = PointF.Empty;
            PointF PuntoFerengis = PointF.Empty;
            PointF PuntoBetasoide = PointF.Empty;
            PointF PuntoVulcano = PointF.Empty;
            PointF img = new PointF(5, 5);
            fg.Clear(Color.Black);
            
            while (dias <= CantDias)
            {
                clima = string.Empty;                
                g.Clear(Color.Black);

                //Planeta1 Ferengis
                PuntoFerengis = CirclePoint(radiusFerengi, angleFerengi, pointFerengi);
                g.FillEllipse(Brushes.Red, PuntoFerengis.X, PuntoFerengis.Y, 10, 10);

                if (angleFerengi < 360)
                    angleFerengi += 1f;
                else
                    angleFerengi = 0;

                //Planeta2 Betasoide
                PuntoBetasoide = CirclePoint(radiusBetas, angleBetas, pointBetas);                
                g.FillEllipse(Brushes.Blue, PuntoBetasoide.X, PuntoBetasoide.Y, 10, 10);

                if (angleBetas < 360)
                    angleBetas += 3f;
                else
                    angleBetas = 0;

                //Planeta3 Vulcanos
                PuntoVulcano = CirclePoint(radiusVulcan, angleVulcan, pointVulcan);                
                g.FillEllipse(Brushes.White, PuntoVulcano.X, PuntoVulcano.Y, 10, 10);

                if (angleVulcan < 360)
                    angleVulcan -= 5f;
                else
                    angleVulcan = 0;
                
                //Sol
                PuntoSol = CirclePoint(radiusSol, angleSol, pointSol);
                g.FillEllipse(Brushes.Yellow, PuntoSol.X, PuntoSol.Y, 10, 10);
                fg.DrawImage(btm, img);

                Thread.Sleep(1);
                
                double valLinea1 = Linea(PuntoSol.X, PuntoSol.Y, PuntoFerengis.X, PuntoFerengis.Y);
                double valLinea2 = Linea(PuntoFerengis.X, PuntoFerengis.Y, PuntoBetasoide.X, PuntoBetasoide.Y);
                double valLinea3 = Linea(PuntoBetasoide.X, PuntoBetasoide.Y, PuntoVulcano.X, PuntoVulcano.Y);

                bool esTriangulo = Triangulo(PuntoFerengis.X, PuntoFerengis.Y, PuntoBetasoide.X, PuntoBetasoide.Y, PuntoVulcano.X, PuntoVulcano.Y, 
                    PuntoSol.X, PuntoSol.Y, dias);

                if (valLinea1 == valLinea2 && valLinea2 == valLinea3)
                {
                    clima = TiposClima.SEQUIA.ToString();                    
                    contLinea++;                                        
                }
                else if (valLinea2 == valLinea3)
                {
                    clima = TiposClima.OPTIMA.ToString();                    
                    contLinea1++;
                }
                else if (esTriangulo)
                {
                    if (periodoLluvia == 1)
                    {
                        clima = TiposClima.PERIODOLLUVIA.ToString();                        
                        contTrianLluvia++;                        
                    }
                    else
                    {
                        clima = TiposClima.SINDATOS.ToString();                        
                    }                    
                }

                //Se Declara y se llena el objeto periodo.
                Periodo periodo = new Periodo()
                {
                    Dia = dias,
                    Clima = clima
                };

                //Se realiza el llamado de res api.
                _ = CreatePeriodoAsync(periodo);

                Console.WriteLine("Dia : " + dias + clima);

                ++dias;                
            }

            //Se verifica el periodo maximo de lluvia.
            if (diasMaximo > 0)
            {
                Periodo periodoMaximo = new Periodo()
                {
                    Dia = diasMaximo,
                    Clima = TiposClima.MAXIMODIALLUVIA.ToString()
                };
                //llamado al servicio para actualizar.
                _ = UpdatePeriodoAsync(periodoMaximo);
            }

            
        }

        /// <summary>
        /// Se encarga de dibujar los puntos o planetas con la formula para calcular x, y.
        /// </summary>
        /// <param name="radius">Radio</param>
        /// <param name="angleInDegrees">Angulo(Corresponde a velocidad angular)</param>
        /// <param name="origin">Punto Origen.</param>
        /// <returns></returns>
        public PointF CirclePoint(float radius, float angleInDegrees, PointF origin)
        {
            float x = (float)((radius * Math.Cos(angleInDegrees * Math.PI / 180F)) + origin.X);
            float y = (float)((radius * Math.Sin(angleInDegrees * Math.PI / 180F)) + origin.Y);

            return new PointF(x, y);
        }

        /// <summary>
        /// Metodo que se encarga de dibujar triangulo entre los planetas o puntos.
        /// </summary>
        /// <param name="x1">valor x planeta1</param>
        /// <param name="y1">valor y planeta1</param>
        /// <param name="x2">valor x planeta2</param>
        /// <param name="y2">valor y planeta2</param>
        /// <param name="x3">valor x planeta3</param>
        /// <param name="y3">valor y planeta3</param>
        /// <param name="x4">valor x sol</param>
        /// <param name="y4">valor y sol</param>
        /// <param name="dias">Dias en el que va.</param>
        /// <returns></returns>
        public bool Triangulo(float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4, int dias)
        {
            bool esTriangulo = false;
            periodoLluvia = 0;
            //Para validar el permitero maximo.
            double a = Math.Round(Math.Abs(x1 * (y2 - y3) + x2 * (y3 - y1) + x3 * (y1 - y2)), 1);

            //Identifica si se forma un triangulo entre los planetas.
            float x = (x1 + x2 + x3) / 3;
            float y = (y1 + y2 + y3) / 3;
            //Se deja un promedio para que exista un rango entre los planetas y el sol.
            int b = Convert.ToInt32(x4) - 200;
            int c = Convert.ToInt32(y4) - 500;

            if ((Enumerable.Range(Convert.ToInt32(x4), 300).Contains(Convert.ToInt32(x)) || Enumerable.Range(Convert.ToInt32(b), 200).Contains(Convert.ToInt32(x)))
                && (Enumerable.Range(Convert.ToInt32(y4), 500).Contains(Convert.ToInt32(y)) || Enumerable.Range(Convert.ToInt32(c), 500).Contains(Convert.ToInt32(y))) 
                && a >= 250000)
            {
                if (a > valorMaximo)
                {            
                    valorMaximo = a;
                    diasMaximo = dias;
                }                
                periodoLluvia = 1;
            }            

            if (a != 0)
                esTriangulo = true;

            return esTriangulo;
        }

        /// <summary>
        /// Identificar si se fi¿orma una linea entre los planetas.
        /// </summary>
        /// <param name="x1">Valor x planeta1</param>
        /// <param name="y1">Valor y planeta1</param>
        /// <param name="x2">Valor x planeta2</param>
        /// <param name="y2">Valor y planeta2</param>
        /// <returns></returns>
        public double Linea(float x1, float y1, float x2, float y2)
        {
            double m;            
            m = (y2 - y1) / (x2 - x1);
            m = Math.Round(Math.Abs(Math.Truncate(m * 10) / 10), 1);
            return m;
        }       

        /// <summary>
        /// Metodo que se encarga de realizar el llamado del metodo de la webApi para crear los dias y sus respectivos climas.
        /// </summary>
        /// <param name="periodo">Objeto Periodo.</param>
        /// <returns>Codigo del estado.</returns>
        private async Task<string> CreatePeriodoAsync(Periodo periodo)
        {
            string url = ConfigurationManager.AppSettings["Url"];
            HttpResponseMessage response;            
            using (var handler = new HttpClientHandler())
            using (var client = new HttpClient(handler))
            {
                //Se escribe la url con el puerto respectivo.
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                string data = JsonConvert.SerializeObject(periodo);

                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

                response = await client.PostAsync("api/SistemaSolar", content);
                response.EnsureSuccessStatusCode();
            }

            return response.StatusCode.ToString();
        }

        /// <summary>
        /// Metodo que se encarga de actualizar el periodo maximo de lluvia.
        /// </summary>
        /// <param name="periodo">Objeto Periodo.</param>
        /// <returns>Codigo del estado.</returns>
        private async Task<string> UpdatePeriodoAsync(Periodo periodo)
        {
            string url = ConfigurationManager.AppSettings["Url"];
            HttpResponseMessage response;            
            using (var handler = new HttpClientHandler())
            using (var client = new HttpClient(handler))
            {                
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                string data = JsonConvert.SerializeObject(periodo);

                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

                response = await client.PutAsync("api/SistemaSolar", content);
                response.EnsureSuccessStatusCode();
            }

            return response.StatusCode.ToString();
        }

    }
}
