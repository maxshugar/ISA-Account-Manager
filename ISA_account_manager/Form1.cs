using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Diagnostics;

namespace ISA_account_manager
{
    public partial class Outlaw_hess_frm : Form
    {

        /* Global variable for passing data sets between tables. */
        System.Data.DataSet ds_global = new System.Data.DataSet();
        /*Used to determine whether to use the global data set. */
        bool use_ds = false;

        /* Class for connecting to the database and executing queries. */
        public class ISA_db
        {

            public static string dbconnect = @"Provider=Microsoft.ACE.OLEDB.12.0;"
            + "Data Source=" + Application.StartupPath + @"\database.accdb;";
            /* Create a connection to the database. */
            public OleDbConnection myConn = new OleDbConnection(dbconnect);

            public int Connect()
            {
                try
                {
                    /* Open the connection. */
                    myConn.Open();
                }
                catch (Exception e)
                {
                    /* Close the connection on error. */
                    myConn.Close();
                    MessageBox.Show(e.Message.ToString());
                }
                return 0;
            }
            /* Helper function to execute commands. */
            public int Execute(String command)
            {
                OleDbCommand cmd = new OleDbCommand(command, myConn);
                cmd.ExecuteNonQuery();
                return 0;
            }
        }

        /* Create an instance of the database class. */
        ISA_db db = new ISA_db();

        /* Constructor. */
        public Outlaw_hess_frm()
        {
            InitializeComponent();
            /* Connect to the database. */
            db.Connect();
            /* Center the form to the screen. */
            this.CenterToScreen();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            /* Disable maximize button. */
            this.MaximizeBox = false;
            /* Render the customers page. */
            view_customers_btn.PerformClick();

        }

        

        /* Creates an input form with the specificed number of inputs. */
        private void Generate_input_form(string title_text, object[,] input, string command_text)
        {
            int start_x = 10;
            int start_y = 10;
            /* Create the form title. */
            Label title = new Label
            {
                Text = title_text,
                Location = new System.Drawing.Point(start_x, start_y),
                Font = new Font("Arial", 20, FontStyle.Bold),
                AutoSize = true
            };
            /* Add title to controls. */
            main_panel.Controls.Add(title);
            title.BringToFront();
            start_y = 50;

            /* For each input item. */
            for (int i = 0; i < input.GetLength(0); i++)
            {
                /* Four items per column. */
                if (i % 4 == 0 && i != 0)
                {
                    start_x += 150;
                    start_y = 50;
                }
                /* Create a label for the input. */
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

            /* Save button click event listener. */
            save_btn.Click += (s, e) =>
            {
                /* Command that will be executed when the user clicks save. */
                OleDbCommand command = new OleDbCommand
                {
                    CommandText = command_text,
                    Connection = db.myConn
                };
                bool is_empty = false;

                /* Parse values from form input elements and check if empty. */
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
                        if (element.Text == "")
                        {
                            is_empty = true;
                            break;
                        }

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

            /* Set the form size. */
            this.Size = new Size(340, 320);
            /* Center form to screen */
            this.CenterToScreen();

            /* Define the forms input elements. */
            object[,] input = new object[7, 2]
            {
                {"Title", new TextBox(){ Text = "" } },
                {"First Name", new TextBox(){ Text = "" } },
                {"Last Name", new TextBox(){ Text = "" } },
                {"Date of Birth", new DateTimePicker() },
                {"National Insurance", new TextBox(){ Text = "" } },
                {"Email", new TextBox(){ Text = "" } },
                {"Password", new TextBox(){ Text = "" } }
            };
            /* Clear all controls from the main panel. */
            main_panel.Controls.Clear();
            /* Generate Command */
            String command_text = "INSERT INTO customers (title, firstname, lastname, dob, natins, email, pswd, allowance) VALUES ( ?, ?, ?, ?, ?, ?, ?, '15240' )";

            /* Generate form. */
            Generate_input_form("New customer", input, command_text);

        }

        private void View_customers_btn_Click(object sender, EventArgs e)
        {

            this.Size = new Size(660, 430);
            main_panel.Controls.Clear();
            this.CenterToScreen();
            int start_x = 10;
            int start_y = 10;

            /* Create a title for the form. */
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

            /* Create a datagrid to display the customer data. */
            DataGridView data_grid = new DataGridView()
            {
                DataSource = ds.Tables["customers"],
                Width = 600,
                Height = 300,
                Location = new System.Drawing.Point(start_x, start_y),
                ReadOnly = true
            };
            /* Add the datagrid to the screen. */
            main_panel.Controls.Add(data_grid);

            /* Event listner for double click on a cell in the datagrid. */
            data_grid.CellDoubleClick += (s, _e) =>
            {
                /* Get the row index of the selected cell. */
                int index = data_grid.SelectedCells[0].RowIndex;
                Debug.WriteLine(index);
                /* Get the customer id of the selected customer cell. */
                string custid = data_grid[0, index].Value.ToString();
                Debug.WriteLine(custid);
                if (custid == "")
                {
                    return;
                }

                /* Set the next datagrid to display accounts owned by the selected customer. */
                command = new OleDbCommand()
                {
                    CommandText = "SELECT * FROM accounts WHERE custid=" + custid,
                    Connection = db.myConn
                };

                /* Store the query result in the local data set. */
                ds = new System.Data.DataSet();

                adapter = new OleDbDataAdapter(command);
                adapter.Fill(ds, "accounts");

                /* Set the use fataset flag to true. */
                use_ds = true;
                /* Store the query result in the globel data set. */
                ds_global = ds;

                /* Render the accounts datagrid. */
                view_accounts_btn.PerformClick();

            };
        }

        private void View_accounts_btn_Click(object sender, EventArgs e)
        {

            this.Size = new Size(760, 480);
            main_panel.Controls.Clear();
            this.CenterToScreen();
            int start_x = 10;
            int start_y = 10;

            /* Form title. */
            Label title = new Label
            {
                Text = "Accounts",
                Location = new System.Drawing.Point(start_x, start_y),
                Font = new Font("Arial", 20, FontStyle.Bold),
                AutoSize = true
            };
            /* Add title to main panel. */
            main_panel.Controls.Add(title);
            start_y += 40;

            /* Get customers. */
            System.Data.DataSet ds = new System.Data.DataSet();
            OleDbCommand command = new OleDbCommand()
            {
                CommandText = "SELECT * FROM accounts",
                Connection = db.myConn
            };
            /* Execute the commend. */
            OleDbDataAdapter adapter = new OleDbDataAdapter(command);
            adapter.Fill(ds, "accounts");

            /* Use global dataset instead if flag has been set. */
            if (use_ds)
            {
                /* Reset the flag. */
                use_ds = false;
                /* Assign the local dataset to the global. */
                ds = ds_global;
            }

            /* Create a datagrid to display the accounts table. */
            DataGridView data_grid = new DataGridView()
            {
                DataSource = ds.Tables["accounts"],
                Width = 600,
                Height = 350,
                Location = new System.Drawing.Point(start_x, start_y),
                ReadOnly = true
            };
            /* Add the datagrid to the main panel. */
            main_panel.Controls.Add(data_grid);

            /* Event listner for double clicking on a cell. */
            data_grid.CellDoubleClick += (s, _e) =>
            {
                /* Get the index of the selected row. */
                int index = data_grid.SelectedCells[0].RowIndex;

                /* Get the account id of the selected row. */
                string accid = data_grid[0, index].Value.ToString();

                if (accid == "")
                {
                    return;
                }

                /* Select transactions assocciated with the selected account. */
                command = new OleDbCommand()
                {
                    CommandText = "SELECT * FROM tranx WHERE accid=" + accid + " ORDER BY event DESC",
                    Connection = db.myConn
                };

                /* Local dataset/ */
                ds = new System.Data.DataSet();

                   /* Execute the command. */
                adapter = new OleDbDataAdapter(command);
                adapter.Fill(ds, "tranx");

                /* Set the flag to true. */
                use_ds = true;
                /* Store the dataset in the global variable. */
                ds_global = ds;

                /* Render the transactions datagrid form. */
                view_transactions_btn.PerformClick();

            };

            /* Create a button for opening and closing accounts. */
            Button toggle_btn = new Button()
            {
                Location = new System.Drawing.Point(620, start_y),
                Text = "Toggle Status",
                Width = 100
            };
            /* Add the button to the main panel. */
            main_panel.Controls.Add(toggle_btn);

            /* Toggle button click event listener. */
            toggle_btn.Click += (s, _e) =>
            {
                /* Validate row selection. */
                if (data_grid.SelectedRows.Count != 1 || data_grid.SelectedCells[0].Value == null)
                {
                    MessageBox.Show("Please select one row.");

                }
                else
                {
                    /* Get the selected accounts current status. */
                    string status = data_grid.SelectedCells[7].Value.ToString();
                    /* Get the account id. */
                    string id = data_grid.SelectedCells[0].Value.ToString();
                    /* Swap the status. */
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
                    /* Build a command to update the account status. */
                    command = new OleDbCommand()
                    {
                        CommandText = "UPDATE accounts SET status='" + status + "' WHERE accid=" + id,
                        Connection = db.myConn
                    };
                    /* Execute the command. */
                    command.ExecuteNonQuery();

                    /* Render the account datagrid form. */
                    view_accounts_btn.PerformClick();

                }

            };

            /* Create a button for calculating accrued interest rate for all accounts. */
            Button accured_interest_btn = new Button()
            {
                Location = new System.Drawing.Point(620, start_y + 40),
                Text = "Calculate all accounts accured interest",
                Width = 100,
                Height = 60
            };
            /* Add the button to the main panel. */
            main_panel.Controls.Add(accured_interest_btn);

            
            accured_interest_btn.Click += (_s, _e) =>
            {

                for (int i = 0; i < ds.Tables["accounts"].Rows.Count; i++)
                {

                    /* Get product interest rate. */
                    string prodid = ds.Tables["accounts"].Rows[i][2].ToString();

                    command = new OleDbCommand()
                    {
                        CommandText = "SELECT inrate FROM products WHERE prodid=" + prodid,
                        Connection = db.myConn
                    };
                    /* Local dataset. */
                    System.Data.DataSet prod_ds = new System.Data.DataSet();
                    adapter = new OleDbDataAdapter(command);
                    adapter.Fill(prod_ds, "products");

                   

                    /* Get interest rate. */
                    double inrate = Convert.ToDouble(prod_ds.Tables["products"].Rows[0]["inrate"].ToString()) / 100;
                    /* Get accured interest. */
                    double accrued_interest = Convert.ToDouble(ds.Tables["accounts"].Rows[i][4].ToString());
                    /* Get balance. */
                    double balance = Convert.ToDouble(ds.Tables["accounts"].Rows[i][3].ToString());
                    /* Get account id. */
                    string accid = ds.Tables["accounts"].Rows[i][0].ToString();
                    /* Calculate accrued interest. */
                    accrued_interest = accrued_interest + (balance * (inrate / 365));
                    /* Build command text. */
                    command = new OleDbCommand()
                    {
                        CommandText = "UPDATE accounts SET accrued='" + accrued_interest + "' WHERE accid=" + accid,
                        Connection = db.myConn
                    };
                    /* Execute query. */
                    command.ExecuteNonQuery();
                    
                }
                /* Display message to user. */
                MessageBox.Show("Interest calculation successful");
                /* Render the accounts datagrid. */
                view_accounts_btn.PerformClick();
            };

            /* Create button for performing end of year tax updates. */
            Button end_of_year_tax_updates_btn = new Button()
            {
                Location = new System.Drawing.Point(620, start_y + 120),
                Text = "End of Year Tax Updates",
                Width = 100,
                Height = 60
            };
            main_panel.Controls.Add(end_of_year_tax_updates_btn);

            end_of_year_tax_updates_btn.Click += (_s, _e) =>
            {

                /*Add accrued interest to balance, set accrued interest to 0 anf active to false for each account. */
                command = new OleDbCommand()
                {
                    CommandText = "UPDATE accounts SET balance = balance + accrued, accrued = 0, active='false'",
                    Connection = db.myConn
                };
                /* Execute the command. */
                command.ExecuteNonQuery();

                /* Reset deposit allowance for all customers. */
                command = new OleDbCommand()
                {
                    CommandText = "UPDATE customers SET allowance = 15240",
                    Connection = db.myConn
                };
                /* Execute the query. */
                command.ExecuteNonQuery();
                /* Render the accounts datagrid. */
                view_accounts_btn.PerformClick();

            };
        }

        /* Prevents characters being entered into a text box and only one colon. */
        private void No_char_func(object s, KeyPressEventArgs e)
        {
            /* Cast sender object. */
            TextBox local = (TextBox)s;
            /* If a colon is present in the text box. */
            if (local.Text.IndexOf('.') != -1)
            {
                /* If a colon has been pressed. */
                if (e.KeyChar == '.')
                    /* Don't add the colon to the text box. */
                    e.Handled = true;
            }
            /* If the character is not a number, control character or colon. */
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != '.')
                /* Don't allow the character. */
                e.Handled = true;
        }

        private void New_product_btn_Click(object sender, EventArgs e)
        {
            /* Set the form size. */
            this.Size = new Size(340, 320);
            /* Center the form to the screen. */
            this.CenterToScreen();

            /* Create a text box. */
            TextBox no_char = new TextBox();
            /* Attach custom event handler to textbox key press event. */
            no_char.KeyPress += new KeyPressEventHandler(No_char_func);
            /* Define input objects. */
            object[,] input = new object[3, 2]
            {
                {"name", new TextBox(){ Text = "" } },
                {"transactions in", new CheckBox() },
                {"interest rate", no_char }
            };
            /* Remove all controls from the main panel. */
            main_panel.Controls.Clear();
            /* Generate Command */
            String command_text = "INSERT INTO products (prod_name, status, transin, inrate) VALUES (?, 'open', ?, ?)";

            /* Generate form. */
            Generate_input_form("New product", input, command_text);
        }

        private void New_account_btn_Click(object sender, EventArgs e)
        {
            /* Set form size. */
            this.Size = new Size(340, 320);
            /* Center form to screen. */
            this.CenterToScreen();

            /* Create a text box. */
            TextBox no_char = new TextBox();
            /* Attach custom event handler to text box key press. */
            no_char.KeyPress += new KeyPressEventHandler(No_char_func);

            /* Create a new dataset. */
            System.Data.DataSet ds = new System.Data.DataSet();
            /* Build a command query for retrieving all customers. */
            OleDbCommand command = new OleDbCommand()
            {
                CommandText = "SELECT custid, firstname, lastname, email FROM customers",
                Connection = db.myConn
            };
            /* Initialize a new instance of the OleDbDataAdapter class. */
            OleDbDataAdapter adapter = new OleDbDataAdapter(command);
            /* Fill the dataset with datasource. */
            adapter.Fill(ds, "customers");
            /* Create a combobox for displaying customers. */
            ComboBox customer_cb = new ComboBox()
            {
                DropDownStyle = ComboBoxStyle.DropDown,
                AutoCompleteMode = AutoCompleteMode.SuggestAppend,
                AutoCompleteSource = AutoCompleteSource.ListItems
            };
            /* Add customers to the combobox. */
            foreach (System.Data.DataRow dr in ds.Tables["customers"].Rows)
                customer_cb.Items.Add(dr["custid"] + " - " + dr["firstname"] + " " + dr["lastname"] + " - " + dr["email"]);

            /* Build a command query for retrieving all products. */
            command = new OleDbCommand()
            {
                CommandText = "SELECT prodid, prod_name, inrate FROM products",
                Connection = db.myConn
            };
            /* Initialize a new instance of the OleDbDataAdapter class. */
            adapter = new OleDbDataAdapter(command);
            /* Fill the dataset/ */
            adapter.Fill(ds, "products");
            /* Create a combobox for product data. */
            ComboBox products_cb = new ComboBox()
            {
                DropDownStyle = ComboBoxStyle.DropDown,
                AutoCompleteMode = AutoCompleteMode.SuggestAppend,
                AutoCompleteSource = AutoCompleteSource.ListItems
            };
            /* Add products to combobox. */
            foreach (System.Data.DataRow dr in ds.Tables["products"].Rows)
                products_cb.Items.Add(dr["prodid"] + " - " + dr["prod_name"] + " - " + dr["inrate"] + "%");

            /* Add input objects to object array. */
            object[,] input = new object[2, 2]
            {
                {"customer", customer_cb },
                {"product", products_cb }
            };
            /* Clear controls from main panel. */
            main_panel.Controls.Clear();
            /* Generate Command */
            DateTime thisDay = DateTime.Today;
            /* Build parameterized query for creating a new account. */ 
            String command_text = "INSERT INTO accounts (custid, prodid, balance, accrued, active, opnd, status) VALUES (?, ?, '0', '0', 'false', '" + thisDay.ToString("d") + "', 'open')";

            /* Generate new account input form. */
            Generate_input_form("New account", input, command_text);
        }

        private void View_products_btn_Click(object sender, EventArgs e)
        {
            /* Clear the controls of the main panel. */
            main_panel.Controls.Clear();
            /* Set the size of the form. */
            this.Size = new Size(770, 490);
            int start_x = 10;
            int start_y = 10;
            /* Create a title. */
            Label title = new Label
            {
                Text = "Products",
                Location = new System.Drawing.Point(start_x, start_y),
                Font = new Font("Arial", 20, FontStyle.Bold),
                AutoSize = true
            };
            /* Add the tile. */
            main_panel.Controls.Add(title);
            start_y += 40;

            /* Get customers. */
            System.Data.DataSet ds = new System.Data.DataSet();
            OleDbCommand command = new OleDbCommand()
            {
                CommandText = "SELECT * FROM products",
                Connection = db.myConn
            };
            /* Initialize a new instance of the OleDbDataAdapter class. */
            OleDbDataAdapter adapter = new OleDbDataAdapter(command);
            /* Fill the dataset. */
            adapter.Fill(ds, "products");
            /* Check if the flag has been set. */
            if (use_ds)
            {
                /* Reset the flag. */
                use_ds = false;
                /* Use the global dataset. */
                ds = ds_global;
            }
            /* Create a datagrid for products. */
            DataGridView data_grid = new DataGridView()
            {
                DataSource = ds.Tables["products"],
                Width = 600,
                Height = 350,
                Location = new System.Drawing.Point(start_x, start_y),
                ReadOnly = true
            };
            /* Add the datagrid to main panel. */
            main_panel.Controls.Add(data_grid);
            /* Set the position of the next control. */
            start_x = 620;
            start_y = 50;

            int el_width = 100;
            /* Create a button for toggling a products status. */
            Button toggle_btn = new Button()
            {
                Location = new System.Drawing.Point(start_x, start_y),
                Text = "Toggle Status",
                Width = el_width
            };
            /* Add the button to the main panel. */
            main_panel.Controls.Add(toggle_btn);
            /* Create a label for updating the interest rate. */
            Label lbl = new Label
            {
                Text = "Interest rate:",
                Location = new System.Drawing.Point(start_x, start_y + 50),
                Font = new Font("Arial", 9),
                AutoSize = true
            };
            /* Create a textbox for inputting the new interest rate. */
            TextBox no_char = new TextBox()
            {
                Width = el_width,
                Location = Location = new System.Drawing.Point(start_x, start_y + 70)
            };
            /* Attach custom event listner to key press event. */
            no_char.KeyPress += new KeyPressEventHandler(No_char_func);
            /* Create a button for updating the products interest rate. */
            Button update_btn = new Button()
            {
                Location = new System.Drawing.Point(start_x, start_y + 95),
                Text = "Update Interest",
                Width = el_width
            };
            /* Add controls to main panel. */
            main_panel.Controls.Add(lbl);
            main_panel.Controls.Add(no_char);
            main_panel.Controls.Add(update_btn);
            /* Center the form on the screen. */
            this.CenterToScreen();

            /* Click event listener. */
            toggle_btn.Click += (s, _e) =>
            {
                /* Ensure one row has been selected. */
                if (data_grid.SelectedRows.Count != 1 || data_grid.SelectedCells[0].Value == null)
                {
                    MessageBox.Show("Please select one row.");

                }
                else
                {
                    /* Retrieve the products status from the data grid. */
                    string status = data_grid.SelectedCells[2].Value.ToString();
                    /* Get the id of the selected product. */
                    string id = data_grid.SelectedCells[0].Value.ToString();
                    /* Close product if open. */
                    if (status == "open")
                    {
                        status = "closed";
                    }
                    /* Open product if closed. */
                    else if (status == "closed")
                    {
                        status = "open";
                    }
                    /* Open by default. */
                    else
                    {
                        status = "open";
                    }
                    /* Build command query for updating products. */
                    command = new OleDbCommand()
                    {
                        CommandText = "UPDATE products SET status='" + status + "' WHERE prodid=" + id,
                        Connection = db.myConn
                    };
                    /* Execute the query. */
                    command.ExecuteNonQuery();
                    /* Display success message to the user. */
                    MessageBox.Show("Update successful");
                    /* Re render data grid. */
                    view_products_btn.PerformClick();
           
                }

            };

            update_btn.Click += (s, _e) =>
            {
                /* Validate selected row. */
                if (data_grid.SelectedRows.Count != 1 || data_grid.SelectedCells[0].Value == null)
                {
                    MessageBox.Show("Please select one row.");

                }
                /* Validate input text box. */
                else if (no_char.Text == "")
                {
                    MessageBox.Show("Please input the interest rate.");
                }
                else
                {
                    /* Get the id of the selected product. */
                    string id = data_grid.SelectedCells[0].Value.ToString();

                    /* Build the command for updating the products interest rate. */
                    command = new OleDbCommand()
                    {
                        CommandText = "UPDATE products SET inrate='" + no_char.Text + "' WHERE prodid=" + id,
                        Connection = db.myConn
                    };
                    /* Execute the query. */
                    command.ExecuteNonQuery();
                    /* Display success message to the user. */
                    MessageBox.Show("Update successful");
                    /* Re render data grid. */
                    view_products_btn.PerformClick();

                }

            };
        }

        private void View_transactions_btn_Click(object sender, EventArgs e)
        {
            /* Set the size of the form. */
            this.Size = new Size(660, 430);
            /* Clear controls from the main panel. */
            main_panel.Controls.Clear();
            /* Center the form to the screen. */
            this.CenterToScreen();

            /* Set the start position of the next control. */
            int start_x = 10;
            int start_y = 10;
            /* Create a label for displaying transactions. */
            Label title = new Label
            {
                Text = "Transactions",
                Location = new System.Drawing.Point(start_x, start_y),
                Font = new Font("Arial", 20, FontStyle.Bold),
                AutoSize = true
            };
            /* Add the title to the main panel. */
            main_panel.Controls.Add(title);
            start_y += 40;

            /* Create a local dataset. */
            System.Data.DataSet ds = new System.Data.DataSet();
            /* Create command for selecting all transactions. */
            OleDbCommand command = new OleDbCommand()
            {
                CommandText = "SELECT * FROM tranx ORDER BY event DESC",
                Connection = db.myConn
            };
            /* Initialize a new instance of the OleDbDataAdapter class. */
            OleDbDataAdapter adapter = new OleDbDataAdapter(command);
            /* Fill the dataset with db data. */
            adapter.Fill(ds, "tranx");
            /* Check if flag has been set. */
            if (use_ds)
            {
                /* Reste the flag. */
                use_ds = false;
                /* Use the global dataset. */
                ds = ds_global;
            }
            /* Create a datagrid for viewing transactions. */
            DataGridView data_grid = new DataGridView()
            {
                DataSource = ds.Tables["tranx"],
                Width = 600,
                Height = 300,
                Location = new System.Drawing.Point(start_x, start_y),
                ReadOnly = true
            };
            /* Add the datagrid to the main panel. */
            main_panel.Controls.Add(data_grid);

        }

        /* Calculate the amount invested in each product. */
        private void product_investment_Click(object sender, EventArgs e)
        {
            /* Clear controls from main panel. */
            main_panel.Controls.Clear();
            this.Size = new Size(660, 430);
            int start_x = 10;
            int start_y = 10;
            /* Center the form to the screen. */
            this.CenterToScreen();
            /* Create a label to display the title. */
            Label title = new Label
            {
                Text = "Product Investment",
                Location = new System.Drawing.Point(start_x, start_y),
                Font = new Font("Arial", 20, FontStyle.Bold),
                AutoSize = true
            };
            /* add title to main panel. */
            main_panel.Controls.Add(title);
            start_y += 40;

            /* Create a local dataset. */
            System.Data.DataSet ds = new System.Data.DataSet();
            /* Create a command for selecting the amount invested in each prodcut. */
            OleDbCommand command = new OleDbCommand()
            {
                CommandText = "SELECT prod_name AS 'Product Name', SUM(balance) AS 'Total Invested' FROM(SELECT * FROM accounts INNER JOIN products ON accounts.prodid = products.prodid) GROUP BY products.prod_name, products.prodid;",
                Connection = db.myConn
            };
            /* Initialize a new instance of the OleDbDataAdapter class. */
            OleDbDataAdapter adapter = new OleDbDataAdapter(command);
            /* Fill the dataset. */
            adapter.Fill(ds, "accounts");
            /* Create a datagrid view. */
            DataGridView data_grid = new DataGridView()
            {
                DataSource = ds.Tables["accounts"],
                Width = 600,
                Height = 300,
                Location = new System.Drawing.Point(start_x, start_y),
                ReadOnly = true
            };
            /* Add datagrid to main panel. */
            main_panel.Controls.Add(data_grid);

        }

        /* Find the most deposits per product. */
        private void most_deposits_btn_Click(object sender, EventArgs e)
        {
            /* Clear all controls from main panel. */
            main_panel.Controls.Clear();
            /* Set the form size. */
            this.Size = new Size(810, 430);
            /* Set the start position of the next control. */
            int start_x = 10;
            int start_y = 10;
            /* Center the form to the screen. */
            this.CenterToScreen();
            /* Create a label for the form. */
            Label title = new Label
            {
                Text = "Product Deposits",
                Location = new System.Drawing.Point(start_x, start_y),
                Font = new Font("Arial", 20, FontStyle.Bold),
                AutoSize = true
            };
            /* Add the label to the main panel. */
            main_panel.Controls.Add(title);
            start_y += 40;
            /* Create a datagrid. */
            DataGridView data_grid = new DataGridView()
            {
                Width = 600,
                Height = 300,
                Location = new System.Drawing.Point(start_x, start_y),
                ReadOnly = true
            };
            /* Add data grid to main panel. */
            main_panel.Controls.Add(data_grid);
            /* Create label for date time picker input. */
            Label lbl = new Label
            {
                Text = "From:",
                Location = new System.Drawing.Point(620, start_y),
                Font = new Font("Arial", 9),
                AutoSize = true
            };
            main_panel.Controls.Add(lbl);
            /* Create date time picker for selecting query start date. */
            DateTimePicker dtp = new DateTimePicker
            {
                Value = DateTime.Today,
                Location = new System.Drawing.Point(620, start_y + 20),
                Width = 150,
                MaxDate = DateTime.Today
            };
            main_panel.Controls.Add(dtp);
            /* Create a button to trigger the query. */
            Button go_btn = new Button()
            {
                Location = new System.Drawing.Point(620, start_y + 45),
                Text = "GO",
                Width = 150
            };
            main_panel.Controls.Add(go_btn);

            go_btn.Click += (s, _e) =>
            {
                /* Get the date. */
                String date = dtp.Value.ToString("MM/dd/yyyy");
                /* Create a query for grouping products by the amount deposited in them. */
                String query = "SELECT prod_name AS Product, SUM(amnt) AS amount FROM " +
                "(SELECT * FROM accounts INNER JOIN " +
                "tranx ON accounts.accid = tranx.accid WHERE tranx.event >= #" + date + "# ) AS X " +
                "INNER JOIN products ON X.prodid = products.prodid WHERE action = \"DEPOSIT\" GROUP BY prod_name";

                /* Create a local dataset. */
                System.Data.DataSet ds = new System.Data.DataSet();
                /* Build the command. */
                OleDbCommand command = new OleDbCommand()
                {
                    CommandText = query,
                    Connection = db.myConn
                };
                /* Initialize a new instance of the OleDbDataAdapter class. */
                OleDbDataAdapter adapter = new OleDbDataAdapter(command);
                /* Fill the dataset. */
                adapter.Fill(ds, "accounts");
                /* Add the dataset to the datagrid. */
                data_grid.DataSource = ds.Tables["accounts"];

            };

        }

        /* Get the most withdrawrals per product. */
        private void most_withdrawals_btn_Click(object sender, EventArgs e)
        {
            /* Clear the controls from the main panel. */
            main_panel.Controls.Clear();
            /* Set the size of the form. */
            this.Size = new Size(810, 430);

            /* Set the start position of the next control. */
            int start_x = 10;
            int start_y = 10;
            /* Center the form to screen. */
            this.CenterToScreen();
            /* Create a title for the form. */
            Label title = new Label
            {
                Text = "Product Withdrawals",
                Location = new System.Drawing.Point(start_x, start_y),
                Font = new Font("Arial", 20, FontStyle.Bold),
                AutoSize = true
            };
            /* Add title to main panel. */
            main_panel.Controls.Add(title);
            start_y += 40;
            /* Create datgrid control. */
            DataGridView data_grid = new DataGridView()
            {
                Width = 600,
                Height = 300,
                Location = new System.Drawing.Point(start_x, start_y),
                ReadOnly = true
            };
            main_panel.Controls.Add(data_grid);

            /* Create label for input. */
            Label lbl = new Label
            {
                Text = "From:",
                Location = new System.Drawing.Point(620, start_y),
                Font = new Font("Arial", 9),
                AutoSize = true
            };
            main_panel.Controls.Add(lbl);

            /* Create date time picker for selecting date range. */
            DateTimePicker dtp = new DateTimePicker
            {
                Value = DateTime.Today,
                Location = new System.Drawing.Point(620, start_y + 20),
                Width = 150,
                MaxDate = DateTime.Today
            };
            main_panel.Controls.Add(dtp);

            /* Create button to rigger the query. */
            Button go_btn = new Button()
            {
                Location = new System.Drawing.Point(620, start_y + 45),
                Text = "GO",
                Width = 150
            };
            main_panel.Controls.Add(go_btn);

            /* Click event listner for the go button. */
            go_btn.Click += (s, _e) =>
            {
                /* Get date. */
                String date = dtp.Value.ToString("MM/dd/yyyy");
                /* Create a query for grouping products by the amount withdrawn. */
                String query = "SELECT prod_name AS Product, SUM(amnt) AS amount FROM " +
                "(SELECT * FROM accounts INNER JOIN " +
                "tranx ON accounts.accid = tranx.accid WHERE tranx.event >= #" + date + "# ) AS X " +
                "INNER JOIN products ON X.prodid = products.prodid WHERE action = \"WITHDRAWAL\" GROUP BY prod_name";

                /* Create local dataset. */
                System.Data.DataSet ds = new System.Data.DataSet();
                /* Build the command. */
                OleDbCommand command = new OleDbCommand()
                {
                    CommandText = query,
                    Connection = db.myConn
                };
                /* Initialize a new instance of the OleDbDataAdapter class. */
                OleDbDataAdapter adapter = new OleDbDataAdapter(command);
                /* Fill the adapter. */
                adapter.Fill(ds, "accounts");
                /* Set the datagrid data to dataset. */
                data_grid.DataSource = ds.Tables["accounts"];
            };
        }
        /* Make a transaction */
        private void New_transaction_btn_Click(object sender, EventArgs e)
        {
            /* Clear main panel controls. */
            main_panel.Controls.Clear();
            /* Set form size. */
            this.Size = new Size(340, 320);
            /* Center form to screen. */
            this.CenterToScreen();

            /* Create a tetbox that prevents characters being typed in and one decimal point. */
            TextBox no_char = new TextBox();
            no_char.KeyPress += new KeyPressEventHandler(No_char_func);

            /* Get accounts. */
            System.Data.DataSet ds = new System.Data.DataSet();
            OleDbCommand command = new OleDbCommand()
            {
                CommandText = "SELECT accid, firstname, lastname, email, status FROM accounts INNER JOIN customers ON accounts.custid = customers.custid WHERE status = 'open';",
                Connection = db.myConn
            };
            OleDbDataAdapter adapter = new OleDbDataAdapter(command);
            adapter.Fill(ds, "accounts");

            /* Create a combobox to display accounts. */
            ComboBox acc_cb = new ComboBox()
            {
                DropDownStyle = ComboBoxStyle.DropDown,
                AutoCompleteMode = AutoCompleteMode.SuggestAppend,
                AutoCompleteSource = AutoCompleteSource.ListItems
            };
            /* Add dataset to combobox. */
            foreach (System.Data.DataRow dr in ds.Tables["accounts"].Rows)
                acc_cb.Items.Add(dr["accid"] + " - " + dr["firstname"] + " " + dr["lastname"] + " - " + dr["email"]);

            /* Create a combobox for selecting an action. */
            ComboBox actn_cb = new ComboBox()
            {
                DropDownStyle = ComboBoxStyle.DropDown,
                AutoCompleteMode = AutoCompleteMode.SuggestAppend,
                AutoCompleteSource = AutoCompleteSource.ListItems
            };
            /* Add items to the combo box. */
            actn_cb.Items.Add("DEPOSIT");
            actn_cb.Items.Add("WITHDRAWAL");

            /* Add controls to input object array. */
            object[,] input = new object[3, 2]
            {
                {"account", acc_cb },
                {"action", actn_cb, },
                {"amount £", no_char }
            };

            /* Set start position of next control. */
            int start_x = 10;
            int start_y = 10;

            /* Create title. */
            Label title = new Label
            {
                Text = "New transaction",
                Location = new System.Drawing.Point(start_x, start_y),
                Font = new Font("Arial", 20, FontStyle.Bold),
                AutoSize = true
            };
            main_panel.Controls.Add(title);
            title.BringToFront();
            start_y = 50;

            /* For each input control. */
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
            /* Save button click event listener. */
            save_btn.Click += (_s, _e) =>
            {
                /* Input validation. */
                if (acc_cb.Text == "")
                {
                    MessageBox.Show("Error: Please select an account.");
                    return;
                }
                if (actn_cb.Text == "")
                {
                    MessageBox.Show("Error: Please select an action.");
                    return;
                }
                if (no_char.Text == "")
                {
                    MessageBox.Show("Error: Please input an amount.");
                    return;
                }

                /* Input parsing. */
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
                    /* Parse allowance. */
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



                }
                else if (actn_cb.Text == "WITHDRAWAL")
                {

                    /* Update balance. */
                    command = new OleDbCommand()
                    {
                        CommandText = "UPDATE accounts SET balance = balance - " + amount + " WHERE accid=" + accid,
                        Connection = db.myConn
                    };
                    /* Try and execute the query. */
                    try
                    {
                        command.ExecuteNonQuery();

                        DateTime thisDay = DateTime.Today;
                        /* Record the transaction. */
                        command = new OleDbCommand()
                        {
                            CommandText = "INSERT INTO tranx (accid, [action], amnt, event) VALUES (" + accid + ", 'WITHDRAWAL', " + amount + ", '" + thisDay.ToString("d") + "')",
                            Connection = db.myConn
                        };
                        command.ExecuteNonQuery();
                        MessageBox.Show("£" + amount + " withdrawn successfully.");
                    }
                    catch /* An error will be caught if there are insufficient funds in the account due to a constraint set in the database. */
                    {
                        MessageBox.Show("Not enough funds in this account.");
                    }
                }
            };
        }

        /* Transfer money from one account to another owned by the same customer. */
        private void account_transaction_btn_Click(object sender, EventArgs e)
        {
            /* Clear controls from main panel. */
            main_panel.Controls.Clear();
            /* Set the size of the form. */
            this.Size = new Size(340, 320);
            /* Center the form. */
            this.CenterToScreen();

            /* Set start postion of next control. */
            int start_x = 10;
            int start_y = 10;
            /* Create title for form. */
            Label title = new Label
            {
                Text = "New transaction",
                Location = new System.Drawing.Point(start_x, start_y),
                Font = new Font("Arial", 20, FontStyle.Bold),
                AutoSize = true
            };
            main_panel.Controls.Add(title);

            /* Create label for input. */
            Label from_lbl = new Label
            {
                Text = "From",
                Location = new System.Drawing.Point(start_x, start_y + 40),
                Font = new Font("Arial", 9),
                AutoSize = true
            };
            main_panel.Controls.Add(from_lbl);

            /* Get accounts. */
            System.Data.DataSet ds = new System.Data.DataSet();
            OleDbCommand command = new OleDbCommand()
            {
                CommandText = "SELECT accid, accounts.custid, firstname, lastname, email, status FROM accounts INNER JOIN customers ON accounts.custid = customers.custid WHERE status = 'open';",
                Connection = db.myConn
            };
            OleDbDataAdapter adapter = new OleDbDataAdapter(command);
            adapter.Fill(ds, "accounts");

            /* Create a combobox. */
            ComboBox acc_cb = new ComboBox()
            {
                DropDownStyle = ComboBoxStyle.DropDown,
                AutoCompleteMode = AutoCompleteMode.SuggestAppend,
                AutoCompleteSource = AutoCompleteSource.ListItems,
                Location = new System.Drawing.Point(start_x, start_y + 60),
                Width = 250
            };

            /* Add accounts to combobox. */
            foreach (System.Data.DataRow dr in ds.Tables["accounts"].Rows)
                acc_cb.Items.Add(dr["accid"] + " - " + dr["custid"] + " - " + dr["firstname"] + " " + dr["lastname"] + " - " + dr["email"]);
            main_panel.Controls.Add(acc_cb);

            /* Create label for input. */
            Label to_lbl = new Label
            {
                Text = "To:",
                Location = new System.Drawing.Point(start_x, start_y + 90),
                Font = new Font("Arial", 9),
                AutoSize = true
            };
            main_panel.Controls.Add(to_lbl);

            /* Create a combobox. */
            ComboBox acc_cb_ = new ComboBox()
            {
                DropDownStyle = ComboBoxStyle.DropDown,
                AutoCompleteMode = AutoCompleteMode.SuggestAppend,
                AutoCompleteSource = AutoCompleteSource.ListItems,
                Location = new System.Drawing.Point(start_x, start_y + 110),
                Width = 250
            };
            main_panel.Controls.Add(acc_cb_);

            /* Create a label for input. */
            Label amnt_lbl = new Label
            {
                Text = "Amount:",
                Location = new System.Drawing.Point(start_x, start_y + 140),
                Font = new Font("Arial", 9),
                AutoSize = true
            };
            main_panel.Controls.Add(amnt_lbl);

            /* Create a text box. */
            TextBox no_char = new TextBox()
            {
                Location = new System.Drawing.Point(start_x, start_y + 160),
                Width = 250
            };
            /* Attach custom event listner. */
            no_char.KeyPress += new KeyPressEventHandler(No_char_func);
            /* Add to main panel. */
            main_panel.Controls.Add(no_char);

            /* Create a button. */
            Button update_btn = new Button()
            {
                Location = new System.Drawing.Point(start_x, start_y + 200),
                Text = "Transfer",
                Width = 250
            };
            main_panel.Controls.Add(update_btn);

            /* Triggered when an item is selected from the combobox. */
            acc_cb.SelectedIndexChanged += (s, _e) =>
            {
                /* Clear the combobox. */
                acc_cb_.Text = "";
                acc_cb_.Items.Clear();
                acc_cb_.SelectedIndex = -1;

                /* Parse input values. */
                string[] stringSeparators = new string[] { " - " };
                String[] arr = acc_cb.Text.Split(stringSeparators, 2, StringSplitOptions.None);
                String accid = acc_cb.Text.Split('-')[0].Split(' ')[0];
                String custid = arr[1].Split('-')[0].Split(' ')[0];
            
                /*  Select accounts owned by the same customer that has not already been selected and allows inward transactions.  */
                String query = "SELECT * FROM(SELECT accid, accounts.prodid, accounts.custid, firstname, lastname, email, status FROM accounts INNER JOIN " +
                                "customers ON accounts.custid = customers.custid WHERE status = 'open' AND accounts.custid= " + custid + " AND accid <> " + accid + ") AS X INNER JOIN " +
                                "products ON X.prodid = products.prodid WHERE transin = true";

                /* Create a local dataset. */
                ds = new System.Data.DataSet();
                /* Construct a command. */
                command = new OleDbCommand()
                {
                    CommandText = query,
                    Connection = db.myConn
                };
                /* Initialize a new instance of the OleDbDataAdapter class. */
                adapter = new OleDbDataAdapter(command);
                /* Fill the dataset. */
                adapter.Fill(ds, "accounts");

                /* Add dataset to combobox. */
                foreach (System.Data.DataRow dr in ds.Tables["accounts"].Rows)
                    acc_cb_.Items.Add(dr["accid"] + " - " + dr["custid"] + " - " + dr["firstname"] + " " + dr["lastname"] + " - " + dr["email"]);

            };
            /* When the user clicks on the update button. */
            update_btn.Click += (s, _e) =>
            {

                /* Input validation. */
                if (acc_cb.Text == "")
                {
                    MessageBox.Show("Error: Please select an account from combobox 1.");
                    return;
                }
                if (acc_cb_.Text == "")
                {
                    MessageBox.Show("Error: Please select an account from combobox 2.");
                    return;
                }
                if (no_char.Text == "")
                {
                    MessageBox.Show("Error: Please input an amount.");
                    return;
                }

                /* Input parsing. */
                double amount = Convert.ToDouble(no_char.Text);
                String accid_from = acc_cb.Text.Split('-')[0].Split(' ')[0];
                String accid_to = acc_cb_.Text.Split('-')[0].Split(' ')[0];

                /* Check if first account has sufficient funds. */
                ds = new System.Data.DataSet();
                /* Construct a command. */
               command = new OleDbCommand()
                {
                    CommandText = "SELECT balance FROM accounts WHERE accid=" + accid_from,
                    Connection = db.myConn
                };
                adapter = new OleDbDataAdapter(command);
                /* Fill the dataset. */
                adapter.Fill(ds, "accounts");
                /* Get balance of account. */
                Double balance = Convert.ToDouble(ds.Tables["accounts"].Rows[0]["balance"].ToString());
                /* Get amount to be transfered. */
                Double amnt = Convert.ToDouble(no_char.Text);
                /* Check for sufficient funds. */
                if(balance - amnt < 0)
                {
                    MessageBox.Show("Insufficent funds. Account balance remaining: £" + balance.ToString());
                    return;
                }

                /* Construct a command. */
                command = new OleDbCommand()
                {
                    CommandText = "UPDATE accounts SET balance = balance - " + Convert.ToString(amnt) + " WHERE accid=" + accid_from,
                    Connection = db.myConn
                };
                /* Execute the command. */
                command.ExecuteNonQuery();

                DateTime thisDay = DateTime.Today;
                /* Construct a command. */
                command = new OleDbCommand()
                {
                    CommandText = "INSERT INTO tranx (accid, [action], amnt, event) VALUES (" + accid_from + ", 'TRANSFER_OUT', " + amnt + ", '" + thisDay.ToString("d") + "')",
                    Connection = db.myConn
                };
                /* Execute the command. */
                command.ExecuteNonQuery();
                /* Construct a command. */
                command = new OleDbCommand()
                {
                    CommandText = "UPDATE accounts SET balance = balance + " + Convert.ToString(amnt) + " WHERE accid=" + accid_to,
                    Connection = db.myConn
                };
                /* Execute the command. */
                command.ExecuteNonQuery();
                /* Construct a command. */
                command = new OleDbCommand()
                {
                    CommandText = "INSERT INTO tranx (accid, [action], amnt, event) VALUES (" + accid_to + ", 'TRANSFER_IN', " + amnt + ", '" + thisDay.ToString("d") + "')",
                    Connection = db.myConn
                };
                /* Execute the query. */
                command.ExecuteNonQuery();
                /* Display success message. */
                MessageBox.Show("Transfer of £" + amnt + " from account " + accid_from + " to " + accid_to + " successful.");

            };
        }
    }
}