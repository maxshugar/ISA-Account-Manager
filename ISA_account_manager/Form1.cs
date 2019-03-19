using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data.OleDb;

namespace ISA_account_manager
{
    public partial class outlaw_hess_frm : Form
    {
        ISA_db db = new ISA_db();

        public outlaw_hess_frm()
        {
            InitializeComponent();
            db.Connect();
        }

        public class ISA_db
        {

            //public OleDbCommand myCmd = new OleDbCommand();
            public OleDbConnection myConn = new OleDbConnection(dbconnect);

            public static string dbconnect = @"Provider=Microsoft.ACE.OLEDB.12.0;"
            + "Data Source=" + Application.StartupPath + @"\database.accdb;";

            public int Connect()
            {
                try
                {
                    myConn.Open();
                }
                catch (Exception e)
                {
                    myConn.Close();
                    MessageBox.Show(e.Message.ToString());
                }
                return 0;
            }

            public int Execute(String command)
            {
                OleDbCommand cmd = new OleDbCommand(command, myConn);
                cmd.ExecuteNonQuery();
                return 0;
            }
        }

        /* Creates an input form with the specificed number of inputs. */
        private void generate_input_form(string title_text, object[,] input, string command_text)
        {
            int start_x = 10;
            int start_y = 10;

            Label title = new Label
            {
                Text = title_text,
                Location = new System.Drawing.Point(start_x, start_y),
                Font = new Font("Arial", 20, FontStyle.Bold),
                AutoSize = true
            };

            main_panel.Controls.Add(title);
            title.BringToFront();
            start_y = 50;

            for (int i = 0; i < input.GetLength(0); i++)
            {
                if (i % 4 == 0 && i != 0)
                {
                    start_x += 150;
                    start_y = 50;
                }
                Label lbl = new Label
                {
                    Text = (string)input[i,0],
                    Location = new System.Drawing.Point(start_x, start_y)
                };
                main_panel.Controls.Add(lbl);
                start_y += 20;
                /* Type casting. */
                if (input[i, 1] is TextBox)
                {
                    TextBox element = (TextBox)input[i, 1];
                    element.Location = new System.Drawing.Point(start_x, start_y);
                    element.Width = 130;
                    main_panel.Controls.Add(element);
                    element.BringToFront();
                }
                if (input[i, 1] is DateTimePicker)
                {
                    DateTimePicker element = (DateTimePicker)input[i, 1];
                    element.Location = new System.Drawing.Point(start_x, start_y);
                    element.Width = 130;
                    main_panel.Controls.Add(element);
                    element.BringToFront();
                }
                if (input[i, 1] is CheckBox)
                {
                    CheckBox element = (CheckBox)input[i, 1];
                    element.Location = new System.Drawing.Point(start_x, start_y);
                    element.Width = 130;
                    main_panel.Controls.Add(element);
                    element.BringToFront();
                }
                if (input[i, 1] is ComboBox)
                {
                    ComboBox element = (ComboBox)input[i, 1];
                    element.Location = new System.Drawing.Point(start_x, start_y);
                    element.Width = 250;
                    main_panel.Controls.Add(element);
                    element.BringToFront();
                }
                start_y += 25;
            }
            start_y += 15;
            /* Save button. */
            Button save_btn = new Button()
            {
                Width = 130,
                Location = new System.Drawing.Point(start_x, start_y),
            Text = "Save"
            };
            main_panel.Controls.Add(save_btn);
            save_btn.BringToFront();
            /* Click event listener. */
            save_btn.Click += (s, e) => {

                OleDbCommand command = new OleDbCommand
                {
                    CommandText = command_text,
                    Connection = db.myConn
                };
                bool is_empty = false;

                for (int i = 0; i < input.GetLength(0); i++)
                {
                    if (input[i, 1] is TextBox)
                    {
                        TextBox element = (TextBox)input[i, 1];
                        if (element.Text == "")
                        {
                            is_empty = true;
                            break;
                        }
                        command.Parameters.Add("?", OleDbType.VarChar, 50).Value = element.Text;
                    }
                    if (input[i, 1] is DateTimePicker)
                    {
                        DateTimePicker element = (DateTimePicker)input[i, 1];
                        command.Parameters.Add("?", OleDbType.Date).Value = Convert.ToDateTime(element.Text);
                        if (element.Text == "")
                        {
                            is_empty = true;
                            break;
                        }

                    }
                    if (input[i, 1] is CheckBox)
                    {
                        CheckBox element = (CheckBox)input[i, 1];
                        command.Parameters.Add("?", OleDbType.Boolean).Value = element.Checked;
                        
                    }

                    if (input[i, 1] is ComboBox)
                    {
                        /* Get id. */
                        ComboBox element = (ComboBox)input[i, 1];
                        String id = element.Text.Split('-')[0].Split(' ')[0];
                        command.Parameters.Add("?", OleDbType.VarChar, 50).Value = id;

                    }


                }

                if (is_empty)
                    MessageBox.Show("error: please fill out all fields.");
                
                else
                {
                    /* Execute Query. */
                    int ret = command.ExecuteNonQuery();
                    if (ret == 1)
                        MessageBox.Show("Save successful");
                    else
                        MessageBox.Show("Save unsuccessful");
                }
            };
        }

        private void new_customer_btn_Click(object sender, EventArgs e)
        {

            object[,] input = new object[7,2]
            {
                {"Title", new TextBox(){ Text = "" } },
                {"First Name", new TextBox(){ Text = "" } },
                {"Last Name", new TextBox(){ Text = "" } },
                {"Date of Birth", new DateTimePicker() },
                {"National Insurance", new TextBox(){ Text = "" } },
                {"Email", new TextBox(){ Text = "" } },
                {"Password", new TextBox(){ Text = "" } }
            };
            main_panel.Controls.Clear();
            /* Generate Command */
            String command_text = "INSERT INTO customers (title, firstname, lastname, dob, natins, email, pswd) VALUES ( ?, ?, ?, ?, ?, ?, ? )";
           
            /* Generate form. */
            generate_input_form("Customers", input, command_text);

        }

        private void view_customers_btn_Click(object sender, EventArgs e)
        {

            main_panel.Controls.Clear();

            int start_x = 10;
            int start_y = 10;

            Label title = new Label
            {
                Text = "Customers",
                Location = new System.Drawing.Point(start_x, start_y),
                Font = new Font("Arial", 20, FontStyle.Bold),
                AutoSize = true
            };

            main_panel.Controls.Add(title);
            start_y += 40;

            /* Get customers. */
            System.Data.DataSet ds = new System.Data.DataSet();
            OleDbCommand command = new OleDbCommand()
            {
                CommandText = "SELECT * FROM customers",
                Connection = db.myConn
            };

            OleDbDataAdapter adapter = new OleDbDataAdapter(command);
            adapter.Fill(ds, "customers");

            
            DataGridView data_grid = new DataGridView()
            {
                DataSource = ds.Tables["customers"],
                Width = 600,
                Height = 350,
                Location = new System.Drawing.Point(start_x, start_y),
            };
            main_panel.Controls.Add(data_grid);
        }

        /* Prevents characters being entered into a text box. */
        private void no_char_func(object s, KeyPressEventArgs e)
        {
            /* Determine if dot has been used. */
            TextBox local = (TextBox)s;
            if(local.Text.IndexOf('.') != -1)
            {
                if(e.KeyChar == '.')
                    e.Handled = true;
            }

            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != '.')
                e.Handled = true;
        }

        private void new_product_btn_Click(object sender, EventArgs e)
        {
            /* Prevent characters being typed in interest rate text box. */
            TextBox no_char = new TextBox();
            no_char.KeyPress += new KeyPressEventHandler(no_char_func);
            
            object[,] input = new object[4, 2]
            {
                {"name", new TextBox(){ Text = "" } },
                {"status", new TextBox(){ Text = "" } },
                {"transactions in", new CheckBox() },
                {"interest rate", no_char }
            };

            main_panel.Controls.Clear();
            /* Generate Command */
            String command_text = "INSERT INTO products (prod_name, status, transin, inrate) VALUES (?, ?, ?, ?)";

            /* Generate form. */
            generate_input_form("Products", input, command_text);
        }

        private void new_account_btn_Click(object sender, EventArgs e)
        {
            /* Prevent characters being typed in interest rate text box. */
            TextBox no_char = new TextBox();
            no_char.KeyPress += new KeyPressEventHandler(no_char_func);

            /* Get customers. */
            System.Data.DataSet ds = new System.Data.DataSet();
            OleDbCommand command = new OleDbCommand()
            {
                CommandText = "SELECT custid, firstname, lastname, email FROM customers",
                Connection = db.myConn
            };

            OleDbDataAdapter adapter = new OleDbDataAdapter(command);
            adapter.Fill(ds, "customers");

            ComboBox customer_cb = new ComboBox()
            {
                DropDownStyle = ComboBoxStyle.DropDown,
                AutoCompleteMode = AutoCompleteMode.SuggestAppend,
                AutoCompleteSource = AutoCompleteSource.ListItems
            };

            foreach (System.Data.DataRow dr in ds.Tables["customers"].Rows)
                customer_cb.Items.Add(dr["custid"] + " - " + dr["firstname"] + " " + dr["lastname"] + " - " + dr["email"]);

            /* Get products. */
            command = new OleDbCommand()
            {
                CommandText = "SELECT prodid, prod_name, inrate FROM products",
                Connection = db.myConn
            };

            adapter = new OleDbDataAdapter(command);
            adapter.Fill(ds, "products");

            ComboBox products_cb = new ComboBox()
            {
                DropDownStyle = ComboBoxStyle.DropDown,
                AutoCompleteMode = AutoCompleteMode.SuggestAppend,
                AutoCompleteSource = AutoCompleteSource.ListItems
            };

            foreach (System.Data.DataRow dr in ds.Tables["products"].Rows)
                products_cb.Items.Add(dr["prodid"] + " - " + dr["prod_name"] + " - " + dr["inrate"] + "%");

            /* Get products. */
            object[,] input = new object[3, 2]
            {
                {"customer", customer_cb },
                {"product", products_cb, },
                {"balance £", no_char }
            };

            main_panel.Controls.Clear();
            /* Generate Command */
            DateTime thisDay = DateTime.Today;
            String command_text = "INSERT INTO accounts (custid, prodid, balance, accrued, active, opnd) VALUES (?, ?, ?, '0', 'false', '" + thisDay.ToString("d") + "')";

            /* Generate form. */
            generate_input_form("Accounts", input, command_text);
        }

        private void new_transaction_btn_Click(object sender, EventArgs e)
        {

            /* Prevent characters being typed in amount text box and limit to one decimal point. */
            TextBox no_char = new TextBox();
            no_char.KeyPress += new KeyPressEventHandler(no_char_func);

            /* Get customers. */
            System.Data.DataSet ds = new System.Data.DataSet();
            OleDbCommand command = new OleDbCommand()
            {
                CommandText = "SELECT accid, firstname, lastname, email FROM accounts INNER JOIN customers ON accounts.custid = customers.custid;",
                Connection = db.myConn
            };

            OleDbDataAdapter adapter = new OleDbDataAdapter(command);
            adapter.Fill(ds, "accounts");

            ComboBox acc_cb = new ComboBox()
            {
                DropDownStyle = ComboBoxStyle.DropDown,
                AutoCompleteMode = AutoCompleteMode.SuggestAppend,
                AutoCompleteSource = AutoCompleteSource.ListItems
            };

            foreach (System.Data.DataRow dr in ds.Tables["accounts"].Rows)
                acc_cb.Items.Add(dr["accid"] + " - " + dr["firstname"] + " " + dr["lastname"] + " - " + dr["email"]);

            ComboBox actn_cb = new ComboBox()
            {
                DropDownStyle = ComboBoxStyle.DropDown,
                AutoCompleteMode = AutoCompleteMode.SuggestAppend,
                AutoCompleteSource = AutoCompleteSource.ListItems
            };

            actn_cb.Items.Add("IN");
            actn_cb.Items.Add("OUT");
            actn_cb.Items.Add("DEPOSIT");
            actn_cb.Items.Add("WITHDRAWAL");

            /* Get products. */
            object[,] input = new object[3, 2]
            {
                {"account", acc_cb },
                {"action", actn_cb, },
                {"amount £", no_char }
            };

            main_panel.Controls.Clear();
            /* Generate Command */
            DateTime thisDay = DateTime.Today;
            String command_text = "INSERT INTO tranx (accid, [action], amnt, [event]) VALUES (?, ?, ?, '" + thisDay.ToString("d") + "')";

            /* Generate form. */
            generate_input_form("Transaction", input, command_text);


        }

        private void view_products_btn_Click(object sender, EventArgs e)
        {

            main_panel.Controls.Clear();

            int start_x = 10;
            int start_y = 10;

            Label title = new Label
            {
                Text = "Products",
                Location = new System.Drawing.Point(start_x, start_y),
                Font = new Font("Arial", 20, FontStyle.Bold),
                AutoSize = true
            };

            main_panel.Controls.Add(title);
            start_y += 40;

            /* Get customers. */
            System.Data.DataSet ds = new System.Data.DataSet();
            OleDbCommand command = new OleDbCommand()
            {
                CommandText = "SELECT * FROM products",
                Connection = db.myConn
            };

            OleDbDataAdapter adapter = new OleDbDataAdapter(command);
            adapter.Fill(ds, "products");


            DataGridView data_grid = new DataGridView()
            {
                DataSource = ds.Tables["products"],
                Width = 600,
                Height = 350,
                Location = new System.Drawing.Point(start_x, start_y),
            };
            main_panel.Controls.Add(data_grid);
        }

        private void view_accounts_btn_Click(object sender, EventArgs e)
        {
            main_panel.Controls.Clear();

            int start_x = 10;
            int start_y = 10;

            Label title = new Label
            {
                Text = "Accounts",
                Location = new System.Drawing.Point(start_x, start_y),
                Font = new Font("Arial", 20, FontStyle.Bold),
                AutoSize = true
            };

            main_panel.Controls.Add(title);
            start_y += 40;

            /* Get customers. */
            System.Data.DataSet ds = new System.Data.DataSet();
            OleDbCommand command = new OleDbCommand()
            {
                CommandText = "SELECT * FROM accounts",
                Connection = db.myConn
            };

            OleDbDataAdapter adapter = new OleDbDataAdapter(command);
            adapter.Fill(ds, "accounts");


            DataGridView data_grid = new DataGridView()
            {
                DataSource = ds.Tables["accounts"],
                Width = 600,
                Height = 350,
                Location = new System.Drawing.Point(start_x, start_y),
            };
            main_panel.Controls.Add(data_grid);
        }

        private void view_transactions_btn_Click(object sender, EventArgs e)
        {
            main_panel.Controls.Clear();

            int start_x = 10;
            int start_y = 10;

            Label title = new Label
            {
                Text = "Transactions",
                Location = new System.Drawing.Point(start_x, start_y),
                Font = new Font("Arial", 20, FontStyle.Bold),
                AutoSize = true
            };

            main_panel.Controls.Add(title);
            start_y += 40;

            /* Get customers. */
            System.Data.DataSet ds = new System.Data.DataSet();
            OleDbCommand command = new OleDbCommand()
            {
                CommandText = "SELECT * FROM tranx",
                Connection = db.myConn
            };

            OleDbDataAdapter adapter = new OleDbDataAdapter(command);
            adapter.Fill(ds, "tranx");


            DataGridView data_grid = new DataGridView()
            {
                DataSource = ds.Tables["tranx"],
                Width = 600,
                Height = 350,
                Location = new System.Drawing.Point(start_x, start_y),
            };
            main_panel.Controls.Add(data_grid);
        }
    }
}
