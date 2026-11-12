using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Login_and_Register_System
{
    public static class Tema
    {
        // Paleta principal "La Casa del Sabor"
        public static readonly Color Oscuro = Color.FromArgb(74, 44, 42);        // Espresso oscuro
        public static readonly Color Vino = Color.FromArgb(123, 72, 76);         // Vino / Frambuesa tostada (marca)
        public static readonly Color VinoHover = Color.FromArgb(103, 57, 61);    // Vino hover
        public static readonly Color VinoOscuro = Color.FromArgb(88, 46, 50);    // Vino presionado
        public static readonly Color Cafe = Color.FromArgb(132, 87, 82);         // Moka cálido
        public static readonly Color Rosa = Color.FromArgb(252, 202, 203);       // Rosa pastel
        public static readonly Color RosaSuave = Color.FromArgb(248, 232, 232);  // Rosa crema suave
        public static readonly Color Crema = Color.FromArgb(253, 248, 246);      // Fondo crema cálido
        public static readonly Color Caramelo = Color.FromArgb(212, 163, 115);   // Galleta / caramelo tostado
        public static readonly Color Vainilla = Color.FromArgb(255, 245, 234);   // Glaseado vainilla
        public static readonly Color FresaSuave = Color.FromArgb(252, 226, 229); // Glaseado fresa
        public static readonly Color Dorado = Color.FromArgb(238, 193, 112);     // Dorado llama / acento repostería

        // Superficies, bordes y texto secundario
        public static readonly Color Blanco = Color.White;
        public static readonly Color Borde = Color.FromArgb(234, 217, 214);
        public static readonly Color BordeFuerte = Color.FromArgb(212, 186, 182);
        public static readonly Color TextoSuave = Color.FromArgb(134, 107, 103);
        public static readonly Color InputFondo = Color.FromArgb(250, 245, 243);
        public static readonly Color FilaAlterna = Color.FromArgb(253, 249, 248);

        // Colores semánticos armonizados
        public static readonly Color Exito = Color.FromArgb(46, 125, 91);
        public static readonly Color ExitoHover = Color.FromArgb(37, 102, 74);
        public static readonly Color ExitoSuave = Color.FromArgb(226, 243, 236);
        public static readonly Color ExitoTexto = Color.FromArgb(25, 90, 62);

        public static readonly Color Alerta = Color.FromArgb(217, 130, 43);
        public static readonly Color AlertaSuave = Color.FromArgb(253, 240, 221);
        public static readonly Color AlertaTexto = Color.FromArgb(138, 75, 8);

        public static readonly Color Peligro = Color.FromArgb(184, 59, 59);
        public static readonly Color PeligroHover = Color.FromArgb(252, 235, 235);
        public static readonly Color PeligroSuave = Color.FromArgb(252, 232, 232);
        public static readonly Color PeligroTexto = Color.FromArgb(140, 35, 35);

        // Tipografía unificada
        public static Font Fuente(float tamano, FontStyle estilo = FontStyle.Regular)
        {
            return new Font("Segoe UI", tamano, estilo);
        }

        private const int EM_SETCUEBANNER = 0x1501;
        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HT_CAPTION = 0x2;

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, string lParam);

        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

        private sealed class TarjetaEstilo
        {
            public Color Borde;
            public int Radio;
        }

        private static readonly ConditionalWeakTable<Control, TarjetaEstilo> _tarjetasDecoradas =
            new ConditionalWeakTable<Control, TarjetaEstilo>();
        private static readonly ConditionalWeakTable<Control, object> _botonesDecorados =
            new ConditionalWeakTable<Control, object>();
        private static readonly ConditionalWeakTable<Control, object> _badgesDecorados =
            new ConditionalWeakTable<Control, object>();
        private static readonly ConditionalWeakTable<Control, object> _arrastreHabilitado =
            new ConditionalWeakTable<Control, object>();

        public static void Aplicar(Control raiz)
        {
            if (raiz is Form)
            {
                raiz.BackColor = Crema;
                HabilitarDobleBuffer(raiz);
            }
            Recorrer(raiz);
        }

        private static void Recorrer(Control padre)
        {
            foreach (Control c in padre.Controls)
            {
                HabilitarDobleBuffer(c);
                NormalizarFuente(c);

                c.BackColor = Fondo(c.BackColor);
                c.ForeColor = Texto(c.ForeColor);

                var boton = c as Button;
                if (boton != null && boton.FlatStyle == FlatStyle.Flat)
                    boton.FlatAppearance.BorderColor = Fondo(boton.FlatAppearance.BorderColor);

                var tabla = c as DataGridView;
                if (tabla != null) EstiloTabla(tabla);

                Recorrer(c);
            }
        }

        private static void NormalizarFuente(Control c)
        {
            if (c.Font == null) return;
            string familia = c.Font.FontFamily.Name;
            if (familia.Equals("MS UI Gothic", StringComparison.OrdinalIgnoreCase) ||
                familia.Equals("Microsoft Sans Serif", StringComparison.OrdinalIgnoreCase) ||
                familia.Equals("Nirmala UI", StringComparison.OrdinalIgnoreCase) ||
                familia.Equals("Segoe UI Historic", StringComparison.OrdinalIgnoreCase))
            {
                c.Font = new Font("Segoe UI", c.Font.Size, c.Font.Style, c.Font.Unit);
            }
        }

        public static void HabilitarDobleBuffer(Control c)
        {
            try
            {
                PropertyInfo prop = typeof(Control).GetProperty(
                    "DoubleBuffered",
                    BindingFlags.Instance | BindingFlags.NonPublic);
                if (prop != null) prop.SetValue(c, true, null);
            }
            catch
            {
                // Ignorar controles que no permitan doble buffer
            }
        }

        public static void HabilitarPlaceholder(TextBox tb, string texto)
        {
            if (tb == null) return;
            Action aplicar = () =>
            {
                if (tb.IsHandleCreated)
                    SendMessage(tb.Handle, EM_SETCUEBANNER, 1, texto ?? "");
            };
            if (tb.IsHandleCreated)
                aplicar();
            else
                tb.HandleCreated += (s, e) => aplicar();
        }

        public static void HabilitarArrastre(Control control, Form formulario)
        {
            if (control == null || formulario == null) return;
            object dummy;
            if (_arrastreHabilitado.TryGetValue(control, out dummy)) return;
            _arrastreHabilitado.Add(control, new object());

            control.MouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Left && formulario.WindowState == FormWindowState.Normal)
                {
                    ReleaseCapture();
                    SendMessage(formulario.Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
                }
            };
        }

        public static void RedondearFormulario(Form form, int radio = 14)
        {
            if (form == null) return;
            Action actualizar = () =>
            {
                if (form.WindowState == FormWindowState.Maximized || form.Width <= 0 || form.Height <= 0)
                {
                    form.Region = null;
                    return;
                }
                using (var path = CrearRutaRedondeada(new Rectangle(0, 0, form.Width, form.Height), radio))
                {
                    form.Region = new Region(path);
                }
            };
            form.Load += (s, e) => actualizar();
            form.Resize += (s, e) => actualizar();
        }

        public static GraphicsPath CrearRutaRedondeada(Rectangle rect, int radio)
        {
            var path = new GraphicsPath();
            int d = Math.Max(2, radio * 2);
            if (d > rect.Width) d = rect.Width;
            if (d > rect.Height) d = rect.Height;

            var arco = new Rectangle(rect.X, rect.Y, d, d);
            path.AddArc(arco, 180, 90);
            arco.X = rect.Right - d;
            path.AddArc(arco, 270, 90);
            arco.Y = rect.Bottom - d;
            path.AddArc(arco, 0, 90);
            arco.X = rect.Left;
            path.AddArc(arco, 90, 90);
            path.CloseFigure();
            return path;
        }

        /// <summary>
        /// Estiliza un Panel como tarjeta elevada con esquinas redondeadas y borde suave anti-aliased.
        /// </summary>
        public static void PintarTarjeta(Panel panel, Color? fondo = null, Color? borde = null, int radio = 12)
        {
            if (panel == null) return;
            HabilitarDobleBuffer(panel);

            Color colorFondo = fondo ?? Blanco;
            Color colorBorde = borde ?? Borde;
            panel.BackColor = colorFondo;

            TarjetaEstilo estilo;
            if (_tarjetasDecoradas.TryGetValue(panel, out estilo))
            {
                estilo.Borde = colorBorde;
                estilo.Radio = radio;
                panel.Invalidate();
                return;
            }

            estilo = new TarjetaEstilo { Borde = colorBorde, Radio = radio };
            _tarjetasDecoradas.Add(panel, estilo);

            panel.Resize += (s, e) => panel.Invalidate();
            panel.Paint += (s, e) =>
            {
                if (panel.Width < 4 || panel.Height < 4) return;
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                Color exterior = panel.Parent != null ? panel.Parent.BackColor : Crema;
                var rect = new Rectangle(0, 0, panel.Width - 1, panel.Height - 1);

                using (var rutaExterior = new GraphicsPath { FillMode = FillMode.Alternate })
                using (var rutaInterior = CrearRutaRedondeada(rect, estilo.Radio))
                {
                    rutaExterior.AddRectangle(new Rectangle(-1, -1, panel.Width + 2, panel.Height + 2));
                    rutaExterior.AddPath(rutaInterior, false);

                    using (var brochaExt = new SolidBrush(exterior))
                    {
                        e.Graphics.FillPath(brochaExt, rutaExterior);
                    }
                    using (var pluma = new Pen(estilo.Borde, 1f))
                    {
                        e.Graphics.DrawPath(pluma, rutaInterior);
                    }
                }
            };
        }

        private static void DecorarEsquinasBoton(Button btn, int radio, Color? colorBorde = null)
        {
            if (btn == null) return;
            btn.FlatAppearance.BorderColor = colorBorde ?? Color.Empty;
            object dummy;
            if (_botonesDecorados.TryGetValue(btn, out dummy))
            {
                btn.Invalidate();
                return;
            }
            _botonesDecorados.Add(btn, new object());

            btn.Resize += (s, e) => btn.Invalidate();
            btn.Paint += (s, e) =>
            {
                if (btn.Width < 4 || btn.Height < 4) return;
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                Color exterior = btn.Parent != null ? btn.Parent.BackColor : Crema;
                var rect = new Rectangle(0, 0, btn.Width - 1, btn.Height - 1);

                using (var rutaExterior = new GraphicsPath { FillMode = FillMode.Alternate })
                using (var rutaInterior = CrearRutaRedondeada(rect, radio))
                {
                    rutaExterior.AddRectangle(new Rectangle(-1, -1, btn.Width + 2, btn.Height + 2));
                    rutaExterior.AddPath(rutaInterior, false);
                    using (var brochaExt = new SolidBrush(exterior))
                    {
                        e.Graphics.FillPath(brochaExt, rutaExterior);
                    }
                    Color bordeActual = btn.FlatAppearance.BorderColor;
                    if (!bordeActual.IsEmpty && bordeActual.A > 0)
                    {
                        using (var pluma = new Pen(bordeActual, 1f))
                        {
                            e.Graphics.DrawPath(pluma, rutaInterior);
                        }
                    }
                }
            };
        }

        public static void EstilizarBotonPrimario(Button btn, Color? fondo = null, Color? hover = null, int radio = 8)
        {
            if (btn == null) return;
            Color f = fondo ?? Vino;
            Color h = hover ?? VinoHover;

            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = h;
            btn.FlatAppearance.MouseDownBackColor = VinoOscuro;
            btn.BackColor = f;
            btn.ForeColor = Blanco;
            btn.Font = Fuente(9.5f, FontStyle.Bold);
            btn.Cursor = Cursors.Hand;
            btn.UseVisualStyleBackColor = false;
            DecorarEsquinasBoton(btn, radio, f);
        }

        public static void EstilizarBotonSecundario(Button btn, int radio = 8)
        {
            if (btn == null) return;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = RosaSuave;
            btn.FlatAppearance.MouseDownBackColor = Rosa;
            btn.BackColor = Blanco;
            btn.ForeColor = Oscuro;
            btn.Font = Fuente(9.5f, FontStyle.Bold);
            btn.Cursor = Cursors.Hand;
            btn.UseVisualStyleBackColor = false;
            DecorarEsquinasBoton(btn, radio, BordeFuerte);
        }

        public static void EstilizarBotonPeligro(Button btn, int radio = 8)
        {
            if (btn == null) return;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = PeligroHover;
            btn.FlatAppearance.MouseDownBackColor = Rosa;
            btn.BackColor = Blanco;
            btn.ForeColor = Peligro;
            btn.Font = Fuente(9.5f, FontStyle.Bold);
            btn.Cursor = Cursors.Hand;
            btn.UseVisualStyleBackColor = false;
            DecorarEsquinasBoton(btn, radio, Color.FromArgb(229, 173, 173));
        }

        public static void EstilizarFiltro(Button btn, bool activo)
        {
            if (btn == null) return;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Cursor = Cursors.Hand;
            btn.UseVisualStyleBackColor = false;
            btn.Font = Fuente(9f, activo ? FontStyle.Bold : FontStyle.Regular);

            if (activo)
            {
                btn.BackColor = Vino;
                btn.ForeColor = Blanco;
                btn.FlatAppearance.MouseOverBackColor = VinoHover;
                btn.FlatAppearance.MouseDownBackColor = VinoOscuro;
                DecorarEsquinasBoton(btn, 14, Vino);
            }
            else
            {
                btn.BackColor = RosaSuave;
                btn.ForeColor = Oscuro;
                btn.FlatAppearance.MouseOverBackColor = Rosa;
                btn.FlatAppearance.MouseDownBackColor = Rosa;
                DecorarEsquinasBoton(btn, 14, Borde);
            }
        }

        /// <summary>
        /// Estiliza un Label como insignia tipo píldora (badge) con esquinas redondeadas.
        /// </summary>
        public static void EstilizarBadge(Label lbl, Color fondo, Color texto, int radio = 10)
        {
            if (lbl == null) return;
            lbl.BackColor = fondo;
            lbl.ForeColor = texto;
            lbl.TextAlign = ContentAlignment.MiddleCenter;

            Action actualizarRegion = () =>
            {
                if (lbl.Width <= 0 || lbl.Height <= 0) return;
                using (var path = CrearRutaRedondeada(new Rectangle(0, 0, lbl.Width, lbl.Height), radio))
                {
                    lbl.Region = new Region(path);
                }
            };
            actualizarRegion();

            object dummy;
            if (!_badgesDecorados.TryGetValue(lbl, out dummy))
            {
                _badgesDecorados.Add(lbl, new object());
                lbl.Resize += (s, e) => actualizarRegion();
            }
        }

        public static void EstiloTabla(DataGridView t)
        {
            if (t == null) return;
            HabilitarDobleBuffer(t);
            t.BackgroundColor = Blanco;
            t.BorderStyle = BorderStyle.None;
            t.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            t.GridColor = Color.FromArgb(240, 226, 224);
            t.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            t.EnableHeadersVisualStyles = false;
            t.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            t.ColumnHeadersHeight = 36;
            t.RowTemplate.Height = 34;

            t.ColumnHeadersDefaultCellStyle.BackColor = RosaSuave;
            t.ColumnHeadersDefaultCellStyle.ForeColor = Oscuro;
            t.ColumnHeadersDefaultCellStyle.Font = Fuente(9.5f, FontStyle.Bold);
            t.ColumnHeadersDefaultCellStyle.Padding = new Padding(8, 0, 8, 0);
            t.ColumnHeadersDefaultCellStyle.SelectionBackColor = RosaSuave;
            t.ColumnHeadersDefaultCellStyle.SelectionForeColor = Oscuro;

            t.DefaultCellStyle.BackColor = Blanco;
            t.DefaultCellStyle.ForeColor = Oscuro;
            t.DefaultCellStyle.Font = Fuente(9.5f, FontStyle.Regular);
            t.DefaultCellStyle.Padding = new Padding(8, 4, 8, 4);
            t.DefaultCellStyle.SelectionBackColor = Rosa;
            t.DefaultCellStyle.SelectionForeColor = Oscuro;

            t.AlternatingRowsDefaultCellStyle.BackColor = FilaAlterna;
            t.AlternatingRowsDefaultCellStyle.ForeColor = Oscuro;
            t.AlternatingRowsDefaultCellStyle.SelectionBackColor = Rosa;
            t.AlternatingRowsDefaultCellStyle.SelectionForeColor = Oscuro;
        }

        private static bool EsAzulMorado(Color c)
        {
            if (c.IsEmpty || c.A == 0) return false;
            float h = c.GetHue();
            return c.GetSaturation() >= 0.1f && h >= 200f && h <= 290f;
        }

        private static Color Fondo(Color c)
        {
            if (!EsAzulMorado(c)) return c;
            float l = c.GetBrightness();
            if (l >= 0.85f) return RosaSuave;
            if (l >= 0.45f) return Cafe;
            return Vino;
        }

        private static Color Texto(Color c)
        {
            if (!EsAzulMorado(c)) return c;
            float l = c.GetBrightness();
            if (l >= 0.85f) return RosaSuave;
            if (l >= 0.40f) return Cafe;
            return Oscuro;
        }

        public enum MotivoPasteleria
        {
            Cupcake,
            PastelPisos,
            Tarta,
            GorroRepostero
        }

        /// <summary>
        /// Dibuja una onda festoneada tipo glaseado o encaje pastelero en el borde superior o inferior.
        /// </summary>
        public static void DibujarOndaGlaseado(Graphics g, int xInicio, int yBase, int ancho, int diametroOnda, Color colorOnda, bool haciaAbajo = true)
        {
            if (g == null || ancho <= 0 || diametroOnda < 4) return;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            int r = diametroOnda / 2;
            using (var brocha = new SolidBrush(colorOnda))
            {
                for (int x = xInicio; x < xInicio + ancho; x += diametroOnda)
                {
                    int w = Math.Min(diametroOnda, xInicio + ancho - x);
                    if (w < 3) break;
                    int y = haciaAbajo ? yBase - r : yBase - diametroOnda;
                    g.FillEllipse(brocha, x, y, diametroOnda, diametroOnda);
                }
            }
        }

        /// <summary>
        /// Dibuja chispas y destellos sutiles de pastelería (baja opacidad) sobre un fondo.
        /// </summary>
        public static void DibujarChispas(Graphics g, Rectangle area, Color[] colores, int semilla = 11, int cantidad = 16)
        {
            if (g == null || area.Width < 30 || area.Height < 30 || colores == null || colores.Length == 0) return;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            var rnd = new Random(semilla);
            for (int i = 0; i < cantidad; i++)
            {
                Color c = colores[i % colores.Length];
                float x = area.X + rnd.Next(12, Math.Max(14, area.Width - 12));
                float y = area.Y + rnd.Next(10, Math.Max(12, area.Height - 10));
                int tipo = i % 3;

                if (tipo == 0)
                {
                    float d = 4 + (i % 3) * 2;
                    using (var b = new SolidBrush(c))
                    {
                        g.FillEllipse(b, x, y, d, d);
                    }
                }
                else if (tipo == 1)
                {
                    float angulo = rnd.Next(0, 180);
                    float dx = (float)(Math.Cos(angulo * Math.PI / 180.0) * 6.0);
                    float dy = (float)(Math.Sin(angulo * Math.PI / 180.0) * 6.0);
                    using (var p = new Pen(c, 2.2f) { StartCap = LineCap.Round, EndCap = LineCap.Round })
                    {
                        g.DrawLine(p, x - dx, y - dy, x + dx, y + dy);
                    }
                }
                else
                {
                    using (var b = new SolidBrush(c))
                    {
                        PointF[] rombo =
                        {
                            new PointF(x, y - 4),
                            new PointF(x + 3.5f, y),
                            new PointF(x, y + 4),
                            new PointF(x - 3.5f, y)
                        };
                        g.FillPolygon(b, rombo);
                    }
                }
            }
        }

        /// <summary>
        /// Dibuja un medallón circular suave con un motivo vectorial de pastelería centrado.
        /// </summary>
        public static void DibujarMedallonPasteleria(Graphics g, Rectangle area, MotivoPasteleria motivo, Color fondoCirculo, Color bordeCirculo, Color acento)
        {
            if (g == null || area.Width < 12 || area.Height < 12) return;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            using (var b = new SolidBrush(fondoCirculo))
            {
                g.FillEllipse(b, area);
            }
            if (!bordeCirculo.IsEmpty && bordeCirculo.A > 0)
            {
                using (var p = new Pen(bordeCirculo, 1f))
                {
                    g.DrawEllipse(p, area);
                }
            }

            int pad = Math.Max(4, area.Width / 5);
            var interior = new Rectangle(area.X + pad, area.Y + pad, area.Width - pad * 2, area.Height - pad * 2);
            DibujarIconoPasteleria(g, interior, motivo, acento);
        }

        /// <summary>
        /// Dibuja un ícono vectorial nítido de pastelería dentro del rectángulo indicado.
        /// </summary>
        public static void DibujarIconoPasteleria(Graphics g, Rectangle area, MotivoPasteleria motivo, Color acento)
        {
            if (g == null || area.Width < 10 || area.Height < 10) return;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Func<float, float> px = f => area.X + area.Width * f;
            Func<float, float> py = f => area.Y + area.Height * f;
            Func<float, float> pw = f => area.Width * f;
            Func<float, float> ph = f => area.Height * f;
            float trazo = Math.Max(1.2f, Math.Min(area.Width, area.Height) * 0.055f);

            switch (motivo)
            {
                case MotivoPasteleria.Cupcake:
                {
                    // Base / capacillo
                    PointF[] basePts =
                    {
                        new PointF(px(0.22f), py(0.56f)),
                        new PointF(px(0.78f), py(0.56f)),
                        new PointF(px(0.69f), py(0.92f)),
                        new PointF(px(0.31f), py(0.92f))
                    };
                    using (var bBase = new SolidBrush(Caramelo))
                    using (var pContorno = new Pen(acento, trazo))
                    using (var pPliegue = new Pen(Color.FromArgb(140, acento), Math.Max(1f, trazo * 0.75f)))
                    {
                        g.FillPolygon(bBase, basePts);
                        g.DrawLine(pPliegue, px(0.41f), py(0.59f), px(0.44f), py(0.89f));
                        g.DrawLine(pPliegue, px(0.59f), py(0.59f), px(0.56f), py(0.89f));
                        g.DrawPolygon(pContorno, basePts);
                    }

                    // Betún / crema en capas
                    using (var bCrema = new SolidBrush(FresaSuave))
                    using (var bCremaTop = new SolidBrush(Vainilla))
                    using (var pContorno = new Pen(acento, trazo))
                    {
                        g.FillEllipse(bCrema, px(0.16f), py(0.37f), pw(0.68f), ph(0.24f));
                        g.FillEllipse(bCremaTop, px(0.24f), py(0.24f), pw(0.52f), ph(0.22f));
                        g.FillEllipse(bCrema, px(0.35f), py(0.14f), pw(0.30f), ph(0.17f));
                        g.DrawArc(pContorno, px(0.16f), py(0.37f), pw(0.68f), ph(0.24f), 0, 180);
                        g.DrawEllipse(pContorno, px(0.16f), py(0.37f), pw(0.68f), ph(0.24f));
                    }

                    // Cereza
                    using (var bCereza = new SolidBrush(acento))
                    using (var pTallo = new Pen(acento, trazo))
                    {
                        g.FillEllipse(bCereza, px(0.43f), py(0.05f), pw(0.15f), ph(0.15f));
                        g.DrawArc(pTallo, px(0.49f), py(0.00f), pw(0.14f), ph(0.12f), 190, 110);
                    }
                    break;
                }

                case MotivoPasteleria.PastelPisos:
                {
                    using (var bBase = new SolidBrush(Vainilla))
                    using (var bGlaseado = new SolidBrush(Rosa))
                    using (var bPlato = new SolidBrush(Caramelo))
                    using (var bLlama = new SolidBrush(Dorado))
                    using (var pContorno = new Pen(acento, trazo))
                    {
                        // Plato
                        g.FillRectangle(bPlato, px(0.10f), py(0.87f), pw(0.80f), ph(0.07f));
                        g.DrawRectangle(pContorno, px(0.10f), py(0.87f), pw(0.80f), ph(0.07f));

                        // Piso inferior
                        g.FillRectangle(bBase, px(0.18f), py(0.55f), pw(0.64f), ph(0.32f));
                        g.FillRectangle(bGlaseado, px(0.18f), py(0.55f), pw(0.64f), ph(0.11f));
                        g.DrawRectangle(pContorno, px(0.18f), py(0.55f), pw(0.64f), ph(0.32f));

                        // Piso superior
                        g.FillRectangle(bBase, px(0.29f), py(0.30f), pw(0.42f), ph(0.25f));
                        g.FillRectangle(bGlaseado, px(0.29f), py(0.30f), pw(0.42f), ph(0.09f));
                        g.DrawRectangle(pContorno, px(0.29f), py(0.30f), pw(0.42f), ph(0.25f));

                        // Vela y llama
                        g.FillRectangle(bGlaseado, px(0.47f), py(0.14f), pw(0.06f), ph(0.16f));
                        g.DrawRectangle(pContorno, px(0.47f), py(0.14f), pw(0.06f), ph(0.16f));
                        g.FillEllipse(bLlama, px(0.45f), py(0.03f), pw(0.11f), ph(0.11f));
                    }
                    break;
                }

                case MotivoPasteleria.Tarta:
                {
                    PointF[] rebanada =
                    {
                        new PointF(px(0.14f), py(0.44f)),
                        new PointF(px(0.84f), py(0.28f)),
                        new PointF(px(0.84f), py(0.84f)),
                        new PointF(px(0.14f), py(0.84f))
                    };
                    PointF[] cubierta =
                    {
                        new PointF(px(0.14f), py(0.44f)),
                        new PointF(px(0.84f), py(0.28f)),
                        new PointF(px(0.84f), py(0.41f)),
                        new PointF(px(0.14f), py(0.53f))
                    };

                    using (var bBizcocho = new SolidBrush(Vainilla))
                    using (var bRelleno = new SolidBrush(Rosa))
                    using (var bCaramelo = new SolidBrush(Caramelo))
                    using (var bFresa = new SolidBrush(acento))
                    using (var pContorno = new Pen(acento, trazo))
                    {
                        g.FillPolygon(bBizcocho, rebanada);
                        g.FillRectangle(bCaramelo, px(0.14f), py(0.65f), pw(0.70f), ph(0.08f));
                        g.FillPolygon(bRelleno, cubierta);
                        g.DrawPolygon(pContorno, rebanada);
                        g.FillEllipse(bFresa, px(0.56f), py(0.14f), pw(0.17f), ph(0.17f));
                    }
                    break;
                }

                case MotivoPasteleria.GorroRepostero:
                {
                    using (var bGorro = new SolidBrush(Vainilla))
                    using (var bBanda = new SolidBrush(RosaSuave))
                    using (var pContorno = new Pen(acento, trazo))
                    {
                        g.FillEllipse(bGorro, px(0.14f), py(0.22f), pw(0.32f), ph(0.34f));
                        g.FillEllipse(bGorro, px(0.54f), py(0.22f), pw(0.32f), ph(0.34f));
                        g.FillEllipse(bGorro, px(0.30f), py(0.10f), pw(0.40f), ph(0.38f));
                        g.FillRectangle(bBanda, px(0.25f), py(0.52f), pw(0.50f), ph(0.30f));

                        g.DrawArc(pContorno, px(0.14f), py(0.22f), pw(0.32f), ph(0.34f), 90, 210);
                        g.DrawArc(pContorno, px(0.30f), py(0.10f), pw(0.40f), ph(0.38f), 195, 150);
                        g.DrawArc(pContorno, px(0.54f), py(0.22f), pw(0.32f), ph(0.34f), 240, 210);
                        g.DrawRectangle(pContorno, px(0.25f), py(0.52f), pw(0.50f), ph(0.30f));
                    }
                    break;
                }
            }
        }

        /// <summary>
        /// Agrega un medallón decorativo de pastelería junto al título de una sección.
        /// </summary>
        public static void DecorarTituloSeccion(Control contenedorHeader, Label lblTitulo, MotivoPasteleria motivo)
        {
            if (contenedorHeader == null || lblTitulo == null) return;
            HabilitarDobleBuffer(contenedorHeader);

            contenedorHeader.Paint += (s, e) =>
            {
                int x = lblTitulo.Right + 12;
                int y = lblTitulo.Top + Math.Max(0, (lblTitulo.Height - 30) / 2);
                if (x + 34 > contenedorHeader.Width) return;

                var rect = new Rectangle(x, y, 30, 30);
                DibujarMedallonPasteleria(e.Graphics, rect, motivo, RosaSuave, Borde, Vino);
            };
            lblTitulo.Resize += (s, e) => contenedorHeader.Invalidate();
        }

        /// <summary>
        /// Decora la tarjeta oscura de totales con una sutil onda de glaseado dentro del borde redondeado.
        /// </summary>
        public static void DecorarTarjetaTotalPasteleria(Panel panelTotal, int radio = 12)
        {
            if (panelTotal == null) return;
            panelTotal.Paint += (s, e) =>
            {
                if (panelTotal.Width < 20 || panelTotal.Height < 20) return;
                using (var clip = CrearRutaRedondeada(new Rectangle(1, 1, panelTotal.Width - 2, panelTotal.Height - 2), radio))
                {
                    var estado = e.Graphics.Save();
                    e.Graphics.SetClip(clip);
                    DibujarOndaGlaseado(e.Graphics, 0, panelTotal.Height - 1, panelTotal.Width + 16, 16, Color.FromArgb(30, Rosa), true);
                    e.Graphics.Restore(estado);
                }
            };
        }
    }
}
