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
        private void generate_input_form(object[,] input, string command_text)
        {
            int start_x = 10;
            int start_y = 10;

            Label title = new Label
            {
                Text = "New Customer",
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
                start_y += 25;
            }
            /* Save button. */
            Button save_btn = new Button()
            {
                Width = 130,
                Location = new System.Drawing.Point(10, 60 + (45 * 4)),
                Text = "Save"
            };
            main_panel.Controls.Add(save_btn);
            save_btn.BringToFront();
            /* Event listener. */
            save_btn.Click += (s, e) => {


                OleDbCommand command = new OleDbCommand();

                // Set the Connection, CommandText and Parameters.
                //command.Connection = db.myConn;
                command.CommandText = command_text;
                //command.Parameters.Add("title", OleDbType.Integer, 6);
                //command.Parameters[0].Value = 20;

                string[] columns =
                {
                    "title", "firstname", "lastname", "dob", "natins", "email", "pswd"

                };

                for (int i = 0; i < input.GetLength(0); i++)
                {
     
                    if (input[i, 1] is TextBox)
                    {
                        TextBox element = (TextBox)input[i, 1];
                        command.Parameters.Add(columns[i], OleDbType.VarChar, 50);
                        command.Parameters[i].Value = element.Text;
                    }
                    if (input[i, 1] is DateTimePicker)
                    {
                        DateTimePicker element = (DateTimePicker)input[i, 1];
                        command.Parameters.Add(columns[i], OleDbType.Date);
                        command.Parameters[i].Value = element.Text;
                    }
   
                }
                MessageBox.Show(command.Parameters[0].ToString());
                //command.Prepare();
                
                
                // cmd.ExecuteNonQuery();

                /* Execute Query. */
                // db.Execute(command);
                /* Send feedback. */
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
            //TextBox test = (TextBox)input[0, 1];
            main_panel.Controls.Clear();
            /* Generate Command */
            String command_text = "INSERT INTO dbo.customers (title, firstname, lastname, dob, natins, email, pswd) VALUES ( ?, ?, ?, ?, ?, ?, ? )";
            /* Generate form. */
            generate_input_form(input, command_text);


            /* 
            // Create the Command.
            OleDbCommand command = new OleDbCommand();

            // Set the Connection, CommandText and Parameters.
            command.CommandText =
                "INSERT INTO dbo.Region (RegionID, RegionDescription) VALUES (?, ?)";
            command.Parameters.Add("RegionID", OleDbType.Integer, 4);
            command.Parameters.Add("RegionDescription", OleDbType.VarWChar, 50);
            command.Parameters[0].Value = 20;
            command.Parameters[1].Value = "First Region";

            MessageBox.Show(command.Parameters[1].Value.ToString());
            */
        }




        private void view_customers_btn_Click(object sender, EventArgs e)
        {
            main_panel.Controls.Clear();
            DataGridView data_grid = new DataGridView()
            {

            };
            main_panel.Controls.Add(data_grid);
        }
    }
}
