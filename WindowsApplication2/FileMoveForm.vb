Imports Oracle.DataAccess.Client
Imports Oracle.DataAccess.Types

Public Class FileMoveForm

    'DBアクセス用クラスのインスタンスを作成
    Public clsOraAccess As OraAccess

    Private iNowFile As Integer
    Private iNowFolder As Integer
    Public sFilePath As String = Nothing
    Private iRank As Integer
    Private bFirstFlag As Boolean
    Private lstImgPath(5) As ArrayList
    Private bKensakuPattern As Boolean

    'Private Sub FileMoveForm_Activated(sender As Object, e As System.EventArgs) Handles Me.Activated
    '    Me.TopMost = False
    'End Sub

    Private Sub FileMoveForm_Click(sender As Object, e As System.EventArgs) Handles Me.Click, lstThumbs.Click, treeDir.Click, TabControl1.Click, TabPage1.Click, TabPage2.Click
        Me.TopMost = False
    End Sub

    'フォームクローズ時
    Private Sub FileMoveForm_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        clsOraAccess = Nothing
    End Sub

    'フォーム初期化
    Public Sub Form1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load


        Dim readerDropList As OracleDataReader = Nothing

        Dim sbuf As String = Nothing

        If (bFirstFlag = False) Then
            'コマンドライン引数から、ファイルパスを取得
            For Each sbuf In My.Application.CommandLineArgs
                Exit For
            Next
            sFilePath = sbuf
        End If

        Console.WriteLine("引数は「" & sFilePath & "」")

        Me.TopMost = True
        'Me.TopMost = False

        '検索フラグをオフ
        bKensakuPattern = False

        btnMove.Visible = True
        btnDelete.Visible = True
        btnUpdate.Visible = False
        btnNowDel.Visible = False
        Me.AcceptButton = Me.btnMove

        '＊＊＊プルダウン生成＊＊＊
        If (bFirstFlag = False) Then
            Dim tabTbl(6) As DataTable

            Try
                'DBアクセス用クラスのインスタンスを作成
                clsOraAccess = New OraAccess()
                bFirstFlag = True

                'プルダウンを取得
                clsOraAccess.queryDropList(readerDropList)

                For i = 0 To 5
                    tabTbl(i) = New DataTable

                    tabTbl(i).Columns.Add("ID", GetType(Integer))
                    tabTbl(i).Columns.Add("NAME", GetType(String))
                Next

                While (readerDropList.Read())

                    For i = 0 To 5
                        '新規行作成
                        Dim row As DataRow = tabTbl(i).NewRow()

                        '各行に値をセット
                        row("ID") = readerDropList.GetValue(0)
                        row("NAME") = readerDropList.GetString(1)
                        tabTbl(i).Rows.Add(row)
                    Next
                End While

                For i = 0 To 5
                    tabTbl(i).AcceptChanges()
                    Dim cmb As ComboBox
                    cmb = CType(Controls("cmbTag" & i + 1), ComboBox)
                    cmb.DataSource = tabTbl(i)
                    cmb.DisplayMember = "NAME"
                    cmb.ValueMember = "ID"
                Next

            Catch ex As Exception
                Console.WriteLine(ex.Message)
            Finally
                readerDropList.Close()
            End Try
        End If

        iRank = 5

        '存在チェック
        Dim readerFilePath As OracleDataReader = Nothing
        Try
            clsOraAccess.queryFilePath(sFilePath, readerFilePath)

            Dim sConditionValue As String = Nothing
            If (readerFilePath.HasRows = False) Then

                'フォーム生成
                '絞込み条件をタグから取得()
                sConditionValue = FormInit()

                '＊＊＊ツリービュー作成処理＊＊＊
                TreeCreate(sConditionValue)

                If (sConditionValue = Nothing) Then
                    cmbRank.SelectedIndex = 5
                End If

            Else
                'フォーム生成
                '絞込み条件をタグから取得()
                sConditionValue = FormInit(readerFilePath)

                '＊＊＊ツリービュー作成処理＊＊＊
                TreeCreate(sConditionValue)

                'サムネイル作成
                lstThumbs.Clear()
                ImageListCreate()

            End If

        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            readerFilePath.Close()
        End Try

    End Sub

    '初期化（既存ファイルなし時）
    Private Function FormInit() As String
        '＊＊＊フォーム生成＊＊＊
        'ファイルパスで指定されたZIP書庫を展開し、
        'フォルダ名、サムネイル、タグを取得

        Dim clsZipOpen As ZipOpen
        Dim sFileName As String
        Dim listTag As ArrayList
        Dim sConditionValue As String = Nothing
        Dim thumbnail As Image
        Dim fi As System.IO.FileInfo


        Console.WriteLine("初期化（既存ファイル無し時)")

        '初期化
        ClearTextBox(Me)
        ClearCombotBox(Me)

        'テキストタグの右クリックメニューを無効化
        txtTag1.ContextMenu = New ContextMenu
        txtTag2.ContextMenu = New ContextMenu
        txtTag3.ContextMenu = New ContextMenu
        txtTag4.ContextMenu = New ContextMenu
        txtTag5.ContextMenu = New ContextMenu
        txtTag6.ContextMenu = New ContextMenu

        Try
            'コンストラクタでファイルパスを指定
            clsZipOpen = New ZipOpen(sFilePath)

            'ファイル名取得し、フォームに値を設定
            sFileName = clsZipOpen.FileName
            Me.txtFileName.Text = sFileName

            Console.WriteLine("ファイル名" & txtFileName.Text)


            'タグ取得し、フォームに値を設定
            listTag = clsZipOpen.Tag
            Dim ilistIdx As Integer = 1

            '絞込み条件をタグから取得
            sConditionValue = Nothing

            For Each sValue In listTag
                Controls("txtTag" & ilistIdx).Text = sValue
                sConditionValue = sValue

                Dim iSelValue As Integer
                iSelValue = clsOraAccess.queryDefaultCategory(sValue)

                Dim cmb As ComboBox
                cmb = CType(Controls("cmbTag" & ilistIdx), ComboBox)
                cmb.SelectedValue = iSelValue
                ilistIdx += 1
            Next

            'イメージを取得し、フォームに設定
            thumbnail = clsZipOpen.Thumbs
            Me.picThumbs.Image = thumbnail

            'パスをフォームに設定
            Me.lblFilePath.Text = sFilePath

            fi = New System.IO.FileInfo(sFilePath)
            'ファイルのサイズを取得
            Me.lblFileSize.Text = ChangeFileSize(fi.Length)

            cmbRank.SelectedIndex = iRank - 1

            'ファイル名をテキストボックスに設定
            txtTagSetting.Text = clsZipOpen.FileMei

            btnUpdate.Focus()
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            fi = Nothing
            thumbnail = Nothing
            clsZipOpen = Nothing
        End Try


        'googleチェックありの場合、ファイル名でgoogle検索
        If (chkGoogle.Checked = True) Then
            Dim sName As String = txtTagSetting.Text
            If (sName <> "") Then
                TabControl1.SelectedTab = TabPage3
                WebBrowser1.Navigate(New Uri("https://www.google.co.jp/search?q=" & Uri.EscapeUriString(sName.Trim)))
                'Process.Start("https://www.google.co.jp/search?q=" & Uri.EscapeUriString(sName.Trim))
            End If
        End If

        'txtFileName.Focus()
        TabControl1.Focus()

        Return sConditionValue

    End Function

    '初期化（既存ファイルあり時）
    Private Function FormInit(ByRef readerFilePath As OracleDataReader) As String

        Console.WriteLine("初期化（既存ファイルあり時）")

        '初期化
        ClearTextBox(Me)
        ClearCombotBox(Me)

        'カレントファイル、フォルダを変数に格納
        iNowFile = readerFilePath.GetValue(0)
        iNowFolder = readerFilePath.GetValue(6)

        'パス、タイトル、ファイルサイズ設定
        Me.lblFilePath.Text = sFilePath
        Me.txtFileName.Text = readerFilePath.GetString(1)
        Me.lblFileSize.Text = ChangeFileSize(readerFilePath.GetValue(3))
        Me.lblNowFolder.Text = readerFilePath.GetString(7)

        btnMove.Visible = False
        btnDelete.Visible = False
        btnUpdate.Visible = True
        btnNowDel.Visible = True
        Me.AcceptButton = Me.btnUpdate

        Dim clsZipOpen As ZipOpen
        Dim readerFileTag As OracleDataReader = Nothing
        Try
            'ランクを設定
            cmbRank.SelectedIndex = readerFilePath.GetValue(5) - 1

            'コンストラクタでファイルパスを指定
            clsZipOpen = New ZipOpen(sFilePath)
            'イメージを取得し、フォームに設定
            Me.picThumbs.Image = clsZipOpen.Thumbs

            'タグ取得
            While (readerFilePath.Read())
                clsOraAccess.queryFileTag(readerFilePath.GetValue(0), readerFileTag)
                Exit While
            End While

            'タグを設定
            Dim ilistIdx As Integer = 1
            While (readerFileTag.Read())

                Controls("txtTag" & ilistIdx).Text = readerFileTag.GetString(1)

                Dim cmb As ComboBox
                cmb = Controls("cmbTag" & ilistIdx)
                cmb.SelectedValue = readerFileTag.GetValue(0)
                ilistIdx += 1
            End While
            readerFileTag.Close()

        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            readerFileTag.Close()
            clsZipOpen = Nothing
        End Try

        'txtFileName.Focus()
        TabControl1.Focus()

        Return readerFilePath.GetString(8)

    End Function

    '初期化(イメージリストクリック時）
    Private Function FormInit(ByVal sFilePath As String) As String

        '初期化
        ClearTextBox(Me)
        ClearCombotBox(Me)

        'カレントファイル、フォルダを変数に格納
        iNowFile = lstImgPath(0)(lstThumbs.SelectedItems(0).Index)

        'パス、タイトル、ファイルサイズ設定
        Me.lblFilePath.Text = sFilePath
        Me.txtFileName.Text = lstThumbs.SelectedItems(0).Text
        Me.lblFileSize.Text = ChangeFileSize(lstImgPath(3)(lstThumbs.SelectedItems(0).Index))

        btnMove.Visible = False
        btnDelete.Visible = False
        btnUpdate.Visible = True
        btnNowDel.Visible = True
        Me.AcceptButton = Me.btnUpdate

        Dim clsZipOpen As ZipOpen
        Dim readerFileTag As OracleDataReader = Nothing

        Try
            'ランクを設定
            cmbRank.SelectedIndex = lstImgPath(2)(lstThumbs.SelectedItems(0).Index) - 1

            'コンストラクタでファイルパスを指定
            clsZipOpen = New ZipOpen(sFilePath)

            'イメージを取得し、フォームに設定
            Me.picThumbs.Image = clsZipOpen.Thumbs

            'タグ取得
            clsOraAccess.queryFileTag(iNowFile, readerFileTag)

            'タグを設定
            Dim ilistIdx As Integer = 1
            While (readerFileTag.Read())

                Controls("txtTag" & ilistIdx).Text = readerFileTag.GetString(1)

                Dim cmb As ComboBox
                cmb = Controls("cmbTag" & ilistIdx)
                cmb.SelectedValue = readerFileTag.GetValue(0)
                ilistIdx += 1
            End While
            readerFileTag.Close()

        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            readerFileTag.Close()
            clsZipOpen = Nothing
        End Try

        'txtFileName.Focus()
        TabControl1.Focus()

        Return 0

    End Function

    'ツリービュー選択
    'カレントフォルダを変更
    Private Sub treeDir_AfterSelect(sender As Object, e As System.Windows.Forms.TreeViewEventArgs) Handles treeDir.AfterSelect
        If (treeDir.SelectedNode Is Nothing) Then Exit Sub
        iNowFolder = treeDir.SelectedNode.Name
        Me.lblNowFolder.Text = treeDir.SelectedNode.Text
    End Sub

    'ツリービューダブルクリック
    'タブ2に切り替え、イメージリストを作成し表示
    Private Sub treeDir_DoubleClick(sender As Object, e As System.EventArgs) Handles treeDir.DoubleClick

        '検索フラグをオン
        bKensakuPattern = False

        lstThumbs.Clear()
        If (treeDir.SelectedNode Is Nothing) Then Exit Sub
        iNowFolder = treeDir.SelectedNode.Name
        Me.lblNowFolder.Text = treeDir.SelectedNode.Text
        ImageListCreate()

    End Sub

    'イメージリスト作成
    Private Sub ImageListCreate()

        TabControl1.SelectedTab = TabPage2

        lstThumbs.Clear()
        Dim readerFileList As OracleDataReader = Nothing
        lstImgPath(0) = New ArrayList
        lstImgPath(1) = New ArrayList
        lstImgPath(2) = New ArrayList
        lstImgPath(3) = New ArrayList
        lstImgPath(4) = New ArrayList
        Try
            clsOraAccess.queryFileList(iNowFolder, readerFileList)

            While (readerFileList.Read())
                'ZipSamnail(readerFileList.GetString(2), readerFileList.GetValue(0), readerFileList.GetString(1), readerFileList.GetValue(3))
                'BLOBからデータを取得する場合は、上のZipSamnailをコメントし、下3行をコメントアウトする。
                'また、DAOについて、UNION ALLに変更する。
                Dim blob As OracleBlob = readerFileList.GetOracleBlob(4)
                Dim ms As New System.IO.MemoryStream(blob.Value)
                AddThumnail(New Bitmap(ms), readerFileList.GetValue(0), readerFileList.GetString(1), readerFileList.GetValue(3), readerFileList.GetValue(5))

                'Dim bmp As New Bitmap("D:\Users\chikurin\Desktop\tes\" & myReader.GetValue(0).ToString & ".bmp")
                'AddThumnail(bmp, 1, 1)

                lstImgPath(0).Add(readerFileList.GetValue(0))
                lstImgPath(1).Add(readerFileList.GetString(2))
                lstImgPath(2).Add(readerFileList.GetValue(5))
                lstImgPath(3).Add(readerFileList.GetValue(3))
                lstImgPath(4).Add(readerFileList.GetValue(7))
            End While
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            readerFileList.Close()
        End Try

    End Sub

    'イメージリスト作成
    Private Sub ImageListCreate(ByVal iKensakuKbn As Integer, ByVal iRank As Integer)

        TabControl1.SelectedTab = TabPage2

        lstThumbs.Clear()
        Dim readerFileList As OracleDataReader = Nothing
        lstImgPath(0) = New ArrayList
        lstImgPath(1) = New ArrayList
        lstImgPath(2) = New ArrayList
        lstImgPath(3) = New ArrayList
        lstImgPath(4) = New ArrayList
        Try
            clsOraAccess.queryFileListKensaku(iKensakuKbn, iRank, iNowFolder, readerFileList)

            While (readerFileList.Read())
                Dim blob As OracleBlob = readerFileList.GetOracleBlob(4)
                Dim ms As New System.IO.MemoryStream(blob.Value)
                AddThumnail(New Bitmap(ms), readerFileList.GetValue(0), readerFileList.GetString(1), readerFileList.GetValue(3), readerFileList.GetValue(5))

                lstImgPath(0).Add(readerFileList.GetValue(0))
                lstImgPath(1).Add(readerFileList.GetString(2))
                lstImgPath(2).Add(readerFileList.GetValue(5))
                lstImgPath(3).Add(readerFileList.GetValue(3))
                lstImgPath(4).Add(readerFileList.GetValue(7))
            End While
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            readerFileList.Close()
        End Try

    End Sub

    'イメージリスト作成
    Private Sub ImageListCreate(ByVal iKensakuKbn As Integer, ByVal iRank As Integer, ByVal sTagName As String)

        TabControl1.SelectedTab = TabPage2

        lstThumbs.Clear()
        Dim readerFileList As OracleDataReader = Nothing
        lstImgPath(0) = New ArrayList
        lstImgPath(1) = New ArrayList
        lstImgPath(2) = New ArrayList
        lstImgPath(3) = New ArrayList
        lstImgPath(4) = New ArrayList
        Try
            clsOraAccess.queryFileListKensaku(iKensakuKbn, iRank, sTagName, readerFileList)

            While (readerFileList.Read())
                Dim blob As OracleBlob = readerFileList.GetOracleBlob(4)
                Dim ms As New System.IO.MemoryStream(blob.Value)
                AddThumnail(New Bitmap(ms), readerFileList.GetValue(0), readerFileList.GetString(1), readerFileList.GetValue(3), readerFileList.GetValue(5))

                lstImgPath(0).Add(readerFileList.GetValue(0))
                lstImgPath(1).Add(readerFileList.GetString(2))
                lstImgPath(2).Add(readerFileList.GetValue(5))
                lstImgPath(3).Add(readerFileList.GetValue(3))
                lstImgPath(4).Add(readerFileList.GetValue(7))
            End While
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            readerFileList.Close()
        End Try

    End Sub

    'ツリービュー作成
    Public Sub TreeCreate(ByVal sConditionValue As String)

        'Readerを宣言
        Dim readerFolder As OracleDataReader = Nothing
        Dim readerGenre As OracleDataReader = Nothing
        Dim readerSubFolder As OracleDataReader = Nothing

        'ジャンルを取得
        '検索条件と合致しない場合は、後方１文字ずつ削除して繰り返し検索する。
        '合致なし、または検索条件なしの場合は、全件取得する。
        Try
            clsOraAccess.queryGenre(sConditionValue, readerGenre)
            If (readerGenre.HasRows = False) Then
                For i = 1 To sConditionValue.Length - 1
                    '後方１文字ずつ削る
                    sConditionValue = sConditionValue.Remove(sConditionValue.Length - 1)
                    clsOraAccess.queryGenre(sConditionValue, readerGenre)

                    'データを取得できれば、ループを抜ける
                    If (readerGenre.HasRows = True) Then
                        Exit For
                    Else
                        readerGenre.Close()
                    End If
                Next
            End If

            'これっている？
            If (readerGenre.IsClosed = True) Then
                'If (readerGenre.IsClosed = True Or readerGenre.HasRows = False) Then
                sConditionValue = Nothing
                clsOraAccess.queryGenre(sConditionValue, readerGenre)
            End If

            'フォルダを取得
            clsOraAccess.queryFolder(sConditionValue, readerFolder)

            treeDir.Nodes.Clear()
            'ツリーノード生成
            Dim iNowGroup As Integer
            Dim nowNode As TreeNode = Nothing
            Dim iNodeIdx As Integer = -1
            Dim iSubNodeIdx As Integer = -1

            While (readerFolder.Read())
                If Not (iNowGroup = readerFolder.GetValue(2)) Then
                    While (readerGenre.Read())
                        treeDir.Nodes.Add(readerGenre.GetValue(0).ToString, readerGenre.GetString(1))
                        iNowGroup = readerGenre.GetValue(0)
                        iNodeIdx = iNodeIdx + 1
                        iSubNodeIdx = -1
                        nowNode = treeDir.Nodes(iNodeIdx)
                        If (iNowGroup = readerFolder.GetValue(2)) Then
                            Exit While
                        End If
                    End While
                End If

                nowNode.Nodes.Add(readerFolder.GetValue(0).ToString, readerFolder.GetString(1))
                iSubNodeIdx = iSubNodeIdx + 1

                'サブフォルダを検索
                clsOraAccess.querySubFolder(readerFolder.GetValue(0).ToString, readerSubFolder)
                While (readerSubFolder.Read())
                    Dim node As TreeNode = Nothing
                    nowNode.Nodes(iSubNodeIdx).Nodes.Add(readerSubFolder.GetValue(0), readerSubFolder.GetString(1))
                End While
                If (readerGenre.IsClosed = False) Then
                    readerSubFolder.Close()
                End If
            End While
            If (sConditionValue <> Nothing) Then
                treeDir.ExpandAll()
            End If
        Catch ex As Exception

            MsgBox(ex.Message)

        Finally
            readerGenre.Close()
            readerFolder.Close()
            If Not (readerSubFolder Is Nothing) Then
                readerSubFolder.Close()
            End If
        End Try

    End Sub

    'ZIP書庫情報取得
    'サムネイルを作成し、ファイルサイズやファイル名を取得
    Public Sub ZipSamnail(ByVal sPath As String, ByVal iNowId As Integer, ByVal sFileName As String, ByVal lFileSize As Long)

        'ZipFileオブジェクトの作成 
        Dim zf As ICSharpCode.SharpZipLib.Zip.ZipFile = Nothing
        Dim zis As ICSharpCode.SharpZipLib.Zip.ZipInputStream = Nothing
        Dim fs As System.IO.FileStream = Nothing
        Dim ze As ICSharpCode.SharpZipLib.Zip.ZipEntry = Nothing

        Try
            zf = New ICSharpCode.SharpZipLib.Zip.ZipFile(sPath)

            'ZIP書庫を読み込む 
            fs = New System.IO.FileStream( _
                sPath, _
                System.IO.FileMode.Open, _
                System.IO.FileAccess.Read)

            'ZipInputStreamオブジェクトの作成
            zis = New ICSharpCode.SharpZipLib.Zip.ZipInputStream(fs)

            'ZIP内のエントリを列挙
            While True
                'ZipEntryを取得
                ze = zis.GetNextEntry()
                If ze Is Nothing Then
                    Exit While
                End If
                '情報を表示する 
                If ze.IsFile Then
                    'ファイルのとき 
                    '閲覧するZIPエントリのStreamを取得 
                    Dim reader As System.IO.Stream = zf.GetInputStream(ze)
                    Dim bmp As New Bitmap(reader)
                    AddThumnail(bmp, iNowId, sFileName, lFileSize, 0) 'ランクの０は仮置き
                    bmp = Nothing
                    reader.Close()
                    Exit While
                ElseIf ze.IsDirectory Then
                    'ディレクトリのとき 
                    'Console.WriteLine("ディレクトリ名 : {0}", ze.Name)
                    'Console.WriteLine("日時 : {0}", ze.DateTime)
                    'Console.WriteLine()
                End If
            End While
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            '閉じる
            ze = Nothing
            zis.Close()
            fs.Close()
            zf.Close()
        End Try

    End Sub


    'サムネイル追加
    Private icount As Integer
    Private Sub AddThumnail(ByRef bmp As Bitmap, ByVal iNowId As Integer, ByVal sFileName As String, ByVal lFileSize As Long, ByVal lRank As Long)

        Dim width As Integer = 96
        Dim height As Integer = 96

        ilstThumbs.ImageSize = New Size(width, height)
        lstThumbs.LargeImageList = ilstThumbs

        Dim original As Image = bmp
        Dim thumbnail As Image = createThumbnail(original, width, height)

        '一括登録用
        'clsOraAccess.updateFile(iNowId, thumbnail)

        ilstThumbs.Images.Add(thumbnail)
        Dim lvi As New ListViewItem(sFileName, icount)
        lvi.ToolTipText = ChangeFileSize(lFileSize)

        'ランクにより文字背景色をかえる
        Select Case lRank
            Case 10
                lvi.BackColor = Color.Gold
            Case 9
                lvi.BackColor = Color.Silver
            Case 8
                lvi.BackColor = Color.Orange
            Case 7
                lvi.BackColor = Color.Aqua
            Case 6
                lvi.BackColor = Color.YellowGreen
            Case 5
                lvi.BackColor = Color.White
            Case Else
                lvi.BackColor = Color.Gray
        End Select

        '        lvi.BackColor = Color.Aqua
        lstThumbs.Items.Add(lvi)
        icount = icount + 1
        original.Dispose()
        thumbnail.Dispose()

    End Sub


    '移動ボタンクリック
    'ファイルを移動し、ファイルTBL,ファイルタグTBLに新規登録
    Private Sub btnMove_Click(sender As System.Object, e As System.EventArgs) Handles btnMove.Click

        If (iNowFolder < 1) Then
            MsgBox("親フォルダが選択されていません。")
            Exit Sub
        End If

        'ファイルサイズを取得
        Dim fi As System.IO.FileInfo = New System.IO.FileInfo(sFilePath)
        Dim lsize As Long = fi.Length
        fi = Nothing

        Try

            'ファイルを移動
            Dim sAftPath As String = clsOraAccess.queryFolderPath(iNowFolder)
            System.IO.File.Move(sFilePath, sAftPath & txtFileName.Text & ".zip")

            'ファイルをTBLに追加
            clsOraAccess.insertFile(txtFileName.Text, txtFileName.Text & ".zip", iNowFolder, lsize, cmbRank.SelectedIndex + 1, picThumbs.Image)

            'プルダウンの選択値を取得

            For i = 1 To 6
                If (Controls("txtTag" & i).Text <> "") Then

                    Dim cmb As ComboBox
                    cmb = Controls("cmbTag" & i)
                    If (cmb.SelectedValue > 0) Then
                        'ファイルタグをTBLに追加
                        clsOraAccess.insertFileTag(cmb.SelectedValue, Controls("txtTag" & i).Text)
                    End If
                End If
            Next

            Process.Start("C:\ProgramData\leeyes\Plugin\ExtractFile.exe", """" & sAftPath & txtFileName.Text & ".zip""")

            If (TabControl1.SelectedTab Is TabPage2) Then
                ImageListCreate()
            End If

            MessageBox.Show("ファイルの追加が完了しました。")

        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
        End Try

    End Sub

    'テキストボックス押下時のイベント
    '右ダブルクリックを取得
    Private Sub txtTag_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles txtTag1.MouseDown, txtTag2.MouseDown, txtTag3.MouseDown, txtTag4.MouseDown, txtTag5.MouseDown, txtTag6.MouseDown

        'マウスのダブルクリックイベント
        If e.Button = MouseButtons.Right AndAlso e.Clicks = 2 Then

            Dim sChildFolderName As String = Nothing

            If sender Is txtTag1 Then
                sChildFolderName = txtTag1.Text
            ElseIf sender Is txtTag2 Then
                sChildFolderName = txtTag2.Text
            ElseIf sender Is txtTag3 Then
                sChildFolderName = txtTag3.Text
            ElseIf sender Is txtTag4 Then
                sChildFolderName = txtTag4.Text
            ElseIf sender Is txtTag5 Then
                sChildFolderName = txtTag5.Text
            ElseIf sender Is txtTag6 Then
                sChildFolderName = txtTag6.Text
            End If

            Dim iKensakuKbn As Integer
            If (chkOver.Checked = True) Then
                iKensakuKbn = 4
            Else
                iKensakuKbn = 3
            End If

            ImageListCreate(iKensakuKbn, cmbRank.SelectedIndex + 1, sChildFolderName)
            TabControl1.SelectedTab = TabPage2
        End If
    End Sub

    'テキストボックスをダブルクリック
    '入力値でフォルダTBLを検索し、ツリービューを変更
    Private Sub txtTag_DoubleClick(sender As Object, e As System.EventArgs) Handles txtTag1.DoubleClick, txtTag2.DoubleClick, txtTag3.DoubleClick, txtTag4.DoubleClick, txtTag5.DoubleClick, txtTag6.DoubleClick

        Dim sChildFolderName As String = Nothing

        If sender Is txtTag1 Then
            sChildFolderName = txtTag1.Text
        ElseIf sender Is txtTag2 Then
            sChildFolderName = txtTag2.Text
        ElseIf sender Is txtTag3 Then
            sChildFolderName = txtTag3.Text
        ElseIf sender Is txtTag4 Then
            sChildFolderName = txtTag4.Text
        ElseIf sender Is txtTag5 Then
            sChildFolderName = txtTag5.Text
        ElseIf sender Is txtTag6 Then
            sChildFolderName = txtTag6.Text
        End If

        TreeCreate(sChildFolderName)
        TabControl1.SelectedTab = TabPage1

    End Sub

    'メモリ解放
    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    'フォルダ追加ボタン
    'フォルダを作成し、フォルダTBL、フォルダタグTBLに新規登録
    Private Sub btnAddFolder_Click(sender As System.Object, e As System.EventArgs) Handles btnAddFolder.Click

        If (iNowFolder < 1) Then
            MsgBox("親フォルダが選択されていません。")
            Exit Sub
        End If

        Dim sChildFolderName As String = Nothing

        For i = 1 To 6
            Dim cmb As ComboBox
            cmb = CType(Controls("cmbTag" & i), ComboBox)

            Select Case iNowFolder
                Case 1, 2, 3, 4
                    If (cmb.SelectedValue = 1) Then
                        If (Controls("txtTag" & i).Text <> "") Then
                            sChildFolderName = Controls("txtTag" & i).Text
                            Exit For
                        End If
                    End If

                Case 5
                    If (cmb.SelectedValue = 2) Then
                        If (Controls("txtTag" & i).Text <> "") Then
                            sChildFolderName = Controls("txtTag" & i).Text
                            Exit For
                        End If
                    End If
                Case 8
                    If (cmb.SelectedValue = 2) Then
                        If (Controls("txtTag" & i).Text <> "") Then
                            sChildFolderName = Controls("txtTag" & i).Text
                            Exit For
                        End If
                    ElseIf (cmb.SelectedValue = 0) Then
                        If (Controls("txtTag" & i).Text <> "") Then
                            If (MsgBox(Controls("txtTag" & i).Text & "をフォルダ名にしますか？", vbYesNo) = vbYes) Then
                                cmb.SelectedValue = 2
                                sChildFolderName = Controls("txtTag" & i).Text
                                Exit For
                            End If
                        End If
                    End If
                Case Else
                    If (cmb.SelectedValue = 1 Or cmb.SelectedValue = 2) Then
                        If (Controls("txtTag" & i).Text <> "") Then
                            sChildFolderName = Controls("txtTag" & i).Text
                            Exit For
                        End If
                    End If
            End Select
        Next

        If (sChildFolderName Is Nothing) Then
            MsgBox("フォルダ名未入力です。")
            Exit Sub
        End If

        If (MsgBox("フォルダを追加します。よろしいですか？" & vbCrLf & vbCrLf & sChildFolderName, vbYesNo) = vbNo) Then Exit Sub

        Try

            'フォルダの作成
            Dim sParentFolderPath As String = clsOraAccess.queryFolderPath(iNowFolder)
            System.IO.Directory.CreateDirectory(sParentFolderPath & sChildFolderName)
            iNowFolder = clsOraAccess.insertFolder(iNowFolder, sChildFolderName)

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

        TreeCreate(sChildFolderName)
        If (TabControl1.SelectedTab Is TabPage2) Then
            ImageListCreate()
        End If

        If (MsgBox("フォルダを追加しました。続けてファイルを移動しますか？" & vbCrLf & vbCrLf & sChildFolderName, vbYesNo) = vbYes) Then

            btnMove.PerformClick()

        End If
    End Sub

    'ファイル削除（DB削除なし）
    Private Sub btnDelete_Click(sender As System.Object, e As System.EventArgs) Handles btnDelete.Click

        If (MsgBox("削除しますか？" & vbCrLf & vbCrLf & sFilePath, vbYesNo) = vbYes) Then
            Try
                My.Computer.FileSystem.DeleteFile(sFilePath, FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.SendToRecycleBin)
                MessageBox.Show("削除が完了しました。")
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        End If
    End Sub

    'サムネイルダブルクリック
    'Hamana起動
    Private Sub picThumbs_DoubleClick(sender As Object, e As System.EventArgs) Handles picThumbs.DoubleClick

        Process.Start("C:\ProgramData\leeyes\Leeyes.exe", """" & sFilePath & """")
        'Process.Start("C:\ProgramData\Hamana\Hamana.exe", """" & sFilePath & """")

    End Sub

    'イメージリストダブルクリック
    '初期化（ファイルあり）を呼び出し、フォームに情報を表示
    Private Sub lstThumbs_DoubleClick(sender As Object, e As System.EventArgs) Handles lstThumbs.DoubleClick

        sFilePath = CType(lstImgPath(1)(lstThumbs.SelectedItems(0).Index), String)

        If (bKensakuPattern = True) Then
            iNowFolder = lstImgPath(4)(lstThumbs.SelectedItems(0).Index)
            lblNowFolder.Text = "同一"
        End If

        'MsgBox("ここで同一ちぇく")
        ''存在チェック
        'Dim readerFilePath As OracleDataReader = Nothing
        'Try
        '    clsOraAccess.queryFilePath(sFilePath, readerFilePath)

        '    Dim sConditionValue As String = Nothing
        '    If (readerFilePath.HasRows = False) Then

        '        Dim cnt As Integer = 0
        '        While (readerFilePath.Read())
        '            cnt = cnt + 1
        '            Exit While
        '        End While
        '        MsgBox(cnt)

        '    End If
        'Catch ex As Exception
        '    MsgBox(ex.Message)
        'End Try


        FormInit(sFilePath)
    End Sub

    '更新ボタン　クリック
    '登録ファイルを移動して、ファイルTBL、ファイルタグTBLを更新
    Private Sub btnUpdate_Click(sender As System.Object, e As System.EventArgs) Handles btnUpdate.Click

        If (iNowFolder < 1) Then
            MsgBox("親フォルダが選択されていません。")
            Exit Sub
        ElseIf (iNowFolder < 9) Then
            MsgBox("ルートフォルダに移動できません")
            Exit Sub
        End If

        Try
            'ファイルを移動
            Dim sAftPath As String = clsOraAccess.queryFolderPath(iNowFolder)
            If (sFilePath <> sAftPath & txtFileName.Text & ".zip") Then
                System.IO.File.Move(sFilePath, sAftPath & txtFileName.Text & ".zip")
            End If

            'ファイルをTBLに追加
            clsOraAccess.updateFile(iNowFile, iNowFolder, txtFileName.Text, txtFileName.Text & ".zip", cmbRank.SelectedIndex + 1, picThumbs.Image)

            'ファイルタグをTBLから削除
            clsOraAccess.deleteFileTag(iNowFile)

            'プルダウンの選択値を取得し、ファイルタグをTBLに追加
            For i = 1 To 6
                If (Controls("txtTag" & i).Text <> "") Then

                    Dim cmb As ComboBox
                    cmb = Controls("cmbTag" & i)

                    If (cmb.SelectedValue > 0) Then
                        'ファイルタグをTBLに追加
                        clsOraAccess.insertFileTag(cmb.SelectedValue, Controls("txtTag" & i).Text)
                    End If
                End If
            Next
            If (TabControl1.SelectedTab Is TabPage2 And
                bKensakuPattern = False) Then
                ImageListCreate()
            End If


            MessageBox.Show("ファイルの更新が完了しました。")

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    '削除ボタンクリック
    'DBの情報を削除と同時に、ファイルの情報も削除する
    Private Sub btnNowDel_Click(sender As System.Object, e As System.EventArgs) Handles btnNowDel.Click

        If (MsgBox("削除しますか？" & vbCrLf & vbCrLf & sFilePath, vbYesNo) = vbYes) Then

            Try
                My.Computer.FileSystem.DeleteFile(sFilePath, FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.SendToRecycleBin)
                clsOraAccess.deleteFile(iNowFile)

                If (TabControl1.SelectedTab Is TabPage2) Then
                    ImageListCreate()
                End If

                MessageBox.Show("削除が完了しました。")
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        End If

    End Sub

    Private Sub txtFileName_Enter(sender As Object, e As System.EventArgs) Handles txtFileName.Enter
        Me.txtFileName.SelectAll()
    End Sub

    Private Sub txtFileName_MouseClick(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles txtFileName.MouseClick
        Me.txtFileName.SelectAll()
    End Sub

    'ランク順ボタンクリック
    Private Sub btnRankSearch_Click(sender As System.Object, e As System.EventArgs) Handles btnRankSearch.Click

        '検索フラグをオン
        bKensakuPattern = True

        If (iNowFolder < 1) Then
            MsgBox("フォルダが選択されていません")
            Exit Sub
        End If

        Dim iKensakuKbn As Integer
        If (chkOver.Checked = True) Then
            iKensakuKbn = 2
        Else
            iKensakuKbn = 1
        End If

        ImageListCreate(iKensakuKbn, cmbRank.SelectedIndex + 1)
    End Sub

    '更新日順ボタンクリック
    Private Sub btnAddDaySearch_Click(sender As System.Object, e As System.EventArgs) Handles btnAddDaySearch.Click

        '検索フラグをオン
        bKensakuPattern = True

        ImageListCreate(0, cmbRank.SelectedIndex + 1)
    End Sub

    'タグ追加 ボタンクリック
    Private Sub btnFolderTagUpdate_Click(sender As System.Object, e As System.EventArgs) Handles btnFolderTagAdd.Click

        If (iNowFolder < 1) Then
            MsgBox("フォルダが選択されていません")
            Exit Sub
        End If

        'プルダウンの選択値を取得し、ファイルタグをTBLに追加
        Try
            For i = 1 To 6
                If (Controls("txtTag" & i).Text <> "") Then

                    Dim cmb As ComboBox
                    cmb = Controls("cmbTag" & i)

                    If (cmb.SelectedValue > 0) Then
                        'ファイルタグをTBLに追加
                        clsOraAccess.insertFolderTag(iNowFolder, Controls("txtTag" & i).Text)
                    End If
                End If
            Next
            MessageBox.Show("フォルダタグの更新が完了しました。")

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    'フォルダ削除クリック
    Private Sub btnFolderDel_Click(sender As System.Object, e As System.EventArgs) Handles btnFolderDel.Click

        If (iNowFolder < 1) Then
            MsgBox("フォルダが選択されていません")
            Exit Sub
        ElseIf (iNowFolder < 9) Then
            MsgBox("ルートフォルダは削除できません")
            Exit Sub
        ElseIf (MsgBox("フォルダを削除しますか？", vbYesNo) = vbNo) Then
            Exit Sub
        End If

        Dim sFolderPath As String

        Try
            'フォルダパスを取得
            sFolderPath = clsOraAccess.queryFolderPath(iNowFolder)

            If (sFolderPath Is Nothing) Then
                MsgBox("フォルダパスを取得できませんでした")
                Exit Sub
            End If

            'DBからフォルダ、フォルダタグを削除
            clsOraAccess.deleteFolder(iNowFolder)

            'フォルダを削除
            My.Computer.FileSystem.DeleteDirectory(sFolderPath, FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.SendToRecycleBin)
            MessageBox.Show("削除が完了しました。")
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub

    'プルダウンの色を設定
    Private Sub cmbRank_DrawItem(sender As Object, e As System.Windows.Forms.DrawItemEventArgs) Handles cmbRank.DrawItem

        If e.Index = -1 Then Exit Sub

        Dim Combo As ComboBox = sender
        Dim TextBrush As Brush = Brushes.Black
        Dim TextString As String = Combo.Items(e.Index).ToString
        Dim BackColor As Color

        Dim TextRect As RectangleF  '文字領域の設定

        With TextRect
            .X = e.Bounds.X
            .Y = e.Bounds.Y
            .Width = e.Bounds.Width
            .Height = e.Bounds.Height
        End With

        e.DrawBackground() 'フォーカス背景色描画用


        Select Case TextString

            Case 10
                BackColor = Color.Gold
            Case 9
                BackColor = Color.Silver
            Case 8
                BackColor = Color.Orange
            Case 7
                BackColor = Color.Aqua
            Case 6
                BackColor = Color.YellowGreen
            Case 5
                BackColor = Color.White
            Case Else
                BackColor = Color.Gray
        End Select

        e.Graphics.FillRectangle(New SolidBrush(BackColor), TextRect)


        '文字の描画
        e.Graphics.DrawString(TextString, e.Font, TextBrush, TextRect)
        e.DrawFocusRectangle()  'フォーカス背景色描画用

    End Sub

    'テンキー押下時にランクのプルダウンを変える

    Private Sub FileMoveForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'フォームでキーボードイベントを認識するにはKeyPreview = True にする。
        Me.KeyPreview = True

    End Sub

    Private Sub FileMoveForm_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp

        Select Case e.KeyCode
            Case Keys.NumPad1
                cmbRank.SelectedIndex = 0
            Case Keys.NumPad2
                cmbRank.SelectedIndex = 1
            Case Keys.NumPad3
                cmbRank.SelectedIndex = 2
            Case Keys.NumPad4
                cmbRank.SelectedIndex = 3
            Case Keys.NumPad5
                cmbRank.SelectedIndex = 4
            Case Keys.NumPad6
                cmbRank.SelectedIndex = 5
            Case Keys.NumPad7
                cmbRank.SelectedIndex = 6
            Case Keys.NumPad8
                cmbRank.SelectedIndex = 7
            Case Keys.NumPad9
                cmbRank.SelectedIndex = 8
            Case Keys.NumPad0
                cmbRank.SelectedIndex = 9
        End Select
    End Sub


    'クリップボード取得
    Private Sub GetClipboardText()
        Dim clipboardText As String = ""
        clipboardText = Clipboard.GetText()
        Console.WriteLine(clipboardText)
    End Sub

    Private viewer As MyClipboardViewer

    Public Sub New()
        viewer = New MyClipboardViewer(Me)
        ' イベントハンドラを登録
        AddHandler viewer.ClipboardHandler, AddressOf OnClipBoardChanged

        ' この呼び出しは、Windows フォーム デザイナで必要です。
        InitializeComponent()
    End Sub

    ' クリップボードにテキストがコピーされると呼び出される
    Private Sub OnClipBoardChanged(ByVal sender As Object, ByVal args As ClipboardEventArgs)

        txtTagSetting.Text = Trim(args.Text)

        'googleにチェックがある場合、タグセットボタンイベントを呼び出し
        If (chkGoogle.Checked = True) Then
            If (txtTagSetting.Text <> "") Then
                btnTagSettinn.PerformClick()
            End If
        End If

    End Sub

    'googleボタンクリック
    Private Sub btnNameSearch_Click(sender As Object, e As EventArgs) Handles btnNameSearch.Click
        Dim sName As String = txtTagSetting.Text
        If (sName <> "") Then
            TabControl1.SelectedTab = TabPage3
            WebBrowser1.Navigate(New Uri("https://www.google.co.jp/search?q=" & Uri.EscapeUriString(sName.Trim)))
        End If
    End Sub

    'タグ再設定テキストダブルクリック
    Private Sub txtTagSetting_DoubleClick(sender As Object, e As EventArgs) Handles txtTagSetting.DoubleClick
        Dim sName As String = txtTagSetting.Text
        If (sName <> "") Then
            TabControl1.SelectedTab = TabPage3
            WebBrowser1.Navigate(New Uri("https://www.google.co.jp/search?q=" & Uri.EscapeUriString(sName.Trim)))
        End If

    End Sub


    'タグ再設定ボタンクリック
    Private Sub btnTagSettinn_Click(sender As Object, e As EventArgs) Handles btnTagSettinn.Click

        '＊＊＊フォーム生成＊＊＊
        'ファイルパスで指定されたZIP書庫を展開し、
        'フォルダ名、サムネイル、タグを取得

        Dim clsZipOpen As ZipOpen
        Dim sFileName As String
        Dim listTag As ArrayList
        Dim sConditionValue As String = Nothing
        Dim befRank As Integer

        Console.WriteLine("タグ再設定")

        '現在のランクを保持
        befRank = cmbRank.SelectedIndex

        '初期化
        ClearTextBox2(Me)
        ClearCombotBox(Me)

        'コンストラクタでファイルパスを指定
        clsZipOpen = New ZipOpen()
        clsZipOpen.tagCreate(txtTagSetting.Text)

        'ファイル名取得し、フォームに値を設定
        sFileName = clsZipOpen.FileName
        Me.txtFileName.Text = sFileName

        Console.WriteLine("ファイル名" & txtFileName.Text)


        'タグ取得し、フォームに値を設定
        listTag = clsZipOpen.Tag
        Dim ilistIdx As Integer = 1

        '絞込み条件をタグから取得
        sConditionValue = Nothing

        For Each sValue In listTag
            Controls("txtTag" & ilistIdx).Text = sValue
            sConditionValue = sValue

            Dim iSelValue As Integer
            iSelValue = clsOraAccess.queryDefaultCategory(sValue)

            Dim cmb As ComboBox
            cmb = CType(Controls("cmbTag" & ilistIdx), ComboBox)
            cmb.SelectedValue = iSelValue
            ilistIdx += 1
        Next

        'ランクを再設定
        cmbRank.SelectedIndex = befRank

        '＊＊＊ツリービュー作成処理＊＊＊
        TreeCreate(sConditionValue)

        'txtFileName.Focus()
        TabControl1.Focus()

    End Sub


End Class



