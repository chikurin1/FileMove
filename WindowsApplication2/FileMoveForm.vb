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
    ''' コマンドライン引数
    ''' 二回目以降の起動時にMyApplication_StartupNextInstanceから取得する
    ''' </summary>
    Public sCommandLine As String

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
    ''' 検索フラグ
    ''' </summary>
    Private bKensakuPattern As Boolean

    ''' <summary>
    ''' ランキング用の☆マーク（白抜き）
    ''' </summary>
    Private imgWhite As Image = Image.FromFile("C:\ProgramData\NeeView\Plugin\星白.png")

    ''' <summary>
    ''' ''' ランキング用の☆マーク（黒埋め）
    ''' </summary>
    Private imgBlack As Image = Image.FromFile("C:\ProgramData\NeeView\Plugin\星黒.png")


    '☆暫定☆
    Private bBookMarkFlag As Boolean

    '************************************************************************************************************************************************************************************
    '************************************************************************************************************************************************************************************
    '************************************************************************************************************************************************************************************

    '                                                                           フォーム関連イベント

    '************************************************************************************************************************************************************************************
    '************************************************************************************************************************************************************************************
    '************************************************************************************************************************************************************************************

    Private Sub FileMoveForm_Click(sender As Object, e As System.EventArgs) Handles Me.Click, lstThumbs.Click, treeDir.Click, TabControl1.Click, tabTree.Click, tabImageList.Click
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


        Dim sFilePath As String

        Dim clsFormView As New FormView(Me)

        Dim clsTreeView As New FolderTreeView(Me)

        Console.WriteLine("Form1_Load開始")

        clsFormView.ShowBookMark(False)

        If (bFirstFlag = False) Then
            'コマンドライン引数から、ファイルパスを取得
            For Each sbuf In My.Application.CommandLineArgs
                sFilePath = sbuf
                Exit For
            Next
        Else
            sFilePath = sCommandLine
        End If

        sFilePath = System.Web.HttpUtility.UrlDecode(sFilePath)
        Console.WriteLine("引数は「" & sFilePath & "」")


        '初回起動時のみStartProcを呼び出し、タグのプルダウン、属性チェックを設定
        If bStartFlag = False Then
            StartProc()
            bStartFlag = True
        End If

        '変数初期化
        iRank = 1
        bBookMarkFlag = False

        'コマンドライン引数が未設定の場合、ツリービューを作成し処理終了。
        If sFilePath Is Nothing Then
            clsTreeView.TreeCreate("")
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
                clsTreeView.TreeCreate("")
                Exit Sub
            Case Else
                Exit Sub
        End Select


        Dim sTreeCond As String = Nothing

        Try
            'コマンドライン引数がブックマーク(jpg,png)の場合
            If sBookMarkPath IsNot Nothing Then

                Dim clsBookMarkBean As New BookMarkBean

                '存在チェック
                'ブックマークテーブルにデータがある場合、TBLからフォームデータを取得
                If clsBookMarkBean.FileExistCheck(sBookMarkPath) = True Then

                    'DBからフォームデータを取得しBeanにセット
                    clsBookMarkBean.getOraData(sBookMarkPath)

                    'Beanからフォーム生成
                    clsFormView.FormSet(clsBookMarkBean)

                    '更新モードでボタンを設定
                    clsFormView.BtnBookMarkUpdateMode()

                    'フォルダ名を検索条件に設定（Beanに設定してないのでnull）
                    sTreeCond = clsBookMarkBean.folder_name

                    'イメージリスト生成
                    Dim clsImageListBean As New ImageListBean
                    clsImageListBean.getOraData(clsBookMarkBean.folder_id, -1, Nothing)

                    Dim clsImageListView As New ImageListView(Me)
                    clsImageListView.ImageListCreate(clsImageListBean.imageListBeans)

                    'ブックマーク更新モードでボタン表示
                    clsFormView.BtnBookMarkUpdateMode()
                    'タブ１を選択
                    TabControl1.Focus()

                    '処理終了
                    Exit Sub
                End If

                'zipの場合
            End If

            Dim clsFormBean As New FormBean

            'zipファイルの存在チェック
            'ファイルTBLにzipファイルがある場合
            If clsFormBean.FileExistCheck(sFilePath) Then


                If sBookMarkPath IsNot Nothing Then
                    'ブックマーク追加モード

                    Console.WriteLine("ブックマーク追加モード：" & sBookMarkPath)

                    'zipファイルのファイルIDを取得
                    clsFormBean.getOraData(sFilePath)
                    iNowFile = clsFormBean.file_id
                    iNowFolder = clsFormBean.folder_id
                    iRank = clsFormBean.rank

                    'フォーム生成
                    '絞込み条件をタグから取得()
                    Dim clsBookMarkBean As New BookMarkBean
                    clsBookMarkBean.getZipData(sBookMarkPath)

                    clsFormView.FormSet(clsBookMarkBean)

                    sBookMarkFileName = clsBookMarkBean.bookmark_file_name

                    clsFormView.BtnBookMarkAddMode()

                    TabControl1.Focus()

                    'サムネイル作成
                    Dim clsImageListBean As New ImageListBean
                    clsImageListBean.getOraData(clsFormBean.folder_id, -1, Nothing)

                    Dim clsImageListView As New ImageListView(Me)
                    clsImageListView.ImageListCreate(clsImageListBean.imageListBeans)


                    Dim clsBookMarkListBean As New BookMarkBean
                    clsBookMarkListBean.getOraDataList(iNowFile)

                    'ブックマークリストを取得できた場合、ブックマークリストを表示
                    If clsBookMarkListBean.bookmark_imageListBeans.Count = 0 Then
                        clsFormView.ShowBookMark(False)
                    Else
                        Dim clsBookMarkListView As New BookMarkImateListView(Me)
                        clsBookMarkListView.BookMarkImageListCreate(clsBookMarkListBean.bookmark_imageListBeans)

                        clsFormView.ShowBookMark(True)
                    End If

                    'ブックマーク追加モードでボタン表示
                    clsFormView.BtnBookMarkAddMode()

                Else
                    Console.WriteLine("'zip更新モード：" & sFilePath)


                    'ファイル表示処理
                    'ファイルTBLから取得した情報をもとにフォーム、ツリービュー設定

                    'フォーム生成
                    clsFormBean.getOraDataImageChange(sFilePath)

                    'フォームにBeanの値を設定
                    clsFormView.FormSet(clsFormBean)

                    iNowFile = clsFormBean.file_id
                    iNowFolder = clsFormBean.folder_id
                    iRank = clsFormBean.rank

                    '更新モードでボタンを設定
                    clsFormView.BtnUpdateMode()

                    TabControl1.Focus()

                    'ブックマークリストを設定
                    Dim clsBookMarkBean As New BookMarkBean
                    clsBookMarkBean.getOraDataList(clsFormBean.file_id)

                    'ブックマークリストを取得できた場合、ブックマークリストを表示
                    If clsBookMarkBean.bookmark_imageListBeans.Count = 0 Then
                        clsFormView.ShowBookMark(False)
                    Else
                        Dim clsBookMarkImageListView As New BookMarkImateListView(Me)
                        clsBookMarkImageListView.BookMarkImageListCreate(clsBookMarkBean.bookmark_imageListBeans)

                        clsFormView.ShowBookMark(True)
                    End If

                    sTreeCond = clsFormBean.folder_name

                    'サムネイル作成
                    Dim clsImageListBean As New ImageListBean
                    clsImageListBean.getOraData(iNowFolder, -1, Nothing)

                    Dim clsImageListView As New ImageListView(Me)
                    clsImageListView.ImageListCreate(clsImageListBean.imageListBeans)

                End If

            Else
                'ファイルTBLにデータが無い場合、パラメータのファイル名からフォーム、ツリービュー設定


                'ブックマーク追加処理では確認MSGを表示
                If sBookMarkPath IsNot Nothing Then

                        MsgBox("ブックマーク追加前にファイルテーブルへの追加を行います")

                    End If

                    'フォーム生成
                    Console.WriteLine("初期化（既存ファイル無し時)")

                    Try
                        'Zipファイルからフォーム情報を取得しBeanにセット
                        clsFormBean.getZipData(sFilePath)
                        'フォームにBeanの値を設定
                        clsFormView.FormSet(clsFormBean)
                        '追加モードでボタンを設定
                        clsFormView.BtnAddMode()

                    Catch ex As Exception
                        MsgBox(ex.Message)
                    End Try


                    'googleチェックありの場合、ファイル名でgoogle検索
                    If (chkGoogle.Checked = True) Then
                        Dim sName As String = txtTagSetting.Text
                        If (sName <> "") Then
                            TabControl1.SelectedTab = tabGoogle
                        WebBrowser1.Navigate(New Uri("https://www.google.co.jp/search?q=" & Uri.EscapeUriString(sName.Replace("_", " ").Trim)))
                        'Process.Start("https://www.google.co.jp/search?q=" & Uri.EscapeUriString(sName.Trim))
                    End If
                    End If

                    'txtFileName.Focus()
                    TabControl1.Focus()

                    sTreeCond = clsFormView.TreeCondGet(clsFormBean.tagname_lst, clsFormBean.tagcat_lst)

                    If sTreeCond = Nothing Then
                        RankImageChange(1)
                    End If

                End If



            '＊＊＊ツリービュー作成処理＊＊＊
            clsTreeView.TreeCreate(sTreeCond)


            '初期処理終了、フォームを表示

        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            'readerFilePath.Close()
        End Try

    End Sub


    ''' <summary>
    ''' 初期起動時にのみ呼ばれる
    ''' タグのプルダウン設定、属性チェックの初期設定を行う
    ''' </summary>
    Private Sub StartProc()

        Dim readerDropList As OracleDataReader = Nothing
        Dim clsFormView As New FormView(Me)

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
        clsFormView.BtnAddMode()

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
                clsOraAccess.queryZokuseiList(15, readerDropList)

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


    'メモリ解放
    Protected Overrides Sub Finalize()
        MyBase.Finalize()
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

    Private Sub FileMoveForm_MouseDown(sender As Object, e As MouseEventArgs) Handles Me.MouseDown
        'マウスのダブルクリックイベント
        If e.Button = MouseButtons.Right AndAlso e.Clicks = 2 Then

            ClearCheckBox(Me)
            iRank = 2
            RankImageChange(iRank)

        End If
    End Sub

    '************************************************************************************************************************************************************************************
    '************************************************************************************************************************************************************************************
    '************************************************************************************************************************************************************************************

    '                                                                           ボタンクリックイベント

    '************************************************************************************************************************************************************************************
    '************************************************************************************************************************************************************************************
    '************************************************************************************************************************************************************************************

    '************************************************************************************************************************************************************************************
    '************************************************************************************************************************************************************************************

    '                                                                           ファイル更新ボタン

    '************************************************************************************************************************************************************************************
    '************************************************************************************************************************************************************************************

    '移動ボタンクリック
    'ファイルを移動し、ファイルTBL,ファイルタグTBLに新規登録
    Private Sub btnMove_Click(sender As System.Object, e As System.EventArgs) Handles btnMove.Click

        Dim sFilePath As String = lblFilePath.Text

        If (iNowFolder < 1) Then
            MsgBox("親フォルダが選択されていません。")
            Exit Sub
        End If

        Dim iMaxGenreID As Integer
        Dim clsOraAccess As New OraAccess
        iMaxGenreID = clsOraAccess.queryGenreMaxId()

        If (iNowFolder <= iMaxGenreID) Then
            MsgBox("ルートフォルダは指定できません")
            Exit Sub
        End If

        'ファイルID
        Dim iFileId As Integer = Nothing

        'ファイルサイズを取得
        Dim fi As System.IO.FileInfo = New System.IO.FileInfo(sFilePath)
        Dim lsize As Long = fi.Length
        fi = Nothing

        Dim clsDBLogic As New DBLogic

        Try
            'ファイルの存在チェック
            Dim sAftDir As String = clsOraAccess.queryFolderPath(iNowFolder)
            Dim sAftPath As String = sAftDir & txtFileName.Text & ".zip"

            If FileExistCheck(sAftPath) Then
                MsgBox("移動先フォルダに同一ファイルが存在しています。")
                Exit Sub
            ElseIf Not System.IO.Directory.Exists(sAftDir) Then
                MsgBox("移動先フォルダが存在しません。")
                Exit Sub
            End If

            'ファイルを移動
            System.IO.File.Move(sFilePath, sAftPath)

            'ファイルをTBLに追加
            Dim clsFormBean As New FormBean

            clsFormBean.folder_id = iNowFolder
            clsFormBean.title = txtFileName.Text
            clsFormBean.file_name = txtFileName.Text & ".zip"
            clsFormBean.rank = iRank
            clsFormBean.thumbnail = picThumbs.Image
            clsFormBean.fullpath = sAftPath
            clsFormBean.file_size = lsize

            '属性をBeanにセット
            '属性をファイルタグTBLに追加
            Dim chk As CheckBox
            For i = 1 To 10
                chk = CType(Controls("chkZoku" & i), CheckBox)
                If (chk.Checked = True) Then
                    clsFormBean.AddTag(chk.Text, 5)
                End If
            Next

            'プルダウンの選択値を取得し、ファイルタグをTBLに追加
            For i = 1 To 9
                If (Controls("txtTag" & i).Text <> "") Then

                    Dim cmb As ComboBox
                    cmb = CType(Controls("cmbTag" & i), ComboBox)

                    If (cmb.SelectedValue > 0) Then
                        'ファイルタグをTBLに追加　
                        clsFormBean.AddTag(Controls("txtTag" & i).Text, cmb.SelectedValue)
                    End If
                End If
            Next

            'DB更新
            clsFormBean.insertFileForm()

            If (TabControl1.SelectedTab Is tabImageList) Then
                'イメージリスト検索
                Dim clsImageListBean As New ImageListBean
                clsImageListBean.getOraData(iNowFolder, -1, Nothing)

                Dim clsImageListView As New ImageListView(Me)
                clsImageListView.ImageListCreate(clsImageListBean.imageListBeans)
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

    '更新ボタン　クリック
    '登録ファイルを移動して、ファイルTBL、ファイルタグTBLを更新
    Private Sub btnUpdate_Click(sender As System.Object, e As System.EventArgs) Handles btnUpdate.Click

        If (iNowFolder < 1) Then
            MsgBox("親フォルダが選択されていません。")
            Exit Sub
        End If

        Dim iMaxGenreID As Integer
        Dim clsOraAccess As New OraAccess
        iMaxGenreID = clsOraAccess.queryGenreMaxId()

        If (iNowFolder <= iMaxGenreID) Then
            MsgBox("ルートフォルダは指定できません")
            Exit Sub
        End If

        Dim sFilePath As String = lblFilePath.Text

       Try
            'ファイルを移動
            Dim sAftPath As String = clsOraAccess.queryFolderPath(iNowFolder) & txtFileName.Text & ".zip"
            If (sFilePath <> sAftPath) Then
                System.IO.File.Move(sFilePath, sAftPath)
            End If


            Dim clsFormBean As New FormBean

            clsFormBean.file_id = iNowFile
            clsFormBean.folder_id = iNowFolder
            clsFormBean.title = txtFileName.Text
            clsFormBean.file_name = txtFileName.Text & ".zip"
            clsFormBean.rank = iRank
            clsFormBean.thumbnail = picThumbs.Image
            clsFormBean.fullpath = sAftPath


            '属性をBeanにセット
            '属性をファイルタグTBLに追加
            Dim chk As CheckBox
            For i = 1 To 10
                chk = CType(Controls("chkZoku" & i), CheckBox)
                If (chk.Checked = True) Then
                    clsFormBean.AddTag(chk.Text, 5)
                End If
            Next

            'プルダウンの選択値を取得し、ファイルタグをTBLに追加
            For i = 1 To 9
                If (Controls("txtTag" & i).Text <> "") Then

                    Dim cmb As ComboBox
                    cmb = CType(Controls("cmbTag" & i), ComboBox)

                    If (cmb.SelectedValue > 0) Then
                        'ファイルタグをTBLに追加　
                        clsFormBean.AddTag(Controls("txtTag" & i).Text, cmb.SelectedValue)
                    End If
                End If
            Next

            'DB更新
            clsFormBean.updateFileForm()

            'イメージリスト更新
            Dim clsImageListView As New ImageListView(Me)

            If (TabControl1.SelectedTab Is tabImageList And
                bKensakuPattern = False) Then
                Dim clsImageListBean As New ImageListBean
                clsImageListBean.getOraData(iNowFolder, -1, Nothing)

                clsImageListView.ImageListCreate(clsImageListBean.imageListBeans)
            Else

                clsImageListView.ImageListChange(clsFormBean)

            End If

            With Me.NotifyIcon1
                .Icon = SystemIcons.Application
                .Visible = True
                .BalloonTipTitle = "FileMove"
                .BalloonTipText = "ファイルの更新が完了しました。"
                .BalloonTipIcon = ToolTipIcon.Info
                .ShowBalloonTip(3000)
            End With

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    'ファイル削除ボタンクリック
    'DBの情報を削除と同時に、ファイルの情報も削除する
    Private Sub btnNowDel_Click(sender As System.Object, e As System.EventArgs) Handles btnNowDel.Click

        Dim sFilePath As String = lblFilePath.Text

        If (MsgBox("削除しますか？" & vbCrLf & vbCrLf & sFilePath, vbYesNo) = vbYes) Then

            Dim clsFormBean As New FormBean
            clsFormBean.file_id = iNowFile

            Try
                'DBからファイルを削除
                clsFormBean.deleteFileForm()

                '実ファイルを削除
                My.Computer.FileSystem.DeleteFile(sFilePath, FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.SendToRecycleBin)

                If (TabControl1.SelectedTab Is tabImageList) Then
                    Dim clsImageListBean As New ImageListBean
                    clsImageListBean.getOraData(iNowFolder, -1, Nothing)

                    Dim clsImageListView As New ImageListView(Me)
                    clsImageListView.ImageListCreate(clsImageListBean.imageListBeans)
                End If

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


    '************************************************************************************************************************************************************************************
    '************************************************************************************************************************************************************************************

    '                                                                          フォルダ更新ボタンイベント

    '************************************************************************************************************************************************************************************
    '************************************************************************************************************************************************************************************

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

        Dim clsTreeView As New FolderTreeView(Me)
        clsTreeView.TreeCreate(sChildFolderName)
        If (TabControl1.SelectedTab Is tabImageList) Then
            Dim clsImageListBean As New ImageListBean
            clsImageListBean.getOraData(iNowFolder, -1, Nothing)

            Dim clsImageListView As New ImageListView(Me)
            clsImageListView.ImageListCreate(clsImageListBean.imageListBeans)
        End If

        If (MsgBox("フォルダを追加しました。続けてファイルを移動しますか？" & vbCrLf & vbCrLf & sChildFolderName, vbYesNo) = vbYes) Then

            btnMove.PerformClick()

        End If
    End Sub

    'フォルダ削除クリック
    Private Sub btnFolderDel_Click(sender As System.Object, e As System.EventArgs) Handles btnFolderDel.Click


        If (iNowFolder < 1) Then
            MsgBox("フォルダが選択されていません")
            Exit Sub
        End If

        Dim iMaxGenreID As Integer
        Dim clsOraAccess As New OraAccess
        iMaxGenreID = clsOraAccess.queryGenreMaxId()

        If (iNowFolder <= iMaxGenreID) Then
            MsgBox("ルートフォルダは削除できません")
            Exit Sub
        End If

        If MsgBox("フォルダを削除しますか？", vbYesNo) = vbNo Then
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

    '************************************************************************************************************************************************************************************
    '************************************************************************************************************************************************************************************

    '                                                                          ブックマーク更新ボタンイベント

    '************************************************************************************************************************************************************************************
    '************************************************************************************************************************************************************************************


    'ブックマーク追加ボタンクリック
    Private Sub btnBookMarkAdd_Click(sender As Object, e As EventArgs) Handles btnBookMarkAdd.Click

        Dim sFilePath As String = lblFilePath.Text

        'ファイルサイズを取得
        'Dim fi As System.IO.FileInfo = New System.IO.FileInfo(sFilePath)
        'Dim lsize As Long = fi.Length
        'fi = Nothing
        Dim lsize As Long = 0

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

            Dim clsBookMarkBean As New BookMarkBean
            clsBookMarkBean.getOraDataList(iNowFile)

            Dim clsFormView As New FormView(Me)

            'ブックマークリストを取得できた場合、ブックマークリストを表示
            If clsBookMarkBean.bookmark_imageListBeans.Count = 0 Then
                clsFormView.ShowBookMark(False)
            Else
                Dim clsBookMarkListView As New BookMarkImateListView(Me)
                clsBookMarkListView.BookMarkImageListCreate(clsBookMarkBean.bookmark_imageListBeans)

                clsFormView.ShowBookMark(True)
            End If

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
            If (TabControl1.SelectedTab Is tabImageList And
                bKensakuPattern = False) Then
                Dim clsImageListBean As New ImageListBean
                clsImageListBean.getOraData(iNowFolder, -1, Nothing)

                Dim clsImageListView As New ImageListView(Me)
                clsImageListView.ImageListCreate(clsImageListBean.imageListBeans)
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

        Dim sFilePath As String = lblFilePath.Text

        If (MsgBox("削除しますか？" & vbCrLf & vbCrLf & sFilePath, vbYesNo) = vbYes) Then

            Dim clsDBLogic As New DBLogic

            Try
                clsDBLogic.delFileTag(clsOraAccess, iBookMarkId, 3)
                clsOraAccess.deleteBookMark(iBookMarkId)

                Dim clsBookMarkBean As New BookMarkBean
                clsBookMarkBean.getOraDataList(iNowFile)

                Dim clsFormView As New FormView(Me)

                'ブックマークリストの存在チェック
                If clsBookMarkBean.bookmark_imageListBeans.Count = 0 Then

                    clsFormView.ShowBookMark(False)

                Else
                    Dim clsBookMarkListView As New BookMarkImateListView(Me)
                    clsBookMarkListView.BookMarkImageListCreate(clsBookMarkBean.bookmark_imageListBeans)

                    clsFormView.ShowBookMark(True)
                End If

                MessageBox.Show("削除が完了しました。")
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        End If

    End Sub

    '************************************************************************************************************************************************************************************
    '************************************************************************************************************************************************************************************

    '                                                                          検索ボタンイベント

    '************************************************************************************************************************************************************************************
    '************************************************************************************************************************************************************************************

    'ランク順ボタンクリック
    Private Sub btnRankSearch_Click(sender As System.Object, e As System.EventArgs) Handles btnRankSearch.Click

        '検索フラグをオン
        bKensakuPattern = True

        Dim iMaxGenreId As Integer
        Dim clsOraAccess As New OraAccess
        iMaxGenreId = clsOraAccess.queryGenreMaxId()

        Select Case iNowFolder
            Case Is < 1
                MsgBox("フォルダが選択されていません")
                Exit Sub
            Case Is <= iMaxGenreId
                If iRank < 2 Then
                    If MsgBox("長時間検索になりますが、検索を続けますか？", vbYesNo) = vbNo Then
                        Exit Sub
                    End If
                End If
        End Select



        Dim clsFormView As New FormView(Me)

        Dim clsImageListBean As New ImageListBean
        Dim lstZokusei As New List(Of String)

        'フォルダ、ランク、属性で検索しBeanにセット
        clsFormView.SetZokusei(lstZokusei)
        clsImageListBean.getOraData(iNowFolder, iRank, lstZokusei)

        'イメージリスト作成
        Dim clsImageListView As New ImageListView(Me)
        clsImageListView.ImageListCreate(clsImageListBean.imageListBeans)

        'イメージタブ選択
        TabControl1.SelectedTab = tabImageList

    End Sub

    '更新日順ボタンクリック
    Private Sub btnAddDaySearch_Click(sender As System.Object, e As System.EventArgs) Handles btnAddDaySearch.Click

        '検索フラグをオン
        bKensakuPattern = True

        Dim clsFormView As New FormView(Me)

        Dim clsImageListBean As New ImageListBean
        Dim lstZokusei As New List(Of String)

        '更新日、ランク、属性で検索しBeanにセット
        clsFormView.SetZokusei(lstZokusei)
        clsImageListBean.getOraData(iRank, lstZokusei)

        'イメージリスト作成
        Dim clsImageListView As New ImageListView(Me)
        clsImageListView.ImageListCreate(clsImageListBean.imageListBeans)

        TabControl1.SelectedTab = tabImageList
    End Sub

    '************************************************************************************************************************************************************************************
    '************************************************************************************************************************************************************************************

    '                                                                          タグボタンイベント

    '************************************************************************************************************************************************************************************
    '************************************************************************************************************************************************************************************

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

    'タグ再設定ボタンクリック
    Private Sub btnTagSettinn_Click(sender As Object, e As EventArgs) Handles btnTagSettinn.Click

        '＊＊＊フォーム生成＊＊＊
        'ファイルパスで指定されたZIP書庫を展開し、
        'フォルダ名、サムネイル、タグを取得

        Dim clsZipOpen As ZipOpen
        Dim sFileName As String
        Dim listTag As List(Of String)
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
        Dim clsTreeView As New FolderTreeView(Me)
        clsTreeView.TreeCreate(sConditionValue)

        'txtFileName.Focus()
        TabControl1.Focus()
        TabControl1.SelectedTab = tabTree

    End Sub

    '************************************************************************************************************************************************************************************
    '************************************************************************************************************************************************************************************

    '                                                                          その他ボタンイベント

    '************************************************************************************************************************************************************************************
    '************************************************************************************************************************************************************************************

    'googleボタンクリック
    Private Sub btnNameSearch_Click(sender As Object, e As EventArgs) Handles btnNameSearch.Click
        Dim sName As String = txtTagSetting.Text
        If (sName <> "") Then
            TabControl1.SelectedTab = tabGoogle
            WebBrowser1.Navigate(New Uri("https://www.google.co.jp/search?q=" & Uri.EscapeUriString(sName.Replace("_", " ").Trim)))
        End If
    End Sub


    'ファイル削除（DB削除なし）
    Private Sub btnDelete_Click(sender As System.Object, e As System.EventArgs) Handles btnDelete.Click

        Dim sFilePath As String = lblFilePath.Text


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


    '************************************************************************************************************************************************************************************
    '************************************************************************************************************************************************************************************
    '************************************************************************************************************************************************************************************

    '                                                                           イメージクリックイベント

    '************************************************************************************************************************************************************************************
    '************************************************************************************************************************************************************************************
    '************************************************************************************************************************************************************************************


    'イメージリストクリック
    '初期化（ファイルあり）を呼び出し、フォームに情報を表示
    Private Sub lstThumbs_Click(sender As Object, e As System.EventArgs) Handles lstThumbs.Click

        Dim itemx As ListViewItem = sender.SelectedItems(0)
        'sFilePath = itemx.SubItems(1).Text
        'Console.WriteLine(itemx.SubItems(1).Text)
        Dim clsFormView As New FormView(Me)

        'タグやチェックをクリア
        clsFormView.DataClear()

        'DBからフォーム情報を取得しBeanにセット
        Dim clsFormBean As New FormBean
        clsFormBean.getOraData(itemx.SubItems(1).Text)

        'フォームにBeanの値を設定
        clsFormView.FormSet(clsFormBean)

        'フォルダIDを設定
        iNowFolder = clsFormBean.folder_id

        'ファイルIDを設定
        iNowFile = clsFormBean.file_id

        'ランクを設定
        iRank = clsFormBean.rank

        '更新モードでボタンを設定
        clsFormView.BtnUpdateMode()

        TabControl1.Focus()

        'ブックマークリストを設定
        Dim clsBookMarkBean As New BookMarkBean
        clsBookMarkBean.getOraDataList(clsFormBean.file_id)

        'ブックマークリストを取得できた場合、ブックマークリストを表示
        If clsBookMarkBean.bookmark_imageListBeans.Count = 0 Then
            clsFormView.ShowBookMark(False)
        Else
            Dim clsBookMarkImageListView As New BookMarkImateListView(Me)
            clsBookMarkImageListView.BookMarkImageListCreate(clsBookMarkBean.bookmark_imageListBeans)

            clsFormView.ShowBookMark(True)
        End If

        bBookMarkFlag = False

    End Sub


    'イメージリスト右ダブルクリック時のイベント
    Private Sub lstThumbs_RightDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lstThumbs.MouseDown

        'マウスのダブルクリックイベント
        If e.Button = MouseButtons.Right AndAlso e.Clicks = 2 Then

            If sender.SelectedItems.count = 0 Then
                Exit Sub
            End If

            Dim itemx As ListViewItem = sender.SelectedItems(0)
            'sFilePath = itemx.SubItems(1).Text

            Dim clsFormView As New FormView(Me)

            'タグやチェックをクリア
            clsFormView.DataClear()

            'DBからフォーム情報を取得しBeanにセット
            Dim clsFormBean As New FormBean
            clsFormBean.getOraData(itemx.SubItems(1).Text)

            'フォームにBeanの値を設定
            clsFormView.FormSet(clsFormBean)

            'フォルダIDを設定
            iNowFolder = clsFormBean.folder_id

            'ファイルIDを設定
            iNowFile = clsFormBean.file_id

            'ランクを設定
            iRank = clsFormBean.rank

            '更新モードでボタンを設定
            clsFormView.BtnUpdateMode()

            TabControl1.Focus()

            'ブックマークリストを設定
            Dim clsBookMarkBean As New BookMarkBean
            clsBookMarkBean.getOraDataList(clsFormBean.file_id)

            'ブックマークリストを取得できた場合、ブックマークリストを表示
            If clsBookMarkBean.bookmark_imageListBeans.Count = 0 Then
                clsFormView.ShowBookMark(False)
            Else
                Dim clsBookMarkImageListView As New BookMarkImateListView(Me)
                clsBookMarkImageListView.BookMarkImageListCreate(clsBookMarkBean.bookmark_imageListBeans)

                clsFormView.ShowBookMark(True)
            End If


            'Process.Start("C:\ProgramData\NeeView\NeeView.exe", """" & lblFilePath.Text & "\" & sFirstFile & """")

            If bBookMarkFlag = True Then
                Process.Start("C:\ProgramData\NeeView\NeeView.exe", """" & lblFilePath.Text & """")
            Else

                Dim sAllPath As String = Nothing
                Dim clsNeeViewWorks As New NeeViewWorks

                clsNeeViewWorks.ScriptCreate(lblFilePath.Text, lstThumbs)
                clsNeeViewWorks.KidouHikisuuCreate(lstThumbs, lblFilePath.Text, sAllPath)
                clsNeeViewWorks = Nothing

                Process.Start("C:\ProgramData\NeeView\NeeView.exe", sAllPath)

            End If

        End If
    End Sub


    ''' <summary>
    ''' ブックマークのイメージリストクリック
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub lstBookMark_Click(sender As Object, e As EventArgs) Handles lstBookMark.Click

        Dim sFilePath As String = lblFilePath.Text

        sFilePath = lstBookMark.SelectedItems(0).SubItems(1).Text

        If (bKensakuPattern = True) Then
            'いったんコメント
            'iNowFolder = lstBookMarkPath(4)(lstBookMark.SelectedItems(0).Index)
            lblNowFolder.Text = "同一"
        End If

        Dim clsBookMarkBean As New BookMarkBean
        'ブックマークテーブルにデータがある場合、TBLからフォームデータを取得しBeanにセット
        clsBookMarkBean.getOraData(sFilePath)

        Dim clsFormView As New FormView(Me)
        'Beanからフォーム生成
        clsFormView.FormSet(clsBookMarkBean)

        clsFormView.BtnBookMarkUpdateMode()

        bBookMarkFlag = True

    End Sub


    ''' <summary>
    ''' ブックマークイメージリストの右ダブルクリック
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub lstBookMark_RightDoubleClick(sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lstBookMark.MouseDown
        'マウスのダブルクリックイベント
        If e.Button = MouseButtons.Right AndAlso e.Clicks = 2 Then

            If sender.SelectedItems.count = 0 Then
                Exit Sub
            End If

            Dim sFilePath As String = lblFilePath.Text

            sFilePath = lstBookMark.SelectedItems(0).SubItems(1).Text

            If (bKensakuPattern = True) Then
                'いったんコメント
                'iNowFolder = lstBookMarkPath(4)(lstBookMark.SelectedItems(0).Index)
                lblNowFolder.Text = "同一"
            End If

            Dim clsBookMarkBean As New BookMarkBean
            'ブックマークテーブルにデータがある場合、TBLからフォームデータを取得しBeanにセット
            clsBookMarkBean.getOraData(sFilePath)

            Dim clsFormView As New FormView(Me)
            'Beanからフォーム生成
            clsFormView.FormSet(clsBookMarkBean)

            clsFormView.BtnBookMarkUpdateMode()

            bBookMarkFlag = True

            Process.Start("C:\ProgramData\NeeView\NeeView.exe", """" & lblFilePath.Text & """")
        End If

    End Sub

    'フォームのサムネイルクリック
    'NeeViewを起動
    Private Sub picThumbs_Click(sender As Object, e As System.EventArgs) Handles picThumbs.Click

        If bBookMarkFlag = True Then
            Process.Start("C:\ProgramData\NeeView\NeeView.exe", """" & lblFilePath.Text & """")
        Else

            Dim sAllPath As String = Nothing
            Dim clsNeeViewWorks As New NeeViewWorks

            clsNeeViewWorks.ScriptCreate(lblFilePath.Text, lstThumbs)
            clsNeeViewWorks.KidouHikisuuCreate(lstThumbs, lblFilePath.Text, sAllPath)
            clsNeeViewWorks = Nothing

            Process.Start("C:\ProgramData\NeeView\NeeView.exe", sAllPath)
        End If
    End Sub

    '☆マウスオーバーイベント
    '☆のイメージを変更し、ランクの値を更新する。System.ComponentModel.Win32Exception: 
    Private Sub picRank_Click(sender As Object, e As EventArgs) Handles picRank0.MouseLeave, picRank1.MouseHover, picRank2.MouseHover, picRank3.MouseHover, picRank4.MouseHover, picRank5.MouseHover


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


    '************************************************************************************************************************************************************************************
    '************************************************************************************************************************************************************************************
    '************************************************************************************************************************************************************************************

    '                                                                           ツリービュークリックイベント

    '************************************************************************************************************************************************************************************
    '************************************************************************************************************************************************************************************
    '************************************************************************************************************************************************************************************

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

        If txtFileName.Text.Length > 0 Then
            Dim sAftPath As String = clsOraAccess.queryFolderPath(iNowFolder)
            Dim sPath = sAftPath & txtFileName.Text & ".zip"

            If FileExistCheck(sPath) Then
                txtFileName.BackColor = Color.Yellow
            Else
                txtFileName.BackColor = Color.White
            End If
        End If
    End Sub

    Private Sub treeDir_MouseDown(sender As Object, e As MouseEventArgs) Handles treeDir.MouseDown

        Dim info As TreeViewHitTestInfo = treeDir.HitTest(e.X, e.Y)

        If ((info.Node IsNot Nothing) And (info.Node Is treeDir.SelectedNode)) Then
            iNowFolder = treeDir.SelectedNode.Name
            Me.lblNowFolder.Text = treeDir.SelectedNode.Text

            If txtFileName.Text.Length > 0 Then
                Dim sAftPath As String = clsOraAccess.queryFolderPath(iNowFolder)
                Dim sPath = sAftPath & txtFileName.Text & ".zip"

                If FileExistCheck(sPath) Then
                    txtFileName.BackColor = Color.Yellow
                Else
                    txtFileName.BackColor = Color.White
                End If
            End If
        End If
    End Sub

    'ツリービューダブルクリック
    'タブ2に切り替え、イメージリストを作成し表示
    Private Sub treeDir_DoubleClick(sender As Object, e As System.EventArgs) Handles treeDir.DoubleClick

        If (treeDir.SelectedNode Is Nothing) Then Exit Sub
        iNowFolder = treeDir.SelectedNode.Name
        Me.lblNowFolder.Text = treeDir.SelectedNode.Text


        Dim iMaxGenreID As Integer
        Dim clsOraAccess As New OraAccess
        iMaxGenreId = clsOraAccess.queryGenreMaxId()

        If iNowFolder <= iMaxGenreID Then Exit Sub


        '検索フラグをオン
        bKensakuPattern = False

        Dim clsImageListBean As New ImageListBean
        clsImageListBean.getOraData(iNowFolder, -1, Nothing)

        Dim clsImageListView As New ImageListView(Me)
        clsImageListView.ImageListCreate(clsImageListBean.imageListBeans)

        TabControl1.SelectedTab = tabImageList

    End Sub

    '************************************************************************************************************************************************************************************
    '************************************************************************************************************************************************************************************
    '************************************************************************************************************************************************************************************

    '                                                                           テキストボックスクリックイベント

    '************************************************************************************************************************************************************************************
    '************************************************************************************************************************************************************************************
    '************************************************************************************************************************************************************************************

    'タグテキストボックスをダブルクリック
    '入力値でフォルダTBLを検索し、ツリービューを変更
    Private Sub txtTag_DoubleClick(sender As Object, e As System.EventArgs) Handles txtTag1.DoubleClick, txtTag2.DoubleClick, txtTag3.DoubleClick, txtTag4.DoubleClick, txtTag5.DoubleClick, txtTag6.DoubleClick, txtTag7.DoubleClick, txtTag8.DoubleClick, txtTag9.DoubleClick


        Dim txt As TextBox = CType(sender, TextBox)
        Dim sChildFolderName As String = txt.Text

        Dim clsTreeView As New FolderTreeView(Me)
        clsTreeView.TreeCreate(sChildFolderName)
        TabControl1.SelectedTab = tabTree

    End Sub

    'タグテキスト、右ダブルクリック時のイベント
    Private Sub txtTag_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles txtTag1.MouseDown, txtTag2.MouseDown, txtTag3.MouseDown, txtTag4.MouseDown, txtTag5.MouseDown, txtTag6.MouseDown, txtTag7.MouseDown, txtTag8.MouseDown, txtTag9.MouseDown

        'マウスのダブルクリックイベント
        If e.Button = MouseButtons.Right AndAlso e.Clicks = 2 Then

            Dim txt As TextBox = CType(sender, TextBox)
            Dim sChildFolderName As String = txt.Text

            'iRankの妥当性
            Dim clsFormView As New FormView(Me)
            Dim clsImageListBean As New ImageListBean
            Dim lstZokusei As New List(Of String)

            'タグで検索しBeanにセット
            clsFormView.SetZokusei(lstZokusei)

            'チェック
            If sChildFolderName = "" And lstZokusei.Count = 0 Then
                MsgBox("条件未設定")
                Exit Sub
            End If

            clsImageListBean.getOraData(sChildFolderName, iRank, lstZokusei)

            Dim clsImageListView As New ImageListView(Me)
            clsImageListView.ImageListCreate(clsImageListBean.imageListBeans)

            TabControl1.SelectedTab = tabImageList
        End If
    End Sub


    'ファイル名テキストボックス、エンターボタン押下時のイベント
    'ファイル名を前選択する
    Private Sub txtFileName_Enter(sender As Object, e As System.EventArgs) Handles txtFileName.Enter
        Me.txtFileName.SelectAll()
    End Sub

    'ファイル名テキストボックス、マウスクリック時のイベント
    'ファイル名を前選択する
    Private Sub txtFileName_MouseClick(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles txtFileName.MouseClick
        Me.txtFileName.SelectAll()
    End Sub

    'タグテキストフォーカスアウト時のイベント
    'タグのテキストボックス設定値をもとに、コンボボックス設定値を自動で取得する
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

    Private Sub txtFileName_Leave(sender As Object, e As EventArgs) Handles txtFileName.Leave

        Dim sAftPath As String = clsOraAccess.queryFolderPath(iNowFolder)
        Dim sPath = sAftPath & txtFileName.Text & ".zip"

        If FileExistCheck(sPath) Then
            txtFileName.BackColor = Color.Yellow
        Else
            txtFileName.BackColor = Color.White
        End If
    End Sub



    'タグ再設定テキストダブルクリック
    Private Sub txtTagSetting_DoubleClick(sender As Object, e As EventArgs) Handles txtTagSetting.DoubleClick
        Dim sName As String = txtTagSetting.Text
        If (sName <> "") Then
            TabControl1.SelectedTab = tabGoogle
            WebBrowser1.Navigate(New Uri("https://www.google.co.jp/search?q=" & Uri.EscapeUriString(sName.Replace("_", " ").Trim)))
        End If

    End Sub

    '************************************************************************************************************************************************************************************
    '************************************************************************************************************************************************************************************
    '************************************************************************************************************************************************************************************

    '                                                                           チェックボックスイベント

    '************************************************************************************************************************************************************************************
    '************************************************************************************************************************************************************************************
    '************************************************************************************************************************************************************************************

    '属性チェックボックスクリック時のイベント
    'オンの場合ラベルを黄色、オフの場合白に変える
    Private Sub chkZoku_CheckedChanged(sender As Object, e As EventArgs) Handles chkZoku1.CheckedChanged, chkZoku2.CheckedChanged, chkZoku3.CheckedChanged, chkZoku4.CheckedChanged, chkZoku5.CheckedChanged, chkZoku6.CheckedChanged, chkZoku7.CheckedChanged, chkZoku8.CheckedChanged, chkZoku9.CheckedChanged, chkZoku10.CheckedChanged, chkZoku11.CheckedChanged, chkZoku12.CheckedChanged, chkZoku13.CheckedChanged, chkZoku14.CheckedChanged, chkZoku15.CheckedChanged

        Dim chk As CheckBox
        For i = 1 To 15
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

End Class



