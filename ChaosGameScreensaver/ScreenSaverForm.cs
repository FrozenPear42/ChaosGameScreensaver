using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows.Forms;

namespace ChaosGameScreensaver
{


    public partial class ScreenSaverForm : Form
    {
        
        /* Import DLLs for prevew mode */
        [DllImport("user32.dll")]
        static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        static extern bool GetClientRect(IntPtr hWnd, out Rectangle lpRect);

        /* vars and objects definitions */

        private bool previewMode = false;

        private Rectangle bounds;

        private Timer updateTimer;
    
        private Graphics g;
        private Random rand;

        private Point lastMouseLocation;

        private List<RenderData> data;
        private int n; 
        private double factor;
           
        private Vector currentPoint;
        private Vector[] verticles;


        /* Constructors */

        public ScreenSaverForm(Rectangle b)
        {
            InitializeComponent();
            this.bounds = b;  
        }

       
        public ScreenSaverForm(IntPtr PreviewWndHandle)
        {
            InitializeComponent();

            // Set the preview window as the parent of this window
            SetParent(this.Handle, PreviewWndHandle);

            // Make this a child window so it will close when the parent dialog closes
            // GWL_STYLE = -16, WS_CHILD = 0x40000000
            SetWindowLong(this.Handle, -16, new IntPtr(GetWindowLong(this.Handle, -16) | 0x40000000));

            // Place our window inside the parent
            Rectangle ParentRect;
            GetClientRect(PreviewWndHandle, out ParentRect);
            Size = ParentRect.Size;
            Location = new Point(0, 0);

            previewMode = true;
        }



        /* Main events */

        private void ScreenSaverForm_Load(object sender, EventArgs e)
        {

            if (this.bounds != null && !previewMode)
                this.Bounds = bounds;

            Cursor.Hide();
            TopMost = true;

            this.g = this.CreateGraphics();
            this.rand = new Random();
            
            this.data = RegistryStorage.GetData();

            int idx = rand.Next(data.Count - 1);
            
            this.n = data[idx].N;
            this.factor = data[idx].F;
            
            this.verticles = new Vector[n];

            Vector orgin = new Vector(Width / 2, Height / 2);
            double radius = Math.Min(Width, Height) / 2 - 20;
            
            currentPoint = new Vector(rand.Next(Width), rand.Next(Height));
            
            for (int i = 0; i < n; i++)
            {
                verticles[i] = new Vector().SetFromRotation(radius, i*(2*Math.PI/n), orgin);            
                Console.WriteLine("v " + i + ": " + verticles[i].ToString());    
            }
            
            updateTimer.Tick += new System.EventHandler(this.timer1_Tick);
            updateTimer.Interval = 10;
            updateTimer.Start();
        }


        private void ScreenSaverForm_Paint(object sender, PaintEventArgs e)
        {
            foreach (Vector v in verticles)
            {
                drawPoint(Color.Yellow, v, 2);
            }

        }


        /* app functions */
        private void nextVerticle()
        {
            currentPoint += (verticles[rand.Next(this.n)] - currentPoint) * (1 - this.factor);
            drawPoint(Color.FromArgb(rand.Next(255), rand.Next(255), rand.Next(255)), currentPoint);
        }


        /* User interaction events */
        private void ScreenSaverForm_MouseClick(object sender, MouseEventArgs e)
        {
            if(!previewMode)
                Application.Exit();
        }

        private void ScreenSaverForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (!previewMode)
                if (!lastMouseLocation.IsEmpty)
                    if ((Math.Abs(lastMouseLocation.X - e.X) > 5) ||
                        (Math.Abs(lastMouseLocation.Y - e.Y) > 5))    
                            Application.Exit();

            lastMouseLocation = e.Location;
        }

        private void ScreenSaverForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!previewMode)
                Application.Exit();
        }

        /* Timer Event */
        private void timer1_Tick(object sender, EventArgs e)
        {
            nextVerticle();
        }

        /* Graphics functions */
        private void drawPoint(Color color, double x, double y, int size)
        {
            g.DrawRectangle(new Pen(color), (float)x, (float)y, size, size);
        }

        private void drawPoint(Color color, double x, double y)
        {
            drawPoint(color, x, y, 1);
        }

        private void drawPoint(Color color, Vector v)
        {
            drawPoint(color, v.X, v.Y);        
        }

        private void drawPoint(Color color, Vector v, int size)
        {
            drawPoint(color, v.X, v.Y, size);
        }
        
    }
}
