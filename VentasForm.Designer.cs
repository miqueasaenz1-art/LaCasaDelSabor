namespace Login_and_Register_System
{
    partial class VentasForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.labelTitulo = new System.Windows.Forms.Label();
            this.labelFecha = new System.Windows.Forms.Label();
            this.buttonNuevaVenta = new System.Windows.Forms.Button();
            this.panelProductos = new System.Windows.Forms.Panel();
            this.panelTablaContenedor = new System.Windows.Forms.Panel();
            this.dataGridViewDetalle = new System.Windows.Forms.DataGridView();
            this.ColumnProducto = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDCantidad = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDPrecio = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnSubtotal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panelAccionesDetalle = new System.Windows.Forms.Panel();
            this.buttonVistaPrevia = new System.Windows.Forms.Button();
            this.buttonQuitar = new System.Windows.Forms.Button();
            this.buttonAgregar = new System.Windows.Forms.Button();
            this.numericUpDownCantidad = new System.Windows.Forms.NumericUpDown();
            this.labelCantidad = new System.Windows.Forms.Label();
            this.comboBoxProducto = new System.Windows.Forms.ComboBox();
            this.label1Producto = new System.Windows.Forms.Label();
            this.label1productoTitulo = new System.Windows.Forms.Label();
            this.panel = new System.Windows.Forms.Panel();
            this.panelBotonesCobro = new System.Windows.Forms.TableLayoutPanel();
            this.buttonCobrar = new System.Windows.Forms.Button();
            this.buttonReporte = new System.Windows.Forms.Button();
            this.panelTotal = new System.Windows.Forms.Panel();
            this.labelTotal = new System.Windows.Forms.Label();
            this.labelTotalTexto = new System.Windows.Forms.Label();
            this.panelPago = new System.Windows.Forms.Panel();
            this.btnExacto = new System.Windows.Forms.Button();
            this.textBox2Cambio = new System.Windows.Forms.TextBox();
            this.labelCambioTexto = new System.Windows.Forms.Label();
            this.textBoxRecibido = new System.Windows.Forms.TextBox();
            this.labelRecibido = new System.Windows.Forms.Label();
            this.comboBoxMetodo = new System.Windows.Forms.ComboBox();
            this.labelMetodo = new System.Windows.Forms.Label();
            this.labelCobro = new System.Windows.Forms.Label();
            this.layoutPrincipal = new System.Windows.Forms.TableLayoutPanel();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.layoutCuerpo = new System.Windows.Forms.TableLayoutPanel();
            this.panelProductos.SuspendLayout();
            this.panelTablaContenedor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDetalle)).BeginInit();
            this.panelAccionesDetalle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCantidad)).BeginInit();
            this.panel.SuspendLayout();
            this.panelBotonesCobro.SuspendLayout();
            this.panelTotal.SuspendLayout();
            this.panelPago.SuspendLayout();
            this.layoutPrincipal.SuspendLayout();
            this.panelHeader.SuspendLayout();
            this.layoutCuerpo.SuspendLayout();
            this.SuspendLayout();
            // 
            // layoutPrincipal
            // 
            this.layoutPrincipal.BackColor = System.Drawing.Color.FromArgb(253, 248, 246);
            this.layoutPrincipal.ColumnCount = 1;
            this.layoutPrincipal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutPrincipal.Controls.Add(this.panelHeader, 0, 0);
            this.layoutPrincipal.Controls.Add(this.layoutCuerpo, 0, 1);
            this.layoutPrincipal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutPrincipal.Location = new System.Drawing.Point(24, 18);
            this.layoutPrincipal.Name = "layoutPrincipal";
            this.layoutPrincipal.RowCount = 2;
            this.layoutPrincipal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 74F));
            this.layoutPrincipal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutPrincipal.Size = new System.Drawing.Size(982, 650);
            this.layoutPrincipal.TabIndex = 0;
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.Color.FromArgb(253, 248, 246);
            this.panelHeader.Controls.Add(this.buttonNuevaVenta);
            this.panelHeader.Controls.Add(this.labelFecha);
            this.panelHeader.Controls.Add(this.labelTitulo);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Margin = new System.Windows.Forms.Padding(0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(982, 74);
            this.panelHeader.TabIndex = 0;
            // 
            // labelTitulo
            // 
            this.labelTitulo.AutoSize = true;
            this.labelTitulo.Font = new System.Drawing.Font("Segoe UI", 19F, System.Drawing.FontStyle.Bold);
            this.labelTitulo.ForeColor = System.Drawing.Color.FromArgb(74, 44, 42);
            this.labelTitulo.Location = new System.Drawing.Point(0, 2);
            this.labelTitulo.Name = "labelTitulo";
            this.labelTitulo.Size = new System.Drawing.Size(202, 36);
            this.labelTitulo.TabIndex = 0;
            this.labelTitulo.Text = "Punto de Venta";
            // 
            // labelFecha
            // 
            this.labelFecha.AutoSize = true;
            this.labelFecha.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.labelFecha.ForeColor = System.Drawing.Color.FromArgb(134, 107, 103);
            this.labelFecha.Location = new System.Drawing.Point(2, 40);
            this.labelFecha.Name = "labelFecha";
            this.labelFecha.Size = new System.Drawing.Size(42, 17);
            this.labelFecha.TabIndex = 1;
            this.labelFecha.Text = "Fecha";
            // 
            // buttonNuevaVenta
            // 
            this.buttonNuevaVenta.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonNuevaVenta.BackColor = System.Drawing.Color.FromArgb(123, 72, 76);
            this.buttonNuevaVenta.FlatAppearance.BorderSize = 0;
            this.buttonNuevaVenta.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonNuevaVenta.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.buttonNuevaVenta.ForeColor = System.Drawing.Color.White;
            this.buttonNuevaVenta.Location = new System.Drawing.Point(820, 14);
            this.buttonNuevaVenta.Name = "buttonNuevaVenta";
            this.buttonNuevaVenta.Size = new System.Drawing.Size(162, 38);
            this.buttonNuevaVenta.TabIndex = 2;
            this.buttonNuevaVenta.Text = "+ Nueva venta";
            this.buttonNuevaVenta.UseVisualStyleBackColor = false;
            // 
            // layoutCuerpo
            // 
            this.layoutCuerpo.BackColor = System.Drawing.Color.FromArgb(253, 248, 246);
            this.layoutCuerpo.ColumnCount = 2;
            this.layoutCuerpo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 58F));
            this.layoutCuerpo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 42F));
            this.layoutCuerpo.Controls.Add(this.panelProductos, 0, 0);
            this.layoutCuerpo.Controls.Add(this.panel, 1, 0);
            this.layoutCuerpo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutCuerpo.Location = new System.Drawing.Point(0, 74);
            this.layoutCuerpo.Margin = new System.Windows.Forms.Padding(0);
            this.layoutCuerpo.Name = "layoutCuerpo";
            this.layoutCuerpo.RowCount = 1;
            this.layoutCuerpo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutCuerpo.Size = new System.Drawing.Size(982, 576);
            this.layoutCuerpo.TabIndex = 1;
            // 
            // panelProductos
            // 
            this.panelProductos.BackColor = System.Drawing.Color.White;
            this.panelProductos.Controls.Add(this.panelTablaContenedor);
            this.panelProductos.Controls.Add(this.panelAccionesDetalle);
            this.panelProductos.Controls.Add(this.buttonAgregar);
            this.panelProductos.Controls.Add(this.numericUpDownCantidad);
            this.panelProductos.Controls.Add(this.labelCantidad);
            this.panelProductos.Controls.Add(this.comboBoxProducto);
            this.panelProductos.Controls.Add(this.label1Producto);
            this.panelProductos.Controls.Add(this.label1productoTitulo);
            this.panelProductos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelProductos.Location = new System.Drawing.Point(0, 0);
            this.panelProductos.Margin = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.panelProductos.Name = "panelProductos";
            this.panelProductos.Padding = new System.Windows.Forms.Padding(20, 18, 20, 18);
            this.panelProductos.Size = new System.Drawing.Size(559, 576);
            this.panelProductos.TabIndex = 0;
            // 
            // label1productoTitulo
            // 
            this.label1productoTitulo.AutoSize = true;
            this.label1productoTitulo.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.label1productoTitulo.ForeColor = System.Drawing.Color.FromArgb(74, 44, 42);
            this.label1productoTitulo.Location = new System.Drawing.Point(20, 18);
            this.label1productoTitulo.Name = "label1productoTitulo";
            this.label1productoTitulo.Size = new System.Drawing.Size(174, 21);
            this.label1productoTitulo.TabIndex = 0;
            this.label1productoTitulo.Text = "Productos de la venta";
            // 
            // label1Producto
            // 
            this.label1Producto.AutoSize = true;
            this.label1Producto.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.label1Producto.ForeColor = System.Drawing.Color.FromArgb(132, 87, 82);
            this.label1Producto.Location = new System.Drawing.Point(20, 56);
            this.label1Producto.Name = "label1Producto";
            this.label1Producto.Size = new System.Drawing.Size(57, 15);
            this.label1Producto.TabIndex = 1;
            this.label1Producto.Text = "Producto";
            // 
            // comboBoxProducto
            // 
            this.comboBoxProducto.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxProducto.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxProducto.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.comboBoxProducto.FormattingEnabled = true;
            this.comboBoxProducto.Location = new System.Drawing.Point(20, 78);
            this.comboBoxProducto.Name = "comboBoxProducto";
            this.comboBoxProducto.Size = new System.Drawing.Size(320, 25);
            this.comboBoxProducto.TabIndex = 2;
            // 
            // labelCantidad
            // 
            this.labelCantidad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelCantidad.AutoSize = true;
            this.labelCantidad.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.labelCantidad.ForeColor = System.Drawing.Color.FromArgb(132, 87, 82);
            this.labelCantidad.Location = new System.Drawing.Point(354, 56);
            this.labelCantidad.Name = "labelCantidad";
            this.labelCantidad.Size = new System.Drawing.Size(57, 15);
            this.labelCantidad.TabIndex = 3;
            this.labelCantidad.Text = "Cantidad";
            // 
            // numericUpDownCantidad
            // 
            this.numericUpDownCantidad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDownCantidad.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.numericUpDownCantidad.Location = new System.Drawing.Point(354, 78);
            this.numericUpDownCantidad.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownCantidad.Name = "numericUpDownCantidad";
            this.numericUpDownCantidad.Size = new System.Drawing.Size(68, 25);
            this.numericUpDownCantidad.TabIndex = 4;
            this.numericUpDownCantidad.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numericUpDownCantidad.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // buttonAgregar
            // 
            this.buttonAgregar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAgregar.BackColor = System.Drawing.Color.FromArgb(123, 72, 76);
            this.buttonAgregar.FlatAppearance.BorderSize = 0;
            this.buttonAgregar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonAgregar.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.buttonAgregar.ForeColor = System.Drawing.Color.White;
            this.buttonAgregar.Location = new System.Drawing.Point(432, 74);
            this.buttonAgregar.Name = "buttonAgregar";
            this.buttonAgregar.Size = new System.Drawing.Size(107, 32);
            this.buttonAgregar.TabIndex = 5;
            this.buttonAgregar.Text = "+ Agregar";
            this.buttonAgregar.UseVisualStyleBackColor = false;
            this.buttonAgregar.Click += new System.EventHandler(this.buttonAgregar_Click_1);
            // 
            // panelTablaContenedor
            // 
            this.panelTablaContenedor.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelTablaContenedor.BackColor = System.Drawing.Color.White;
            this.panelTablaContenedor.Controls.Add(this.dataGridViewDetalle);
            this.panelTablaContenedor.Location = new System.Drawing.Point(20, 122);
            this.panelTablaContenedor.Name = "panelTablaContenedor";
            this.panelTablaContenedor.Size = new System.Drawing.Size(519, 378);
            this.panelTablaContenedor.TabIndex = 6;
            // 
            // dataGridViewDetalle
            // 
            this.dataGridViewDetalle.AllowUserToAddRows = false;
            this.dataGridViewDetalle.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewDetalle.BackgroundColor = System.Drawing.Color.White;
            this.dataGridViewDetalle.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridViewDetalle.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewDetalle.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnProducto,
            this.colDCantidad,
            this.colDPrecio,
            this.ColumnSubtotal});
            this.dataGridViewDetalle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewDetalle.EnableHeadersVisualStyles = false;
            this.dataGridViewDetalle.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewDetalle.MultiSelect = false;
            this.dataGridViewDetalle.Name = "dataGridViewDetalle";
            this.dataGridViewDetalle.ReadOnly = true;
            this.dataGridViewDetalle.RowHeadersVisible = false;
            this.dataGridViewDetalle.RowHeadersWidth = 51;
            this.dataGridViewDetalle.RowTemplate.Height = 34;
            this.dataGridViewDetalle.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewDetalle.Size = new System.Drawing.Size(519, 378);
            this.dataGridViewDetalle.TabIndex = 0;
            // 
            // ColumnProducto
            // 
            this.ColumnProducto.FillWeight = 42F;
            this.ColumnProducto.HeaderText = "Producto";
            this.ColumnProducto.MinimumWidth = 6;
            this.ColumnProducto.Name = "ColumnProducto";
            this.ColumnProducto.ReadOnly = true;
            // 
            // colDCantidad
            // 
            this.colDCantidad.FillWeight = 16F;
            this.colDCantidad.HeaderText = "Cant.";
            this.colDCantidad.MinimumWidth = 6;
            this.colDCantidad.Name = "colDCantidad";
            this.colDCantidad.ReadOnly = true;
            // 
            // colDPrecio
            // 
            this.colDPrecio.FillWeight = 21F;
            this.colDPrecio.HeaderText = "Precio";
            this.colDPrecio.MinimumWidth = 6;
            this.colDPrecio.Name = "colDPrecio";
            this.colDPrecio.ReadOnly = true;
            // 
            // ColumnSubtotal
            // 
            this.ColumnSubtotal.FillWeight = 21F;
            this.ColumnSubtotal.HeaderText = "Subtotal";
            this.ColumnSubtotal.MinimumWidth = 6;
            this.ColumnSubtotal.Name = "ColumnSubtotal";
            this.ColumnSubtotal.ReadOnly = true;
            // 
            // panelAccionesDetalle
            // 
            this.panelAccionesDetalle.BackColor = System.Drawing.Color.White;
            this.panelAccionesDetalle.Controls.Add(this.buttonVistaPrevia);
            this.panelAccionesDetalle.Controls.Add(this.buttonQuitar);
            this.panelAccionesDetalle.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelAccionesDetalle.Location = new System.Drawing.Point(20, 512);
            this.panelAccionesDetalle.Name = "panelAccionesDetalle";
            this.panelAccionesDetalle.Size = new System.Drawing.Size(519, 46);
            this.panelAccionesDetalle.TabIndex = 7;
            // 
            // buttonVistaPrevia
            // 
            this.buttonVistaPrevia.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonVistaPrevia.Location = new System.Drawing.Point(0, 6);
            this.buttonVistaPrevia.Name = "buttonVistaPrevia";
            this.buttonVistaPrevia.Size = new System.Drawing.Size(166, 36);
            this.buttonVistaPrevia.TabIndex = 0;
            this.buttonVistaPrevia.Text = "Vista previa recibo";
            this.buttonVistaPrevia.UseVisualStyleBackColor = true;
            // 
            // buttonQuitar
            // 
            this.buttonQuitar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonQuitar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonQuitar.ForeColor = System.Drawing.Color.FromArgb(184, 59, 59);
            this.buttonQuitar.Location = new System.Drawing.Point(369, 6);
            this.buttonQuitar.Name = "buttonQuitar";
            this.buttonQuitar.Size = new System.Drawing.Size(150, 36);
            this.buttonQuitar.TabIndex = 1;
            this.buttonQuitar.Text = "Quitar producto";
            this.buttonQuitar.UseVisualStyleBackColor = true;
            // 
            // panel (Cobro derecho)
            // 
            this.panel.BackColor = System.Drawing.Color.White;
            this.panel.Controls.Add(this.panelBotonesCobro);
            this.panel.Controls.Add(this.panelTotal);
            this.panel.Controls.Add(this.panelPago);
            this.panel.Controls.Add(this.labelCobro);
            this.panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel.Location = new System.Drawing.Point(579, 0);
            this.panel.Margin = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.panel.Name = "panel";
            this.panel.Padding = new System.Windows.Forms.Padding(20, 18, 20, 18);
            this.panel.Size = new System.Drawing.Size(403, 576);
            this.panel.TabIndex = 1;
            // 
            // labelCobro
            // 
            this.labelCobro.AutoSize = true;
            this.labelCobro.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.labelCobro.ForeColor = System.Drawing.Color.FromArgb(74, 44, 42);
            this.labelCobro.Location = new System.Drawing.Point(20, 18);
            this.labelCobro.Name = "labelCobro";
            this.labelCobro.Size = new System.Drawing.Size(142, 21);
            this.labelCobro.TabIndex = 0;
            this.labelCobro.Text = "Resumen y cobro";
            // 
            // panelPago
            // 
            this.panelPago.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelPago.BackColor = System.Drawing.Color.FromArgb(248, 232, 232);
            this.panelPago.Controls.Add(this.btnExacto);
            this.panelPago.Controls.Add(this.textBox2Cambio);
            this.panelPago.Controls.Add(this.labelCambioTexto);
            this.panelPago.Controls.Add(this.textBoxRecibido);
            this.panelPago.Controls.Add(this.labelRecibido);
            this.panelPago.Controls.Add(this.comboBoxMetodo);
            this.panelPago.Controls.Add(this.labelMetodo);
            this.panelPago.Location = new System.Drawing.Point(20, 56);
            this.panelPago.Name = "panelPago";
            this.panelPago.Size = new System.Drawing.Size(363, 214);
            this.panelPago.TabIndex = 1;
            // 
            // labelMetodo
            // 
            this.labelMetodo.AutoSize = true;
            this.labelMetodo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.labelMetodo.ForeColor = System.Drawing.Color.FromArgb(74, 44, 42);
            this.labelMetodo.Location = new System.Drawing.Point(16, 16);
            this.labelMetodo.Name = "labelMetodo";
            this.labelMetodo.Size = new System.Drawing.Size(102, 15);
            this.labelMetodo.TabIndex = 0;
            this.labelMetodo.Text = "Método de pago";
            // 
            // comboBoxMetodo
            // 
            this.comboBoxMetodo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxMetodo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMetodo.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.comboBoxMetodo.FormattingEnabled = true;
            this.comboBoxMetodo.Location = new System.Drawing.Point(16, 38);
            this.comboBoxMetodo.Name = "comboBoxMetodo";
            this.comboBoxMetodo.Size = new System.Drawing.Size(331, 27);
            this.comboBoxMetodo.TabIndex = 1;
            // 
            // labelRecibido
            // 
            this.labelRecibido.AutoSize = true;
            this.labelRecibido.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.labelRecibido.ForeColor = System.Drawing.Color.FromArgb(74, 44, 42);
            this.labelRecibido.Location = new System.Drawing.Point(16, 86);
            this.labelRecibido.Name = "labelRecibido";
            this.labelRecibido.Size = new System.Drawing.Size(83, 15);
            this.labelRecibido.TabIndex = 2;
            this.labelRecibido.Text = "Recibido (C$)";
            // 
            // textBoxRecibido
            // 
            this.textBoxRecibido.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxRecibido.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.textBoxRecibido.Location = new System.Drawing.Point(16, 108);
            this.textBoxRecibido.Name = "textBoxRecibido";
            this.textBoxRecibido.Size = new System.Drawing.Size(160, 27);
            this.textBoxRecibido.TabIndex = 3;
            // 
            // btnExacto
            // 
            this.btnExacto.Location = new System.Drawing.Point(16, 146);
            this.btnExacto.Name = "btnExacto";
            this.btnExacto.Size = new System.Drawing.Size(160, 30);
            this.btnExacto.TabIndex = 4;
            this.btnExacto.Text = "Pago exacto";
            this.btnExacto.UseVisualStyleBackColor = true;
            // 
            // labelCambioTexto
            // 
            this.labelCambioTexto.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelCambioTexto.AutoSize = true;
            this.labelCambioTexto.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.labelCambioTexto.ForeColor = System.Drawing.Color.FromArgb(74, 44, 42);
            this.labelCambioTexto.Location = new System.Drawing.Point(198, 86);
            this.labelCambioTexto.Name = "labelCambioTexto";
            this.labelCambioTexto.Size = new System.Drawing.Size(76, 15);
            this.labelCambioTexto.TabIndex = 5;
            this.labelCambioTexto.Text = "Cambio (C$)";
            // 
            // textBox2Cambio
            // 
            this.textBox2Cambio.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox2Cambio.BackColor = System.Drawing.Color.White;
            this.textBox2Cambio.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox2Cambio.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.textBox2Cambio.ForeColor = System.Drawing.Color.FromArgb(46, 125, 91);
            this.textBox2Cambio.Location = new System.Drawing.Point(198, 108);
            this.textBox2Cambio.Name = "textBox2Cambio";
            this.textBox2Cambio.ReadOnly = true;
            this.textBox2Cambio.Size = new System.Drawing.Size(149, 27);
            this.textBox2Cambio.TabIndex = 6;
            this.textBox2Cambio.Text = "0.00";
            this.textBox2Cambio.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // panelTotal
            // 
            this.panelTotal.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelTotal.BackColor = System.Drawing.Color.FromArgb(123, 72, 76);
            this.panelTotal.Controls.Add(this.labelTotal);
            this.panelTotal.Controls.Add(this.labelTotalTexto);
            this.panelTotal.Location = new System.Drawing.Point(20, 408);
            this.panelTotal.Name = "panelTotal";
            this.panelTotal.Size = new System.Drawing.Size(363, 76);
            this.panelTotal.TabIndex = 2;
            // 
            // labelTotalTexto
            // 
            this.labelTotalTexto.AutoSize = true;
            this.labelTotalTexto.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold);
            this.labelTotalTexto.ForeColor = System.Drawing.Color.FromArgb(248, 232, 232);
            this.labelTotalTexto.Location = new System.Drawing.Point(18, 26);
            this.labelTotalTexto.Name = "labelTotalTexto";
            this.labelTotalTexto.Size = new System.Drawing.Size(96, 19);
            this.labelTotalTexto.TabIndex = 0;
            this.labelTotalTexto.Text = "Total a pagar";
            // 
            // labelTotal
            // 
            this.labelTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelTotal.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.labelTotal.ForeColor = System.Drawing.Color.White;
            this.labelTotal.Location = new System.Drawing.Point(120, 18);
            this.labelTotal.Name = "labelTotal";
            this.labelTotal.Size = new System.Drawing.Size(226, 40);
            this.labelTotal.TabIndex = 1;
            this.labelTotal.Text = "C$ 0.00";
            this.labelTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panelBotonesCobro
            // 
            this.panelBotonesCobro.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelBotonesCobro.BackColor = System.Drawing.Color.White;
            this.panelBotonesCobro.ColumnCount = 2;
            this.panelBotonesCobro.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.panelBotonesCobro.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.panelBotonesCobro.Controls.Add(this.buttonCobrar, 0, 0);
            this.panelBotonesCobro.Controls.Add(this.buttonReporte, 1, 0);
            this.panelBotonesCobro.Location = new System.Drawing.Point(20, 500);
            this.panelBotonesCobro.Name = "panelBotonesCobro";
            this.panelBotonesCobro.RowCount = 1;
            this.panelBotonesCobro.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.panelBotonesCobro.Size = new System.Drawing.Size(363, 54);
            this.panelBotonesCobro.TabIndex = 3;
            // 
            // buttonCobrar
            // 
            this.buttonCobrar.BackColor = System.Drawing.Color.FromArgb(46, 125, 91);
            this.buttonCobrar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonCobrar.FlatAppearance.BorderSize = 0;
            this.buttonCobrar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCobrar.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold);
            this.buttonCobrar.ForeColor = System.Drawing.Color.White;
            this.buttonCobrar.Location = new System.Drawing.Point(0, 0);
            this.buttonCobrar.Margin = new System.Windows.Forms.Padding(0, 0, 6, 0);
            this.buttonCobrar.Name = "buttonCobrar";
            this.buttonCobrar.Size = new System.Drawing.Size(175, 54);
            this.buttonCobrar.TabIndex = 0;
            this.buttonCobrar.Text = "Cobrar venta";
            this.buttonCobrar.UseVisualStyleBackColor = false;
            // 
            // buttonReporte
            // 
            this.buttonReporte.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonReporte.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonReporte.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.buttonReporte.ForeColor = System.Drawing.Color.FromArgb(74, 44, 42);
            this.buttonReporte.Location = new System.Drawing.Point(187, 0);
            this.buttonReporte.Margin = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.buttonReporte.Name = "buttonReporte";
            this.buttonReporte.Size = new System.Drawing.Size(176, 54);
            this.buttonReporte.TabIndex = 1;
            this.buttonReporte.Text = "Reporte PDF";
            this.buttonReporte.UseVisualStyleBackColor = true;
            // 
            // VentasForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(253, 248, 246);
            this.ClientSize = new System.Drawing.Size(1030, 690);
            this.Controls.Add(this.layoutPrincipal);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "VentasForm";
            this.Padding = new System.Windows.Forms.Padding(24, 18, 24, 22);
            this.Text = "VentasForm";
            this.panelProductos.ResumeLayout(false);
            this.panelProductos.PerformLayout();
            this.panelTablaContenedor.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDetalle)).EndInit();
            this.panelAccionesDetalle.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCantidad)).EndInit();
            this.panel.ResumeLayout(false);
            this.panel.PerformLayout();
            this.panelBotonesCobro.ResumeLayout(false);
            this.panelTotal.ResumeLayout(false);
            this.panelTotal.PerformLayout();
            this.panelPago.ResumeLayout(false);
            this.panelPago.PerformLayout();
            this.layoutPrincipal.ResumeLayout(false);
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.layoutCuerpo.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelTitulo;
        private System.Windows.Forms.Label labelFecha;
        private System.Windows.Forms.Button buttonNuevaVenta;
        private System.Windows.Forms.Panel panelProductos;
        private System.Windows.Forms.Label label1productoTitulo;
        private System.Windows.Forms.Label label1Producto;
        private System.Windows.Forms.ComboBox comboBoxProducto;
        private System.Windows.Forms.NumericUpDown numericUpDownCantidad;
        private System.Windows.Forms.Label labelCantidad;
        private System.Windows.Forms.Button buttonAgregar;
        private System.Windows.Forms.Button buttonQuitar;
        private System.Windows.Forms.Button buttonVistaPrevia;
        private System.Windows.Forms.Panel panelAccionesDetalle;
        private System.Windows.Forms.Panel panelTablaContenedor;
        private System.Windows.Forms.DataGridView dataGridViewDetalle;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnProducto;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDCantidad;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDPrecio;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnSubtotal;
        private System.Windows.Forms.Panel panel;
        private System.Windows.Forms.Label labelCobro;
        private System.Windows.Forms.Panel panelPago;
        private System.Windows.Forms.Button btnExacto;
        private System.Windows.Forms.Panel panelTotal;
        private System.Windows.Forms.TableLayoutPanel panelBotonesCobro;
        private System.Windows.Forms.Button buttonCobrar;
        private System.Windows.Forms.Label labelRecibido;
        private System.Windows.Forms.ComboBox comboBoxMetodo;
        private System.Windows.Forms.Label labelMetodo;
        private System.Windows.Forms.Label labelTotalTexto;
        private System.Windows.Forms.TextBox textBox2Cambio;
        private System.Windows.Forms.Label labelCambioTexto;
        private System.Windows.Forms.TextBox textBoxRecibido;
        private System.Windows.Forms.Label labelTotal;
        private System.Windows.Forms.Button buttonReporte;
        private System.Windows.Forms.TableLayoutPanel layoutPrincipal;
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.TableLayoutPanel layoutCuerpo;
    }
}
