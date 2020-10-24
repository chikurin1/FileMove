Imports Oracle.DataAccess.Client
Imports Oracle.DataAccess.Types

Public Class FileMoveForm

    ''' <summary>
    ''' DBアクセス用クラスのインスタンス
    ''' </summary>
    Public clsOraAccess As OraAccess

    ''' <summary>
    ''' 初回起動の判定用
    ''' FALSEの場合はStartProcを起動する
    ''' </summary>
    Private bStartFlag As Boolean = False
    Private bFirstFlag As Boolean

    ''' <summary>
    ''' 現在のファイルID
    ''' </summary>
    Private iNowFile As Integer

    ''' <summary>
    ''' 現在のフォルダID
    ''' </summary>
    Private iNowFolder As Integer

    ''' <summary>
    ''' 現在のブックマークID
    ''' </summary>
    Private iBookMarkId As Integer

    ''' <summary>
    ''' ファイルのフルパス
    ''' </summary>
    Public sFilePath As String = Nothing

    ''' <summary>
    ''' ブックマークファイルのフルパス
    ''' </summary>
    Public sBookMarkPath As String = Nothing

    ''' <summary>
    ''' ブックマークのファイル名
    ''' </summary>
    Public sBookMarkFileName As String = Nothing



    ''' <summary>
    ''' 現在のランク
    ''' </summary>
    Private iRank As Integer

    ''' <summary>
    ''' ファイルリスト(ファイルリストビューの設定値）
    ''' 0:ファイルID
    ''' 1:ファイルパス
    ''' 2:ランク
    ''' 3:サイズ
    ''' 4:フォルダID
    ''' </summary>
    Private lstImgPath(5) As ArrayList

    ''' <summary>
    ''' ブックマークリスト(ブックマークリストビューの設定値）
    ''' 0:ファイルID
    ''' 1:ファイルパス
    ''' 2:ランク
    ''' 3:サイズ
    ''' 4:フォルダID
    ''' 5:ブックマークID
    ''' 6:ファイル名
    ''' </summary>
    Private lstBookMarkPath(7) As ArrayList

    ''' <summary>
    ''' 検索フラグ
    ''' </summary>
    Private bKensakuPattern As Boolean

    ''' <summary>
    ''' ランキング用の☆マーク（白抜き）
    ''' </summary>
    Private imgWhite As System.Drawing.Image = System.Drawing.Image.FromFile("C:\ProgramData\NeeView\Plugin\星白.png")

    ''' <summary>
    ''' ''' ランキング用の☆マーク（黒埋め）
    ''' </summary>
    Private imgBlack As System.Drawing.Image = System.Drawing.Image.FromFile("C:\ProgramData\NeeView\Plugin\星黒.png")


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


    ''' <summary>
    ''' フォーム初期化
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Public Sub Form1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load


        Dim sbuf As String = Nothing
        Dim clsFormUtil As New FormUtil(Me)

        Console.WriteLine("Form1_Load開始")

        clsFormUtil.ShowBookMark(False)

        If (bFirstFlag = False) Then
            'コマンドライン引数から、ファイルパスを取得
            For Each sbuf In My.Application.CommandLineArgs
                sFilePath = sbuf
                Exit For
            Next
        End If

        sFilePath = System.Web.HttpUtility.UrlDecode(sFilePath)
        Console.WriteLine("引数は「" & sFilePath & "」")


        '初回起動時のみStartProcを呼び出し、タグのプルダウン、属性チェックを設定
        If bStartFlag = False Then
            StartProc()
            bStartFlag = True
        End If

        iRank = 1

        'コマンドライン引数が未設定の場合、ツリービューを作成し処理終了。
        If sFilePath Is Nothing Then
            TreeCreate("")
            Exit Sub
        End If

        Select Case System.IO.Path.GetExtension(sFilePath).ToLower
            Case ".zip"
                sBookMarkPath = Nothing
            Case ".jpg", ".jpeg", ".png"
                sBookMarkPath = sFilePath

                'ファイルパスにzip書庫を設定
                sFilePath = sFilePath.Substring(0, sFilePath.IndexOf(".zip") + 4)

                Console.WriteLine(sFilePath)

            Case ".rar"
                If MsgBox("rarファイルです。変換しますか？", vbYesNo) = vbYes Then
                    Process.Start("C:\Users\chikurin\source\repos\ExtractFile\ExtractFile\bin\x64\Release\ExtractFile.exe", """" & sFilePath & """")
                End If
                sBookMarkPath = Nothing
                TreeCreate("")
                Exit Sub
            Case Else
                Exit Sub
        End Select


        Dim readerFilePath As OracleDataReader = Nothing
        Dim sConditionValue As String = Nothing

        Try
            'コマンドライン引数がブックマークの場合
            If sBookMarkPath IsNot Nothing Then

                'パスからブックマーク情報を取得
                clsOraAccess.queryBookMarkPath(sBookMarkPath, readerFilePath)

                '存在チェック
                If readerFilePath.HasRows = True Then
                    'ブックマークテーブルにデータがある場合、ファイルを取得

                    'フォーム生成
                    '絞込み条件をタグから取得()
                    sConditionValue = BMFormInit(readerFilePath)

                    '＊＊＊ツリービュー作成処理＊＊＊
                    TreeCreate(sConditionValue)

                    'サムネイル作成
                    lstThumbs.Clear()
                    ImageListCreate()

                    readerFilePath.Close()

                    '初期処理終了、フォームを表示
                    Exit Sub
                End If
            End If


            'パスからファイル情報を取得
            clsOraAccess.queryFilePath(sFilePath, readerFilePath)

            'zipファイルの存在チェック
            If readerFilePath.HasRows = True Then
                'ファイルTBLにzipファイルがある場合

                If sBookMarkPath IsNot Nothing Then

                    'zipファイルのファイルIDを取得
                    clsFormUtil.FileIDGet(readerFilePath, iNowFile, iNowFolder)

                    'フォーム生成
                    '絞込み条件をタグから取得()
                    sConditionValue = BMFormInit()

                    '＊＊＊ツリービュー作成処理＊＊＊
                    TreeCreate(sConditionValue)

                    'サムネイル作成
                    lstThumbs.Clear()
                    ImageListCreate()


                    '初期処理終了、フォームを表示


                Else

                    'ファイル表示処理
                    'ファイルTBLから取得した情報をもとにフォーム、ツリービュー設定

                    'フォーム生成
                    '絞込み条件をタグから取得()
                    sConditionValue = FormInit(readerFilePath)

                    '＊＊＊ツリービュー作成処理＊＊＊
                    TreeCreate(sConditionValue)

                    'サムネイル作成
                    lstThumbs.Clear()
                    ImageListCreate()

                End If

            Else
                'ファイルTBLにデータが無い場合、パラメータのファイル名からフォーム、ツリービュー設定


                'ブックマーク追加処理では確認MSGを表示
                If sBookMarkPath IsNot Nothing Then

                    MsgBox("ブックマーク追加前にファイルテーブルへの追加を行います")

                End If

                'フォーム生成
                '絞込み条件をタグから取得()
                sConditionValue = FormInit()

                '＊＊＊ツリービュー作成処理＊＊＊
                TreeCreate(sConditionValue)

                If (sConditionValue = Nothing) Then
                    RankImageChange(1)
                End If

            End If



            '初期処理終了、フォームを表示

        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            readerFilePath.Close()
        End Try

    End Sub


    ''' <summary>
    ''' 初期起動時にのみ呼ばれる
    ''' タグのプルダウン設定、属性チェックの初期設定を行う
    ''' </summary>
    Private Sub StartProc()

        Dim readerDropList As OracleDataReader = Nothing
        Dim clsFormUtil As New FormUtil(Me)

        Console.WriteLine("FStartProc開始")

        Me.TopMost = True
        'Me.TopMost = False

        'テキストタグの右クリックメニューを無効化
        txtTag1.ContextMenu = New ContextMenu
        txtTag2.ContextMenu = New ContextMenu
        txtTag3.ContextMenu = New ContextMenu
        txtTag4.ContextMenu = New ContextMenu
        txtTag5.ContextMenu = New ContextMenu
        txtTag6.ContextMenu = New ContextMenu
        txtTag7.ContextMenu = New ContextMenu
        txtTag8.ContextMenu = New ContextMenu
        txtTag9.ContextMenu = New ContextMenu

        '検索フラグをオフ
        bKensakuPattern = False

        '追加モードでボタン設定
        clsFormUtil.BtnAddMode()

        '＊＊＊プルダウン生成＊＊＊
        If (bFirstFlag = False) Then
            Dim tabTbl(9) As DataTable

            Try
                'DBアクセス用クラスのインスタンスを作成
                clsOraAccess = New OraAccess()
                bFirstFlag = True

                'プルダウンを取得
                clsOraAccess.queryDropList(readerDropList)

                For i = 0 To 8
                    tabTbl(i) = New DataTable

                    tabTbl(i).Columns.Add("ID", GetType(Integer))
                    tabTbl(i).Columns.Add("NAME", GetType(String))
                Next

                While (readerDropList.Read())

                    For i = 0 To 8
                        '新規行作成
                        Dim row As DataRow = tabTbl(i).NewRow()

                        '各行に値をセット
                        row("ID") = readerDropList.GetValue(0)
                        row("NAME") = readerDropList.GetString(1)
                        tabTbl(i).Rows.Add(row)
                    Next
                End While

                Dim cmb As ComboBox
                For i = 0 To 8
                    tabTbl(i).AcceptChanges()
                    cmb = CType(Controls("cmbTag" & i + 1), ComboBox)
                    cmb.DataSource = tabTbl(i)
                    cmb.DisplayMember = "NAME"
                    cmb.ValueMember = "ID"
                Next


                '＊＊＊＊属性生成＊＊＊＊＊
                ClearCheckBox(Me)
                clsOraAccess.queryZokuseiList(10, readerDropList)

                Dim ii As Integer = 1
                Dim chk As CheckBox
                While (readerDropList.Read())
                    chk = CType(Controls("chkZoku" & ii), CheckBox)
                    chk.Text = readerDropList.GetValue(0)
                    ii = ii + 1
                End While


            Catch ex As Exception
                Console.WriteLine(ex.Message)
            Finally
                readerDropList.Close()
            End Try
        End If

        Dim txt(8) As TextBox

        Dim readerTagList As OracleDataReader
        Dim autoCompList As New AutoCompleteStringCollection

        clsOraAccess.queryTagList("", readerTagList)

        For i = 0 To 8

            txt(i) = CType(Controls("txtTag" & i + 1), TextBox)

            txt(i).AutoCompleteMode = Windows.Forms.AutoCompleteMode.SuggestAppend           ' サジェストモード
            txt(i).AutoCompleteSource = Windows.Forms.AutoCompleteSource.CustomSource    ' カスタムソースに指定


            While (readerTagList.Read())
                autoCompList.Add(readerTagList.GetString(0))
            End While

            txt(i).AutoCompleteCustomSource = autoCompList



        Next
        readerTagList.Close()
    End Sub

    ''' <summary>
    ''' 初期化（既存ファイルなし時）
    ''' </summary>
    ''' <returns>フォルダ名</returns>
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
        Dim clsFormUtil As New FormUtil(Me)


        Console.WriteLine("初期化（既存ファイル無し時)")

        '初期化
        clsFormUtil.DataClear()

        Try
            'コンストラクタでファイルパスを指定
            clsZipOpen = New ZipOpen(sFilePath)
            clsZipOpen.tagCreate(clsZipOpen.FileName)

            'ファイル名取得し、フォームに値を設定
            sFileName = clsZipOpen.FileName
            Me.txtFileName.Text = sFileName

            Console.WriteLine("ファイル名" & txtFileName.Text)


            '絞込み条件をタグから取得
            sConditionValue = Nothing


            'タグ取得し、フォームに値を設定
            listTag = clsZipOpen.Tag
            Dim ilistIdx As Integer = 1
            Dim sBufTag As String = Nothing


            For Each sValue In listTag
                Controls("txtTag" & ilistIdx).Text = sValue


                Dim iSelValue As Integer
                iSelValue = clsOraAccess.queryDefaultCategory(sValue)

                Select Case iSelValue
                    Case 1
                        sConditionValue = sValue
                    Case 2
                        If sConditionValue Is Nothing Then
                            sConditionValue = sValue
                        End If
                End Select

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

            RankImageChange(iRank)

            'ファイル名をテキストボックスに設定
            txtTagSetting.Text = clsZipOpen.FileMei

            clsFormUtil.BtnAddMode()

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


    ''' <summary>
    ''' 初期化（既存ファイルあり時）
    ''' </summary>
    ''' <param name="readerFilePath">ファイルパステーブルのレコード</param>
    ''' <returns></returns>
    Private Function FormInit(ByRef readerFilePath As OracleDataReader) As String

        Console.WriteLine("初期化（既存ファイルあり時）")

        Dim clsFormUtil As New FormUtil(Me)

        'タグやチェックをクリア
        clsFormUtil.DataClear()

        '更新モードでボタン設定
        clsFormUtil.BtnUpdateMode()

        'サムネイルを設定
        clsFormUtil.ImageSetFile(sFilePath)


        'カレントファイル、フォルダを変数に格納
        While (readerFilePath.Read())
            iNowFile = readerFilePath.GetValue(0)
            iNowFolder = readerFilePath.GetValue(6)


            'パス、タイトル、ファイルサイズ設定
            Me.lblFilePath.Text = sFilePath
            Me.txtFileName.Text = readerFilePath.GetString(1)
            Me.lblFileSize.Text = ChangeFileSize(readerFilePath.GetValue(3))
            Me.lblNowFolder.Text = readerFilePath.GetString(7)

            'ランクを設定
            RankImageChange(readerFilePath.GetValue(5))

            clsFormUtil.TagCreate(iBookMarkId, 3)
            Exit While
        End While

        'txtFileName.Focus()
        TabControl1.Focus()

        Return readerFilePath.GetString(8)

    End Function

    ''' <summary>
    ''' 初期化(イメージリストクリック時）
    ''' </summary>
    ''' <param name="sFilePath">ファイルパス</param>
    ''' <returns>0固定</returns>
    Private Function FormInit(ByVal sFilePath As String) As String

        Dim clsFormUtil As New FormUtil(Me)

        'タグやチェックをクリア
        clsFormUtil.DataClear()

        'カレントファイル、フォルダを変数に格納
        iNowFile = CInt(lstImgPath(0)(lstThumbs.SelectedItems(0).Index))

        'パス、タイトル、ファイルサイズ設定
        Me.lblFilePath.Text = sFilePath
        Me.txtFileName.Text = lstThumbs.SelectedItems(0).Text
        Me.lblFileSize.Text = CType(ChangeFileSize(lstImgPath(3)(lstThumbs.SelectedItems(0).Index)), String)

        '更新モードでボタン設定
        clsFormUtil.BtnUpdateMode()

        'ランクを設定
        RankImageChange(CInt(lstImgPath(2)(lstThumbs.SelectedItems(0).Index)))

        'サムネイルを指定
        clsFormUtil.ImageSetFile(sFilePath)

        'タグを設定
        clsFormUtil.TagCreate(iNowFile, 1)


        BookMarkListCreate(iNowFile)


        clsFormUtil.BtnUpdateMode()
        TabControl1.Focus()

        Return 0

    End Function

    ''' <summary>
    ''' 初期化（既存ファイルなし時）
    ''' </summary>
    ''' <returns>フォルダ名</returns>
    Private Function BMFormInit() As String
        '＊＊＊フォーム生成＊＊＊
        'ファイルパスで指定されたZIP書庫を展開し、
        'フォルダ名、サムネイル、タグを取得


        Dim clsZipOpen As ZipOpen
        Dim sFileName As String
        Dim listTag As ArrayList
        Dim sConditionValue As String = Nothing
        Dim thumbnail As Image
        Dim fi As System.IO.FileInfo
        Dim clsFormUtil As New FormUtil(Me)


        Console.WriteLine("初期化（既存ファイル無し時)")

        '初期化
        clsFormUtil.DataClear()

        Try
            'コンストラクタでファイルパスを指定
            clsZipOpen = New ZipOpen()
            clsZipOpen.zipWorks(sBookMarkPath)
            clsZipOpen.tagCreate(clsZipOpen.BookMarkName)

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
            Me.lblFilePath.Text = sBookMarkPath

            fi = New System.IO.FileInfo(sFilePath)
            'ファイルのサイズを取得
            Me.lblFileSize.Text = ChangeFileSize(fi.Length)

            RankImageChange(iRank)

            'ファイル名をテキストボックスに設定
            txtTagSetting.Text = clsZipOpen.BookMarkName

            sBookMarkFileName = clsZipOpen.BookMarkName

            clsFormUtil.BtnBookMarkAddMode()

        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            fi = Nothing
            thumbnail = Nothing
            clsZipOpen = Nothing
        End Try


        'txtFileName.Focus()
        TabControl1.Focus()

        Return sConditionValue

    End Function

    ''' <summary>
    ''' 初期化(イメージリストクリック時）
    ''' </summary>
    ''' <param name="sFilePath">ファイルパス</param>
    ''' <returns>0固定</returns>
    Private Function BMFormInit(ByVal sFilePath As String) As String

        Dim clsFormUtil As New FormUtil(Me)

        'タグやチェックをクリア
        clsFormUtil.DataClear()


        'ブックマークIDを設定
        iBookMarkId = CInt(lstBookMarkPath(5)(lstBookMark.SelectedItems(0).Index))

        'ファイルIDを設定
        iNowFile = CInt(lstBookMarkPath(0)(lstBookMark.SelectedItems(0).Index))

        'ブックマーク名を設定
        sBookMarkFileName = lstBookMarkPath(6)(lstBookMark.SelectedItems(0).Index)

        'パス、タイトル、ファイルサイズ設定
        Me.lblFilePath.Text = sFilePath
        Me.txtFileName.Text = lstBookMark.SelectedItems(0).Text
        Me.lblFileSize.Text = CType(ChangeFileSize(lstBookMarkPath(3)(lstBookMark.SelectedItems(0).Index)), String)

        '更新モードでボタン設定
        clsFormUtil.BtnBookMarkUpdateMode()

        'ランクを設定
        RankImageChange(CInt(lstBookMarkPath(2)(lstBookMark.SelectedItems(0).Index)))

        'サムネイルを指定
        clsFormUtil.ImageSetBookMark(sFilePath)

        'タグを設定
        clsFormUtil.TagCreate(iBookMarkId, 3)


        Return 0

    End Function

    ''' <summary>
    ''' 初期化（既存ファイルあり時）
    ''' </summary>
    ''' <param name="readerFilePath">ファイルパステーブルのレコード</param>
    ''' <returns></returns>
    Private Function BMFormInit(ByRef readerMBFilePath As OracleDataReader) As String

        Console.WriteLine("初期化（既存ファイルあり時）")

        Dim clsFormUtil As New FormUtil(Me)

        'タグやチェックをクリア
        clsFormUtil.DataClear()

        '更新モードでボタン設定
        clsFormUtil.BtnBookMarkAddMode()

        While (readerMBFilePath.Read())
            'カレントファイル、フォルダを変数に格納
            iNowFile = readerMBFilePath.GetValue(0)
            iNowFolder = readerMBFilePath.GetValue(6)
            iBookMarkId = readerMBFilePath.GetValue(7)

            'パス、タイトル、ファイルサイズ設定
            Me.lblFilePath.Text = sBookMarkPath
            Me.txtFileName.Text = readerMBFilePath.GetString(1)
            Me.lblFileSize.Text = ChangeFileSize(readerMBFilePath.GetValue(3))
            Me.lblNowFolder.Text = readerMBFilePath.GetValue(7)

            'ランクを設定
            RankImageChange(readerMBFilePath.GetValue(5))

            'サムネイルを設定
            clsFormUtil.ImageSetBookMark(sBookMarkPath)

            'タグ設定
            clsFormUtil.TagCreate(iBookMarkId, 3)
        End While

        clsFormUtil.BtnBookMarkUpdateMode()
        TabControl1.Focus()

        Return 0

    End Function




    ''' <summary>
    ''' ツリービュー選択
    ''' カレントフォルダを変更
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
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


    ''' <summary>
    ''' イメージリスト作成
    ''' </summary>
    Private Sub ImageListCreate()

        Dim clsFormUtil As New FormUtil(Me)
        clsFormUtil.AddThumnailClear()

        TabControl1.SelectedTab = TabPage2

        lstThumbs.Clear()
        ilstThumbs.Images.Clear()

        Dim readerFileList As OracleDataReader = Nothing

        Try
            clsOraAccess.queryFileList(iNowFolder, readerFileList)

            clsFormUtil.lstImageListInit(lstImgPath)
            clsFormUtil.lstImageListSet(lstImgPath, readerFileList, ilstThumbs, lstThumbs)

        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            readerFileList.Close()
        End Try

    End Sub

    ''' <summary>
    ''' イメージリスト作成    ''' 
    ''' </summary>
    ''' <param name="iKensakuKbn">
    ''' 検索区分
    ''' 0：最終更新日順
    ''' 1：フォルダ検索（ランク以下）
    ''' </param>
    ''' <param name="iRank">ランク</param>
    'イメージリスト作成
    Private Sub ImageListCreate(ByVal iKensakuKbn As Integer, ByVal iRank As Integer)

        Dim readerFileList As OracleDataReader = Nothing
        Dim clsFormUtil As New FormUtil(Me)
        Dim lstZokusei = New List(Of String)

        clsFormUtil.AddThumnailClear()
        lstThumbs.Clear()
        ilstThumbs.Images.Clear()

        TabControl1.SelectedTab = TabPage2



        '属性チェックを配列に格納
        clsFormUtil.SetZokusei(lstZokusei)

        Try
            clsOraAccess.queryFileListKensaku(iKensakuKbn, lstZokusei, iRank, iNowFolder, readerFileList)

            clsFormUtil.lstImageListInit(lstImgPath)
            clsFormUtil.lstImageListSet(lstImgPath, readerFileList, ilstThumbs, lstThumbs)
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            readerFileList.Close()
        End Try

    End Sub


    ''' <summary>
    ''' イメージリスト作成
    ''' </summary>
    ''' <param name="iRank">ランク</param>
    ''' <param name="sTagName">タグ名</param>
    Private Sub ImageListCreate(ByVal iRank As Integer, ByVal sTagName As String)

        Dim clsFormUtil As New FormUtil(Me)
        Dim readerFileList As OracleDataReader = Nothing
        Dim lstZokusei = New List(Of String)

        TabControl1.SelectedTab = TabPage2

        clsFormUtil.AddThumnailClear()
        lstThumbs.Clear()
        ilstThumbs.Images.Clear()


        '属性チェックを配列に格納
        clsFormUtil.SetZokusei(lstZokusei)


        If (sTagName = "" And lstZokusei.Count = 0) Then
            MsgBox("からっぽ")
            Exit Sub
        End If

        Try
            clsOraAccess.queryFileListKensaku(lstZokusei, iRank, sTagName, readerFileList)

            clsFormUtil.lstImageListInit(lstImgPath)
            clsFormUtil.lstImageListSet(lstImgPath, readerFileList, ilstThumbs, lstThumbs)

        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            readerFileList.Close()
        End Try

    End Sub

    ''' <summary>
    ''' イメージリスト作成    ''' 
    ''' </summary>
    ''' <param name="iKensakuKbn">
    ''' 検索区分
    ''' 0：最終更新日順
    ''' 1：フォルダ検索（ランク以下）
    ''' </param>
    ''' <param name="iRank">ランク</param>
    'イメージリスト作成
    Private Sub BookMarkListCreate(ByVal iFileID As Integer)

        Dim clsFormUtil As New FormUtil(Me)
        Dim readerFileList As OracleDataReader = Nothing

        clsFormUtil.AddThumnailClear()
        lstBookMark.Clear()
        ilstBMThumbs.Images.Clear()


        Try
            clsOraAccess.queryBookMarkListKensaku(iFileID, readerFileList)

            clsFormUtil.lstBookMarkPathInit(lstBookMarkPath)
            clsFormUtil.lstBookMarkPathSet(lstBookMarkPath, readerFileList, ilstBMThumbs, lstBookMark)


            'ブックマークデータが無ければ、リストを非表示
            If readerFileList.HasRows = True Then
                clsFormUtil.ShowBookMark(True)
            Else
                clsFormUtil.ShowBookMark(False)
            End If
            'End While
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            readerFileList.Close()
        End Try

    End Sub


    ''' <summary>
    ''' ツリービュー作成
    ''' </summary>
    ''' <param name="sConditionValue">フォルダ名</param>
    Public Sub TreeCreate(ByVal sConditionValue As String)

        'Readerを宣言
        Dim readerFolder As OracleDataReader = Nothing
        Dim readerGenre As OracleDataReader = Nothing
        Dim readerSubFolder As OracleDataReader = Nothing

        Console.WriteLine("TreeCreate開始")

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




    '移動ボタンクリック
    'ファイルを移動し、ファイルTBL,ファイルタグTBLに新規登録
    Private Sub btnMove_Click(sender As System.Object, e As System.EventArgs) Handles btnMove.Click

        Select Case iNowFolder
            Case 0
                MsgBox("親フォルダが選択されていません。")
                Exit Sub
            Case 1 To 9
                MsgBox("ジャンルは指定できません。")
                Exit Sub
        End Select

        'ファイルID
        Dim iFileId As Integer = Nothing

        'ファイルサイズを取得
        Dim fi As System.IO.FileInfo = New System.IO.FileInfo(sFilePath)
        Dim lsize As Long = fi.Length
        fi = Nothing

        Dim clsDBLogic As New DBLogic

        Try


            'ファイルの存在チェック
            Dim sAftPath As String = clsOraAccess.queryFolderPath(iNowFolder)

            If System.IO.File.Exists(sAftPath & txtFileName.Text & ".zip") Then
                MsgBox("移動先フォルダに同一ファイルが存在しています。")
                Exit Sub
            ElseIf Not System.IO.Directory.Exists(sAftPath) Then
                MsgBox("移動先フォルダが存在しません。")
                Exit Sub
            End If

            'ファイルを移動
            System.IO.File.Move(sFilePath, sAftPath & txtFileName.Text & ".zip")

            'ファイルをTBLに追加
            iFileId = clsOraAccess.insertFile(txtFileName.Text, txtFileName.Text & ".zip", iNowFolder, lsize, iRank, picThumbs.Image)


            '属性をファイルタグTBLに追加
            Dim chk As CheckBox
            For i = 1 To 10
                chk = CType(Controls("chkZoku" & i), CheckBox)
                If (chk.Checked = True) Then
                    clsDBLogic.insertFileTag(clsOraAccess, 5, chk.Text, iFileId, 1)
                End If
            Next

            'タグをファイルタグTBLに追加
            For i = 1 To 9
                If (Controls("txtTag" & i).Text <> "") Then

                    Dim cmb As ComboBox
                    cmb = Controls("cmbTag" & i)
                    If (cmb.SelectedValue > 0) Then
                        'ファイルタグをTBLに追加
                        clsDBLogic.insertFileTag(clsOraAccess, cmb.SelectedValue, Controls("txtTag" & i).Text, iFileId, 1)
                    End If
                End If
            Next

            'Process.Start("C:\ProgramData\NeeView\NeeView.exe", """" & sAftPath & txtFileName.Text & ".zip""")

            If (TabControl1.SelectedTab Is TabPage2) Then
                ImageListCreate()
            End If

            With Me.NotifyIcon1
                .Icon = SystemIcons.Application
                .Visible = True
                .BalloonTipTitle = "FileMove"
                .BalloonTipText = "ファイルの追加が完了しました。"
                .BalloonTipIcon = ToolTipIcon.Info
                .ShowBalloonTip(3000)
            End With

        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
        End Try

    End Sub

    'テキストボックス押下時のイベント
    '右ダブルクリックを取得
    Private Sub txtTag_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles txtTag1.MouseDown, txtTag2.MouseDown, txtTag3.MouseDown, txtTag4.MouseDown, txtTag5.MouseDown, txtTag6.MouseDown, txtTag7.MouseDown, txtTag8.MouseDown, txtTag9.MouseDown

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
            ElseIf sender Is txtTag7 Then
                sChildFolderName = txtTag7.Text
            ElseIf sender Is txtTag8 Then
                sChildFolderName = txtTag8.Text
            ElseIf sender Is txtTag9 Then
                sChildFolderName = txtTag9.Text
            End If


            'iRankの妥当性
            ImageListCreate(iRank, sChildFolderName)
            TabControl1.SelectedTab = TabPage2
        End If
    End Sub

    'テキストボックスをダブルクリック
    '入力値でフォルダTBLを検索し、ツリービューを変更
    Private Sub txtTag_DoubleClick(sender As Object, e As System.EventArgs) Handles txtTag1.DoubleClick, txtTag2.DoubleClick, txtTag3.DoubleClick, txtTag4.DoubleClick, txtTag5.DoubleClick, txtTag6.DoubleClick, txtTag7.DoubleClick, txtTag8.DoubleClick, txtTag9.DoubleClick

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
        ElseIf sender Is txtTag7 Then
            sChildFolderName = txtTag7.Text
        ElseIf sender Is txtTag8 Then
            sChildFolderName = txtTag8.Text
        ElseIf sender Is txtTag9 Then
            sChildFolderName = txtTag9.Text
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
        Dim iCategory As Integer

        For i = 1 To 6
            Dim cmb As ComboBox
            cmb = CType(Controls("cmbTag" & i), ComboBox)

            Select Case iNowFolder
                Case 1, 2, 3, 4
                    If (cmb.SelectedValue = 1) Then
                        If (Controls("txtTag" & i).Text <> "") Then
                            sChildFolderName = Controls("txtTag" & i).Text
                            iCategory = cmb.SelectedValue
                            Exit For
                        End If
                    End If

                Case 5
                    If (cmb.SelectedValue = 2) Then
                        If (Controls("txtTag" & i).Text <> "") Then
                            sChildFolderName = Controls("txtTag" & i).Text
                            iCategory = cmb.SelectedValue
                            Exit For
                        End If
                    End If
                Case 8
                    If (cmb.SelectedValue = 2) Then
                        If (Controls("txtTag" & i).Text <> "") Then
                            sChildFolderName = Controls("txtTag" & i).Text
                            iCategory = cmb.SelectedValue
                            Exit For
                        End If
                    ElseIf (cmb.SelectedValue = 0) Then
                        If (Controls("txtTag" & i).Text <> "") Then
                            If (MsgBox(Controls("txtTag" & i).Text & "をフォルダ名にしますか？", vbYesNo) = vbYes) Then
                                cmb.SelectedValue = 2
                                sChildFolderName = Controls("txtTag" & i).Text
                                iCategory = cmb.SelectedValue
                                Exit For
                            End If
                        End If
                    End If
                Case Else
                    If (cmb.SelectedValue = 1 Or cmb.SelectedValue = 2) Then
                        If (Controls("txtTag" & i).Text <> "") Then
                            sChildFolderName = Controls("txtTag" & i).Text
                            iCategory = cmb.SelectedValue
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
            Dim clsDBLogic As New DBLogic

            'フォルダの追加
            iNowFolder = clsOraAccess.insertFolder(iNowFolder, sChildFolderName)
            'ファイルタグの追加
            clsDBLogic.insertFileTag(clsOraAccess, iCategory, sChildFolderName, iNowFolder, 2)


            System.IO.Directory.CreateDirectory(sParentFolderPath & sChildFolderName)

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
                With Me.NotifyIcon1
                    .Icon = SystemIcons.Application
                    .Visible = True
                    .BalloonTipTitle = "FileMove"
                    .BalloonTipText = "削除が完了しました。"
                    .BalloonTipIcon = ToolTipIcon.Info
                    .ShowBalloonTip(3000)
                End With
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        End If
    End Sub

    'サムネイルダブルクリック
    'Hamana起動
    Private Sub picThumbs_Click(sender As Object, e As System.EventArgs) Handles picThumbs.Click

        Process.Start("C:\ProgramData\NeeView\NeeView.exe", """" & sFilePath & """")
        'Process.Start("C:\ProgramData\Hamana\Hamana.exe", """" & sFilePath & """")

    End Sub

    'イメージリストダブルクリック
    '初期化（ファイルあり）を呼び出し、フォームに情報を表示
    Private Sub lstThumbs_Click(sender As Object, e As System.EventArgs) Handles lstThumbs.Click

        sFilePath = CType(lstImgPath(1)(lstThumbs.SelectedItems(0).Index), String)

        If (bKensakuPattern = True) Then
            iNowFolder = lstImgPath(4)(lstThumbs.SelectedItems(0).Index)
            lblNowFolder.Text = "同一"
        End If

        FormInit(sFilePath)
    End Sub

    ''' <summary>
    ''' ブックマークのイメージリストクリック
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub lstBookMark_Click(sender As Object, e As EventArgs) Handles lstBookMark.Click
        sFilePath = CType(lstBookMarkPath(1)(lstBookMark.SelectedItems(0).Index), String)

        If (bKensakuPattern = True) Then
            'いったんコメント
            'iNowFolder = lstBookMarkPath(4)(lstBookMark.SelectedItems(0).Index)
            lblNowFolder.Text = "同一"
        End If

        BMFormInit(sFilePath)
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

            Dim clsDBLogic As New DBLogic

            'ファイルをTBLに追加
            clsOraAccess.updateFile(iNowFile, iNowFolder, txtFileName.Text, txtFileName.Text & ".zip", iRank, picThumbs.Image)

            'ファイルタグをTBLから削除
            clsDBLogic.delFileTag(clsOraAccess, iNowFile, 1)

            '属性をファイルタグTBLに追加
            Dim chk As CheckBox
            For i = 1 To 10
                chk = CType(Controls("chkZoku" & i), CheckBox)
                If (chk.Checked = True) Then
                    clsDBLogic.insertFileTag(clsOraAccess, 5, chk.Text, iNowFile, 1)
                End If
            Next

            'プルダウンの選択値を取得し、ファイルタグをTBLに追加
            For i = 1 To 9
                If (Controls("txtTag" & i).Text <> "") Then

                    Dim cmb As ComboBox
                    cmb = CType(Controls("cmbTag" & i), ComboBox)

                    If (cmb.SelectedValue > 0) Then
                        'ファイルタグをTBLに追加　
                        clsDBLogic.insertFileTag(clsOraAccess, cmb.SelectedValue, Controls("txtTag" & i).Text, iNowFile, 1)
                    End If
                End If
            Next
            If (TabControl1.SelectedTab Is TabPage2 And
                bKensakuPattern = False) Then
                ImageListCreate()
            End If

            With Me.NotifyIcon1
                .Icon = SystemIcons.Application
                .Visible = True
                .BalloonTipTitle = "FileMove"
                .BalloonTipText = "ファイルの更新が完了しました。"
                .BalloonTipIcon = ToolTipIcon.Info
                .ShowBalloonTip(3000)
            End With

            'MessageBox.Show("ファイルの更新が完了しました。")

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    '削除ボタンクリック
    'DBの情報を削除と同時に、ファイルの情報も削除する
    Private Sub btnNowDel_Click(sender As System.Object, e As System.EventArgs) Handles btnNowDel.Click

        If (MsgBox("削除しますか？" & vbCrLf & vbCrLf & sFilePath, vbYesNo) = vbYes) Then

            Dim clsDBLogic As New DBLogic

            Try
                clsDBLogic.delFileTag(clsOraAccess, iNowFile, 1)
                clsOraAccess.deleteFile(iNowFile)

                My.Computer.FileSystem.DeleteFile(sFilePath, FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.SendToRecycleBin)

                If (TabControl1.SelectedTab Is TabPage2) Then
                    ImageListCreate()
                End If

                MessageBox.Show("削除が完了しました。")
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        End If

    End Sub




    Private Sub btnBookMarkAdd_Click(sender As Object, e As EventArgs) Handles btnBookMarkAdd.Click

        'ファイルサイズを取得
        Dim fi As System.IO.FileInfo = New System.IO.FileInfo(sFilePath)
        Dim lsize As Long = fi.Length
        fi = Nothing

        Dim clsDBLogic As New DBLogic

        Try
            'ファイルの存在チェック
            Dim sAftPath As String = clsOraAccess.queryFolderPath(iNowFolder)

            'ブックマークをTBLに追加
            iBookMarkId = clsOraAccess.insertBookMark(txtFileName.Text, sBookMarkFileName, iNowFile, lsize, iRank, picThumbs.Image)

            '属性をファイルタグTBLに追加
            Dim chk As CheckBox
            For i = 1 To 10
                chk = CType(Controls("chkZoku" & i), CheckBox)
                If (chk.Checked = True) Then
                    clsDBLogic.insertFileTag(clsOraAccess, 5, chk.Text, iBookMarkId, 3)
                End If
            Next

            'タグをファイルタグTBLに追加
            For i = 1 To 9
                If (Controls("txtTag" & i).Text <> "") Then

                    Dim cmb As ComboBox
                    cmb = CType(Controls("cmbTag" & i), ComboBox)
                    If (cmb.SelectedValue > 0) Then
                        'ファイルタグをTBLに追加
                        clsDBLogic.insertFileTag(clsOraAccess, cmb.SelectedValue, Controls("txtTag" & i).Text, iBookMarkId, 3)
                    End If
                End If
            Next

            'Process.Start("C:\ProgramData\NeeView\NeeView.exe", """" & sAftPath & txtFileName.Text & ".zip""")

            BookMarkListCreate(iNowFile)


            With Me.NotifyIcon1
                .Icon = SystemIcons.Application
                .Visible = True
                .BalloonTipTitle = "FileMove"
                .BalloonTipText = "ブックマークの追加が完了しました。"
                .BalloonTipIcon = ToolTipIcon.Info
                .ShowBalloonTip(3000)
            End With

        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
        End Try
    End Sub

    ''' <summary>
    ''' ブクマ更新ボタン押下時の処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnBookMarkUpdate_Click(sender As Object, e As EventArgs) Handles btnBookMarkUpdate.Click

        Try
            Dim clsDBLogic As New DBLogic

            'ファイルをTBLに追加
            clsOraAccess.updateBookMark(iBookMarkId, iNowFile, txtFileName.Text, sBookMarkFileName, iRank, picThumbs.Image)

            'ファイルタグをTBLから削除
            clsDBLogic.delFileTag(clsOraAccess, iBookMarkId, 3)

            '属性をファイルタグTBLに追加
            Dim chk As CheckBox
            For i = 1 To 10
                chk = CType(Controls("chkZoku" & i), CheckBox)
                If (chk.Checked = True) Then
                    clsDBLogic.insertFileTag(clsOraAccess, 5, chk.Text, iBookMarkId, 3)
                End If
            Next

            'プルダウンの選択値を取得し、ファイルタグをTBLに追加
            For i = 1 To 9
                If (Controls("txtTag" & i).Text <> "") Then

                    Dim cmb As ComboBox
                    cmb = CType(Controls("cmbTag" & i), ComboBox)

                    If (cmb.SelectedValue > 0) Then
                        'ファイルタグをTBLに追加　
                        clsDBLogic.insertFileTag(clsOraAccess, cmb.SelectedValue, Controls("txtTag" & i).Text, iBookMarkId, 3)
                    End If
                End If
            Next
            If (TabControl1.SelectedTab Is TabPage2 And
                bKensakuPattern = False) Then
                ImageListCreate()
            End If

            With Me.NotifyIcon1
                .Icon = SystemIcons.Application
                .Visible = True
                .BalloonTipTitle = "FileMove"
                .BalloonTipText = "ブックマークの更新が完了しました。"
                .BalloonTipIcon = ToolTipIcon.Info
                .ShowBalloonTip(3000)
            End With

            'MessageBox.Show("ファイルの更新が完了しました。")

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' ブクマ削除ボタン押下時
    ''' ファイルタグテーブル、ファイルタグ紐づけテーブル、ファイルタグテーブルのレコードを削除
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>

    Private Sub btnBookMarkDel_Click(sender As Object, e As EventArgs) Handles btnBookMarkDel.Click

        If (MsgBox("削除しますか？" & vbCrLf & vbCrLf & sFilePath, vbYesNo) = vbYes) Then

            Dim clsDBLogic As New DBLogic

            Try
                clsDBLogic.delFileTag(clsOraAccess, iBookMarkId, 3)
                clsOraAccess.deleteBookMark(iBookMarkId)

                BookMarkListCreate(iNowFile)

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


        ImageListCreate(1, iRank)
    End Sub

    '更新日順ボタンクリック
    Private Sub btnAddDaySearch_Click(sender As System.Object, e As System.EventArgs) Handles btnAddDaySearch.Click

        '検索フラグをオン
        bKensakuPattern = True

        ImageListCreate(0, iRank)
    End Sub

    'タグ追加 ボタンクリック
    Private Sub btnFolderTagUpdate_Click(sender As System.Object, e As System.EventArgs) Handles btnFolderTagAdd.Click

        If (iNowFolder < 1) Then
            MsgBox("フォルダが選択されていません")
            Exit Sub
        End If

        Dim clsDBLogic As New DBLogic

        'プルダウンの選択値を取得し、ファイルタグをTBLに追加
        Try
            For i = 1 To 6
                If (Controls("txtTag" & i).Text <> "") Then

                    Dim cmb As ComboBox
                    cmb = CType(Controls("cmbTag" & i), ComboBox)

                    If cmb.SelectedValue > 0 Then
                        'フォルダタグをTBLに追加
                        clsDBLogic.insertFileTag(clsOraAccess, cmb.SelectedValue, Controls("txtTag" & i).Text, iNowFolder, 2)
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
        Dim clsDBLogic As New DBLogic

        Try
            'フォルダパスを取得
            sFolderPath = clsOraAccess.queryFolderPath(iNowFolder)

            If (sFolderPath Is Nothing) Then
                MsgBox("フォルダパスを取得できませんでした")
                Exit Sub
            End If

            'DBからフォルダ、フォルダタグを削除
            clsDBLogic.delFileTag(clsOraAccess, iNowFolder, 2)


            clsOraAccess.deleteFolder(iNowFolder)

            'フォルダを削除
            My.Computer.FileSystem.DeleteDirectory(sFolderPath, FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.SendToRecycleBin)
            MessageBox.Show("削除が完了しました。")
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub


    'テンキー押下時にランクのプルダウンを変える

    Private Sub FileMoveForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'フォームでキーボードイベントを認識するにはKeyPreview = True にする。
        Me.KeyPreview = True

    End Sub


    Private Sub RankImageChange(ByVal iIdx As Integer)


        picRank1.Image = imgWhite
        picRank2.Image = imgWhite
        picRank3.Image = imgWhite
        picRank4.Image = imgWhite
        picRank5.Image = imgWhite

        iRank = iIdx

        If (iIdx = 0) Then
            Exit Sub
        End If

        Dim pic As PictureBox
        For i = 1 To iIdx
            pic = CType(Controls("picRank" & i), PictureBox)
            pic.Image = imgBlack
        Next

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
        befRank = iRank

        '初期化
        ClearTextBox2(Me)
        ClearCombotBox(Me)
        ClearCheckBox(Me)

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
        RankImageChange(befRank)

        '＊＊＊ツリービュー作成処理＊＊＊
        TreeCreate(sConditionValue)

        'txtFileName.Focus()
        TabControl1.Focus()

    End Sub

    Private Sub ListBox1_DragEnter(ByVal sender As Object,
        ByVal e As System.Windows.Forms.DragEventArgs) _
        Handles ListBox1.DragEnter
        'コントロール内にドラッグされたとき実行される
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            'ドラッグされたデータ形式を調べ、ファイルのときはコピーとする
            e.Effect = DragDropEffects.Copy
        Else
            'ファイル以外は受け付けない
            e.Effect = DragDropEffects.None
        End If
    End Sub

    Private Sub ListBox1_DragDrop(ByVal sender As Object,
        ByVal e As System.Windows.Forms.DragEventArgs) _
        Handles ListBox1.DragDrop
        'コントロール内にドロップされたとき実行される
        'ドロップされたすべてのファイル名を取得する
        Dim fileName As String() = CType(
        e.Data.GetData(DataFormats.FileDrop, False),
        String())
        'ListBoxに追加する
        ListBox1.Items.AddRange(fileName)

        'クリップボードにBitmapデータがあるか調べる（調べなくても良い）
        If Clipboard.ContainsImage() Then
            'クリップボードにあるデータの取得
            Dim img As Image = Clipboard.GetImage()
            If img IsNot Nothing Then
                'データが取得できたときは表示する
                PictureBox1.Image = img
            End If
        End If

    End Sub

    Private Sub chkZoku_CheckedChanged(sender As Object, e As EventArgs) Handles chkZoku1.CheckedChanged, chkZoku2.CheckedChanged, chkZoku3.CheckedChanged, chkZoku4.CheckedChanged, chkZoku5.CheckedChanged, chkZoku6.CheckedChanged, chkZoku7.CheckedChanged, chkZoku8.CheckedChanged, chkZoku9.CheckedChanged, chkZoku10.CheckedChanged

        Dim chk As CheckBox
        For i = 1 To 10
            chk = CType(Controls("chkZoku" & i), CheckBox)


            If sender Is chk Then
                If chk.Checked = True Then
                    chk.BackColor = Color.Yellow
                Else
                    chk.BackColor = Color.Transparent
                End If
            End If
        Next

    End Sub

    Private Sub picRank1_Click(sender As Object, e As EventArgs) Handles picRank0.MouseLeave, picRank1.MouseHover, picRank2.MouseHover, picRank3.MouseHover, picRank4.MouseHover, picRank5.MouseHover


        picRank1.Image = imgWhite
        picRank2.Image = imgWhite
        picRank3.Image = imgWhite
        picRank4.Image = imgWhite
        picRank5.Image = imgWhite

        Dim pic As PictureBox
        For i = 0 To 5
            pic = CType(Controls("picRank" & i), PictureBox)
            If i <> 0 Then
                pic.Image = imgBlack
            End If

            If (sender Is pic) Then
                iRank = i
                Exit For
            End If
        Next

    End Sub

    'コンボボックスの自動設定
    'タグのテキストボックスからフォーカスアウト時に、コンボボックス設定値を自動で取得する
    Private Sub txtTag_Leave(sender As Object, e As EventArgs) Handles txtTag1.Leave, txtTag2.Leave, txtTag3.Leave, txtTag4.Leave, txtTag5.Leave, txtTag6.Leave, txtTag7.Leave, txtTag8.Leave, txtTag9.Leave


        Dim txt As TextBox = CType(sender, TextBox)
        Dim iCategory As Integer

        For i = 1 To 9

            If txt.Name = "txtTag" & i Then
                If txt.Text <> "" Then

                    iCategory = clsOraAccess.queryDefaultCategory(txt.Text)

                    Dim cmb As ComboBox = CType(Controls("cmbTag" & i), ComboBox)
                    cmb.SelectedIndex = iCategory

                    Exit For
                End If
            End If
        Next
    End Sub

    'コンボボックスの自動設定
    'タグのテキストボックスからフォーカスアウト時に、コンボボックス設定値を自動で取得する
    'Private Sub txtTag_TextChanged(sender As Object, e As EventArgs) Handles txtTag1.TextChanged, txtTag2.TextChanged, txtTag3.TextChanged, txtTag4.TextChanged, txtTag5.TextChanged, txtTag6.TextChanged, txtTag7.TextChanged, txtTag8.TextChanged, txtTag9.TextChanged


    '    Dim txt As TextBox = CType(sender, TextBox)
    '    Dim readerTagList As OracleDataReader
    '    Dim autoCompList As New AutoCompleteStringCollection


    '    For i = 1 To 9

    '        If txt.Name = "txtTag" & i Then
    '            If txt.Text <> "" Then

    '                txt.AutoCompleteMode = Windows.Forms.AutoCompleteMode.Suggest           ' サジェストモード
    '                txt.AutoCompleteSource = Windows.Forms.AutoCompleteSource.CustomSource 　　　' カスタムソースに指定

    '                clsOraAccess.queryTagList(txt.Text, readerTagList)
    '                While (readerTagList.Read())
    '                    autoCompList.Add(readerTagList.GetString(0))
    '                End While

    '                txt.AutoCompleteCustomSource = autoCompList


    '            End If
    '        End If
    '    Next
    'End Sub

    'Private Sub ComboBox1_TextChanged(sender As Object, e As EventArgs)


    '    Dim txt As ComboBox = CType(sender, ComboBox)
    '    Dim readerTagList As OracleDataReader
    '    Dim autoCompList As New AutoCompleteStringCollection

    '    Dim tabTbl(1) As DataTable

    '    clsOraAccess.queryTagList(txt.Text, readerTagList)

    '    For i = 0 To 0
    '        tabTbl(i) = New DataTable

    '        'tabTbl(i).Columns.Add("ID", GetType(Integer))
    '        tabTbl(i).Columns.Add("NAME", GetType(String))
    '    Next

    '    While (readerTagList.Read())

    '        For i = 0 To 0
    '            '新規行作成
    '            Dim row As DataRow = tabTbl(i).NewRow()

    '            '各行に値をセット
    '            'row("ID") = readerTagList.GetValue(0)
    '            row("NAME") = readerTagList.GetString(0)
    '            tabTbl(i).Rows.Add(row)
    '        Next
    '    End While

    '    Dim cmb As ComboBox
    '    For i = 0 To 0
    '        tabTbl(i).AcceptChanges()
    '        cmb = CType(Controls("ComboBox" & i + 1), ComboBox)
    '        cmb.DataSource = tabTbl(i)
    '        cmb.DisplayMember = "NAME"
    '        'cmb.ValueMember = "ID"
    '    Next


    '    'For i = 1 To 1

    '    '    If txt.Name = "ComboBox" & i Then
    '    '        If txt.Text <> "" Then

    '    '            txt.AutoCompleteMode = Windows.Forms.AutoCompleteMode.Suggest           ' サジェストモード
    '    '            txt.AutoCompleteSource = Windows.Forms.AutoCompleteSource.CustomSource 　　　' カスタムソースに指定

    '    '            clsOraAccess.queryTagList(txt.Text, readerTagList)
    '    '            While (readerTagList.Read())
    '    '                autoCompList.Add(readerTagList.GetString(0))
    '    '            End While

    '    '            txt.AutoCompleteCustomSource = autoCompList


    '    '        End If
    '    '    End If
    '    'Next

    'End Sub
End Class



