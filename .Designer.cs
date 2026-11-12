namespace Login_and_Register_System
{
    partial class Formpedidos
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
            this.labelfecha = new System.Windows.Forms.Label();
            this.panelBuscar = new System.Windows.Forms.Panel();
            this.textBuscar = new System.Windows.Forms.TextBox();
            this.buttonNuevo = new System.Windows.Forms.Button();
            this.tableTarjeta = new System.Windows.Forms.TableLayoutPanel();
            this.panelLista = new System.Windows.Forms.Panel();
            this.panelListaHeader = new System.Windows.Forms.Panel();
            this.buttonPendiente = new System.Windows.Forms.Button();
            this.buttonHoy = new System.Windows.Forms.Button();
            this.buttonTodos = new System.Windows.Forms.Button();
            this.labelListaTitulo = new System.Windows.Forms.Label();
            this.dataGridViewPedidos = new System.Windows.Forms.DataGridView();
            this.colId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCliente = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEntrega = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEstado = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTotal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panelDetalle = new System.Windows.Forms.Panel();
            this.panelDetalleTabla = new System.Windows.Forms.Panel();
            this.dataGridViewDetalle = new System.Windows.Forms.DataGridView();
            this.ColumnProducto = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDCantidad = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDPrecio = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnSubtotal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panelDetalleBottom = new System.Windows.Forms.Panel();
            this.panelBotonesDetalle = new System.Windows.Forms.TableLayoutPanel();
            this.buttonGuardar = new System.Windows.Forms.Button();
            this.buttonImprimir = new System.Windows.Forms.Button();
            this.buttonEliminarPedido = new System.Windows.Forms.Button();
            this.panelTotal = new System.Windows.Forms.Panel();
            this.labelTotal = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panelAgregarProd = new System.Windows.Forms.Panel();
            this.buttonAgregar = new System.Windows.Forms.Button();
            this.numericUpDownCantidad = new System.Windows.Forms.NumericUpDown();
            this.label1Cantidad = new System.Windows.Forms.Label();
            this.comboBoxProducto = new System.Windows.Forms.ComboBox();
            this.labelProducto = new System.Windows.Forms.Label();
            this.panelDatos = new System.Windows.Forms.Panel();
            this.comboBoxEstado = new System.Windows.Forms.ComboBox();
            this.labelEstado = new System.Windows.Forms.Label();
            this.dateEntrega = new System.Windows.Forms.DateTimePicker();
            this.labelEntrega = new System.Windows.Forms.Label();
            this.comboBox1Cliente = new System.Windows.Forms.ComboBox();
            this.labelCliente = new System.Windows.Forms.Label();
            this.panelDetalleTop = new System.Windows.Forms.Panel();
            this.labelEstadoBadge = new System.Windows.Forms.Label();
            this.labelDetalleTitulo = new System.Windows.Forms.Label();
            this.layoutPrincipal = new System.Windows.Forms.TableLayoutPanel();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.layoutCuerpo = new System.Windows.Forms.TableLayoutPanel();
            this.panelBuscar.SuspendLayout();
            this.panelLista.SuspendLayout();
            this.panelListaHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPedidos)).BeginInit();
            this.panelDetalle.SuspendLayout();
            this.panelDetalleTabla.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDetalle)).BeginInit();
            this.panelDetalleBottom.SuspendLayout();
            this.panelBotonesDetalle.SuspendLayout();
            this.panelTotal.SuspendLayout();
            this.panelAgregarProd.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCantidad)).BeginInit();
            this.panelDatos.SuspendLayout();
            this.panelDetalleTop.SuspendLayout();
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
            this.layoutPrincipal.Controls.Add(this.tableTarjeta, 0, 1);
            this.layoutPrincipal.Controls.Add(this.layoutCuerpo, 0, 2);
            this.layoutPrincipal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutPrincipal.Location = new System.Drawing.Point(24, 18);
            this.layoutPrincipal.Name = "layoutPrincipal";
            this.layoutPrincipal.RowCount = 3;
            this.layoutPrincipal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.layoutPrincipal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 104F));
            this.layoutPrincipal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutPrincipal.Size = new System.Drawing.Size(982, 650);
            this.layoutPrincipal.TabIndex = 0;
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.Color.FromArgb(253, 248, 246);
            this.panelHeader.Controls.Add(this.buttonNuevo);
            this.panelHeader.Controls.Add(this.panelBuscar);
            this.panelHeader.Controls.Add(this.labelfecha);
            this.panelHeader.Controls.Add(this.labelTitulo);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Margin = new System.Windows.Forms.Padding(0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(982, 70);
            this.panelHeader.TabIndex = 0;
            // 
            // labelTitulo
            // 
            this.labelTitulo.AutoSize = true;
            this.labelTitulo.Font = new System.Drawing.Font("Segoe UI", 19F, System.Drawing.FontStyle.Bold);
            this.labelTitulo.ForeColor = System.Drawing.Color.FromArgb(74, 44, 42);
            this.labelTitulo.Location = new System.Drawing.Point(0, 2);
            this.labelTitulo.Name = "labelTitulo";
            this.labelTitulo.Size = new System.Drawing.Size(235, 36);
            this.labelTitulo.TabIndex = 0;
            this.labelTitulo.Text = "Encargos y Pedidos";
            // 
            // labelfecha
            // 
            this.labelfecha.AutoSize = true;
            this.labelfecha.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.labelfecha.ForeColor = System.Drawing.Color.FromArgb(134, 107, 103);
            this.labelfecha.Location = new System.Drawing.Point(2, 39);
            this.labelfecha.Name = "labelfecha";
            this.labelfecha.Size = new System.Drawing.Size(42, 17);
            this.labelfecha.TabIndex = 1;
            this.labelfecha.Text = "Fecha";
            // 
            // panelBuscar
            // 
            this.panelBuscar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panelBuscar.BackColor = System.Drawing.Color.White;
            this.panelBuscar.Controls.Add(this.textBuscar);
            this.panelBuscar.Location = new System.Drawing.Point(548, 13);
            this.panelBuscar.Name = "panelBuscar";
            this.panelBuscar.Size = new System.Drawing.Size(262, 38);
            this.panelBuscar.TabIndex = 2;
            // 
            // textBuscar
            // 
            this.textBuscar.BackColor = System.Drawing.Color.White;
            this.textBuscar.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBuscar.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.textBuscar.ForeColor = System.Drawing.Color.FromArgb(74, 44, 42);
            this.textBuscar.Location = new System.Drawing.Point(12, 9);
            this.textBuscar.Name = "textBuscar";
            this.textBuscar.Size = new System.Drawing.Size(238, 18);
            this.textBuscar.TabIndex = 0;
            this.textBuscar.TextChanged += new System.EventHandler(this.textBuscar_TextChanged);
            // 
            // buttonNuevo
            // 
            this.buttonNuevo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonNuevo.BackColor = System.Drawing.Color.FromArgb(123, 72, 76);
            this.buttonNuevo.FlatAppearance.BorderSize = 0;
            this.buttonNuevo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonNuevo.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.buttonNuevo.ForeColor = System.Drawing.Color.White;
            this.buttonNuevo.Location = new System.Drawing.Point(824, 13);
            this.buttonNuevo.Name = "buttonNuevo";
            this.buttonNuevo.Size = new System.Drawing.Size(158, 38);
            this.buttonNuevo.TabIndex = 3;
            this.buttonNuevo.Text = "+ Nuevo encargo";
            this.buttonNuevo.UseVisualStyleBackColor = false;
            this.buttonNuevo.Click += new System.EventHandler(this.buttonNuevo_Click);
            // 
            // tableTarjeta
            // 
            this.tableTarjeta.BackColor = System.Drawing.Color.FromArgb(253, 248, 246);
            this.tableTarjeta.ColumnCount = 4;
            this.tableTarjeta.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableTarjeta.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableTarjeta.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableTarjeta.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableTarjeta.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableTarjeta.Location = new System.Drawing.Point(0, 70);
            this.tableTarjeta.Margin = new System.Windows.Forms.Padding(0, 2, 0, 12);
            this.tableTarjeta.Name = "tableTarjeta";
            this.tableTarjeta.RowCount = 1;
            this.tableTarjeta.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableTarjeta.Size = new System.Drawing.Size(982, 90);
            this.tableTarjeta.TabIndex = 1;
            this.tableTarjeta.Paint += new System.Windows.Forms.PaintEventHandler(this.tableTarjeta_Paint);
            // 
            // layoutCuerpo
            // 
            this.layoutCuerpo.BackColor = System.Drawing.Color.FromArgb(253, 248, 246);
            this.layoutCuerpo.ColumnCount = 2;
            this.layoutCuerpo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layoutCuerpo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layoutCuerpo.Controls.Add(this.panelLista, 0, 0);
            this.layoutCuerpo.Controls.Add(this.panelDetalle, 1, 0);
            this.layoutCuerpo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutCuerpo.Location = new System.Drawing.Point(0, 174);
            this.layoutCuerpo.Margin = new System.Windows.Forms.Padding(0);
            this.layoutCuerpo.Name = "layoutCuerpo";
            this.layoutCuerpo.RowCount = 1;
            this.layoutCuerpo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutCuerpo.Size = new System.Drawing.Size(982, 476);
            this.layoutCuerpo.TabIndex = 2;
            // 
            // panelLista
            // 
            this.panelLista.BackColor = System.Drawing.Color.White;
            this.panelLista.Controls.Add(this.dataGridViewPedidos);
            this.panelLista.Controls.Add(this.panelListaHeader);
            this.panelLista.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelLista.Location = new System.Drawing.Point(0, 0);
            this.panelLista.Margin = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.panelLista.Name = "panelLista";
            this.panelLista.Padding = new System.Windows.Forms.Padding(18, 14, 18, 18);
            this.panelLista.Size = new System.Drawing.Size(481, 476);
            this.panelLista.TabIndex = 0;
            // 
            // panelListaHeader
            // 
            this.panelListaHeader.BackColor = System.Drawing.Color.White;
            this.panelListaHeader.Controls.Add(this.buttonPendiente);
            this.panelListaHeader.Controls.Add(this.buttonHoy);
            this.panelListaHeader.Controls.Add(this.buttonTodos);
            this.panelListaHeader.Controls.Add(this.labelListaTitulo);
            this.panelListaHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelListaHeader.Location = new System.Drawing.Point(18, 14);
            this.panelListaHeader.Name = "panelListaHeader";
            this.panelListaHeader.Size = new System.Drawing.Size(445, 44);
            this.panelListaHeader.TabIndex = 0;
            // 
            // labelListaTitulo
            // 
            this.labelListaTitulo.AutoSize = true;
            this.labelListaTitulo.Font = new System.Drawing.Font("Segoe UI", 11.5F, System.Drawing.FontStyle.Bold);
            this.labelListaTitulo.ForeColor = System.Drawing.Color.FromArgb(74, 44, 42);
            this.labelListaTitulo.Location = new System.Drawing.Point(0, 8);
            this.labelListaTitulo.Name = "labelListaTitulo";
            this.labelListaTitulo.Size = new System.Drawing.Size(134, 21);
            this.labelListaTitulo.TabIndex = 0;
            this.labelListaTitulo.Text = "Lista de encargos";
            // 
            // buttonTodos
            // 
            this.buttonTodos.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonTodos.Location = new System.Drawing.Point(198, 6);
            this.buttonTodos.Name = "buttonTodos";
            this.buttonTodos.Size = new System.Drawing.Size(74, 28);
            this.buttonTodos.TabIndex = 1;
            this.buttonTodos.Text = "Todos";
            this.buttonTodos.Click += new System.EventHandler(this.buttonTodos_Click);
            // 
            // buttonHoy
            // 
            this.buttonHoy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonHoy.Location = new System.Drawing.Point(278, 6);
            this.buttonHoy.Name = "buttonHoy";
            this.buttonHoy.Size = new System.Drawing.Size(66, 28);
            this.buttonHoy.TabIndex = 2;
            this.buttonHoy.Text = "Hoy";
            this.buttonHoy.Click += new System.EventHandler(this.buttonHoy_Click);
            // 
            // buttonPendiente
            // 
            this.buttonPendiente.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonPendiente.Location = new System.Drawing.Point(350, 6);
            this.buttonPendiente.Name = "buttonPendiente";
            this.buttonPendiente.Size = new System.Drawing.Size(95, 28);
            this.buttonPendiente.TabIndex = 3;
            this.buttonPendiente.Text = "Pendiente";
            this.buttonPendiente.Click += new System.EventHandler(this.buttonPendiente_Click);
            // 
            // dataGridViewPedidos
            // 
            this.dataGridViewPedidos.AllowUserToAddRows = false;
            this.dataGridViewPedidos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewPedidos.BackgroundColor = System.Drawing.Color.White;
            this.dataGridViewPedidos.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridViewPedidos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewPedidos.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colId,
            this.colCliente,
            this.colEntrega,
            this.colEstado,
            this.colTotal});
            this.dataGridViewPedidos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewPedidos.EnableHeadersVisualStyles = false;
            this.dataGridViewPedidos.Location = new System.Drawing.Point(18, 58);
            this.dataGridViewPedidos.MultiSelect = false;
            this.dataGridViewPedidos.Name = "dataGridViewPedidos";
            this.dataGridViewPedidos.ReadOnly = true;
            this.dataGridViewPedidos.RowHeadersVisible = false;
            this.dataGridViewPedidos.RowHeadersWidth = 51;
            this.dataGridViewPedidos.RowTemplate.Height = 32;
            this.dataGridViewPedidos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewPedidos.Size = new System.Drawing.Size(445, 400);
            this.dataGridViewPedidos.TabIndex = 1;
            this.dataGridViewPedidos.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewPedidos_CellContentClick);
            // 
            // colId
            // 
            this.colId.FillWeight = 22F;
            this.colId.HeaderText = "N.º";
            this.colId.MinimumWidth = 6;
            this.colId.Name = "colId";
            this.colId.ReadOnly = true;
            // 
            // colCliente
            // 
            this.colCliente.FillWeight = 40F;
            this.colCliente.HeaderText = "Cliente";
            this.colCliente.MinimumWidth = 6;
            this.colCliente.Name = "colCliente";
            this.colCliente.ReadOnly = true;
            // 
            // colEntrega
            // 
            this.colEntrega.FillWeight = 24F;
            this.colEntrega.HeaderText = "Entrega";
            this.colEntrega.MinimumWidth = 6;
            this.colEntrega.Name = "colEntrega";
            this.colEntrega.ReadOnly = true;
            // 
            // colEstado
            // 
            this.colEstado.FillWeight = 28F;
            this.colEstado.HeaderText = "Estado";
            this.colEstado.MinimumWidth = 6;
            this.colEstado.Name = "colEstado";
            this.colEstado.ReadOnly = true;
            // 
            // colTotal
            // 
            this.colTotal.FillWeight = 28F;
            this.colTotal.HeaderText = "Total";
            this.colTotal.MinimumWidth = 6;
            this.colTotal.Name = "colTotal";
            this.colTotal.ReadOnly = true;
            // 
            // panelDetalle
            // 
            this.panelDetalle.BackColor = System.Drawing.Color.White;
            this.panelDetalle.Controls.Add(this.panelDetalleTabla);
            this.panelDetalle.Controls.Add(this.panelDetalleBottom);
            this.panelDetalle.Controls.Add(this.panelAgregarProd);
            this.panelDetalle.Controls.Add(this.panelDatos);
            this.panelDetalle.Controls.Add(this.panelDetalleTop);
            this.panelDetalle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDetalle.Location = new System.Drawing.Point(491, 0);
            this.panelDetalle.Margin = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.panelDetalle.Name = "panelDetalle";
            this.panelDetalle.Padding = new System.Windows.Forms.Padding(18, 14, 18, 18);
            this.panelDetalle.Size = new System.Drawing.Size(491, 476);
            this.panelDetalle.TabIndex = 1;
            // 
            // panelDetalleTop
            // 
            this.panelDetalleTop.BackColor = System.Drawing.Color.White;
            this.panelDetalleTop.Controls.Add(this.labelEstadoBadge);
            this.panelDetalleTop.Controls.Add(this.labelDetalleTitulo);
            this.panelDetalleTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelDetalleTop.Location = new System.Drawing.Point(18, 14);
            this.panelDetalleTop.Name = "panelDetalleTop";
            this.panelDetalleTop.Size = new System.Drawing.Size(455, 36);
            this.panelDetalleTop.TabIndex = 0;
            // 
            // labelDetalleTitulo
            // 
            this.labelDetalleTitulo.AutoSize = true;
            this.labelDetalleTitulo.Font = new System.Drawing.Font("Segoe UI", 11.5F, System.Drawing.FontStyle.Bold);
            this.labelDetalleTitulo.ForeColor = System.Drawing.Color.FromArgb(74, 44, 42);
            this.labelDetalleTitulo.Location = new System.Drawing.Point(0, 6);
            this.labelDetalleTitulo.Name = "labelDetalleTitulo";
            this.labelDetalleTitulo.Size = new System.Drawing.Size(121, 21);
            this.labelDetalleTitulo.TabIndex = 0;
            this.labelDetalleTitulo.Text = "Nuevo encargo";
            // 
            // labelEstadoBadge
            // 
            this.labelEstadoBadge.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelEstadoBadge.BackColor = System.Drawing.Color.FromArgb(253, 240, 221);
            this.labelEstadoBadge.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.labelEstadoBadge.ForeColor = System.Drawing.Color.FromArgb(138, 75, 8);
            this.labelEstadoBadge.Location = new System.Drawing.Point(351, 5);
            this.labelEstadoBadge.Name = "labelEstadoBadge";
            this.labelEstadoBadge.Size = new System.Drawing.Size(104, 25);
            this.labelEstadoBadge.TabIndex = 1;
            this.labelEstadoBadge.Text = "Pendiente";
            this.labelEstadoBadge.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelDatos
            // 
            this.panelDatos.BackColor = System.Drawing.Color.FromArgb(248, 232, 232);
            this.panelDatos.Controls.Add(this.comboBoxEstado);
            this.panelDatos.Controls.Add(this.labelEstado);
            this.panelDatos.Controls.Add(this.dateEntrega);
            this.panelDatos.Controls.Add(this.labelEntrega);
            this.panelDatos.Controls.Add(this.comboBox1Cliente);
            this.panelDatos.Controls.Add(this.labelCliente);
            this.panelDatos.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelDatos.Location = new System.Drawing.Point(18, 50);
            this.panelDatos.Name = "panelDatos";
            this.panelDatos.Size = new System.Drawing.Size(455, 128);
            this.panelDatos.TabIndex = 1;
            // 
            // labelCliente
            // 
            this.labelCliente.AutoSize = true;
            this.labelCliente.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.labelCliente.ForeColor = System.Drawing.Color.FromArgb(74, 44, 42);
            this.labelCliente.Location = new System.Drawing.Point(14, 10);
            this.labelCliente.Name = "labelCliente";
            this.labelCliente.Size = new System.Drawing.Size(45, 15);
            this.labelCliente.TabIndex = 0;
            this.labelCliente.Text = "Cliente";
            // 
            // comboBox1Cliente
            // 
            this.comboBox1Cliente.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox1Cliente.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1Cliente.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.comboBox1Cliente.FormattingEnabled = true;
            this.comboBox1Cliente.Location = new System.Drawing.Point(14, 29);
            this.comboBox1Cliente.Name = "comboBox1Cliente";
            this.comboBox1Cliente.Size = new System.Drawing.Size(427, 25);
            this.comboBox1Cliente.TabIndex = 1;
            // 
            // labelEntrega
            // 
            this.labelEntrega.AutoSize = true;
            this.labelEntrega.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.labelEntrega.ForeColor = System.Drawing.Color.FromArgb(74, 44, 42);
            this.labelEntrega.Location = new System.Drawing.Point(14, 66);
            this.labelEntrega.Name = "labelEntrega";
            this.labelEntrega.Size = new System.Drawing.Size(99, 15);
            this.labelEntrega.TabIndex = 2;
            this.labelEntrega.Text = "Fecha de entrega";
            // 
            // dateEntrega
            // 
            this.dateEntrega.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.dateEntrega.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateEntrega.Location = new System.Drawing.Point(14, 86);
            this.dateEntrega.Name = "dateEntrega";
            this.dateEntrega.ShowCheckBox = true;
            this.dateEntrega.Size = new System.Drawing.Size(185, 24);
            this.dateEntrega.TabIndex = 3;
            // 
            // labelEstado
            // 
            this.labelEstado.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelEstado.AutoSize = true;
            this.labelEstado.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.labelEstado.ForeColor = System.Drawing.Color.FromArgb(74, 44, 42);
            this.labelEstado.Location = new System.Drawing.Point(226, 66);
            this.labelEstado.Name = "labelEstado";
            this.labelEstado.Size = new System.Drawing.Size(45, 15);
            this.labelEstado.TabIndex = 4;
            this.labelEstado.Text = "Estado";
            this.labelEstado.Click += new System.EventHandler(this.labelEstado_Click);
            // 
            // comboBoxEstado
            // 
            this.comboBoxEstado.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxEstado.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxEstado.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.comboBoxEstado.FormattingEnabled = true;
            this.comboBoxEstado.Location = new System.Drawing.Point(226, 86);
            this.comboBoxEstado.Name = "comboBoxEstado";
            this.comboBoxEstado.Size = new System.Drawing.Size(215, 25);
            this.comboBoxEstado.TabIndex = 5;
            // 
            // panelAgregarProd
            // 
            this.panelAgregarProd.BackColor = System.Drawing.Color.White;
            this.panelAgregarProd.Controls.Add(this.buttonAgregar);
            this.panelAgregarProd.Controls.Add(this.numericUpDownCantidad);
            this.panelAgregarProd.Controls.Add(this.label1Cantidad);
            this.panelAgregarProd.Controls.Add(this.comboBoxProducto);
            this.panelAgregarProd.Controls.Add(this.labelProducto);
            this.panelAgregarProd.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelAgregarProd.Location = new System.Drawing.Point(18, 178);
            this.panelAgregarProd.Name = "panelAgregarProd";
            this.panelAgregarProd.Size = new System.Drawing.Size(455, 62);
            this.panelAgregarProd.TabIndex = 2;
            // 
            // labelProducto
            // 
            this.labelProducto.AutoSize = true;
            this.labelProducto.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.labelProducto.ForeColor = System.Drawing.Color.FromArgb(132, 87, 82);
            this.labelProducto.Location = new System.Drawing.Point(0, 10);
            this.labelProducto.Name = "labelProducto";
            this.labelProducto.Size = new System.Drawing.Size(57, 15);
            this.labelProducto.TabIndex = 0;
            this.labelProducto.Text = "Producto";
            // 
            // comboBoxProducto
            // 
            this.comboBoxProducto.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxProducto.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxProducto.DropDownWidth = 240;
            this.comboBoxProducto.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.comboBoxProducto.FormattingEnabled = true;
            this.comboBoxProducto.Location = new System.Drawing.Point(0, 30);
            this.comboBoxProducto.Name = "comboBoxProducto";
            this.comboBoxProducto.Size = new System.Drawing.Size(272, 25);
            this.comboBoxProducto.TabIndex = 1;
            // 
            // label1Cantidad
            // 
            this.label1Cantidad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1Cantidad.AutoSize = true;
            this.label1Cantidad.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold);
            this.label1Cantidad.ForeColor = System.Drawing.Color.FromArgb(132, 87, 82);
            this.label1Cantidad.Location = new System.Drawing.Point(282, 10);
            this.label1Cantidad.Name = "label1Cantidad";
            this.label1Cantidad.Size = new System.Drawing.Size(57, 15);
            this.label1Cantidad.TabIndex = 2;
            this.label1Cantidad.Text = "Cantidad";
            // 
            // numericUpDownCantidad
            // 
            this.numericUpDownCantidad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDownCantidad.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.numericUpDownCantidad.Location = new System.Drawing.Point(282, 30);
            this.numericUpDownCantidad.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownCantidad.Name = "numericUpDownCantidad";
            this.numericUpDownCantidad.Size = new System.Drawing.Size(62, 24);
            this.numericUpDownCantidad.TabIndex = 3;
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
            this.buttonAgregar.Location = new System.Drawing.Point(354, 27);
            this.buttonAgregar.Name = "buttonAgregar";
            this.buttonAgregar.Size = new System.Drawing.Size(101, 30);
            this.buttonAgregar.TabIndex = 4;
            this.buttonAgregar.Text = "+ Agregar";
            this.buttonAgregar.UseVisualStyleBackColor = false;
            this.buttonAgregar.Click += new System.EventHandler(this.buttonAgregar_Click);
            // 
            // panelDetalleBottom
            // 
            this.panelDetalleBottom.BackColor = System.Drawing.Color.White;
            this.panelDetalleBottom.Controls.Add(this.panelBotonesDetalle);
            this.panelDetalleBottom.Controls.Add(this.panelTotal);
            this.panelDetalleBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelDetalleBottom.Location = new System.Drawing.Point(18, 344);
            this.panelDetalleBottom.Name = "panelDetalleBottom";
            this.panelDetalleBottom.Size = new System.Drawing.Size(455, 114);
            this.panelDetalleBottom.TabIndex = 3;
            // 
            // panelTotal
            // 
            this.panelTotal.BackColor = System.Drawing.Color.FromArgb(123, 72, 76);
            this.panelTotal.Controls.Add(this.labelTotal);
            this.panelTotal.Controls.Add(this.label1);
            this.panelTotal.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTotal.Location = new System.Drawing.Point(0, 0);
            this.panelTotal.Name = "panelTotal";
            this.panelTotal.Size = new System.Drawing.Size(455, 56);
            this.panelTotal.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(248, 232, 232);
            this.label1.Location = new System.Drawing.Point(16, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(123, 19);
            this.label1.TabIndex = 0;
            this.label1.Text = "Total del encargo";
            // 
            // labelTotal
            // 
            this.labelTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelTotal.Font = new System.Drawing.Font("Segoe UI", 17F, System.Drawing.FontStyle.Bold);
            this.labelTotal.ForeColor = System.Drawing.Color.White;
            this.labelTotal.Location = new System.Drawing.Point(210, 11);
            this.labelTotal.Name = "labelTotal";
            this.labelTotal.Size = new System.Drawing.Size(229, 34);
            this.labelTotal.TabIndex = 1;
            this.labelTotal.Text = "C$ 0.00";
            this.labelTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panelBotonesDetalle
            // 
            this.panelBotonesDetalle.BackColor = System.Drawing.Color.White;
            this.panelBotonesDetalle.ColumnCount = 3;
            this.panelBotonesDetalle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 36F));
            this.panelBotonesDetalle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32F));
            this.panelBotonesDetalle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32F));
            this.panelBotonesDetalle.Controls.Add(this.buttonGuardar, 0, 0);
            this.panelBotonesDetalle.Controls.Add(this.buttonImprimir, 1, 0);
            this.panelBotonesDetalle.Controls.Add(this.buttonEliminarPedido, 2, 0);
            this.panelBotonesDetalle.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBotonesDetalle.Location = new System.Drawing.Point(0, 68);
            this.panelBotonesDetalle.Name = "panelBotonesDetalle";
            this.panelBotonesDetalle.RowCount = 1;
            this.panelBotonesDetalle.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.panelBotonesDetalle.Size = new System.Drawing.Size(455, 46);
            this.panelBotonesDetalle.TabIndex = 1;
            // 
            // buttonGuardar
            // 
            this.buttonGuardar.BackColor = System.Drawing.Color.FromArgb(46, 125, 91);
            this.buttonGuardar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonGuardar.FlatAppearance.BorderSize = 0;
            this.buttonGuardar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonGuardar.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.buttonGuardar.ForeColor = System.Drawing.Color.White;
            this.buttonGuardar.Location = new System.Drawing.Point(0, 0);
            this.buttonGuardar.Margin = new System.Windows.Forms.Padding(0, 0, 6, 0);
            this.buttonGuardar.Name = "buttonGuardar";
            this.buttonGuardar.Size = new System.Drawing.Size(157, 46);
            this.buttonGuardar.TabIndex = 0;
            this.buttonGuardar.Text = "Guardar";
            this.buttonGuardar.UseVisualStyleBackColor = false;
            this.buttonGuardar.Click += new System.EventHandler(this.buttonGuardar_Click);
            // 
            // buttonImprimir
            // 
            this.buttonImprimir.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonImprimir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonImprimir.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.buttonImprimir.Location = new System.Drawing.Point(168, 0);
            this.buttonImprimir.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.buttonImprimir.Name = "buttonImprimir";
            this.buttonImprimir.Size = new System.Drawing.Size(135, 46);
            this.buttonImprimir.TabIndex = 1;
            this.buttonImprimir.Text = "Recibo";
            this.buttonImprimir.UseVisualStyleBackColor = true;
            this.buttonImprimir.Click += new System.EventHandler(this.buttonImprimir_Click);
            // 
            // buttonEliminarPedido
            // 
            this.buttonEliminarPedido.BackColor = System.Drawing.Color.White;
            this.buttonEliminarPedido.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonEliminarPedido.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonEliminarPedido.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.buttonEliminarPedido.ForeColor = System.Drawing.Color.FromArgb(184, 59, 59);
            this.buttonEliminarPedido.Location = new System.Drawing.Point(314, 0);
            this.buttonEliminarPedido.Margin = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.buttonEliminarPedido.Name = "buttonEliminarPedido";
            this.buttonEliminarPedido.Size = new System.Drawing.Size(141, 46);
            this.buttonEliminarPedido.TabIndex = 2;
            this.buttonEliminarPedido.Text = "Quitar";
            this.buttonEliminarPedido.UseVisualStyleBackColor = false;
            this.buttonEliminarPedido.Click += new System.EventHandler(this.button1_Click);
            // 
            // panelDetalleTabla
            // 
            this.panelDetalleTabla.BackColor = System.Drawing.Color.White;
            this.panelDetalleTabla.Controls.Add(this.dataGridViewDetalle);
            this.panelDetalleTabla.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDetalleTabla.Location = new System.Drawing.Point(18, 240);
            this.panelDetalleTabla.Name = "panelDetalleTabla";
            this.panelDetalleTabla.Padding = new System.Windows.Forms.Padding(0, 8, 0, 8);
            this.panelDetalleTabla.Size = new System.Drawing.Size(455, 104);
            this.panelDetalleTabla.TabIndex = 4;
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
            this.dataGridViewDetalle.Location = new System.Drawing.Point(0, 8);
            this.dataGridViewDetalle.MultiSelect = false;
            this.dataGridViewDetalle.Name = "dataGridViewDetalle";
            this.dataGridViewDetalle.ReadOnly = true;
            this.dataGridViewDetalle.RowHeadersVisible = false;
            this.dataGridViewDetalle.RowHeadersWidth = 51;
            this.dataGridViewDetalle.RowTemplate.Height = 30;
            this.dataGridViewDetalle.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewDetalle.Size = new System.Drawing.Size(455, 88);
            this.dataGridViewDetalle.TabIndex = 0;
            // 
            // ColumnProducto
            // 
            this.ColumnProducto.FillWeight = 40F;
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
            this.colDPrecio.FillWeight = 22F;
            this.colDPrecio.HeaderText = "Precio";
            this.colDPrecio.MinimumWidth = 6;
            this.colDPrecio.Name = "colDPrecio";
            this.colDPrecio.ReadOnly = true;
            // 
            // ColumnSubtotal
            // 
            this.ColumnSubtotal.FillWeight = 22F;
            this.ColumnSubtotal.HeaderText = "Subtotal";
            this.ColumnSubtotal.MinimumWidth = 6;
            this.ColumnSubtotal.Name = "ColumnSubtotal";
            this.ColumnSubtotal.ReadOnly = true;
            // 
            // Formpedidos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(253, 248, 246);
            this.ClientSize = new System.Drawing.Size(1030, 690);
            this.Controls.Add(this.layoutPrincipal);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Formpedidos";
            this.Padding = new System.Windows.Forms.Padding(24, 18, 24, 22);
            this.Load += new System.EventHandler(this.Formpedidos_Load_1);
            this.panelBuscar.ResumeLayout(false);
            this.panelBuscar.PerformLayout();
            this.panelLista.ResumeLayout(false);
            this.panelListaHeader.ResumeLayout(false);
            this.panelListaHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPedidos)).EndInit();
            this.panelDetalle.ResumeLayout(false);
            this.panelDetalleTabla.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDetalle)).EndInit();
            this.panelDetalleBottom.ResumeLayout(false);
            this.panelBotonesDetalle.ResumeLayout(false);
            this.panelTotal.ResumeLayout(false);
            this.panelTotal.PerformLayout();
            this.panelAgregarProd.ResumeLayout(false);
            this.panelAgregarProd.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCantidad)).EndInit();
            this.panelDatos.ResumeLayout(false);
            this.panelDatos.PerformLayout();
            this.panelDetalleTop.ResumeLayout(false);
            this.panelDetalleTop.PerformLayout();
            this.layoutPrincipal.ResumeLayout(false);
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.layoutCuerpo.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelTitulo;
        private System.Windows.Forms.Label labelfecha;
        private System.Windows.Forms.Panel panelBuscar;
        private System.Windows.Forms.TextBox textBuscar;
        private System.Windows.Forms.Button buttonNuevo;
        private System.Windows.Forms.TableLayoutPanel tableTarjeta;
        private System.Windows.Forms.Panel panelLista;
        private System.Windows.Forms.Panel panelListaHeader;
        private System.Windows.Forms.Label labelListaTitulo;
        private System.Windows.Forms.Button buttonTodos;
        private System.Windows.Forms.Button buttonPendiente;
        private System.Windows.Forms.Button buttonHoy;
        private System.Windows.Forms.DataGridView dataGridViewPedidos;
        private System.Windows.Forms.DataGridViewTextBoxColumn colId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCliente;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEntrega;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEstado;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTotal;
        private System.Windows.Forms.Panel panelDetalle;
        private System.Windows.Forms.Panel panelDetalleTop;
        private System.Windows.Forms.Label labelDetalleTitulo;
        private System.Windows.Forms.Label labelEstadoBadge;
        private System.Windows.Forms.Panel panelDatos;
        private System.Windows.Forms.Label labelEntrega;
        private System.Windows.Forms.ComboBox comboBox1Cliente;
        private System.Windows.Forms.Label labelCliente;
        private System.Windows.Forms.Label labelEstado;
        private System.Windows.Forms.DateTimePicker dateEntrega;
        private System.Windows.Forms.ComboBox comboBoxEstado;
        private System.Windows.Forms.Panel panelAgregarProd;
        private System.Windows.Forms.Label label1Cantidad;
        private System.Windows.Forms.ComboBox comboBoxProducto;
        private System.Windows.Forms.Label labelProducto;
        private System.Windows.Forms.Button buttonAgregar;
        private System.Windows.Forms.NumericUpDown numericUpDownCantidad;
        private System.Windows.Forms.Panel panelDetalleTabla;
        private System.Windows.Forms.DataGridView dataGridViewDetalle;
        private System.Windows.Forms.Panel panelDetalleBottom;
        private System.Windows.Forms.TableLayoutPanel panelBotonesDetalle;
        private System.Windows.Forms.Panel panelTotal;
        private System.Windows.Forms.Label labelTotal;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonGuardar;
        private System.Windows.Forms.Button buttonEliminarPedido;
        private System.Windows.Forms.Button buttonImprimir;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnProducto;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDCantidad;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDPrecio;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnSubtotal;
        private System.Windows.Forms.TableLayoutPanel layoutPrincipal;
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.TableLayoutPanel layoutCuerpo;
    }
}
