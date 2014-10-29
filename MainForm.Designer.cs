namespace xml_valid
{
    partial class MainForm
    {
        /// <summary>
        /// Требуется переменная конструктора.
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
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.start = new System.Windows.Forms.Button();
            this.log = new System.Windows.Forms.Label();
            this.field = new System.Windows.Forms.RichTextBox();
            this.result = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // start
            // 
            this.start.Location = new System.Drawing.Point(811, 354);
            this.start.Name = "start";
            this.start.Size = new System.Drawing.Size(110, 23);
            this.start.TabIndex = 0;
            this.start.Text = "Валидировать";
            this.start.UseVisualStyleBackColor = true;
            this.start.Click += new System.EventHandler(this.btnClick);
            // 
            // log
            // 
            this.log.AutoSize = true;
            this.log.Location = new System.Drawing.Point(821, 408);
            this.log.Name = "log";
            this.log.Size = new System.Drawing.Size(100, 13);
            this.log.TabIndex = 2;
            this.log.Text = "Ожидание ошибок";
            // 
            // field
            // 
            this.field.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.field.Location = new System.Drawing.Point(12, 13);
            this.field.Name = "field";
            this.field.Size = new System.Drawing.Size(922, 310);
            this.field.TabIndex = 3;
            this.field.Text = "";
            // 
            // result
            // 
            this.result.Font = new System.Drawing.Font("Segoe UI Symbol", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.result.Location = new System.Drawing.Point(12, 354);
            this.result.Name = "result";
            this.result.ReadOnly = true;
            this.result.Size = new System.Drawing.Size(763, 100);
            this.result.TabIndex = 4;
            this.result.Text = "";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(949, 471);
            this.Controls.Add(this.result);
            this.Controls.Add(this.field);
            this.Controls.Add(this.log);
            this.Controls.Add(this.start);
            this.Name = "MainForm";
            this.Text = "Xml Valid";
            this.Load += new System.EventHandler(this.formLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button start;
        private System.Windows.Forms.Label log;
        private System.Windows.Forms.RichTextBox field;
        private System.Windows.Forms.RichTextBox result;


    }
}

