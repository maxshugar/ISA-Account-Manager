﻿namespace ISA_account_manager
{
    partial class outlaw_hess_frm
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.customersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.new_customer_btn = new System.Windows.Forms.ToolStripMenuItem();
            this.view_customers_btn = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.new_product_btn = new System.Windows.Forms.ToolStripMenuItem();
            this.view_products_btn = new System.Windows.Forms.ToolStripMenuItem();
            this.accountsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.new_account_btn = new System.Windows.Forms.ToolStripMenuItem();
            this.view_accounts_btn = new System.Windows.Forms.ToolStripMenuItem();
            this.transactionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.new_transaction_btn = new System.Windows.Forms.ToolStripMenuItem();
            this.view_transactions_btn = new System.Windows.Forms.ToolStripMenuItem();
            this.main_panel = new System.Windows.Forms.Panel();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.customersToolStripMenuItem,
            this.toolStripMenuItem1,
            this.accountsToolStripMenuItem,
            this.transactionsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(9, 3, 0, 3);
            this.menuStrip1.Size = new System.Drawing.Size(1200, 35);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // customersToolStripMenuItem
            // 
            this.customersToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.new_customer_btn,
            this.view_customers_btn});
            this.customersToolStripMenuItem.Name = "customersToolStripMenuItem";
            this.customersToolStripMenuItem.Size = new System.Drawing.Size(109, 29);
            this.customersToolStripMenuItem.Text = "Customers";
            // 
            // new_customer_btn
            // 
            this.new_customer_btn.Name = "new_customer_btn";
            this.new_customer_btn.Size = new System.Drawing.Size(220, 30);
            this.new_customer_btn.Text = "New customer";
            this.new_customer_btn.Click += new System.EventHandler(this.new_customer_btn_Click);
            // 
            // view_customers_btn
            // 
            this.view_customers_btn.Name = "view_customers_btn";
            this.view_customers_btn.Size = new System.Drawing.Size(220, 30);
            this.view_customers_btn.Text = "View customers";
            this.view_customers_btn.Click += new System.EventHandler(this.view_customers_btn_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.new_product_btn,
            this.view_products_btn});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(94, 29);
            this.toolStripMenuItem1.Text = "Products";
            // 
            // new_product_btn
            // 
            this.new_product_btn.Name = "new_product_btn";
            this.new_product_btn.Size = new System.Drawing.Size(209, 30);
            this.new_product_btn.Text = "New product";
            this.new_product_btn.Click += new System.EventHandler(this.new_product_btn_Click);
            // 
            // view_products_btn
            // 
            this.view_products_btn.Name = "view_products_btn";
            this.view_products_btn.Size = new System.Drawing.Size(252, 30);
            this.view_products_btn.Text = "View products";
            this.view_products_btn.Click += new System.EventHandler(this.view_products_btn_Click);
            // 
            // accountsToolStripMenuItem
            // 
            this.accountsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.new_account_btn,
            this.view_accounts_btn});
            this.accountsToolStripMenuItem.Name = "accountsToolStripMenuItem";
            this.accountsToolStripMenuItem.Size = new System.Drawing.Size(97, 29);
            this.accountsToolStripMenuItem.Text = "Accounts";
            // 
            // new_account_btn
            // 
            this.new_account_btn.Name = "new_account_btn";
            this.new_account_btn.Size = new System.Drawing.Size(208, 30);
            this.new_account_btn.Text = "New account";
            this.new_account_btn.Click += new System.EventHandler(this.new_account_btn_Click);
            // 
            // view_accounts_btn
            // 
            this.view_accounts_btn.Name = "view_accounts_btn";
            this.view_accounts_btn.Size = new System.Drawing.Size(252, 30);
            this.view_accounts_btn.Text = "View accounts";
            this.view_accounts_btn.Click += new System.EventHandler(this.view_accounts_btn_Click);
            // 
            // transactionsToolStripMenuItem
            // 
            this.transactionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.new_transaction_btn,
            this.view_transactions_btn});
            this.transactionsToolStripMenuItem.Name = "transactionsToolStripMenuItem";
            this.transactionsToolStripMenuItem.Size = new System.Drawing.Size(120, 29);
            this.transactionsToolStripMenuItem.Text = "Transactions";
            // 
            // new_transaction_btn
            // 
            this.new_transaction_btn.Name = "new_transaction_btn";
            this.new_transaction_btn.Size = new System.Drawing.Size(252, 30);
            this.new_transaction_btn.Text = "New transaction";
            this.new_transaction_btn.Click += new System.EventHandler(this.new_transaction_btn_Click);
            // 
            // view_transactions_btn
            // 
            this.view_transactions_btn.Name = "view_transactions_btn";
            this.view_transactions_btn.Size = new System.Drawing.Size(252, 30);
            this.view_transactions_btn.Text = "View transactions";
            this.view_transactions_btn.Click += new System.EventHandler(this.view_transactions_btn_Click);
            // 
            // main_panel
            // 
            this.main_panel.Location = new System.Drawing.Point(18, 42);
            this.main_panel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.main_panel.Name = "main_panel";
            this.main_panel.Size = new System.Drawing.Size(1164, 632);
            this.main_panel.TabIndex = 1;
            // 
            // outlaw_hess_frm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 692);
            this.Controls.Add(this.main_panel);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "outlaw_hess_frm";
            this.Text = "Outlaw Hess";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem customersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem new_customer_btn;
        private System.Windows.Forms.ToolStripMenuItem view_customers_btn;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem new_product_btn;
        private System.Windows.Forms.ToolStripMenuItem view_products_btn;
        private System.Windows.Forms.ToolStripMenuItem accountsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem new_account_btn;
        private System.Windows.Forms.ToolStripMenuItem view_accounts_btn;
        private System.Windows.Forms.ToolStripMenuItem transactionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem new_transaction_btn;
        private System.Windows.Forms.ToolStripMenuItem view_transactions_btn;
        private System.Windows.Forms.Panel main_panel;
    }
}

