namespace MAPZ1_Interpreter
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.CodeBox = new System.Windows.Forms.RichTextBox();
            this.run_btn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.richTextBox2 = new System.Windows.Forms.RichTextBox();
            this.display_tree_btn = new System.Windows.Forms.Button();
            this.display_tokens_btn = new System.Windows.Forms.Button();
            this.print_functions_btn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // CodeBox
            // 
            this.CodeBox.Location = new System.Drawing.Point(12, 12);
            this.CodeBox.Name = "CodeBox";
            this.CodeBox.Size = new System.Drawing.Size(311, 203);
            this.CodeBox.TabIndex = 0;
            this.CodeBox.Text = "";
            // 
            // run_btn
            // 
            this.run_btn.Location = new System.Drawing.Point(134, 221);
            this.run_btn.Name = "run_btn";
            this.run_btn.Size = new System.Drawing.Size(75, 23);
            this.run_btn.TabIndex = 1;
            this.run_btn.Text = "Run";
            this.run_btn.UseVisualStyleBackColor = true;
            this.run_btn.Click += new System.EventHandler(this.run_btn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(384, 282);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 13);
            this.label1.TabIndex = 2;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(1, 326);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(798, 124);
            this.richTextBox1.TabIndex = 3;
            this.richTextBox1.Text = "";
            // 
            // richTextBox2
            // 
            this.richTextBox2.Location = new System.Drawing.Point(490, 12);
            this.richTextBox2.Name = "richTextBox2";
            this.richTextBox2.Size = new System.Drawing.Size(298, 202);
            this.richTextBox2.TabIndex = 4;
            this.richTextBox2.Text = "";
            // 
            // display_tree_btn
            // 
            this.display_tree_btn.Location = new System.Drawing.Point(592, 221);
            this.display_tree_btn.Name = "display_tree_btn";
            this.display_tree_btn.Size = new System.Drawing.Size(102, 23);
            this.display_tree_btn.TabIndex = 5;
            this.display_tree_btn.Text = "Display the tree";
            this.display_tree_btn.UseVisualStyleBackColor = true;
            this.display_tree_btn.Click += new System.EventHandler(this.display_btn_Click);
            // 
            // display_tokens_btn
            // 
            this.display_tokens_btn.Location = new System.Drawing.Point(592, 250);
            this.display_tokens_btn.Name = "display_tokens_btn";
            this.display_tokens_btn.Size = new System.Drawing.Size(102, 23);
            this.display_tokens_btn.TabIndex = 7;
            this.display_tokens_btn.Text = "Display tokens";
            this.display_tokens_btn.UseVisualStyleBackColor = true;
            this.display_tokens_btn.Click += new System.EventHandler(this.display_tokens_btn_Click);
            // 
            // print_functions_btn
            // 
            this.print_functions_btn.Location = new System.Drawing.Point(592, 282);
            this.print_functions_btn.Name = "print_functions_btn";
            this.print_functions_btn.Size = new System.Drawing.Size(102, 23);
            this.print_functions_btn.TabIndex = 8;
            this.print_functions_btn.Text = "Display functions list";
            this.print_functions_btn.UseVisualStyleBackColor = true;
            this.print_functions_btn.Click += new System.EventHandler(this.print_functions_btn_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.print_functions_btn);
            this.Controls.Add(this.display_tokens_btn);
            this.Controls.Add(this.display_tree_btn);
            this.Controls.Add(this.richTextBox2);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.run_btn);
            this.Controls.Add(this.CodeBox);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox CodeBox;
        private System.Windows.Forms.Button run_btn;
        public System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox richTextBox2;
        private System.Windows.Forms.Button display_tree_btn;
        public System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button display_tokens_btn;
        private System.Windows.Forms.Button print_functions_btn;
    }
}

