<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FileMoveForm
    Inherits System.Windows.Forms.Form

    'フォームがコンポーネントの一覧をクリーンアップするために dispose をオーバーライドします。
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FileMoveForm))
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
        Me.lblNowFolder = New System.Windows.Forms.Label()
        Me.btnRankSearch = New System.Windows.Forms.Button()
        Me.btnAddDaySearch = New System.Windows.Forms.Button()
        Me.btnFolderTagAdd = New System.Windows.Forms.Button()
        Me.btnFolderDel = New System.Windows.Forms.Button()
        Me.btnNameSearch = New System.Windows.Forms.Button()
        Me.btnTagSettinn = New System.Windows.Forms.Button()
        Me.txtTagSetting = New System.Windows.Forms.TextBox()
        Me.chkGoogle = New System.Windows.Forms.CheckBox()
        Me.cmbTag9 = New System.Windows.Forms.ComboBox()
        Me.cmbTag8 = New System.Windows.Forms.ComboBox()
        Me.txtTag9 = New System.Windows.Forms.TextBox()
        Me.txtTag8 = New System.Windows.Forms.TextBox()
        Me.cmbTag7 = New System.Windows.Forms.ComboBox()
        Me.txtTag7 = New System.Windows.Forms.TextBox()
        Me.chkZoku1 = New System.Windows.Forms.CheckBox()
        Me.chkZoku2 = New System.Windows.Forms.CheckBox()
        Me.chkZoku3 = New System.Windows.Forms.CheckBox()
        Me.chkZoku4 = New System.Windows.Forms.CheckBox()
        Me.chkZoku5 = New System.Windows.Forms.CheckBox()
        Me.chkZoku6 = New System.Windows.Forms.CheckBox()
        Me.chkZoku7 = New System.Windows.Forms.CheckBox()
        Me.chkZoku8 = New System.Windows.Forms.CheckBox()
        Me.chkZoku9 = New System.Windows.Forms.CheckBox()
        Me.chkZoku10 = New System.Windows.Forms.CheckBox()
        Me.NotifyIcon1 = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.picRank1 = New System.Windows.Forms.PictureBox()
        Me.picRank2 = New System.Windows.Forms.PictureBox()
        Me.picRank3 = New System.Windows.Forms.PictureBox()
        Me.picRank4 = New System.Windows.Forms.PictureBox()
        Me.picRank5 = New System.Windows.Forms.PictureBox()
        Me.picRank0 = New System.Windows.Forms.PictureBox()
        Me.lstBookMark = New System.Windows.Forms.ListView()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.tabTree = New System.Windows.Forms.TabPage()
        Me.treeDir = New System.Windows.Forms.TreeView()
        Me.tabImageList = New System.Windows.Forms.TabPage()
        Me.lstThumbs = New System.Windows.Forms.ListView()
        Me.tabGoogle = New System.Windows.Forms.TabPage()
        Me.WebBrowser1 = New System.Windows.Forms.WebBrowser()
        Me.ilstBMThumbs = New System.Windows.Forms.ImageList(Me.components)
        Me.btnBookMarkAdd = New System.Windows.Forms.Button()
        Me.btnBookMarkDel = New System.Windows.Forms.Button()
        Me.btnBookMarkUpdate = New System.Windows.Forms.Button()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.chkZoku11 = New System.Windows.Forms.CheckBox()
        Me.chkZoku12 = New System.Windows.Forms.CheckBox()
        Me.chkZoku13 = New System.Windows.Forms.CheckBox()
        Me.chkZoku14 = New System.Windows.Forms.CheckBox()
        Me.chkZoku15 = New System.Windows.Forms.CheckBox()
        CType(Me.picThumbs, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picRank1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picRank2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picRank3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picRank4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picRank5, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picRank0, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabControl1.SuspendLayout()
        Me.tabTree.SuspendLayout()
        Me.tabImageList.SuspendLayout()
        Me.tabGoogle.SuspendLayout()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.SuspendLayout()
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
        Me.txtFileName.Location = New System.Drawing.Point(98, 35)
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
        Me.txtTag1.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.txtTag1.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource
        Me.txtTag1.Font = New System.Drawing.Font("メイリオ", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.txtTag1.ImeMode = System.Windows.Forms.ImeMode.Hiragana
        Me.txtTag1.Location = New System.Drawing.Point(100, 136)
        Me.txtTag1.Name = "txtTag1"
        Me.txtTag1.Size = New System.Drawing.Size(151, 25)
        Me.txtTag1.TabIndex = 2
        '
        'txtTag2
        '
        Me.txtTag2.Font = New System.Drawing.Font("メイリオ", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.txtTag2.ImeMode = System.Windows.Forms.ImeMode.Hiragana
        Me.txtTag2.Location = New System.Drawing.Point(389, 137)
        Me.txtTag2.Name = "txtTag2"
        Me.txtTag2.Size = New System.Drawing.Size(151, 25)
        Me.txtTag2.TabIndex = 4
        '
        'cmbTag1
        '
        Me.cmbTag1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbTag1.Font = New System.Drawing.Font("メイリオ", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.cmbTag1.FormattingEnabled = True
        Me.cmbTag1.Location = New System.Drawing.Point(257, 136)
        Me.cmbTag1.Name = "cmbTag1"
        Me.cmbTag1.Size = New System.Drawing.Size(121, 26)
        Me.cmbTag1.TabIndex = 3
        '
        'cmbTag2
        '
        Me.cmbTag2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbTag2.Font = New System.Drawing.Font("メイリオ", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.cmbTag2.FormattingEnabled = True
        Me.cmbTag2.Location = New System.Drawing.Point(546, 135)
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
        Me.cmbTag4.Location = New System.Drawing.Point(257, 164)
        Me.cmbTag4.Name = "cmbTag4"
        Me.cmbTag4.Size = New System.Drawing.Size(121, 26)
        Me.cmbTag4.TabIndex = 9
        '
        'cmbTag3
        '
        Me.cmbTag3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbTag3.Font = New System.Drawing.Font("メイリオ", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.cmbTag3.FormattingEnabled = True
        Me.cmbTag3.Location = New System.Drawing.Point(832, 134)
        Me.cmbTag3.Name = "cmbTag3"
        Me.cmbTag3.Size = New System.Drawing.Size(121, 26)
        Me.cmbTag3.TabIndex = 7
        '
        'txtTag4
        '
        Me.txtTag4.Font = New System.Drawing.Font("メイリオ", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.txtTag4.ImeMode = System.Windows.Forms.ImeMode.Hiragana
        Me.txtTag4.Location = New System.Drawing.Point(100, 167)
        Me.txtTag4.Name = "txtTag4"
        Me.txtTag4.Size = New System.Drawing.Size(151, 25)
        Me.txtTag4.TabIndex = 8
        '
        'txtTag3
        '
        Me.txtTag3.Font = New System.Drawing.Font("メイリオ", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.txtTag3.ImeMode = System.Windows.Forms.ImeMode.Hiragana
        Me.txtTag3.Location = New System.Drawing.Point(675, 135)
        Me.txtTag3.Name = "txtTag3"
        Me.txtTag3.Size = New System.Drawing.Size(151, 25)
        Me.txtTag3.TabIndex = 6
        '
        'cmbTag6
        '
        Me.cmbTag6.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbTag6.Font = New System.Drawing.Font("メイリオ", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.cmbTag6.FormattingEnabled = True
        Me.cmbTag6.Location = New System.Drawing.Point(832, 164)
        Me.cmbTag6.Name = "cmbTag6"
        Me.cmbTag6.Size = New System.Drawing.Size(121, 26)
        Me.cmbTag6.TabIndex = 13
        '
        'cmbTag5
        '
        Me.cmbTag5.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbTag5.Font = New System.Drawing.Font("メイリオ", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.cmbTag5.FormattingEnabled = True
        Me.cmbTag5.Location = New System.Drawing.Point(546, 165)
        Me.cmbTag5.Name = "cmbTag5"
        Me.cmbTag5.Size = New System.Drawing.Size(121, 26)
        Me.cmbTag5.TabIndex = 11
        '
        'txtTag6
        '
        Me.txtTag6.Font = New System.Drawing.Font("メイリオ", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.txtTag6.ImeMode = System.Windows.Forms.ImeMode.Hiragana
        Me.txtTag6.Location = New System.Drawing.Point(675, 166)
        Me.txtTag6.Name = "txtTag6"
        Me.txtTag6.Size = New System.Drawing.Size(151, 25)
        Me.txtTag6.TabIndex = 12
        '
        'txtTag5
        '
        Me.txtTag5.Font = New System.Drawing.Font("メイリオ", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.txtTag5.ImeMode = System.Windows.Forms.ImeMode.Hiragana
        Me.txtTag5.Location = New System.Drawing.Point(389, 166)
        Me.txtTag5.Name = "txtTag5"
        Me.txtTag5.Size = New System.Drawing.Size(151, 25)
        Me.txtTag5.TabIndex = 10
        '
        'btnAddFolder
        '
        Me.btnAddFolder.Font = New System.Drawing.Font("メイリオ", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnAddFolder.Location = New System.Drawing.Point(734, 61)
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
        Me.lblFileSize.Location = New System.Drawing.Point(547, 39)
        Me.lblFileSize.Name = "lblFileSize"
        Me.lblFileSize.Size = New System.Drawing.Size(46, 18)
        Me.lblFileSize.TabIndex = 20
        Me.lblFileSize.Text = "Label1"
        '
        'lblNowFolder
        '
        Me.lblNowFolder.AutoSize = True
        Me.lblNowFolder.Font = New System.Drawing.Font("メイリオ", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblNowFolder.Location = New System.Drawing.Point(625, 41)
        Me.lblNowFolder.Name = "lblNowFolder"
        Me.lblNowFolder.Size = New System.Drawing.Size(0, 18)
        Me.lblNowFolder.TabIndex = 22
        '
        'btnRankSearch
        '
        Me.btnRankSearch.Font = New System.Drawing.Font("メイリオ", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnRankSearch.Location = New System.Drawing.Point(550, 62)
        Me.btnRankSearch.Name = "btnRankSearch"
        Me.btnRankSearch.Size = New System.Drawing.Size(75, 22)
        Me.btnRankSearch.TabIndex = 23
        Me.btnRankSearch.Text = "ランク順"
        Me.btnRankSearch.UseVisualStyleBackColor = True
        '
        'btnAddDaySearch
        '
        Me.btnAddDaySearch.Font = New System.Drawing.Font("メイリオ", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnAddDaySearch.Location = New System.Drawing.Point(550, 90)
        Me.btnAddDaySearch.Name = "btnAddDaySearch"
        Me.btnAddDaySearch.Size = New System.Drawing.Size(75, 23)
        Me.btnAddDaySearch.TabIndex = 24
        Me.btnAddDaySearch.Text = "更新日順"
        Me.btnAddDaySearch.UseVisualStyleBackColor = True
        '
        'btnFolderTagAdd
        '
        Me.btnFolderTagAdd.Font = New System.Drawing.Font("メイリオ", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnFolderTagAdd.Location = New System.Drawing.Point(834, 62)
        Me.btnFolderTagAdd.Name = "btnFolderTagAdd"
        Me.btnFolderTagAdd.Size = New System.Drawing.Size(88, 23)
        Me.btnFolderTagAdd.TabIndex = 26
        Me.btnFolderTagAdd.Text = "タグ追加"
        Me.btnFolderTagAdd.UseVisualStyleBackColor = True
        '
        'btnFolderDel
        '
        Me.btnFolderDel.Font = New System.Drawing.Font("メイリオ", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnFolderDel.Location = New System.Drawing.Point(735, 90)
        Me.btnFolderDel.Name = "btnFolderDel"
        Me.btnFolderDel.Size = New System.Drawing.Size(88, 23)
        Me.btnFolderDel.TabIndex = 27
        Me.btnFolderDel.Text = "フォルダ削除"
        Me.btnFolderDel.UseVisualStyleBackColor = True
        '
        'btnNameSearch
        '
        Me.btnNameSearch.Location = New System.Drawing.Point(535, 228)
        Me.btnNameSearch.Name = "btnNameSearch"
        Me.btnNameSearch.Size = New System.Drawing.Size(100, 19)
        Me.btnNameSearch.TabIndex = 28
        Me.btnNameSearch.Text = "Google"
        Me.btnNameSearch.UseVisualStyleBackColor = True
        '
        'btnTagSettinn
        '
        Me.btnTagSettinn.Location = New System.Drawing.Point(425, 227)
        Me.btnTagSettinn.Name = "btnTagSettinn"
        Me.btnTagSettinn.Size = New System.Drawing.Size(104, 19)
        Me.btnTagSettinn.TabIndex = 29
        Me.btnTagSettinn.Text = "タグ再設定"
        Me.btnTagSettinn.UseVisualStyleBackColor = True
        '
        'txtTagSetting
        '
        Me.txtTagSetting.Location = New System.Drawing.Point(98, 227)
        Me.txtTagSetting.Name = "txtTagSetting"
        Me.txtTagSetting.Size = New System.Drawing.Size(320, 19)
        Me.txtTagSetting.TabIndex = 30
        '
        'chkGoogle
        '
        Me.chkGoogle.AutoSize = True
        Me.chkGoogle.Location = New System.Drawing.Point(641, 233)
        Me.chkGoogle.Name = "chkGoogle"
        Me.chkGoogle.Size = New System.Drawing.Size(15, 14)
        Me.chkGoogle.TabIndex = 31
        Me.chkGoogle.UseVisualStyleBackColor = True
        '
        'cmbTag9
        '
        Me.cmbTag9.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbTag9.Font = New System.Drawing.Font("メイリオ", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.cmbTag9.FormattingEnabled = True
        Me.cmbTag9.Location = New System.Drawing.Point(832, 196)
        Me.cmbTag9.Name = "cmbTag9"
        Me.cmbTag9.Size = New System.Drawing.Size(121, 26)
        Me.cmbTag9.TabIndex = 37
        '
        'cmbTag8
        '
        Me.cmbTag8.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbTag8.Font = New System.Drawing.Font("メイリオ", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.cmbTag8.FormattingEnabled = True
        Me.cmbTag8.Location = New System.Drawing.Point(546, 196)
        Me.cmbTag8.Name = "cmbTag8"
        Me.cmbTag8.Size = New System.Drawing.Size(121, 26)
        Me.cmbTag8.TabIndex = 35
        '
        'txtTag9
        '
        Me.txtTag9.Font = New System.Drawing.Font("メイリオ", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.txtTag9.ImeMode = System.Windows.Forms.ImeMode.Hiragana
        Me.txtTag9.Location = New System.Drawing.Point(675, 197)
        Me.txtTag9.Name = "txtTag9"
        Me.txtTag9.Size = New System.Drawing.Size(151, 25)
        Me.txtTag9.TabIndex = 36
        '
        'txtTag8
        '
        Me.txtTag8.Font = New System.Drawing.Font("メイリオ", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.txtTag8.ImeMode = System.Windows.Forms.ImeMode.Hiragana
        Me.txtTag8.Location = New System.Drawing.Point(389, 196)
        Me.txtTag8.Name = "txtTag8"
        Me.txtTag8.Size = New System.Drawing.Size(151, 25)
        Me.txtTag8.TabIndex = 34
        '
        'cmbTag7
        '
        Me.cmbTag7.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbTag7.Font = New System.Drawing.Font("メイリオ", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.cmbTag7.FormattingEnabled = True
        Me.cmbTag7.Location = New System.Drawing.Point(257, 196)
        Me.cmbTag7.Name = "cmbTag7"
        Me.cmbTag7.Size = New System.Drawing.Size(121, 26)
        Me.cmbTag7.TabIndex = 33
        '
        'txtTag7
        '
        Me.txtTag7.Font = New System.Drawing.Font("メイリオ", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.txtTag7.ImeMode = System.Windows.Forms.ImeMode.Hiragana
        Me.txtTag7.Location = New System.Drawing.Point(100, 197)
        Me.txtTag7.Name = "txtTag7"
        Me.txtTag7.Size = New System.Drawing.Size(151, 25)
        Me.txtTag7.TabIndex = 32
        '
        'chkZoku1
        '
        Me.chkZoku1.AutoSize = True
        Me.chkZoku1.Location = New System.Drawing.Point(100, 69)
        Me.chkZoku1.Name = "chkZoku1"
        Me.chkZoku1.Size = New System.Drawing.Size(82, 16)
        Me.chkZoku1.TabIndex = 38
        Me.chkZoku1.Text = "CheckBox1"
        Me.chkZoku1.UseVisualStyleBackColor = True
        '
        'chkZoku2
        '
        Me.chkZoku2.AutoSize = True
        Me.chkZoku2.Location = New System.Drawing.Point(100, 91)
        Me.chkZoku2.Name = "chkZoku2"
        Me.chkZoku2.Size = New System.Drawing.Size(82, 16)
        Me.chkZoku2.TabIndex = 39
        Me.chkZoku2.Text = "CheckBox2"
        Me.chkZoku2.UseVisualStyleBackColor = True
        '
        'chkZoku3
        '
        Me.chkZoku3.AutoSize = True
        Me.chkZoku3.Location = New System.Drawing.Point(188, 69)
        Me.chkZoku3.Name = "chkZoku3"
        Me.chkZoku3.Size = New System.Drawing.Size(82, 16)
        Me.chkZoku3.TabIndex = 40
        Me.chkZoku3.Text = "CheckBox3"
        Me.chkZoku3.UseVisualStyleBackColor = True
        '
        'chkZoku4
        '
        Me.chkZoku4.AutoSize = True
        Me.chkZoku4.Location = New System.Drawing.Point(188, 91)
        Me.chkZoku4.Name = "chkZoku4"
        Me.chkZoku4.Size = New System.Drawing.Size(82, 16)
        Me.chkZoku4.TabIndex = 41
        Me.chkZoku4.Text = "CheckBox4"
        Me.chkZoku4.UseVisualStyleBackColor = True
        '
        'chkZoku5
        '
        Me.chkZoku5.AutoSize = True
        Me.chkZoku5.Location = New System.Drawing.Point(276, 69)
        Me.chkZoku5.Name = "chkZoku5"
        Me.chkZoku5.Size = New System.Drawing.Size(82, 16)
        Me.chkZoku5.TabIndex = 42
        Me.chkZoku5.Text = "CheckBox5"
        Me.chkZoku5.UseVisualStyleBackColor = True
        '
        'chkZoku6
        '
        Me.chkZoku6.AutoSize = True
        Me.chkZoku6.Location = New System.Drawing.Point(276, 91)
        Me.chkZoku6.Name = "chkZoku6"
        Me.chkZoku6.Size = New System.Drawing.Size(82, 16)
        Me.chkZoku6.TabIndex = 43
        Me.chkZoku6.Text = "CheckBox6"
        Me.chkZoku6.UseVisualStyleBackColor = True
        '
        'chkZoku7
        '
        Me.chkZoku7.AutoSize = True
        Me.chkZoku7.Location = New System.Drawing.Point(364, 69)
        Me.chkZoku7.Name = "chkZoku7"
        Me.chkZoku7.Size = New System.Drawing.Size(82, 16)
        Me.chkZoku7.TabIndex = 44
        Me.chkZoku7.Text = "CheckBox7"
        Me.chkZoku7.UseVisualStyleBackColor = True
        '
        'chkZoku8
        '
        Me.chkZoku8.AutoSize = True
        Me.chkZoku8.Location = New System.Drawing.Point(364, 91)
        Me.chkZoku8.Name = "chkZoku8"
        Me.chkZoku8.Size = New System.Drawing.Size(82, 16)
        Me.chkZoku8.TabIndex = 45
        Me.chkZoku8.Text = "CheckBox8"
        Me.chkZoku8.UseVisualStyleBackColor = True
        '
        'chkZoku9
        '
        Me.chkZoku9.AutoSize = True
        Me.chkZoku9.Location = New System.Drawing.Point(447, 69)
        Me.chkZoku9.Name = "chkZoku9"
        Me.chkZoku9.Size = New System.Drawing.Size(82, 16)
        Me.chkZoku9.TabIndex = 46
        Me.chkZoku9.Text = "CheckBox9"
        Me.chkZoku9.UseVisualStyleBackColor = True
        '
        'chkZoku10
        '
        Me.chkZoku10.AutoSize = True
        Me.chkZoku10.Location = New System.Drawing.Point(447, 91)
        Me.chkZoku10.Name = "chkZoku10"
        Me.chkZoku10.Size = New System.Drawing.Size(88, 16)
        Me.chkZoku10.TabIndex = 47
        Me.chkZoku10.Text = "CheckBox10"
        Me.chkZoku10.UseVisualStyleBackColor = True
        '
        'NotifyIcon1
        '
        Me.NotifyIcon1.Text = "NotifyIcon1"
        Me.NotifyIcon1.Visible = True
        '
        'picRank1
        '
        Me.picRank1.Image = CType(resources.GetObject("picRank1.Image"), System.Drawing.Image)
        Me.picRank1.Location = New System.Drawing.Point(447, 39)
        Me.picRank1.Margin = New System.Windows.Forms.Padding(0)
        Me.picRank1.Name = "picRank1"
        Me.picRank1.Size = New System.Drawing.Size(19, 19)
        Me.picRank1.TabIndex = 48
        Me.picRank1.TabStop = False
        '
        'picRank2
        '
        Me.picRank2.Image = CType(resources.GetObject("picRank2.Image"), System.Drawing.Image)
        Me.picRank2.Location = New System.Drawing.Point(466, 39)
        Me.picRank2.Margin = New System.Windows.Forms.Padding(0)
        Me.picRank2.Name = "picRank2"
        Me.picRank2.Size = New System.Drawing.Size(19, 19)
        Me.picRank2.TabIndex = 49
        Me.picRank2.TabStop = False
        '
        'picRank3
        '
        Me.picRank3.Image = CType(resources.GetObject("picRank3.Image"), System.Drawing.Image)
        Me.picRank3.Location = New System.Drawing.Point(485, 39)
        Me.picRank3.Margin = New System.Windows.Forms.Padding(0)
        Me.picRank3.Name = "picRank3"
        Me.picRank3.Size = New System.Drawing.Size(19, 19)
        Me.picRank3.TabIndex = 50
        Me.picRank3.TabStop = False
        '
        'picRank4
        '
        Me.picRank4.Image = CType(resources.GetObject("picRank4.Image"), System.Drawing.Image)
        Me.picRank4.Location = New System.Drawing.Point(504, 39)
        Me.picRank4.Margin = New System.Windows.Forms.Padding(0)
        Me.picRank4.Name = "picRank4"
        Me.picRank4.Size = New System.Drawing.Size(19, 19)
        Me.picRank4.TabIndex = 51
        Me.picRank4.TabStop = False
        '
        'picRank5
        '
        Me.picRank5.Image = CType(resources.GetObject("picRank5.Image"), System.Drawing.Image)
        Me.picRank5.Location = New System.Drawing.Point(522, 39)
        Me.picRank5.Margin = New System.Windows.Forms.Padding(0)
        Me.picRank5.Name = "picRank5"
        Me.picRank5.Size = New System.Drawing.Size(19, 19)
        Me.picRank5.TabIndex = 52
        Me.picRank5.TabStop = False
        '
        'picRank0
        '
        Me.picRank0.Location = New System.Drawing.Point(444, 38)
        Me.picRank0.Margin = New System.Windows.Forms.Padding(0)
        Me.picRank0.Name = "picRank0"
        Me.picRank0.Size = New System.Drawing.Size(3, 19)
        Me.picRank0.TabIndex = 53
        Me.picRank0.TabStop = False
        '
        'lstBookMark
        '
        Me.lstBookMark.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lstBookMark.HideSelection = False
        Me.lstBookMark.Location = New System.Drawing.Point(0, 332)
        Me.lstBookMark.Margin = New System.Windows.Forms.Padding(0)
        Me.lstBookMark.Name = "lstBookMark"
        Me.lstBookMark.Size = New System.Drawing.Size(1005, 126)
        Me.lstBookMark.SmallImageList = Me.ilstBMThumbs
        Me.lstBookMark.TabIndex = 0
        Me.lstBookMark.UseCompatibleStateImageBehavior = False
        '
        'TabControl1
        '
        Me.TabControl1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TabControl1.Controls.Add(Me.tabTree)
        Me.TabControl1.Controls.Add(Me.tabImageList)
        Me.TabControl1.Controls.Add(Me.tabGoogle)
        Me.TabControl1.Location = New System.Drawing.Point(0, 0)
        Me.TabControl1.Margin = New System.Windows.Forms.Padding(0)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.Padding = New System.Drawing.Point(0, 0)
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(1005, 332)
        Me.TabControl1.TabIndex = 0
        '
        'tabTree
        '
        Me.tabTree.Controls.Add(Me.treeDir)
        Me.tabTree.Location = New System.Drawing.Point(4, 22)
        Me.tabTree.Name = "tabTree"
        Me.tabTree.Padding = New System.Windows.Forms.Padding(3)
        Me.tabTree.Size = New System.Drawing.Size(997, 306)
        Me.tabTree.TabIndex = 0
        Me.tabTree.Text = "フォルダ"
        Me.tabTree.UseVisualStyleBackColor = True
        '
        'treeDir
        '
        Me.treeDir.Dock = System.Windows.Forms.DockStyle.Fill
        Me.treeDir.Font = New System.Drawing.Font("メイリオ", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.treeDir.ForeColor = System.Drawing.SystemColors.WindowText
        Me.treeDir.Location = New System.Drawing.Point(3, 3)
        Me.treeDir.Margin = New System.Windows.Forms.Padding(0)
        Me.treeDir.Name = "treeDir"
        Me.treeDir.Size = New System.Drawing.Size(991, 300)
        Me.treeDir.TabIndex = 16
        '
        'tabImageList
        '
        Me.tabImageList.Controls.Add(Me.lstThumbs)
        Me.tabImageList.Location = New System.Drawing.Point(4, 22)
        Me.tabImageList.Margin = New System.Windows.Forms.Padding(0)
        Me.tabImageList.Name = "tabImageList"
        Me.tabImageList.Size = New System.Drawing.Size(997, 306)
        Me.tabImageList.TabIndex = 1
        Me.tabImageList.Text = "検索結果"
        Me.tabImageList.UseVisualStyleBackColor = True
        '
        'lstThumbs
        '
        Me.lstThumbs.BackColor = System.Drawing.SystemColors.Window
        Me.lstThumbs.Cursor = System.Windows.Forms.Cursors.Default
        Me.lstThumbs.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lstThumbs.Font = New System.Drawing.Font("メイリオ", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lstThumbs.HideSelection = False
        Me.lstThumbs.Location = New System.Drawing.Point(0, 0)
        Me.lstThumbs.Margin = New System.Windows.Forms.Padding(0)
        Me.lstThumbs.Name = "lstThumbs"
        Me.lstThumbs.ShowItemToolTips = True
        Me.lstThumbs.Size = New System.Drawing.Size(997, 306)
        Me.lstThumbs.TabIndex = 28
        Me.lstThumbs.UseCompatibleStateImageBehavior = False
        '
        'tabGoogle
        '
        Me.tabGoogle.Controls.Add(Me.WebBrowser1)
        Me.tabGoogle.Location = New System.Drawing.Point(4, 22)
        Me.tabGoogle.Name = "tabGoogle"
        Me.tabGoogle.Size = New System.Drawing.Size(997, 300)
        Me.tabGoogle.TabIndex = 2
        Me.tabGoogle.Text = "google"
        Me.tabGoogle.UseVisualStyleBackColor = True
        '
        'WebBrowser1
        '
        Me.WebBrowser1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.WebBrowser1.Location = New System.Drawing.Point(0, 0)
        Me.WebBrowser1.MinimumSize = New System.Drawing.Size(20, 20)
        Me.WebBrowser1.Name = "WebBrowser1"
        Me.WebBrowser1.Size = New System.Drawing.Size(997, 300)
        Me.WebBrowser1.TabIndex = 0
        Me.WebBrowser1.Url = New System.Uri("about:blank", System.UriKind.Absolute)
        '
        'ilstBMThumbs
        '
        Me.ilstBMThumbs.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit
        Me.ilstBMThumbs.ImageSize = New System.Drawing.Size(16, 16)
        Me.ilstBMThumbs.TransparentColor = System.Drawing.Color.Transparent
        '
        'btnBookMarkAdd
        '
        Me.btnBookMarkAdd.Font = New System.Drawing.Font("メイリオ", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnBookMarkAdd.Location = New System.Drawing.Point(254, 37)
        Me.btnBookMarkAdd.Name = "btnBookMarkAdd"
        Me.btnBookMarkAdd.Size = New System.Drawing.Size(88, 23)
        Me.btnBookMarkAdd.TabIndex = 56
        Me.btnBookMarkAdd.Text = "ブクマ追加"
        Me.btnBookMarkAdd.UseVisualStyleBackColor = True
        Me.btnBookMarkAdd.Visible = False
        '
        'btnBookMarkDel
        '
        Me.btnBookMarkDel.Font = New System.Drawing.Font("メイリオ", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnBookMarkDel.Location = New System.Drawing.Point(348, 34)
        Me.btnBookMarkDel.Name = "btnBookMarkDel"
        Me.btnBookMarkDel.Size = New System.Drawing.Size(88, 23)
        Me.btnBookMarkDel.TabIndex = 57
        Me.btnBookMarkDel.Text = "ブクマ削除"
        Me.btnBookMarkDel.UseVisualStyleBackColor = True
        Me.btnBookMarkDel.Visible = False
        '
        'btnBookMarkUpdate
        '
        Me.btnBookMarkUpdate.Font = New System.Drawing.Font("メイリオ", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnBookMarkUpdate.Location = New System.Drawing.Point(254, 34)
        Me.btnBookMarkUpdate.Name = "btnBookMarkUpdate"
        Me.btnBookMarkUpdate.Size = New System.Drawing.Size(88, 23)
        Me.btnBookMarkUpdate.TabIndex = 58
        Me.btnBookMarkUpdate.Text = "ブクマ更新"
        Me.btnBookMarkUpdate.UseVisualStyleBackColor = True
        Me.btnBookMarkUpdate.Visible = False
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.ColumnCount = 1
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.lstBookMark, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.TabControl1, 0, 0)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 262)
        Me.TableLayoutPanel1.Margin = New System.Windows.Forms.Padding(0)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 2
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(1005, 458)
        Me.TableLayoutPanel1.TabIndex = 59
        '
        'chkZoku11
        '
        Me.chkZoku11.AutoSize = True
        Me.chkZoku11.Location = New System.Drawing.Point(100, 113)
        Me.chkZoku11.Name = "chkZoku11"
        Me.chkZoku11.Size = New System.Drawing.Size(88, 16)
        Me.chkZoku11.TabIndex = 60
        Me.chkZoku11.Text = "CheckBox11"
        Me.chkZoku11.UseVisualStyleBackColor = True
        '
        'chkZoku12
        '
        Me.chkZoku12.AutoSize = True
        Me.chkZoku12.Location = New System.Drawing.Point(188, 113)
        Me.chkZoku12.Name = "chkZoku12"
        Me.chkZoku12.Size = New System.Drawing.Size(88, 16)
        Me.chkZoku12.TabIndex = 61
        Me.chkZoku12.Text = "CheckBox12"
        Me.chkZoku12.UseVisualStyleBackColor = True
        '
        'chkZoku13
        '
        Me.chkZoku13.AutoSize = True
        Me.chkZoku13.Location = New System.Drawing.Point(276, 113)
        Me.chkZoku13.Name = "chkZoku13"
        Me.chkZoku13.Size = New System.Drawing.Size(88, 16)
        Me.chkZoku13.TabIndex = 62
        Me.chkZoku13.Text = "CheckBox13"
        Me.chkZoku13.UseVisualStyleBackColor = True
        '
        'chkZoku14
        '
        Me.chkZoku14.AutoSize = True
        Me.chkZoku14.Location = New System.Drawing.Point(364, 113)
        Me.chkZoku14.Name = "chkZoku14"
        Me.chkZoku14.Size = New System.Drawing.Size(88, 16)
        Me.chkZoku14.TabIndex = 63
        Me.chkZoku14.Text = "CheckBox14"
        Me.chkZoku14.UseVisualStyleBackColor = True
        '
        'chkZoku15
        '
        Me.chkZoku15.AutoSize = True
        Me.chkZoku15.Location = New System.Drawing.Point(447, 113)
        Me.chkZoku15.Name = "chkZoku15"
        Me.chkZoku15.Size = New System.Drawing.Size(88, 16)
        Me.chkZoku15.TabIndex = 64
        Me.chkZoku15.Text = "CheckBox15"
        Me.chkZoku15.UseVisualStyleBackColor = True
        '
        'FileMoveForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1006, 721)
        Me.Controls.Add(Me.chkZoku15)
        Me.Controls.Add(Me.chkZoku14)
        Me.Controls.Add(Me.chkZoku13)
        Me.Controls.Add(Me.chkZoku12)
        Me.Controls.Add(Me.chkZoku11)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Controls.Add(Me.btnBookMarkUpdate)
        Me.Controls.Add(Me.btnBookMarkDel)
        Me.Controls.Add(Me.btnBookMarkAdd)
        Me.Controls.Add(Me.picRank0)
        Me.Controls.Add(Me.picRank5)
        Me.Controls.Add(Me.picRank4)
        Me.Controls.Add(Me.picRank3)
        Me.Controls.Add(Me.picRank2)
        Me.Controls.Add(Me.picRank1)
        Me.Controls.Add(Me.chkZoku10)
        Me.Controls.Add(Me.chkZoku9)
        Me.Controls.Add(Me.chkZoku8)
        Me.Controls.Add(Me.chkZoku7)
        Me.Controls.Add(Me.chkZoku6)
        Me.Controls.Add(Me.chkZoku5)
        Me.Controls.Add(Me.chkZoku4)
        Me.Controls.Add(Me.chkZoku3)
        Me.Controls.Add(Me.chkZoku2)
        Me.Controls.Add(Me.chkZoku1)
        Me.Controls.Add(Me.cmbTag9)
        Me.Controls.Add(Me.cmbTag8)
        Me.Controls.Add(Me.txtTag9)
        Me.Controls.Add(Me.txtTag8)
        Me.Controls.Add(Me.cmbTag7)
        Me.Controls.Add(Me.txtTag7)
        Me.Controls.Add(Me.chkGoogle)
        Me.Controls.Add(Me.txtTagSetting)
        Me.Controls.Add(Me.btnTagSettinn)
        Me.Controls.Add(Me.btnNameSearch)
        Me.Controls.Add(Me.btnFolderDel)
        Me.Controls.Add(Me.btnFolderTagAdd)
        Me.Controls.Add(Me.btnAddDaySearch)
        Me.Controls.Add(Me.btnRankSearch)
        Me.Controls.Add(Me.lblNowFolder)
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
        Me.Name = "FileMoveForm"
        Me.Text = "Form1"
        CType(Me.picThumbs, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picRank1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picRank2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picRank3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picRank4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picRank5, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picRank0, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabControl1.ResumeLayout(False)
        Me.tabTree.ResumeLayout(False)
        Me.tabImageList.ResumeLayout(False)
        Me.tabGoogle.ResumeLayout(False)
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
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
    Friend WithEvents btnDelete As System.Windows.Forms.Button
    Friend WithEvents btnUpdate As System.Windows.Forms.Button
    Friend WithEvents btnNowDel As System.Windows.Forms.Button
    Friend WithEvents lblFileSize As System.Windows.Forms.Label
    Friend WithEvents lblNowFolder As System.Windows.Forms.Label
    Friend WithEvents btnRankSearch As System.Windows.Forms.Button
    Friend WithEvents btnAddDaySearch As System.Windows.Forms.Button
    Friend WithEvents btnFolderTagAdd As System.Windows.Forms.Button
    Friend WithEvents btnFolderDel As System.Windows.Forms.Button
    Friend WithEvents btnNameSearch As Button
    Friend WithEvents btnTagSettinn As Button
    Friend WithEvents txtTagSetting As TextBox
    Friend WithEvents chkGoogle As CheckBox
    Friend WithEvents cmbTag9 As ComboBox
    Friend WithEvents cmbTag8 As ComboBox
    Friend WithEvents txtTag9 As TextBox
    Friend WithEvents txtTag8 As TextBox
    Friend WithEvents cmbTag7 As ComboBox
    Friend WithEvents txtTag7 As TextBox
    Friend WithEvents chkZoku1 As CheckBox
    Friend WithEvents chkZoku2 As CheckBox
    Friend WithEvents chkZoku3 As CheckBox
    Friend WithEvents chkZoku4 As CheckBox
    Friend WithEvents chkZoku5 As CheckBox
    Friend WithEvents chkZoku6 As CheckBox
    Friend WithEvents chkZoku7 As CheckBox
    Friend WithEvents chkZoku8 As CheckBox
    Friend WithEvents chkZoku9 As CheckBox
    Friend WithEvents chkZoku10 As CheckBox
    Friend WithEvents NotifyIcon1 As NotifyIcon
    Friend WithEvents picRank1 As PictureBox
    Friend WithEvents picRank2 As PictureBox
    Friend WithEvents picRank3 As PictureBox
    Friend WithEvents picRank4 As PictureBox
    Friend WithEvents picRank5 As PictureBox
    Friend WithEvents picRank0 As PictureBox
    Friend WithEvents lstBookMark As ListView
    Friend WithEvents TabControl1 As TabControl
    Friend WithEvents tabTree As TabPage
    Friend WithEvents treeDir As TreeView
    Friend WithEvents tabGoogle As TabPage
    Friend WithEvents WebBrowser1 As WebBrowser
    Friend WithEvents tabImageList As TabPage
    Friend WithEvents lstThumbs As ListView
    Friend WithEvents ilstBMThumbs As ImageList
    Friend WithEvents btnBookMarkAdd As Button
    Friend WithEvents btnBookMarkDel As Button
    Friend WithEvents btnBookMarkUpdate As Button
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents chkZoku11 As CheckBox
    Friend WithEvents chkZoku12 As CheckBox
    Friend WithEvents chkZoku13 As CheckBox
    Friend WithEvents chkZoku14 As CheckBox
    Friend WithEvents chkZoku15 As CheckBox
End Class
