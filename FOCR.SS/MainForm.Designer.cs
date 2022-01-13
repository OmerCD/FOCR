using System.ComponentModel;

namespace FOCR.SS;

partial class MainForm
{
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private IContainer components = null;
    private PictureBox _pictureBox = null;
    private Label _matchCountLabel = null;

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
        this.components = new System.ComponentModel.Container();
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(800, 450);
        
        _pictureBox = new PictureBox();
        _pictureBox.Dock = DockStyle.Fill;
        _pictureBox.SizeMode= PictureBoxSizeMode.StretchImage;
        
        _matchCountLabel = new Label();
        _matchCountLabel.Anchor= AnchorStyles.Top | AnchorStyles.Left;
        _matchCountLabel.Text = "0";
        _matchCountLabel.AutoSize = true;
        _matchCountLabel.Font = new Font("Microsoft Sans Serif", 24F, FontStyle.Bold);
        _matchCountLabel.ForeColor = Color.White;
        _matchCountLabel.BackColor = Color.Transparent;
        
        Controls.Add(_pictureBox);
        Controls.Add(_matchCountLabel);
        _matchCountLabel.BringToFront();
        this.Text = "MainForm";
    }

    #endregion
}