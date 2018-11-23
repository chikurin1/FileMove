<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FileMoveForm
    Inherits System.Windows.Forms.Form

    'フォームがコンポーネントの一覧をクリーンアップするために dispose をオーバーライドします。
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

    'Windows フォーム デザイナーで必要です。
    Private components As System.ComponentModel.IContainer

    'メモ: 以下のプロシージャは Windows フォーム デザイナーで必要です。
    'Windows フォーム デザイナーを使用して変更できます。  
    'コード エディターを使って変更しないでください。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.treeDir = New System.Windows.Forms.TreeView()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.lstThumbs = New System.Windows.Forms.ListView()
        Me.ilstThumbs = New System.Windows.Forms.ImageList(Me.components)
        Me.picThumbs = New System.Windows.Forms.PictureBox()
        Me.txtFileName = New System.Windows.Forms.TextBox()
        Me.lblFilePath = New System.Windows.Forms.Label()
        Me.txtTag1 = New System.Windows.Forms.TextBox()
        Me.txtTag2 = New System.Windows.Forms.TextBox()
        Me.cmbTag1 = New System.Windows.Forms.ComboBox()
        Me.cmbTag2 = New System.Windows.Forms.ComboBox()
        Me.btnMove = New System.Windows.Forms.Button()
        Me.cmbTag4 = New System.Windows.Forms.ComboBox()
        Me.cmbTag3 = New System.Windows.Forms.ComboBox()
        Me.txtTag4 = New System.Windows.Forms.TextBox()
        Me.txtTag3 = New System.Windows.Forms.TextBox()
        Me.cmbTag6 = New System.Windows.Forms.ComboBox()
        Me.cmbTag5 = New System.Windows.Forms.ComboBox()
        Me.txtTag6 = New System.Windows.Forms.TextBox()
        Me.txtTag5 = New System.Windows.Forms.TextBox()
        Me.btnAddFolder = New System.Windows.Forms.Button()
        Me.btnDelete = New System.Windows.Forms.Button()
        Me.btnUpdate = New System.Windows.Forms.Button()
        Me.btnNowDel = New System.Windows.Forms.Button()
        Me.lblFileSize = New System.Windows.Forms.Label()
        Me.cmbRank = New System.Windows.Forms.ComboBox()
        Me.lblNowFolder = New System.Windows.Forms.Label()
        Me.btnRankSearch = New System.Windows.Forms.Button()
        Me.btnAddDaySearch = New System.Windows.Forms.Button()
        Me.chkOver = New System.Windows.Forms.CheckBox()
        Me.btnFolderTagAdd = New System.Windows.Forms.Button()
        Me.btnFolderDel = New System.Windows.Forms.Button()
        Me.btnNameSearch = New System.Windows.Forms.Button()
        Me.btnTagSettinn = New System.Windows.Forms.Button()
        Me.txtTagSetting = New System.Windows.Forms.TextBox()
        Me.chkGoogle = New System.Windows.Forms.CheckBox()
        Me.TabPage3 = New System.Windows.Forms.TabPage()
        Me.WebBrowser1 = New System.Windows.Forms.WebBrowser()
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        CType(Me.picThumbs, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage3.SuspendLayout()
        Me.SuspendLayout()
        '
        'TabControl1
        '
        Me.TabControl1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Controls.Add(Me.TabPage3)
        Me.TabControl1.Location = New System.Drawing.Point(0, 150)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(1003, 491)
        Me.TabControl1.TabIndex = 0
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.treeDir)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(995, 465)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "TabPage1"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'treeDir
        '
        Me.treeDir.Dock = System.Windows.Forms.DockStyle.Fill
        Me.treeDir.Font = New System.Drawing.Font("メイリオ", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.treeDir.ForeColor = System.Drawing.SystemColors.WindowText
        Me.treeDir.Location = New System.Drawing.Point(3, 3)
        Me.treeDir.Name = "treeDir"
        Me.treeDir.Size = New System.Drawing.Size(989, 459)
        Me.treeDir.TabIndex = 16
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.lstThumbs)
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(995, 465)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "TabPage2"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'lstThumbs
        '
        Me.lstThumbs.BackColor = System.Drawing.SystemColors.Window
        Me.lstThumbs.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lstThumbs.Font = New System.Drawing.Font("メイリオ", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lstThumbs.Location = New System.Drawing.Point(3, 3)
        Me.lstThumbs.Name = "lstThumbs"
        Me.lstThumbs.ShowItemToolTips = True
        Me.lstThumbs.Size = New System.Drawing.Size(989, 459)
        Me.lstThumbs.SmallImageList = Me.ilstThumbs
        Me.lstThumbs.TabIndex = 28
        Me.lstThumbs.UseCompatibleStateImageBehavior = False
        '
        'ilstThumbs
        '
        Me.ilstThumbs.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit
        Me.ilstThumbs.ImageSize = New System.Drawing.Size(16, 16)
        Me.ilstThumbs.TransparentColor = System.Drawing.Color.Transparent
        '
        'picThumbs
        '
        Me.picThumbs.Location = New System.Drawing.Point(0, 0)
        Me.picThumbs.Name = "picThumbs"
        Me.picThumbs.Size = New System.Drawing.Size(88, 124)
        Me.picThumbs.TabIndex = 1
        Me.picThumbs.TabStop = False
        '
        'txtFileName
        '
        Me.txtFileName.Font = New System.Drawing.Font("メイリオ", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.txtFileName.ImeMode = System.Windows.Forms.ImeMode.Hiragana
        Me.txtFileName.Location = New System.Drawing.Point(100, 38)
        Me.txtFileName.Name = "txtFileName"
        Me.txtFileName.Size = New System.Drawing.Size(151, 25)
        Me.txtFileName.TabIndex = 1
        '
        'lblFilePath
        '
        Me.lblFilePath.AutoSize = True
        Me.lblFilePath.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFilePath.Location = New System.Drawing.Point(95, 9)
        Me.lblFilePath.Name = "lblFilePath"
        Me.lblFilePath.Size = New System.Drawing.Size(49, 14)
        Me.lblFilePath.TabIndex = 3
        Me.lblFilePath.Text = "Label1"
        '
        'txtTag1
        '
        Me.txtTag1.Font = New System.Drawing.Font("メイリオ", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.txtTag1.ImeMode = System.Windows.Forms.ImeMode.Hiragana
        Me.txtTag1.Location = New System.Drawing.Point(100, 63)
        Me.txtTag1.Name = "txtTag1"
        Me.txtTag1.Size = New System.Drawing.Size(148, 25)
        Me.txtTag1.TabIndex = 2
        '
        'txtTag2
        '
        Me.txtTag2.Font = New System.Drawing.Font("メイリオ", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.txtTag2.ImeMode = System.Windows.Forms.ImeMode.Hiragana
        Me.txtTag2.Location = New System.Drawing.Point(391, 62)
        Me.txtTag2.Name = "txtTag2"
        Me.txtTag2.Size = New System.Drawing.Size(151, 25)
        Me.txtTag2.TabIndex = 4
        '
        'cmbTag1
        '
        Me.cmbTag1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbTag1.Font = New System.Drawing.Font("メイリオ", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.cmbTag1.FormattingEnabled = True
        Me.cmbTag1.Location = New System.Drawing.Point(254, 62)
        Me.cmbTag1.Name = "cmbTag1"
        Me.cmbTag1.Size = New System.Drawing.Size(121, 26)
        Me.cmbTag1.TabIndex = 3
        '
        'cmbTag2
        '
        Me.cmbTag2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbTag2.Font = New System.Drawing.Font("メイリオ", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.cmbTag2.FormattingEnabled = True
        Me.cmbTag2.Location = New System.Drawing.Point(548, 62)
        Me.cmbTag2.Name = "cmbTag2"
        Me.cmbTag2.Size = New System.Drawing.Size(121, 26)
        Me.cmbTag2.TabIndex = 5
        '
        'btnMove
        '
        Me.btnMove.Location = New System.Drawing.Point(254, 35)
        Me.btnMove.Name = "btnMove"
        Me.btnMove.Size = New System.Drawing.Size(75, 23)
        Me.btnMove.TabIndex = 15
        Me.btnMove.Text = "移動"
        Me.btnMove.UseVisualStyleBackColor = True
        '
        'cmbTag4
        '
        Me.cmbTag4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbTag4.Font = New System.Drawing.Font("メイリオ", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.cmbTag4.FormattingEnabled = True
        Me.cmbTag4.Location = New System.Drawing.Point(254, 91)
        Me.cmbTag4.Name = "cmbTag4"
        Me.cmbTag4.Size = New System.Drawing.Size(121, 26)
        Me.cmbTag4.TabIndex = 9
        '
        'cmbTag3
        '
        Me.cmbTag3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbTag3.Font = New System.Drawing.Font("メイリオ", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.cmbTag3.FormattingEnabled = True
        Me.cmbTag3.Location = New System.Drawing.Point(824, 62)
        Me.cmbTag3.Name = "cmbTag3"
        Me.cmbTag3.Size = New System.Drawing.Size(121, 26)
        Me.cmbTag3.TabIndex = 7
        '
        'txtTag4
        '
        Me.txtTag4.Font = New System.Drawing.Font("メイリオ", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.txtTag4.ImeMode = System.Windows.Forms.ImeMode.Hiragana
        Me.txtTag4.Location = New System.Drawing.Point(100, 91)
        Me.txtTag4.Name = "txtTag4"
        Me.txtTag4.Size = New System.Drawing.Size(148, 25)
        Me.txtTag4.TabIndex = 8
        '
        'txtTag3
        '
        Me.txtTag3.Font = New System.Drawing.Font("メイリオ", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.txtTag3.ImeMode = System.Windows.Forms.ImeMode.Hiragana
        Me.txtTag3.Location = New System.Drawing.Point(684, 62)
        Me.txtTag3.Name = "txtTag3"
        Me.txtTag3.Size = New System.Drawing.Size(134, 25)
        Me.txtTag3.TabIndex = 6
        '
        'cmbTag6
        '
        Me.cmbTag6.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbTag6.Font = New System.Drawing.Font("メイリオ", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.cmbTag6.FormattingEnabled = True
        Me.cmbTag6.Location = New System.Drawing.Point(824, 91)
        Me.cmbTag6.Name = "cmbTag6"
        Me.cmbTag6.Size = New System.Drawing.Size(121, 26)
        Me.cmbTag6.TabIndex = 13
        '
        'cmbTag5
        '
        Me.cmbTag5.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbTag5.Font = New System.Drawing.Font("メイリオ", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.cmbTag5.FormattingEnabled = True
        Me.cmbTag5.Location = New System.Drawing.Point(548, 92)
        Me.cmbTag5.Name = "cmbTag5"
        Me.cmbTag5.Size = New System.Drawing.Size(121, 26)
        Me.cmbTag5.TabIndex = 11
        '
        'txtTag6
        '
        Me.txtTag6.Font = New System.Drawing.Font("メイリオ", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.txtTag6.ImeMode = System.Windows.Forms.ImeMode.Hiragana
        Me.txtTag6.Location = New System.Drawing.Point(684, 93)
        Me.txtTag6.Name = "txtTag6"
        Me.txtTag6.Size = New System.Drawing.Size(134, 25)
        Me.txtTag6.TabIndex = 12
        '
        'txtTag5
        '
        Me.txtTag5.Font = New System.Drawing.Font("メイリオ", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.txtTag5.ImeMode = System.Windows.Forms.ImeMode.Hiragana
        Me.txtTag5.Location = New System.Drawing.Point(391, 92)
        Me.txtTag5.Name = "txtTag5"
        Me.txtTag5.Size = New System.Drawing.Size(151, 25)
        Me.txtTag5.TabIndex = 10
        '
        'btnAddFolder
        '
        Me.btnAddFolder.Font = New System.Drawing.Font("メイリオ", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnAddFolder.Location = New System.Drawing.Point(761, 7)
        Me.btnAddFolder.Name = "btnAddFolder"
        Me.btnAddFolder.Size = New System.Drawing.Size(89, 23)
        Me.btnAddFolder.TabIndex = 14
        Me.btnAddFolder.Text = "フォルダ追加"
        Me.btnAddFolder.UseVisualStyleBackColor = True
        '
        'btnDelete
        '
        Me.btnDelete.Font = New System.Drawing.Font("メイリオ", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnDelete.Location = New System.Drawing.Point(348, 35)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(88, 23)
        Me.btnDelete.TabIndex = 16
        Me.btnDelete.Text = "削除"
        Me.btnDelete.UseVisualStyleBackColor = True
        '
        'btnUpdate
        '
        Me.btnUpdate.Font = New System.Drawing.Font("メイリオ", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnUpdate.Location = New System.Drawing.Point(254, 35)
        Me.btnUpdate.Name = "btnUpdate"
        Me.btnUpdate.Size = New System.Drawing.Size(88, 23)
        Me.btnUpdate.TabIndex = 17
        Me.btnUpdate.Text = "ファイル更新"
        Me.btnUpdate.UseVisualStyleBackColor = True
        Me.btnUpdate.Visible = False
        '
        'btnNowDel
        '
        Me.btnNowDel.Font = New System.Drawing.Font("メイリオ", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnNowDel.Location = New System.Drawing.Point(348, 34)
        Me.btnNowDel.Name = "btnNowDel"
        Me.btnNowDel.Size = New System.Drawing.Size(88, 24)
        Me.btnNowDel.TabIndex = 18
        Me.btnNowDel.Text = "ファイル削除"
        Me.btnNowDel.UseVisualStyleBackColor = True
        '
        'lblFileSize
        '
        Me.lblFileSize.AutoSize = True
        Me.lblFileSize.Font = New System.Drawing.Font("メイリオ", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblFileSize.Location = New System.Drawing.Point(444, 40)
        Me.lblFileSize.Name = "lblFileSize"
        Me.lblFileSize.Size = New System.Drawing.Size(46, 18)
        Me.lblFileSize.TabIndex = 20
        Me.lblFileSize.Text = "Label1"
        '
        'cmbRank
        '
        Me.cmbRank.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.cmbRank.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbRank.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbRank.FormattingEnabled = True
        Me.cmbRank.Items.AddRange(New Object() {"1", "2", "3", "4", "5", "6", "7", "8", "9", "10"})
        Me.cmbRank.Location = New System.Drawing.Point(502, 34)
        Me.cmbRank.Name = "cmbRank"
        Me.cmbRank.Size = New System.Drawing.Size(44, 23)
        Me.cmbRank.TabIndex = 21
        '
        'lblNowFolder
        '
        Me.lblNowFolder.AutoSize = True
        Me.lblNowFolder.Font = New System.Drawing.Font("メイリオ", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblNowFolder.Location = New System.Drawing.Point(733, 40)
        Me.lblNowFolder.Name = "lblNowFolder"
        Me.lblNowFolder.Size = New System.Drawing.Size(0, 18)
        Me.lblNowFolder.TabIndex = 22
        '
        'btnRankSearch
        '
        Me.btnRankSearch.Font = New System.Drawing.Font("メイリオ", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnRankSearch.Location = New System.Drawing.Point(573, 35)
        Me.btnRankSearch.Name = "btnRankSearch"
        Me.btnRankSearch.Size = New System.Drawing.Size(75, 22)
        Me.btnRankSearch.TabIndex = 23
        Me.btnRankSearch.Text = "ランク順"
        Me.btnRankSearch.UseVisualStyleBackColor = True
        '
        'btnAddDaySearch
        '
        Me.btnAddDaySearch.Font = New System.Drawing.Font("メイリオ", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnAddDaySearch.Location = New System.Drawing.Point(654, 35)
        Me.btnAddDaySearch.Name = "btnAddDaySearch"
        Me.btnAddDaySearch.Size = New System.Drawing.Size(75, 23)
        Me.btnAddDaySearch.TabIndex = 24
        Me.btnAddDaySearch.Text = "更新日順"
        Me.btnAddDaySearch.UseVisualStyleBackColor = True
        '
        'chkOver
        '
        Me.chkOver.AutoSize = True
        Me.chkOver.Checked = True
        Me.chkOver.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkOver.Location = New System.Drawing.Point(552, 39)
        Me.chkOver.Name = "chkOver"
        Me.chkOver.Size = New System.Drawing.Size(15, 14)
        Me.chkOver.TabIndex = 25
        Me.chkOver.UseVisualStyleBackColor = True
        '
        'btnFolderTagAdd
        '
        Me.btnFolderTagAdd.Font = New System.Drawing.Font("メイリオ", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnFolderTagAdd.Location = New System.Drawing.Point(857, 7)
        Me.btnFolderTagAdd.Name = "btnFolderTagAdd"
        Me.btnFolderTagAdd.Size = New System.Drawing.Size(88, 23)
        Me.btnFolderTagAdd.TabIndex = 26
        Me.btnFolderTagAdd.Text = "タグ追加"
        Me.btnFolderTagAdd.UseVisualStyleBackColor = True
        '
        'btnFolderDel
        '
        Me.btnFolderDel.Font = New System.Drawing.Font("メイリオ", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnFolderDel.Location = New System.Drawing.Point(857, 35)
        Me.btnFolderDel.Name = "btnFolderDel"
        Me.btnFolderDel.Size = New System.Drawing.Size(88, 23)
        Me.btnFolderDel.TabIndex = 27
        Me.btnFolderDel.Text = "フォルダ削除"
        Me.btnFolderDel.UseVisualStyleBackColor = True
        '
        'btnNameSearch
        '
        Me.btnNameSearch.Location = New System.Drawing.Point(573, 124)
        Me.btnNameSearch.Name = "btnNameSearch"
        Me.btnNameSearch.Size = New System.Drawing.Size(100, 19)
        Me.btnNameSearch.TabIndex = 28
        Me.btnNameSearch.Text = "Google"
        Me.btnNameSearch.UseVisualStyleBackColor = True
        '
        'btnTagSettinn
        '
        Me.btnTagSettinn.Location = New System.Drawing.Point(438, 123)
        Me.btnTagSettinn.Name = "btnTagSettinn"
        Me.btnTagSettinn.Size = New System.Drawing.Size(104, 19)
        Me.btnTagSettinn.TabIndex = 29
        Me.btnTagSettinn.Text = "タグ再設定"
        Me.btnTagSettinn.UseVisualStyleBackColor = True
        '
        'txtTagSetting
        '
        Me.txtTagSetting.Location = New System.Drawing.Point(100, 123)
        Me.txtTagSetting.Name = "txtTagSetting"
        Me.txtTagSetting.Size = New System.Drawing.Size(320, 19)
        Me.txtTagSetting.TabIndex = 30
        '
        'chkGoogle
        '
        Me.chkGoogle.AutoSize = True
        Me.chkGoogle.Location = New System.Drawing.Point(701, 130)
        Me.chkGoogle.Name = "chkGoogle"
        Me.chkGoogle.Size = New System.Drawing.Size(15, 14)
        Me.chkGoogle.TabIndex = 31
        Me.chkGoogle.UseVisualStyleBackColor = True
        '
        'TabPage3
        '
        Me.TabPage3.Controls.Add(Me.WebBrowser1)
        Me.TabPage3.Location = New System.Drawing.Point(4, 22)
        Me.TabPage3.Name = "TabPage3"
        Me.TabPage3.Size = New System.Drawing.Size(995, 465)
        Me.TabPage3.TabIndex = 2
        Me.TabPage3.Text = "TabPage3"
        Me.TabPage3.UseVisualStyleBackColor = True
        '
        'WebBrowser1
        '
        Me.WebBrowser1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.WebBrowser1.Location = New System.Drawing.Point(0, 0)
        Me.WebBrowser1.MinimumSize = New System.Drawing.Size(20, 20)
        Me.WebBrowser1.Name = "WebBrowser1"
        Me.WebBrowser1.Size = New System.Drawing.Size(995, 465)
        Me.WebBrowser1.TabIndex = 0
        Me.WebBrowser1.Url = New System.Uri("about:blank", System.UriKind.Absolute)
        '
        'FileMoveForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1001, 635)
        Me.Controls.Add(Me.chkGoogle)
        Me.Controls.Add(Me.txtTagSetting)
        Me.Controls.Add(Me.btnTagSettinn)
        Me.Controls.Add(Me.btnNameSearch)
        Me.Controls.Add(Me.btnFolderDel)
        Me.Controls.Add(Me.btnFolderTagAdd)
        Me.Controls.Add(Me.chkOver)
        Me.Controls.Add(Me.btnAddDaySearch)
        Me.Controls.Add(Me.btnRankSearch)
        Me.Controls.Add(Me.lblNowFolder)
        Me.Controls.Add(Me.cmbRank)
        Me.Controls.Add(Me.lblFileSize)
        Me.Controls.Add(Me.btnNowDel)
        Me.Controls.Add(Me.btnUpdate)
        Me.Controls.Add(Me.btnDelete)
        Me.Controls.Add(Me.btnAddFolder)
        Me.Controls.Add(Me.cmbTag6)
        Me.Controls.Add(Me.cmbTag5)
        Me.Controls.Add(Me.txtTag6)
        Me.Controls.Add(Me.txtTag5)
        Me.Controls.Add(Me.cmbTag4)
        Me.Controls.Add(Me.cmbTag3)
        Me.Controls.Add(Me.txtTag4)
        Me.Controls.Add(Me.txtTag3)
        Me.Controls.Add(Me.btnMove)
        Me.Controls.Add(Me.cmbTag2)
        Me.Controls.Add(Me.cmbTag1)
        Me.Controls.Add(Me.txtTag2)
        Me.Controls.Add(Me.txtTag1)
        Me.Controls.Add(Me.lblFilePath)
        Me.Controls.Add(Me.txtFileName)
        Me.Controls.Add(Me.picThumbs)
        Me.Controls.Add(Me.TabControl1)
        Me.Name = "FileMoveForm"
        Me.Text = "Form1"
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage2.ResumeLayout(False)
        CType(Me.picThumbs, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage3.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents lstThumbs As System.Windows.Forms.ListView
    Friend WithEvents picThumbs As System.Windows.Forms.PictureBox
    Friend WithEvents txtFileName As System.Windows.Forms.TextBox
    Friend WithEvents lblFilePath As System.Windows.Forms.Label
    Friend WithEvents txtTag1 As System.Windows.Forms.TextBox
    Friend WithEvents txtTag2 As System.Windows.Forms.TextBox
    Friend WithEvents cmbTag1 As System.Windows.Forms.ComboBox
    Friend WithEvents cmbTag2 As System.Windows.Forms.ComboBox
    Friend WithEvents btnMove As System.Windows.Forms.Button
    Friend WithEvents cmbTag4 As System.Windows.Forms.ComboBox
    Friend WithEvents cmbTag3 As System.Windows.Forms.ComboBox
    Friend WithEvents txtTag4 As System.Windows.Forms.TextBox
    Friend WithEvents txtTag3 As System.Windows.Forms.TextBox
    Friend WithEvents cmbTag6 As System.Windows.Forms.ComboBox
    Friend WithEvents cmbTag5 As System.Windows.Forms.ComboBox
    Friend WithEvents txtTag6 As System.Windows.Forms.TextBox
    Friend WithEvents txtTag5 As System.Windows.Forms.TextBox
    Friend WithEvents ilstThumbs As System.Windows.Forms.ImageList
    Friend WithEvents btnAddFolder As System.Windows.Forms.Button
    Friend WithEvents treeDir As System.Windows.Forms.TreeView
    Friend WithEvents btnDelete As System.Windows.Forms.Button
    Friend WithEvents btnUpdate As System.Windows.Forms.Button
    Friend WithEvents btnNowDel As System.Windows.Forms.Button
    Friend WithEvents lblFileSize As System.Windows.Forms.Label
    Friend WithEvents cmbRank As System.Windows.Forms.ComboBox
    Friend WithEvents lblNowFolder As System.Windows.Forms.Label
    Friend WithEvents btnRankSearch As System.Windows.Forms.Button
    Friend WithEvents btnAddDaySearch As System.Windows.Forms.Button
    Friend WithEvents chkOver As System.Windows.Forms.CheckBox
    Friend WithEvents btnFolderTagAdd As System.Windows.Forms.Button
    Friend WithEvents btnFolderDel As System.Windows.Forms.Button
    Friend WithEvents btnNameSearch As Button
    Friend WithEvents btnTagSettinn As Button
    Friend WithEvents txtTagSetting As TextBox
    Friend WithEvents chkGoogle As CheckBox
    Friend WithEvents TabPage3 As TabPage
    Friend WithEvents WebBrowser1 As WebBrowser
End Class
