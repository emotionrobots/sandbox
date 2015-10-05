<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SplashScreenForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub
    Friend WithEvents CopyrightLabel As System.Windows.Forms.Label
    Friend WithEvents SuperDroidLabel As System.Windows.Forms.Label
    Friend WithEvents MainLayoutPanel As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents DetailsLayoutPanel As System.Windows.Forms.TableLayoutPanel

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SplashScreenForm))
        Me.MainLayoutPanel = New System.Windows.Forms.TableLayoutPanel()
        Me.ApplicationTitle = New System.Windows.Forms.Label()
        Me.VersionLabel = New System.Windows.Forms.Label()
        Me.DateLabel = New System.Windows.Forms.Label()
        Me.DetailsLayoutPanel = New System.Windows.Forms.TableLayoutPanel()
        Me.CopyrightLabel = New System.Windows.Forms.Label()
        Me.SuperDroidLabel = New System.Windows.Forms.Label()
        Me.FooterPanel = New System.Windows.Forms.Panel()
        Me.JoystickMissingLabel = New System.Windows.Forms.Label()
        Me.LoadingInformationLabel = New System.Windows.Forms.Label()
        Me.CloseButton = New System.Windows.Forms.Button()
        Me.TermsLabel = New System.Windows.Forms.Label()
        Me.WarningLabel = New System.Windows.Forms.Label()
        Me.SplasScreenTimer = New System.Windows.Forms.Timer(Me.components)
        Me.MainLayoutPanel.SuspendLayout()
        Me.DetailsLayoutPanel.SuspendLayout()
        Me.FooterPanel.SuspendLayout()
        Me.SuspendLayout()
        '
        'MainLayoutPanel
        '
        Me.MainLayoutPanel.BackColor = System.Drawing.Color.White
        Me.MainLayoutPanel.BackgroundImage = CType(resources.GetObject("MainLayoutPanel.BackgroundImage"), System.Drawing.Image)
        Me.MainLayoutPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.MainLayoutPanel.ColumnCount = 2
        Me.MainLayoutPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 311.0!))
        Me.MainLayoutPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 189.0!))
        Me.MainLayoutPanel.Controls.Add(Me.ApplicationTitle, 1, 0)
        Me.MainLayoutPanel.Controls.Add(Me.VersionLabel, 1, 1)
        Me.MainLayoutPanel.Controls.Add(Me.DateLabel, 1, 2)
        Me.MainLayoutPanel.Controls.Add(Me.DetailsLayoutPanel, 0, 3)
        Me.MainLayoutPanel.Controls.Add(Me.FooterPanel, 0, 7)
        Me.MainLayoutPanel.Controls.Add(Me.CloseButton, 1, 6)
        Me.MainLayoutPanel.Controls.Add(Me.TermsLabel, 0, 5)
        Me.MainLayoutPanel.Controls.Add(Me.WarningLabel, 0, 4)
        Me.MainLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.MainLayoutPanel.Location = New System.Drawing.Point(5, 5)
        Me.MainLayoutPanel.Name = "MainLayoutPanel"
        Me.MainLayoutPanel.RowCount = 8
        Me.MainLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 74.0!))
        Me.MainLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24.0!))
        Me.MainLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24.0!))
        Me.MainLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 91.0!))
        Me.MainLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 54.0!))
        Me.MainLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 104.0!))
        Me.MainLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34.0!))
        Me.MainLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.MainLayoutPanel.Size = New System.Drawing.Size(500, 434)
        Me.MainLayoutPanel.TabIndex = 0
        '
        'ApplicationTitle
        '
        Me.ApplicationTitle.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.ApplicationTitle.BackColor = System.Drawing.Color.Transparent
        Me.ApplicationTitle.Font = New System.Drawing.Font("Microsoft Sans Serif", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ApplicationTitle.ForeColor = System.Drawing.Color.RoyalBlue
        Me.ApplicationTitle.Location = New System.Drawing.Point(314, 2)
        Me.ApplicationTitle.Name = "ApplicationTitle"
        Me.ApplicationTitle.Size = New System.Drawing.Size(183, 69)
        Me.ApplicationTitle.TabIndex = 3
        Me.ApplicationTitle.Text = "RP2W"
        Me.ApplicationTitle.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'VersionLabel
        '
        Me.VersionLabel.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.VersionLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.VersionLabel.Location = New System.Drawing.Point(317, 75)
        Me.VersionLabel.Name = "VersionLabel"
        Me.VersionLabel.Size = New System.Drawing.Size(177, 22)
        Me.VersionLabel.TabIndex = 4
        Me.VersionLabel.Text = "---"
        '
        'DateLabel
        '
        Me.DateLabel.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.DateLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.DateLabel.Location = New System.Drawing.Point(320, 99)
        Me.DateLabel.Name = "DateLabel"
        Me.DateLabel.Size = New System.Drawing.Size(171, 21)
        Me.DateLabel.TabIndex = 5
        Me.DateLabel.Text = "February 2010"
        Me.DateLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'DetailsLayoutPanel
        '
        Me.DetailsLayoutPanel.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.DetailsLayoutPanel.BackColor = System.Drawing.Color.Transparent
        Me.DetailsLayoutPanel.ColumnCount = 1
        Me.MainLayoutPanel.SetColumnSpan(Me.DetailsLayoutPanel, 2)
        Me.DetailsLayoutPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 247.0!))
        Me.DetailsLayoutPanel.Controls.Add(Me.CopyrightLabel, 0, 1)
        Me.DetailsLayoutPanel.Controls.Add(Me.SuperDroidLabel, 0, 0)
        Me.DetailsLayoutPanel.Location = New System.Drawing.Point(126, 148)
        Me.DetailsLayoutPanel.Name = "DetailsLayoutPanel"
        Me.DetailsLayoutPanel.RowCount = 2
        Me.DetailsLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.DetailsLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.DetailsLayoutPanel.Size = New System.Drawing.Size(247, 39)
        Me.DetailsLayoutPanel.TabIndex = 1
        '
        'CopyrightLabel
        '
        Me.CopyrightLabel.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.CopyrightLabel.BackColor = System.Drawing.Color.Transparent
        Me.CopyrightLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CopyrightLabel.Location = New System.Drawing.Point(3, 19)
        Me.CopyrightLabel.Name = "CopyrightLabel"
        Me.CopyrightLabel.Size = New System.Drawing.Size(241, 20)
        Me.CopyrightLabel.TabIndex = 1
        Me.CopyrightLabel.Text = "Copyrighted 2012"
        Me.CopyrightLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'SuperDroidLabel
        '
        Me.SuperDroidLabel.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.SuperDroidLabel.BackColor = System.Drawing.Color.Transparent
        Me.SuperDroidLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SuperDroidLabel.Location = New System.Drawing.Point(3, 0)
        Me.SuperDroidLabel.Name = "SuperDroidLabel"
        Me.SuperDroidLabel.Size = New System.Drawing.Size(241, 19)
        Me.SuperDroidLabel.TabIndex = 2
        Me.SuperDroidLabel.Text = "SuperDroid Robots Inc."
        Me.SuperDroidLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'FooterPanel
        '
        Me.FooterPanel.BackColor = System.Drawing.Color.RoyalBlue
        Me.MainLayoutPanel.SetColumnSpan(Me.FooterPanel, 2)
        Me.FooterPanel.Controls.Add(Me.JoystickMissingLabel)
        Me.FooterPanel.Controls.Add(Me.LoadingInformationLabel)
        Me.FooterPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FooterPanel.Location = New System.Drawing.Point(0, 405)
        Me.FooterPanel.Margin = New System.Windows.Forms.Padding(0)
        Me.FooterPanel.Name = "FooterPanel"
        Me.FooterPanel.Size = New System.Drawing.Size(500, 29)
        Me.FooterPanel.TabIndex = 6
        '
        'JoystickMissingLabel
        '
        Me.JoystickMissingLabel.BackColor = System.Drawing.Color.MidnightBlue
        Me.JoystickMissingLabel.Dock = System.Windows.Forms.DockStyle.Right
        Me.JoystickMissingLabel.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.JoystickMissingLabel.ForeColor = System.Drawing.Color.Red
        Me.JoystickMissingLabel.Location = New System.Drawing.Point(309, 0)
        Me.JoystickMissingLabel.Name = "JoystickMissingLabel"
        Me.JoystickMissingLabel.Padding = New System.Windows.Forms.Padding(0, 0, 25, 0)
        Me.JoystickMissingLabel.Size = New System.Drawing.Size(191, 29)
        Me.JoystickMissingLabel.TabIndex = 1
        Me.JoystickMissingLabel.Text = "No joystick attached."
        Me.JoystickMissingLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.JoystickMissingLabel.Visible = False
        '
        'LoadingInformationLabel
        '
        Me.LoadingInformationLabel.BackColor = System.Drawing.Color.Transparent
        Me.LoadingInformationLabel.Dock = System.Windows.Forms.DockStyle.Left
        Me.LoadingInformationLabel.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.LoadingInformationLabel.ForeColor = System.Drawing.Color.White
        Me.LoadingInformationLabel.Location = New System.Drawing.Point(0, 0)
        Me.LoadingInformationLabel.Name = "LoadingInformationLabel"
        Me.LoadingInformationLabel.Padding = New System.Windows.Forms.Padding(25, 0, 0, 0)
        Me.LoadingInformationLabel.Size = New System.Drawing.Size(247, 29)
        Me.LoadingInformationLabel.TabIndex = 0
        Me.LoadingInformationLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'CloseButton
        '
        Me.CloseButton.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.CloseButton.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.CloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.CloseButton.FlatAppearance.BorderColor = System.Drawing.SystemColors.ButtonFace
        Me.CloseButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CloseButton.Location = New System.Drawing.Point(377, 376)
        Me.CloseButton.Margin = New System.Windows.Forms.Padding(3, 3, 25, 3)
        Me.CloseButton.Name = "CloseButton"
        Me.CloseButton.Size = New System.Drawing.Size(98, 23)
        Me.CloseButton.TabIndex = 7
        Me.CloseButton.Text = "Close"
        Me.CloseButton.UseVisualStyleBackColor = False
        '
        'TermsLabel
        '
        Me.TermsLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.MainLayoutPanel.SetColumnSpan(Me.TermsLabel, 2)
        Me.TermsLabel.Location = New System.Drawing.Point(3, 277)
        Me.TermsLabel.Name = "TermsLabel"
        Me.TermsLabel.Size = New System.Drawing.Size(494, 84)
        Me.TermsLabel.TabIndex = 8
        Me.TermsLabel.Text = resources.GetString("TermsLabel.Text")
        '
        'WarningLabel
        '
        Me.WarningLabel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.WarningLabel.AutoSize = True
        Me.MainLayoutPanel.SetColumnSpan(Me.WarningLabel, 2)
        Me.WarningLabel.Location = New System.Drawing.Point(3, 228)
        Me.WarningLabel.Name = "WarningLabel"
        Me.WarningLabel.Size = New System.Drawing.Size(494, 39)
        Me.WarningLabel.TabIndex = 9
        Me.WarningLabel.Text = resources.GetString("WarningLabel.Text")
        '
        'SplasScreenTimer
        '
        Me.SplasScreenTimer.Interval = 3000
        '
        'SplashScreenForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.CloseButton
        Me.ClientSize = New System.Drawing.Size(510, 444)
        Me.ControlBox = False
        Me.Controls.Add(Me.MainLayoutPanel)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "SplashScreenForm"
        Me.Padding = New System.Windows.Forms.Padding(5)
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.MainLayoutPanel.ResumeLayout(False)
        Me.MainLayoutPanel.PerformLayout()
        Me.DetailsLayoutPanel.ResumeLayout(False)
        Me.FooterPanel.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ApplicationTitle As System.Windows.Forms.Label
    Friend WithEvents VersionLabel As System.Windows.Forms.Label
    Friend WithEvents DateLabel As System.Windows.Forms.Label
    Friend WithEvents FooterPanel As System.Windows.Forms.Panel
    Friend WithEvents CloseButton As System.Windows.Forms.Button
    Friend WithEvents TermsLabel As System.Windows.Forms.Label
    Friend WithEvents WarningLabel As System.Windows.Forms.Label
    Friend WithEvents JoystickMissingLabel As System.Windows.Forms.Label
    Friend WithEvents LoadingInformationLabel As System.Windows.Forms.Label
    Friend WithEvents SplasScreenTimer As System.Windows.Forms.Timer

End Class
