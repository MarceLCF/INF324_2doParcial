/*
 * Created by SharpDevelop.
 * User: Marce
 * Date: 13/11/2023
 * Time: 14:41
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace xd{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form{
		int Rm, Gm, Bm;
		
		int L=10;
		int Rt, Gt, Bt;
					
		public MainForm(){
			InitializeComponent();
            // Crear y abrir la conexión            
            //conSQL("delete from color");
		}
		
		MySqlDataReader conSQL(string cons){
			string connectionString = "Server=localhost;Database=mibd_cusi;User ID=root;Password=;";
			MySqlConnection conexion = new MySqlConnection(connectionString);
            conexion.Open();
            MySqlCommand comando = new MySqlCommand(cons, conexion);
            MySqlDataReader lector = comando.ExecuteReader();
            return lector;
		}
		
		void Button1Click(object sender, EventArgs e){
			openFileDialog1.Filter = "Archivos JPG|*.JPG|Archivos BMP|*.bmp";
			openFileDialog1.ShowDialog();
			if(openFileDialog1.FileName != string.Empty){
				Bitmap bmp = new Bitmap(openFileDialog1.FileName);
				pictureBox1.Image = bmp;
				conSQL("delete from color");
			}
		}
		
		void Button2Click(object sender, EventArgs e){
			MySqlDataReader res = conSQL("select * from color");
			Bitmap bmp = new Bitmap(pictureBox1.Image);
			Bitmap bmp2 = new Bitmap(bmp.Width, bmp.Height);
			Color c = new Color();
            while (res.Read()){
				int rr = (int) res["r"];
                int gg = (int) res["g"];
                int bb = (int) res["b"];
               	//textBox1.Text = textBox1.Text +","+(rr.ToString());
				for (int i = 0; i < bmp.Width - L; i = i + L){
	                for (int j = 0; j < bmp.Height - L; j = j + L){
	                    Rt = 0; Gt = 0; Bt = 0;
	                    for (int o = i; o < i + L; o++)
	                        for (int p = j; p < j + L; p++){
	                            c = bmp.GetPixel(o, p);
	                            Rt += c.R;
	                            Gt += c.G;
	                            Bt += c.B;
	                        }
	                    Rt = Rt / (L * L); Gt = Gt / (L * L); Bt = Bt / (L * L);
	                    if (((rr - L < Rt) && (Rt < rr + L))
	                        && ((gg - L < Gt) && (Gt < gg + L))
	                        && ((bb - L < Bt) && (Bt < bb + L))){
	                        for (int o = i; o < i + L; o++)
	                            for (int p = j; p < j + L; p++){
	                                bmp2.SetPixel(o, p, Color.FromArgb(Math.Abs(rr-255), Math.Abs(gg-255), Math.Abs(bb-255)));
	                            }
	                    }else{
	                        for (int o = i; o < i + L; o++)
	                            for (int p = j; p < j + L; p++){
	                                c = bmp.GetPixel(o, p);
	                                bmp2.SetPixel(o, p, Color.FromArgb(c.R, c.G, c.B));
	                            }
	                    }
	                }
	            }
	            bmp = bmp2;
			}
			pictureBox1.Image = bmp2;
		}
		
		void PictureBox1MouseClick(object sender, MouseEventArgs e){
			Bitmap bmp = new Bitmap(pictureBox1.Image);
			Color c = new Color();
			c = bmp.GetPixel(e.X,e.Y);
			Rm = c.R;
			Gm = c.G;
			Bm = c.B;
			textBox1.Text = c.R.ToString();
			textBox2.Text = c.G.ToString();
			textBox3.Text = c.B.ToString();
			
			string a = "insert into color values("+Rm.ToString()+","+Gm.ToString()+","+Bm.ToString()+")";
			conSQL(a);
		}
	}
}
