﻿namespace JSON_Tools.Forms
{
    partial class JsonToCsvForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(JsonToCsvForm));
            this.KeySepBox = new System.Windows.Forms.TextBox();
            this.JsonToCsvFormTitle = new System.Windows.Forms.Label();
            this.DelimBox = new System.Windows.Forms.ComboBox();
            this.BoolsToIntsCheckBox = new System.Windows.Forms.CheckBox();
            this.StrategyBox = new System.Windows.Forms.ComboBox();
            this.StrategyBoxLabel = new System.Windows.Forms.Label();
            this.KeySepBoxLabel = new System.Windows.Forms.Label();
            this.DelimBoxLabel = new System.Windows.Forms.Label();
            this.GenerateCSVButton = new System.Windows.Forms.Button();
            this.DocsButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // KeySepBox
            // 
            this.KeySepBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.KeySepBox.Location = new System.Drawing.Point(30, 56);
            this.KeySepBox.MaxLength = 1;
            this.KeySepBox.Name = "KeySepBox";
            this.KeySepBox.Size = new System.Drawing.Size(52, 24);
            this.KeySepBox.TabIndex = 0;
            this.KeySepBox.Text = ".";
            // 
            // JsonToCsvFormTitle
            // 
            this.JsonToCsvFormTitle.AutoSize = true;
            this.JsonToCsvFormTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.JsonToCsvFormTitle.Location = new System.Drawing.Point(53, 9);
            this.JsonToCsvFormTitle.Name = "JsonToCsvFormTitle";
            this.JsonToCsvFormTitle.Size = new System.Drawing.Size(242, 25);
            this.JsonToCsvFormTitle.TabIndex = 1;
            this.JsonToCsvFormTitle.Text = "Create CSV from JSON";
            // 
            // DelimBox
            // 
            this.DelimBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DelimBox.FormattingEnabled = true;
            this.DelimBox.Items.AddRange(new object[] {
            ",",
            "\\t (Tab)",
            "|"});
            this.DelimBox.Location = new System.Drawing.Point(30, 102);
            this.DelimBox.Name = "DelimBox";
            this.DelimBox.Size = new System.Drawing.Size(75, 26);
            this.DelimBox.TabIndex = 1;
            // 
            // BoolsToIntsCheckBox
            // 
            this.BoolsToIntsCheckBox.AutoSize = true;
            this.BoolsToIntsCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BoolsToIntsCheckBox.Location = new System.Drawing.Point(30, 148);
            this.BoolsToIntsCheckBox.Name = "BoolsToIntsCheckBox";
            this.BoolsToIntsCheckBox.Size = new System.Drawing.Size(195, 22);
            this.BoolsToIntsCheckBox.TabIndex = 2;
            this.BoolsToIntsCheckBox.Text = "Convert true/false to 1/0?";
            this.BoolsToIntsCheckBox.UseVisualStyleBackColor = true;
            // 
            // StrategyBox
            // 
            this.StrategyBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StrategyBox.FormattingEnabled = true;
            this.StrategyBox.Items.AddRange(new object[] {
            "Default",
            "Full recursive",
            "No recursion",
            "Stringify iterables"});
            this.StrategyBox.Location = new System.Drawing.Point(30, 188);
            this.StrategyBox.Name = "StrategyBox";
            this.StrategyBox.Size = new System.Drawing.Size(195, 26);
            this.StrategyBox.TabIndex = 3;
            // 
            // StrategyBoxLabel
            // 
            this.StrategyBoxLabel.AutoSize = true;
            this.StrategyBoxLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StrategyBoxLabel.Location = new System.Drawing.Point(245, 191);
            this.StrategyBoxLabel.Name = "StrategyBoxLabel";
            this.StrategyBoxLabel.Size = new System.Drawing.Size(62, 18);
            this.StrategyBoxLabel.TabIndex = 5;
            this.StrategyBoxLabel.Text = "Strategy";
            // 
            // KeySepBoxLabel
            // 
            this.KeySepBoxLabel.AutoSize = true;
            this.KeySepBoxLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.KeySepBoxLabel.Location = new System.Drawing.Point(123, 59);
            this.KeySepBoxLabel.Name = "KeySepBoxLabel";
            this.KeySepBoxLabel.Size = new System.Drawing.Size(102, 18);
            this.KeySepBoxLabel.TabIndex = 6;
            this.KeySepBoxLabel.Text = "Key Separator";
            // 
            // DelimBoxLabel
            // 
            this.DelimBoxLabel.AutoSize = true;
            this.DelimBoxLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DelimBoxLabel.Location = new System.Drawing.Point(123, 105);
            this.DelimBoxLabel.Name = "DelimBoxLabel";
            this.DelimBoxLabel.Size = new System.Drawing.Size(148, 18);
            this.DelimBoxLabel.TabIndex = 7;
            this.DelimBoxLabel.Text = "Delimiter in output file";
            // 
            // GenerateCSVButton
            // 
            this.GenerateCSVButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GenerateCSVButton.Location = new System.Drawing.Point(30, 239);
            this.GenerateCSVButton.Name = "GenerateCSVButton";
            this.GenerateCSVButton.Size = new System.Drawing.Size(120, 27);
            this.GenerateCSVButton.TabIndex = 8;
            this.GenerateCSVButton.Text = "Generate CSV";
            this.GenerateCSVButton.UseVisualStyleBackColor = true;
            this.GenerateCSVButton.Click += new System.EventHandler(this.GenerateCSVButton_Click);
            // 
            // DocsButton
            // 
            this.DocsButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DocsButton.Location = new System.Drawing.Point(226, 239);
            this.DocsButton.Name = "DocsButton";
            this.DocsButton.Size = new System.Drawing.Size(69, 27);
            this.DocsButton.TabIndex = 9;
            this.DocsButton.Text = "Docs";
            this.DocsButton.UseVisualStyleBackColor = true;
            this.DocsButton.Click += new System.EventHandler(this.DocsButton_Click);
            // 
            // JsonToCsvForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(355, 278);
            this.Controls.Add(this.DocsButton);
            this.Controls.Add(this.GenerateCSVButton);
            this.Controls.Add(this.DelimBoxLabel);
            this.Controls.Add(this.KeySepBoxLabel);
            this.Controls.Add(this.StrategyBoxLabel);
            this.Controls.Add(this.StrategyBox);
            this.Controls.Add(this.BoolsToIntsCheckBox);
            this.Controls.Add(this.DelimBox);
            this.Controls.Add(this.JsonToCsvFormTitle);
            this.Controls.Add(this.KeySepBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "JsonToCsvForm";
            this.Text = "JSON to CSV";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox KeySepBox;
        private System.Windows.Forms.Label JsonToCsvFormTitle;
        private System.Windows.Forms.ComboBox DelimBox;
        private System.Windows.Forms.CheckBox BoolsToIntsCheckBox;
        private System.Windows.Forms.ComboBox StrategyBox;
        private System.Windows.Forms.Label StrategyBoxLabel;
        private System.Windows.Forms.Label KeySepBoxLabel;
        private System.Windows.Forms.Label DelimBoxLabel;
        private System.Windows.Forms.Button GenerateCSVButton;
        private System.Windows.Forms.Button DocsButton;
    }
}