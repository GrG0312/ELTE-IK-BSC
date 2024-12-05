namespace BlackHole.WinForms.View
{
    partial class GameForm
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
            mainMenu = new MenuStrip();
            newGameMenuItem = new ToolStripMenuItem();
            smallNewGameMenuItem = new ToolStripMenuItem();
            mediumNewGameMenuItem = new ToolStripMenuItem();
            largeNewGameMenuItem = new ToolStripMenuItem();
            saveGameMenuItem = new ToolStripMenuItem();
            loadGameMenuItem = new ToolStripMenuItem();
            statusStrip1 = new StatusStrip();
            player1Text = new ToolStripStatusLabel();
            player1shipsNumber = new ToolStripStatusLabel();
            player2Text = new ToolStripStatusLabel();
            player2shipsNumber = new ToolStripStatusLabel();
            saveFileDialog = new SaveFileDialog();
            openFileDialog = new OpenFileDialog();
            mainMenu.SuspendLayout();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // mainMenu
            // 
            mainMenu.Items.AddRange(new ToolStripItem[] { newGameMenuItem, saveGameMenuItem, loadGameMenuItem });
            mainMenu.Location = new Point(0, 0);
            mainMenu.Name = "mainMenu";
            mainMenu.Size = new Size(984, 24);
            mainMenu.TabIndex = 0;
            mainMenu.Text = "menuStrip1";
            // 
            // newGameMenuItem
            // 
            newGameMenuItem.DropDownItems.AddRange(new ToolStripItem[] { smallNewGameMenuItem, mediumNewGameMenuItem, largeNewGameMenuItem });
            newGameMenuItem.Name = "newGameMenuItem";
            newGameMenuItem.Size = new Size(77, 20);
            newGameMenuItem.Text = "New Game";
            // 
            // smallNewGameMenuItem
            // 
            smallNewGameMenuItem.Name = "smallNewGameMenuItem";
            smallNewGameMenuItem.Size = new Size(119, 22);
            smallNewGameMenuItem.Text = "Small";
            smallNewGameMenuItem.Click += smallNewGameMenuItem_Click;
            // 
            // mediumNewGameMenuItem
            // 
            mediumNewGameMenuItem.Name = "mediumNewGameMenuItem";
            mediumNewGameMenuItem.Size = new Size(119, 22);
            mediumNewGameMenuItem.Text = "Medium";
            mediumNewGameMenuItem.Click += mediumNewGameMenuItem_Click;
            // 
            // largeNewGameMenuItem
            // 
            largeNewGameMenuItem.Name = "largeNewGameMenuItem";
            largeNewGameMenuItem.Size = new Size(119, 22);
            largeNewGameMenuItem.Text = "Large";
            largeNewGameMenuItem.Click += largeNewGameMenuItem_Click;
            // 
            // saveGameMenuItem
            // 
            saveGameMenuItem.Name = "saveGameMenuItem";
            saveGameMenuItem.Size = new Size(77, 20);
            saveGameMenuItem.Text = "Save Game";
            saveGameMenuItem.Click += saveGameMenuItem_Click;
            // 
            // loadGameMenuItem
            // 
            loadGameMenuItem.Name = "loadGameMenuItem";
            loadGameMenuItem.Size = new Size(79, 20);
            loadGameMenuItem.Text = "Load Game";
            loadGameMenuItem.Click += loadGameMenuItem_Click;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { player1Text, player1shipsNumber, player2Text, player2shipsNumber });
            statusStrip1.Location = new Point(0, 920);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(984, 41);
            statusStrip1.TabIndex = 1;
            statusStrip1.Text = "statusStrip1";
            // 
            // player1Text
            // 
            player1Text.BackColor = Color.Red;
            player1Text.Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point);
            player1Text.Name = "player1Text";
            player1Text.Size = new Size(144, 36);
            player1Text.Text = "Player ships:";
            // 
            // player1shipsNumber
            // 
            player1shipsNumber.BackColor = Color.Red;
            player1shipsNumber.BorderSides = ToolStripStatusLabelBorderSides.Right;
            player1shipsNumber.BorderStyle = Border3DStyle.Etched;
            player1shipsNumber.Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point);
            player1shipsNumber.Name = "player1shipsNumber";
            player1shipsNumber.Size = new Size(31, 36);
            player1shipsNumber.Text = "0";
            // 
            // player2Text
            // 
            player2Text.BackColor = Color.Blue;
            player2Text.Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point);
            player2Text.ForeColor = SystemColors.Control;
            player2Text.Name = "player2Text";
            player2Text.Size = new Size(144, 36);
            player2Text.Text = "Player ships:";
            // 
            // player2shipsNumber
            // 
            player2shipsNumber.BackColor = Color.Blue;
            player2shipsNumber.BorderSides = ToolStripStatusLabelBorderSides.Right;
            player2shipsNumber.BorderStyle = Border3DStyle.Etched;
            player2shipsNumber.Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point);
            player2shipsNumber.ForeColor = SystemColors.Control;
            player2shipsNumber.Name = "player2shipsNumber";
            player2shipsNumber.Size = new Size(31, 36);
            player2shipsNumber.Text = "0";
            // 
            // saveFileDialog
            // 
            saveFileDialog.Filter = "Black-hole table (*.bht)|*.bht";
            saveFileDialog.Title = "Saving Black-Hole table";
            // 
            // openFileDialog
            // 
            openFileDialog.FileName = "openFileDialog1";
            openFileDialog.Filter = "Black-hole table (*.bht)|*.bht";
            openFileDialog.Title = "Load Black-Hole table";
            // 
            // GameForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(984, 961);
            Controls.Add(statusStrip1);
            Controls.Add(mainMenu);
            MainMenuStrip = mainMenu;
            Name = "GameForm";
            Text = "Black-hole Game";
            mainMenu.ResumeLayout(false);
            mainMenu.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip mainMenu;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel player1Text;
        private ToolStripStatusLabel player1shipsNumber;
        private ToolStripStatusLabel player2Text;
        private ToolStripStatusLabel player2shipsNumber;
        private ToolStripMenuItem newGameMenuItem;
        private ToolStripMenuItem smallNewGameMenuItem;
        private ToolStripMenuItem mediumNewGameMenuItem;
        private ToolStripMenuItem largeNewGameMenuItem;
        private ToolStripMenuItem loadGameMenuItem;
        private ToolStripMenuItem saveGameMenuItem;
        private SaveFileDialog saveFileDialog;
        private OpenFileDialog openFileDialog;
    }
}