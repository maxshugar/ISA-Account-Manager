using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data.OleDb;

namespace ISA_account_manager
{
    public partial class Outlaw_hess_frm : Form
    {
        ISA_db db = new ISA_db();

        public Outlaw_hess_frm()
        {
            InitializeComponent();
            this.CenterToScreen();
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
        private void Generate_input_form(string title_text, object[,] input, string command_text)
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

        private void New_customer_btn_Click(object sender, EventArgs e)
        {
            this.CenterToScreen();
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
            String command_text = "INSERT INTO customers (title, firstname, lastname, dob, natins, email, pswd, allowance) VALUES ( ?, ?, ?, ?, ?, ?, ?, '15240' )";
           
            /* Generate form. */
            Generate_input_form("Customers", input, command_text);

        }

        private void View_customers_btn_Click(object sender, EventArgs e)
        {

            main_panel.Controls.Clear();
            this.CenterToScreen();
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

            data_grid.CellDoubleClick += (s, _e) =>
            {
                int index = data_grid.SelectedCells[0].RowIndex;

                string custid = data_grid[0, index].Value.ToString();

                command = new OleDbCommand()
                {
                    CommandText = "SELECT * FROM accounts WHERE custid=" + custid,
                    Connection = db.myConn
                };

                ds = new System.Data.DataSet();
               
                adapter = new OleDbDataAdapter(command);
                adapter.Fill(ds, "accounts");

                View_accounts_btn_Click_datatable(sender, e, ds);

            };
        }

        private void View_accounts_btn_Click_datatable(object sender, EventArgs e, System.Data.DataSet ds)
        {
            main_panel.Controls.Clear();
            this.CenterToScreen();
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

            DataGridView data_grid = new DataGridView()
            {
                DataSource = ds.Tables["accounts"],
                Width = 600,
                Height = 350,
                Location = new System.Drawing.Point(start_x, start_y),
            };
            main_panel.Controls.Add(data_grid);


            data_grid.CellDoubleClick += (s, _e) =>
            {
                int index = data_grid.SelectedCells[0].RowIndex;

                string accid = data_grid[0, index].Value.ToString();

                OleDbCommand command = new OleDbCommand()
                {
                    CommandText = "SELECT * FROM tranx WHERE accid=" + accid,
                    Connection = db.myConn
                };

                ds = new System.Data.DataSet();

                OleDbDataAdapter adapter = new OleDbDataAdapter(command);
                adapter.Fill(ds, "tranx");

                View_transactions_btn_Click_datatable(sender, e, ds);

            };

        }

        private void View_accounts_btn_Click(object sender, EventArgs e)
        {

            main_panel.Controls.Clear();
            this.CenterToScreen();
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

            data_grid.CellDoubleClick += (s, _e) =>
            {
                int index = data_grid.SelectedCells[0].RowIndex;

                string accid = data_grid[0, index].Value.ToString();

                command = new OleDbCommand()
                {
                    CommandText = "SELECT * FROM tranx WHERE accid=" + accid,
                    Connection = db.myConn
                };

                ds = new System.Data.DataSet();

                adapter = new OleDbDataAdapter(command);
                adapter.Fill(ds, "tranx");

                View_transactions_btn_Click_datatable(sender, e, ds);

            };

            Button toggle_btn = new Button()
            {
                Location = new System.Drawing.Point(650, start_y),
                Text = "Toggle Status",
                Width = 100
            };
            main_panel.Controls.Add(toggle_btn);

            /* Click event listener. */
            toggle_btn.Click += (s, _e) =>
            {

                if (data_grid.SelectedRows.Count != 1 || data_grid.SelectedCells[0].Value == null)
                {
                    MessageBox.Show("Please select one row.");

                }
                else
                {

                    string status = data_grid.SelectedCells[7].Value.ToString();
                    string id = data_grid.SelectedCells[0].Value.ToString();
                    if (status == "open")
                    {
                        status = "closed";
                    }
                    else if (status == "closed")
                    {
                        status = "open";
                    }
                    else
                    {
                        status = "open";
                    }

                    command = new OleDbCommand()
                    {
                        CommandText = "UPDATE accounts SET status='" + status + "' WHERE accid=" + id,
                        Connection = db.myConn
                    };
                    int ret = command.ExecuteNonQuery();

                     command.ExecuteNonQuery();
                     view_accounts_btn.PerformClick();

                }

            };

            /* Calculate accrued interest rate for all accounts. */
            Button accured_interest_btn = new Button()
            {
                Location = new System.Drawing.Point(650, start_y + 40),
                Text = "Calculate all accounts accured interest",
                Width = 100,
                Height = 60
            };
            main_panel.Controls.Add(accured_interest_btn);

            accured_interest_btn.Click += (_s, _e) => {

                for(int i = 0; i < ds.Tables["accounts"].Rows.Count; i++)
                {

                    /* Get product interest rate. */
                    string prodid = ds.Tables["accounts"].Rows[i][2].ToString();

                    command = new OleDbCommand()
                    {
                        CommandText = "SELECT inrate FROM products WHERE prodid=" + prodid,
                        Connection = db.myConn
                    };

                    System.Data.DataSet prod_ds = new System.Data.DataSet();
                    adapter = new OleDbDataAdapter(command);
                    adapter.Fill(prod_ds, "products");

                    /* Calculate accrued interest and write to db. */
                    double inrate = Convert.ToDouble(prod_ds.Tables["products"].Rows[0]["inrate"].ToString()) / 100;
                    double accrued_interest = Convert.ToDouble(ds.Tables["accounts"].Rows[i][4].ToString());
                    double balance = Convert.ToDouble(ds.Tables["accounts"].Rows[i][3].ToString());
                    string accid = ds.Tables["accounts"].Rows[i][0].ToString();

                    accrued_interest = accrued_interest + (balance * (inrate / 365));

                    command = new OleDbCommand()
                    {
                        CommandText = "UPDATE accounts SET accrued='" + accrued_interest + "' WHERE accid=" + accid,
                        Connection = db.myConn
                    };

                    command.ExecuteNonQuery();
                    MessageBox.Show("Interest calculation successful");

                }
                view_accounts_btn.PerformClick();
            };

            /* Calculate accrued interest rate for all accounts. */
            Button end_of_year_tax_updates_btn = new Button()
            {
                Location = new System.Drawing.Point(650, start_y + 120),
                Text = "End of Year Tax Updates",
                Width = 100,
                Height = 60
            };
            main_panel.Controls.Add(end_of_year_tax_updates_btn);

            end_of_year_tax_updates_btn.Click += (_s, _e) => {

                /*Add accrued interest to balance and set accrued interest to 0 for each account. */
                command = new OleDbCommand()
                {
                    CommandText = "UPDATE accounts SET balance = balance + accrued, accrued = 0, active='false'",
                    Connection = db.myConn
                };

                command.ExecuteNonQuery();
 
                /* Reset all deposit allowances for all customers. */
                command = new OleDbCommand()
                {
                    CommandText = "UPDATE customers SET allowance = 15240",
                    Connection = db.myConn
                };

                command.ExecuteNonQuery();
                view_accounts_btn.PerformClick();

            };
        }

        /* Prevents characters being entered into a text box. */
        private void No_char_func(object s, KeyPressEventArgs e)
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

        private void New_product_btn_Click(object sender, EventArgs e)
        {
            this.CenterToScreen();

            /* Prevent characters being typed in interest rate text box. */
            TextBox no_char = new TextBox();
            no_char.KeyPress += new KeyPressEventHandler(No_char_func);
            
            object[,] input = new object[3, 2]
            {
                {"name", new TextBox(){ Text = "" } },
                {"transactions in", new CheckBox() },
                {"interest rate", no_char }
            };

            main_panel.Controls.Clear();
            /* Generate Command */
            String command_text = "INSERT INTO products (prod_name, status, transin, inrate) VALUES (?, 'open', ?, ?)";

            /* Generate form. */
            Generate_input_form("Products", input, command_text);
        }

        private void New_account_btn_Click(object sender, EventArgs e)
        {
            this.CenterToScreen();

            /* Prevent characters being typed in interest rate text box. */
            TextBox no_char = new TextBox();
            no_char.KeyPress += new KeyPressEventHandler(No_char_func);

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
            object[,] input = new object[2, 2]
            {
                {"customer", customer_cb },
                {"product", products_cb }
            };

            main_panel.Controls.Clear();
            /* Generate Command */
            DateTime thisDay = DateTime.Today;
            String command_text = "INSERT INTO accounts (custid, prodid, balance, accrued, active, opnd, status) VALUES (?, ?, '0', '0', 'false', '" + thisDay.ToString("d") + "', 'open')";

            /* Generate form. */
            Generate_input_form("Accounts", input, command_text);
        }

        private void New_transaction_btn_Click(object sender, EventArgs e)
        {
            main_panel.Controls.Clear();
            this.CenterToScreen();

            /* Prevent characters being typed in amount text box and limit to one decimal point. */
            TextBox no_char = new TextBox();
            no_char.KeyPress += new KeyPressEventHandler(No_char_func);

            /* Get customers. */
            System.Data.DataSet ds = new System.Data.DataSet();
            OleDbCommand command = new OleDbCommand()
            {
                CommandText = "SELECT accid, firstname, lastname, email, status FROM accounts INNER JOIN customers ON accounts.custid = customers.custid WHERE status = 'open';",
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

            actn_cb.Items.Add("DEPOSIT");
            actn_cb.Items.Add("WITHDRAWAL");

            /* Get products. */
            object[,] input = new object[3, 2]
            {
                {"account", acc_cb },
                {"action", actn_cb, },
                {"amount £", no_char }
            };
    
            /* Generate form. */
            int start_x = 10;
            int start_y = 10;

            Label title = new Label
            {
                Text = "New Transaction",
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
                    Text = (string)input[i, 0],
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
            save_btn.Click += (_s, _e) =>
            {
                double amount = Convert.ToDouble(no_char.Text);
                String accid = acc_cb.Text.Split('-')[0].Split(' ')[0];
                if (actn_cb.Text == "DEPOSIT")
                {
                    /* Get the customers deposit allowance. */

                    command = new OleDbCommand()
                    {
                        CommandText = "SELECT allowance FROM customers AS a INNER JOIN accounts AS b ON a.custid=b.custid WHERE accid=" + accid,
                        Connection = db.myConn
                    };

                    System.Data.DataSet cust_ds = new System.Data.DataSet();
                    adapter = new OleDbDataAdapter(command);
                    adapter.Fill(cust_ds, "customers");

                    double allowance = Convert.ToDouble(cust_ds.Tables["customers"].Rows[0]["allowance"].ToString());
                    double remaining_allowance = allowance - amount;
                    /* If the deposit allowance - amount inputted is less than zero,  error. */
                    if (remaining_allowance < 0)
                    {
                        MessageBox.Show("The amount inputted exceeds the deposit allowance for this financial year.");
                    }
                    /* Else - decrease the customer deposit allowance and increase the account balance. */
                    else
                    {

                        /* Update allowance. */
                        command = new OleDbCommand()
                        {
                            CommandText = "UPDATE ( SELECT allowance FROM customers AS a INNER JOIN accounts AS b ON a.custid = b.custid WHERE accid = " + accid.ToString() + " ) SET allowance = " + remaining_allowance.ToString() + " ;",
                            Connection = db.myConn
                        };

                        command.ExecuteNonQuery();

                        /* Update balance. */
                        command = new OleDbCommand()
                        {
                            CommandText = "UPDATE accounts SET balance = balance + " + amount + ", active='true' WHERE accid=" + accid,
                            Connection = db.myConn
                        };

                        command.ExecuteNonQuery();

                        /* Insert transaction. */
                        DateTime thisDay = DateTime.Today;

                        command = new OleDbCommand()
                        {
                            CommandText = "INSERT INTO tranx (accid, [action], amnt, event) VALUES (" + accid + ", 'DEPOSIT', " + amount + ", '" + thisDay.ToString("d") + "')",
                            Connection = db.myConn
                        };

                        command.ExecuteNonQuery();

                        MessageBox.Show("Deposit of £" + amount.ToString() + " successful. £" + remaining_allowance.ToString() + " deposit allowance remaining.");

                    }



                } else if (actn_cb.Text == "WITHDRAWAL")
                {

                    /* Update balance. */
                    command = new OleDbCommand()
                    {
                        CommandText = "UPDATE accounts SET balance = balance - " + amount + " WHERE accid=" + accid,
                        Connection = db.myConn
                    };
                    try {
                        command.ExecuteNonQuery();

                        DateTime thisDay = DateTime.Today;

                        command = new OleDbCommand()
                        {
                            CommandText = "INSERT INTO tranx (accid, [action], amnt, event) VALUES (" + accid + ", 'WITHDRAWAL', " + amount + ", '" + thisDay.ToString("d") + "')",
                            Connection = db.myConn
                        };

                        command.ExecuteNonQuery();

                        MessageBox.Show("£" + amount + " withdrawn successfully.");
                    }
                    catch
                    {
                        MessageBox.Show("Not enough funds in this account.");
                    }
                    
                }

            };

    

        }

        private void View_products_btn_Click(object sender, EventArgs e)
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


            start_x = 620;
            start_y = 50;

            int el_width = 100;

            Button toggle_btn = new Button()
            {
                Location = new System.Drawing.Point(start_x, start_y),
                Text = "Toggle Status",
                Width= el_width
            };
            main_panel.Controls.Add(toggle_btn);

            TextBox no_char = new TextBox()
            {
                Width = el_width,
                Location = Location = new System.Drawing.Point(start_x, start_y + 40),
            };
            no_char.KeyPress += new KeyPressEventHandler(No_char_func);
            Button update_btn = new Button()
            {
                Location = new System.Drawing.Point(start_x, start_y + 60),
                Text = "Update Interest",
                Width= el_width
            };
            main_panel.Controls.Add(no_char);
            main_panel.Controls.Add(update_btn);

            this.CenterToScreen();

            /* Click event listener. */
            toggle_btn.Click += (s, _e) =>
            {
 
                if(data_grid.SelectedRows.Count != 1 || data_grid.SelectedCells[0].Value == null)
                {
                    MessageBox.Show("Please select one row.");
                    
                } else
                {

                    string status = data_grid.SelectedCells[2].Value.ToString();
                    string id = data_grid.SelectedCells[0].Value.ToString();
                    if (status == "open")
                    {
                        status = "closed";
                    }
                    else if(status == "closed")
                    {
                        status = "open"; 
                    }
                    else
                    {
                        status = "open";
                    }
                    
                    command = new OleDbCommand()
                    {
                        CommandText = "UPDATE products SET status='" + status + "' WHERE prodid=" + id,
                        Connection = db.myConn
                    };
                    command.ExecuteNonQuery();
                    MessageBox.Show("Update successful");
                    view_products_btn.PerformClick();
                    
                }
               
            };

            update_btn.Click += (s, _e) =>
            {
                if (data_grid.SelectedRows.Count != 1 || data_grid.SelectedCells[0].Value == null)
                {
                    MessageBox.Show("Please select one row.");

                }
                else if (no_char.Text == "")
                {
                    MessageBox.Show("Please input the interest rate.");
                }
                else
                {

                    string id = data_grid.SelectedCells[0].Value.ToString();
                   

                    command = new OleDbCommand()
                    {
                        CommandText = "UPDATE products SET inrate='" + no_char.Text + "' WHERE prodid=" + id,
                        Connection = db.myConn
                    };
                    command.ExecuteNonQuery();
                    MessageBox.Show("Update successful");
                    view_products_btn.PerformClick();
                    
                }

            };
        }

        private void View_transactions_btn_Click(object sender, EventArgs e)
        {
            main_panel.Controls.Clear();
            this.CenterToScreen();
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

        private void View_transactions_btn_Click_datatable(object sender, EventArgs e, System.Data.DataSet ds)
        {
            main_panel.Controls.Clear();
            this.CenterToScreen();
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
