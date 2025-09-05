using FlixHub.Tools.Security.WinApp.Extension;

namespace FlixHub.Tools.Security.WinApp;

public partial class frmMain : Form
{
    public frmMain()
    {
        InitializeComponent();
    }

    private void OnBtnExitClicked(object sender, EventArgs e)
    {
        Application.Exit();
    }

    private void OnBtnEncryptClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtToEncrypt.Text))
        {
            MessageBox.Show("Enter text to encrypt");
            return;
        }

        try
        {
            // do encryption...
            txtToDecrypt.Text = txtToEncrypt.Text.Encrypt();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void OnBtnDecryptClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtToDecrypt.Text))
        {
            MessageBox.Show("Enter text to decrypt");
            return;
        }

        try
        {
            // do decryption...
            txtToEncrypt.Text = txtToDecrypt.Text.Decrypt();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void OnBtnClearAllClicked(object sender, EventArgs e)
    {
        txtToEncrypt.Text = txtToDecrypt.Text = string.Empty;
    }

    private void OnBtnCopyEncryptTextClicked(object sender, EventArgs e)
    {
        Clipboard.SetText(txtToEncrypt.Text);
    }

    private void OnBtnCopyDecryptTextClicked(object sender, EventArgs e)
    {
        Clipboard.SetText(txtToDecrypt.Text);
    }

    private void OnTextToEncryptPreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
    {
        if (e.Control && e.Alt)
        {
            OnBtnEncryptClicked(sender, e);
            e.IsInputKey = false;
        }
    }

    private void OnTextToDecryptPreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
    {
        if (e.Control && e.Alt)
        {
            OnBtnDecryptClicked(sender, e);
            e.IsInputKey = false;
        }
    }

    private void OnBtnToUpperClicked(object sender, EventArgs e)
    {
        txtToEncrypt.Text = txtToEncrypt.Text.ToUpper();
    }

    private void txtToEncrypt_TextChanged(object sender, EventArgs e)
    {
        lblEncryptInputLength.Text = txtToEncrypt.Text.Length.ToString();
    }

    private void txtToDecrypt_TextChanged(object sender, EventArgs e)
    {
        lblDecryptInputLength.Text = txtToDecrypt.Text.Length.ToString();
    }
}