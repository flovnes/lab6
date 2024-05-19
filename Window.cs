using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace lab6
{
    public partial class Window : Form
    {
        readonly Color textColor = Color.FromArgb(230, 230, 233);
        readonly Color backgroundColor = Color.FromArgb(22, 24, 29);
        readonly Color primaryColor = Color.FromArgb(178, 177, 200);
        readonly Color secondaryColor = Color.FromArgb(26, 27, 38);
        readonly Color secondaryLightColor = Color.FromArgb(76, 75, 111);
        readonly Color secondaryLighterColor = Color.FromArgb(36, 35, 47);
        readonly Color accentColor = Color.FromArgb(184, 27, 27);

        readonly Font font = new Font("Mintsoda - Lime Green 16x16", 16F, FontStyle.Regular, GraphicsUnit.Pixel, 204);

        Timer timerTriangle;
        Timer timerDrums;

        readonly int canvas_x = 100;
        readonly int canvas_y = 30;
        float triangleAngle = 0f;
        float drumsAngle = 0f;
        float angleStep = 1f;
        bool shapes = false, laptop = false, triangle = false, drums = false;

        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;

        public Window()
        {
            InitializeComponent();
            this.topMenu.Renderer = new CustomToolStripProfessionalRenderer_Top();
            this.sideMenu.Renderer = new CustomToolStripProfessionalRenderer_Left();
            timerTriangle = new Timer { Interval = 30 };
            timerTriangle.Tick += new EventHandler(this.OnTriangleTimerTick);

            timerDrums = new Timer { Interval = 1 };
            timerDrums.Tick += new EventHandler(this.OnDrumsTimerTick);  
        }

        void OnTriangleTimerTick(object sender, EventArgs e)
        {
            triangleAngle += 5f;
            if (triangleAngle >= 360f) triangleAngle = 0f;
            this.Invalidate();
        }

        private void OnDrumsTimerTick(object sender, EventArgs e)
        {
            drumsAngle += angleStep;

            if (drumsAngle > 7 || drumsAngle < -7)
                angleStep = -angleStep;

            this.Invalidate();
        }

        public void InitializeComponent()
        {
            ComponentResourceManager resources = new ComponentResourceManager(typeof(Window));

            InitializeTopMenu(resources);
            InitializeSideMenu();

            ClientSize = new Size(768, 512);
            BackColor = backgroundColor;
            FormBorderStyle = FormBorderStyle.None;
            Icon = (Icon) resources.GetObject("windowIcon");

            Controls.Add(this.sideMenu);
            Controls.Add(this.topMenu);

            ResumeLayout(false);
        }

        private void InitializeTopMenu(ComponentResourceManager resources)
        {
            topMenu = new ToolStrip
            {
                AutoSize = false,
                BackColor = secondaryLighterColor,
                GripStyle = ToolStripGripStyle.Hidden,
                Location = new Point(0, 0),
                Padding = new Padding(0),
                Size = new Size(768, 30),
                TabIndex = 2
            };
            topMenu.Items.AddRange(new ToolStripItem[]
            {
                InitializeCloseButton(),
                InitializeWindowLabel(resources)
            });

            topMenu.MouseDown += new MouseEventHandler(this.TopMenu_MouseDown);
            topMenu.MouseMove += new MouseEventHandler(this.TopMenu_MouseMove);
            topMenu.MouseUp += new MouseEventHandler(this.TopMenu_MouseUp);

            topMenu.ResumeLayout(false);
            topMenu.PerformLayout();
        }

        private ToolStripButton InitializeCloseButton()
        {
            closeButton = new ToolStripButton
            {
                Alignment = ToolStripItemAlignment.Right,
                AutoSize = false,
                AutoToolTip = false,
                BackColor = secondaryColor,
                DisplayStyle = ToolStripItemDisplayStyle.Text,
                Font = new Font("Times New Roman", 9F, FontStyle.Bold, GraphicsUnit.Point, 204),
                ForeColor = textColor,
                Margin = new Padding(0),
                Size = new Size(30, 30),
                Text = "❌",
                TextDirection = ToolStripTextDirection.Horizontal,
            };
            closeButton.MouseUp += new MouseEventHandler(CloseButton_MouseUp);
            return closeButton;
        }

        private ToolStripLabel InitializeWindowLabel(ComponentResourceManager resources)
        {
            windowLabel = new ToolStripLabel
            {
                ActiveLinkColor = Color.RosyBrown,
                BackColor = secondaryColor,
                ForeColor = textColor,
                Image = (Image) resources.GetObject("windowLabel"),
                Font = font,
                Margin = new Padding(5, 0, 0, 0),
                Overflow = ToolStripItemOverflow.Never,
                Size = new Size(56, 30),
                Text = "* фігурки"
            };
            return windowLabel;
        }

        private void InitializeSideMenu()
        {
            sideMenu = new ToolStrip
            {
                AutoSize = false,
                BackColor = secondaryColor,
                Dock = DockStyle.Left,
                GripMargin = new Padding(0),
                GripStyle = ToolStripGripStyle.Hidden,
                Location = new Point(0, 30),
                Padding = new Padding(0),
                Size = new Size(102, 482),
                TabIndex = 4
            };

            sideMenu.Items.AddRange(new ToolStripItem[]
            {
                InitializeSideMenuButton("Task &A", RunTaskA),
                InitializeSideMenuButton("Ta&sk B", RunTaskB),
                InitializeSideMenuButton("Task &C", RunTaskC),
                InitializeSideMenuButton("Task &D", RunTaskD),
                InitializeSideMenuButton("Clea&r", ClearScreen)
            });

            sideMenu.ResumeLayout(false);
            sideMenu.PerformLayout();
        }

        public ToolStripButton addClickEvent(ToolStripButton button, EventHandler clickEvent)
        {
            button.Click += clickEvent;
            return button;
        }

        private ToolStripButton InitializeSideMenuButton(string text, EventHandler clickEvent)
        {
            ToolStripButton btn = new ToolStripButton
            {
                AutoToolTip = false,
                Margin = (text == "Clea&r") ? new Padding(0, 20, 0, 5) : new Padding(0, 8, 0, 5),
                Padding = new Padding(0, 4, 0, 4),
                Size = new Size(100, 27),
                DisplayStyle = ToolStripItemDisplayStyle.Text,
                Text = text,
                ForeColor = textColor,
                TextDirection = ToolStripTextDirection.Horizontal,
                Font = font,
            };
            addClickEvent(btn, clickEvent);
            return btn;
        }

        public void TopMenu_MouseDown(object sender, MouseEventArgs e)
        {
            dragging = true;
            dragCursorPoint = Cursor.Position;
            dragFormPoint = this.Location;
        }

        public void TopMenu_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                Point diff = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
                this.Location = Point.Add(dragFormPoint, new Size(diff));
            }
        }

        public void TopMenu_MouseUp(object sender, MouseEventArgs e) { dragging = false; }

        public void CloseButton_MouseUp(object sender, EventArgs e) { this.Close(); }

        private void RunTaskA(object sender, EventArgs e)
        {
            ResetFlags();
            shapes = true;
            this.Invalidate();
        }

        private void RunTaskB(object sender, EventArgs e)
        {
            ResetFlags();
            laptop = true;
            this.Invalidate();
        }

        private void RunTaskC(object sender, EventArgs e)
        {
            ResetFlags();
            timerTriangle.Start();
            triangle = true;
            this.Invalidate();
        }

        private void RunTaskD(object sender, EventArgs e)
        {
            ResetFlags();
            timerDrums.Start();
            drums = true;
            this.Invalidate();
        }


        private void DrawPie(Graphics g, Color color, Rectangle r, float from, float to)
        {
            g.FillPie(new SolidBrush(color), r, from, to);
        }

        private void DrawEllipse(Graphics g, Color color, Rectangle r)
        {
            g.FillEllipse(new SolidBrush(color), r);
        }

        private void DrawHeptagon(Graphics g, Color color, int start_x, int start_y, int radius)
        {
            const double k = 2.0 * Math.PI / 7.0;
            const double offset = k / 2.0;

            int center_x = start_x + radius;
            int center_y = start_y + radius;

            PointF[] polygon = new PointF[7];
            for (int i = 0; i < 7; i++)
            {
                polygon[i] = new PointF((float)(Math.Sin(k * i + offset) * radius + center_x), (float)(Math.Cos(k * i + offset) * radius + center_y));
            }

            g.FillPolygon(new SolidBrush(color), polygon);
        }


        private void DrawCube(Graphics g, Color color, int start_x, int start_y, int side)
        {
            SolidBrush brush = new SolidBrush(color);
            PointF[] polygon = new PointF[4];

            const float x = 1.0f;
            const float y = 1.0f;
            const float z = 3.0f;
            const float x_min = 3.5f;
            const float x_max = 4.5f;
            const float y_min = 2.5f;
            const float y_max = 3.5f;
            const float c = 0.2f;

            const float x_m = (x_min + x_max) / 2.0f;
            const float y_m = (y_min + y_max) / 2.0f;

            float quo = -0.54f * side;

            int[] x_1 = { 1, -1, -1, 1 };
            int[] y_1 = { 1, 1, -1, -1 };
            int[] z_1 = { 1, 1, 3, 3 };
            int[] z_2 = { 1, 3, 3, 1 };

            // front
            for (int i = 0; i < 4; i++)
            {
                polygon[i] = new PointF(((x_1[i] - x_m) * ((z - c) / z) + x_m) * quo + start_x, ((y_1[i] - y_m) * ((z - c) / z) + y_m) * quo + start_y);
            }
            g.FillPolygon(brush, polygon);

            // top
            for (int i = 0; i < 4; i++)
            {
                polygon[i] = new PointF(((x_1[i] - x_m) * ((z_1[i] - c) / z_1[i]) + x_m) * quo + start_x, ((y - y_m) * ((z_1[i] - c) / z_1[i]) + y_m) * quo + start_y);
            }
            brush.Color = Color.FromArgb(color.R + 10, color.G + 10, color.B + 10);
            g.FillPolygon(brush, polygon);

            // side
            for (int i = 0; i < 4; i++)
            {
                polygon[i] = new PointF(((x- x_m) * ((z_2[i] - c) / z_2[i]) + x_m) * quo + start_x, ((y_1[i] - y_m) * ((z_2[i] - c) / z_2[i]) + y_m) * quo + start_y);
            }
            brush.Color = Color.FromArgb(color.R - 10, color.G - 10, color.B - 10);
            g.FillPolygon(brush, polygon);

            brush.Color = color;
        }

        private void DrawLaptop(Graphics g, int start_x, int start_y)
        {
            Color gray = Color.FromArgb(55, 62, 72);
            Color darkgray = Color.FromArgb(27, 34, 42);
            Color lightblue = Color.FromArgb(221, 225, 236); 

            //top
            SolidBrush brush = new SolidBrush(gray);
            g.FillRectangle(brush, new Rectangle(start_x, start_y, 240, 140));

            // bottom
            PointF[] polygon = new PointF[4];
            polygon[0] = new PointF(start_x, start_y + 143);
            polygon[1] = new PointF(start_x + 240, start_y + 143);
            polygon[2] = new PointF(start_x + 260, start_y + 220);
            polygon[3] = new PointF(start_x - 20, start_y + 220);

            brush.Color = darkgray;
            g.FillPolygon(brush, polygon);

            polygon[2].Y -= 4;
            polygon[3].Y -= 4;
            brush.Color = gray;
            g.FillPolygon(brush, polygon);

            // keyboard
            polygon[0].X += 25;
            polygon[0].Y += 5;
            polygon[1].X -= 25;
            polygon[1].Y += 5;
            polygon[2].X -= 37;
            polygon[2].Y -= 35;
            polygon[3].X += 37;
            polygon[3].Y -= 35;

            brush.Color = darkgray;
            g.FillPolygon(brush, polygon);

            // touchpad
            polygon[0].X += 70;
            polygon[0].Y += 40;
            polygon[1].X -= 70;
            polygon[1].Y += 40;
            polygon[2].X -= 75;
            polygon[2].Y += 30;
            polygon[3].X += 75;
            polygon[3].Y += 30;

            g.FillPolygon(brush, polygon);

            // screen
            brush.Color = lightblue;
            g.FillRectangle(brush, new Rectangle(start_x+5, start_y+5, 230, 130));
        }

        private void DrawSpinningTriangle(Graphics g, Color color, int startX, int startY)
        {
            SolidBrush brush = new SolidBrush(color);
            PointF[] points = {
                new PointF(0, 0),
                new PointF(100, 50),
                new PointF(100, -50)
            };

            Matrix matrix = new Matrix();
            matrix.RotateAt(triangleAngle, new PointF(startX, startY));
            g.Transform = matrix;
            g.TranslateTransform(startX, startY);

            g.FillPolygon(brush, points);

            g.ResetTransform();
        }

        private void DrawBase(Graphics g, int start_x, int start_y)
        {
            int lineWidth = 3;
            int circleRadius = 5;
            Color top = Color.FromArgb(244, 218, 195);
            Color body = Color.FromArgb(223, 160, 88);

            SolidBrush brush = new SolidBrush(body);
            g.FillRectangle(brush, new Rectangle(start_x, start_y+25, 200, 90));
            g.FillEllipse(brush, new Rectangle(start_x, start_y+90, 200, 50));

            brush.Color = top;
            g.FillEllipse(brush, new Rectangle(start_x, start_y, 200, 50));

            Pen pen = new Pen(top, lineWidth);
            int leftX = start_x + 10;
            int bottomY = start_y + 110;
            int topY = start_y + 55;

            Point[] points = {
                new Point(leftX, topY),
                new Point(leftX + 20, bottomY),
                new Point(leftX + 50, topY + 10),
                new Point(leftX + 90, bottomY + 10),
                new Point(leftX + 130, topY + 10),
                new Point(leftX + 160, bottomY),
                new Point(leftX + 180, topY),
            };

            g.DrawLines(pen, points);

            brush.Color = top;
            foreach (Point point in points) {
                g.FillEllipse(brush, new Rectangle(point.X - circleRadius, point.Y - circleRadius, 2 * circleRadius, 2 * circleRadius));
            }
        }

        private void DrawStick(Graphics g, int startX, int startY, bool faceRight)
        {
            Color stickColor = Color.FromArgb(244, 218, 195);
            Color ballColor = Color.FromArgb(223, 160, 88);

            int stickWidth = 5;
            int stickLength = 120;
            int ballRadius = 10;

            Matrix matrix = new Matrix();
            matrix.RotateAt(drumsAngle, new PointF(startX, startY));
            g.Transform = matrix;
            g.TranslateTransform(0, 0);

            SolidBrush stickBrush = new SolidBrush(stickColor);
            SolidBrush ballBrush = new SolidBrush(ballColor);

            if (faceRight)
            {
                g.FillRectangle(stickBrush, new Rectangle(startX, startY, stickLength, stickWidth));
                g.FillEllipse(ballBrush, new Rectangle(startX + stickLength - ballRadius, startY - ballRadius + stickWidth / 2, ballRadius * 2, ballRadius * 2));
            }
            else
            {
                g.FillRectangle(stickBrush, new Rectangle(startX - stickLength, startY, stickLength, stickWidth));
                g.FillEllipse(ballBrush, new Rectangle(startX - stickLength - ballRadius, startY - ballRadius + stickWidth / 2, ballRadius * 2, ballRadius * 2));
            }

        }

        private void ResetFlags()
        {
            shapes = false;
            laptop = false;
            triangle = false;
            drums = false;
            timerTriangle.Stop();
            timerDrums.Stop();
        }

        private void ClearScreen(object sender, EventArgs e)
        {
            ResetFlags();
            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            this.DoubleBuffered = true;
            int canvas_width = this.Width - canvas_x;
            int canvas_height = this.Height - canvas_y;
            int center_x = canvas_width / 2 + canvas_x;
            int center_y = canvas_height / 2 + canvas_y;

            int borderWidth = 5;
            Pen pen = new Pen(secondaryColor, borderWidth);

            e.Graphics.DrawRectangle(pen, 0, 30, this.ClientSize.Width - (borderWidth-3), this.ClientSize.Height - (borderWidth + 27));

            if (shapes)
            {
                DrawHeptagon(e.Graphics, primaryColor, center_x + 50, center_y - 200, 100);

                DrawPie(e.Graphics, Color.FromArgb(242, 222, 68), new Rectangle(center_x + 100, center_y + 50, 100, 100), 30.0f, 300.0f);
                
                DrawCube(e.Graphics, accentColor, center_x - 100, center_y - 100, 100);
                
                DrawEllipse(e.Graphics, secondaryLightColor, new Rectangle(center_x - 200, center_y + 50 , 200, 100));
            }

            if (laptop) { DrawLaptop(e.Graphics, center_x - 120, center_y - 100); }

            if (triangle) 
            {
                DrawSpinningTriangle(e.Graphics, accentColor, center_x, center_y);
            }

            if (drums)
            {
                DrawBase(e.Graphics, center_x-100, center_y-50);
                DrawStick(e.Graphics, center_x + 160, center_y - 65, false);
                DrawStick(e.Graphics, center_x - 160, center_y - 65, true);
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            this.Invalidate();
        }
    }

    public class CustomToolStripProfessionalRenderer_Top : ToolStripProfessionalRenderer
    {
        private readonly Color secondary = Color.FromArgb(66, 65, 101);
        private readonly Color accent = Color.FromArgb(184, 27, 27);

        public CustomToolStripProfessionalRenderer_Top() { this.RoundedEdges = false; }
        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e) { }
        // stop doing whatever this is please

        protected override void OnRenderButtonBackground(ToolStripItemRenderEventArgs e)
        {
            ToolStripButton button = e.Item as ToolStripButton;
            if (button != null)
            {
                if (button.Selected)
                {
                    e.Graphics.FillRectangle(new SolidBrush(accent), e.Item.ContentRectangle);
                }
                else
                {
                    e.Graphics.FillRectangle(new SolidBrush(secondary), e.Item.ContentRectangle);
                }
            }
            else
            {
                base.OnRenderButtonBackground(e);
            }
        }
    }

    public class CustomToolStripProfessionalRenderer_Left : ToolStripProfessionalRenderer
    {
        private readonly Color secondaryDarkerColor = Color.FromArgb(56, 55, 91);
        private readonly Color secondaryLightColor = Color.FromArgb(76, 75, 111);

        public CustomToolStripProfessionalRenderer_Left() { this.RoundedEdges = false; }
        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e) { }
        protected override void OnRenderButtonBackground(ToolStripItemRenderEventArgs e)
        {
            ToolStripButton button = e.Item as ToolStripButton;
            if (button != null)
            {
                if (button.Selected)
                {
                    e.Graphics.FillRectangle(new SolidBrush(secondaryDarkerColor), e.Item.ContentRectangle);
                }
                else
                {
                    e.Graphics.FillRectangle(new SolidBrush(secondaryLightColor), e.Item.ContentRectangle);
                }
            }
            else
            {
                base.OnRenderButtonBackground(e);
            }
        }
    }
}
