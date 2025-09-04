namespace Mabusall.Tools.Security.WinApp;

partial class frmMain
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
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
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        btnExitApp = new Button();
        btnDecrypt = new Button();
        txtToDecrypt = new TextBox();
        label2 = new Label();
        btnEncrypt = new Button();
        txtToEncrypt = new TextBox();
        label1 = new Label();
        btnClearAll = new Button();
        button1 = new Button();
        button2 = new Button();
        button3 = new Button();
        lblEncryptInputLength = new Label();
        lblDecryptInputLength = new Label();
        SuspendLayout();
        // 
        // btnExitApp
        // 
        btnExitApp.Location = new Point(352, 347);
        btnExitApp.Name = "btnExitApp";
        btnExitApp.Size = new Size(75, 23);
        btnExitApp.TabIndex = 20;
        btnExitApp.Text = "Exit";
        btnExitApp.UseVisualStyleBackColor = true;
        btnExitApp.Click += OnBtnExitClicked;
        // 
        // btnDecrypt
        // 
        btnDecrypt.Location = new Point(127, 292);
        btnDecrypt.Name = "btnDecrypt";
        btnDecrypt.Size = new Size(75, 23);
        btnDecrypt.TabIndex = 19;
        btnDecrypt.Text = "Decrypt";
        btnDecrypt.UseVisualStyleBackColor = true;
        btnDecrypt.Click += OnBtnDecryptClicked;
        // 
        // txtToDecrypt
        // 
        txtToDecrypt.Location = new Point(127, 208);
        txtToDecrypt.Multiline = true;
        txtToDecrypt.Name = "txtToDecrypt";
        txtToDecrypt.Size = new Size(300, 78);
        txtToDecrypt.TabIndex = 18;
        txtToDecrypt.TextChanged += txtToDecrypt_TextChanged;
        txtToDecrypt.PreviewKeyDown += OnTextToDecryptPreviewKeyDown;
        // 
        // label2
        // 
        label2.AutoSize = true;
        label2.Location = new Point(33, 211);
        label2.Name = "label2";
        label2.Size = new Size(88, 15);
        label2.TabIndex = 17;
        label2.Text = "Text to decrypt:";
        // 
        // btnEncrypt
        // 
        btnEncrypt.Location = new Point(127, 117);
        btnEncrypt.Name = "btnEncrypt";
        btnEncrypt.Size = new Size(75, 23);
        btnEncrypt.TabIndex = 16;
        btnEncrypt.Text = "Encrypt";
        btnEncrypt.UseVisualStyleBackColor = true;
        btnEncrypt.Click += OnBtnEncryptClicked;
        // 
        // txtToEncrypt
        // 
        txtToEncrypt.Location = new Point(127, 33);
        txtToEncrypt.Multiline = true;
        txtToEncrypt.Name = "txtToEncrypt";
        txtToEncrypt.Size = new Size(300, 78);
        txtToEncrypt.TabIndex = 15;
        txtToEncrypt.TextChanged += txtToEncrypt_TextChanged;
        txtToEncrypt.PreviewKeyDown += OnTextToEncryptPreviewKeyDown;
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Location = new Point(33, 36);
        label1.Name = "label1";
        label1.Size = new Size(88, 15);
        label1.TabIndex = 14;
        label1.Text = "Text to encrypt:";
        // 
        // btnClearAll
        // 
        btnClearAll.Location = new Point(271, 347);
        btnClearAll.Name = "btnClearAll";
        btnClearAll.Size = new Size(75, 23);
        btnClearAll.TabIndex = 21;
        btnClearAll.Text = "Clear All";
        btnClearAll.UseVisualStyleBackColor = true;
        btnClearAll.Click += OnBtnClearAllClicked;
        // 
        // button1
        // 
        button1.Location = new Point(352, 117);
        button1.Name = "button1";
        button1.Size = new Size(75, 23);
        button1.TabIndex = 22;
        button1.Text = "Copy";
        button1.UseVisualStyleBackColor = true;
        button1.Click += OnBtnCopyEncryptTextClicked;
        // 
        // button2
        // 
        button2.Location = new Point(352, 292);
        button2.Name = "button2";
        button2.Size = new Size(75, 23);
        button2.TabIndex = 23;
        button2.Text = "Copy";
        button2.UseVisualStyleBackColor = true;
        button2.Click += OnBtnCopyDecryptTextClicked;
        // 
        // button3
        // 
        button3.Location = new Point(208, 117);
        button3.Name = "button3";
        button3.Size = new Size(75, 23);
        button3.TabIndex = 16;
        button3.Text = "To Upper";
        button3.UseVisualStyleBackColor = true;
        button3.Click += OnBtnToUpperClicked;
        // 
        // lblEncryptInputLength
        // 
        lblEncryptInputLength.AutoSize = true;
        lblEncryptInputLength.Location = new Point(414, 15);
        lblEncryptInputLength.Name = "lblEncryptInputLength";
        lblEncryptInputLength.Size = new Size(13, 15);
        lblEncryptInputLength.TabIndex = 14;
        lblEncryptInputLength.Text = "0";
        // 
        // lblDecryptInputLength
        // 
        lblDecryptInputLength.AutoSize = true;
        lblDecryptInputLength.Location = new Point(414, 190);
        lblDecryptInputLength.Name = "lblDecryptInputLength";
        lblDecryptInputLength.Size = new Size(13, 15);
        lblDecryptInputLength.TabIndex = 14;
        lblDecryptInputLength.Text = "0";
        // 
        // frmMain
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = btnExitApp;
        ClientSize = new Size(464, 394);
        Controls.Add(button2);
        Controls.Add(button1);
        Controls.Add(btnClearAll);
        Controls.Add(btnExitApp);
        Controls.Add(btnDecrypt);
        Controls.Add(txtToDecrypt);
        Controls.Add(label2);
        Controls.Add(button3);
        Controls.Add(btnEncrypt);
        Controls.Add(txtToEncrypt);
        Controls.Add(lblDecryptInputLength);
        Controls.Add(lblEncryptInputLength);
        Controls.Add(label1);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        Name = "frmMain";
        Padding = new Padding(30);
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Tasheer Security Tool";
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Button btnExitApp;
    private Button btnDecrypt;
    private TextBox txtToDecrypt;
    private Label label2;
    private Button btnEncrypt;
    private TextBox txtToEncrypt;
    private Label label1;
    private Button btnClearAll;
    private Button button1;
    private Button button2;
    private Button button3;
    private Label lblEncryptInputLength;
    private Label lblDecryptInputLength;
}