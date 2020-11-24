Imports Oracle.DataAccess.Client
Imports Oracle.DataAccess.Types

Public Class BookMarkBean
    Inherits FormBean

    Private _bookmark_id As Integer
    Public Property bookmark_id() As Integer
        Get
            Return _bookmark_id
        End Get
        Set(ByVal value As Integer)
            _bookmark_id = value
        End Set
    End Property

    Private _bookmark_file_name As String
    Public Property bookmark_file_name() As String
        Get
            Return _bookmark_file_name
        End Get
        Set(ByVal value As String)
            _bookmark_file_name = value
        End Set
    End Property

    Private _bookmark_imageListBeans As New List(Of BookMarkBean)
    Public Property bookmark_imageListBeans() As List(Of BookMarkBean)
        Get
            Return _bookmark_imageListBeans
        End Get
        Set(ByVal value As List(Of BookMarkBean))
            _bookmark_imageListBeans = value
        End Set
    End Property
    Public Overrides Sub getZipData(ByVal sFilePath As String)

        Dim clsZipOpen As ZipOpen
        Dim fi As System.IO.FileInfo

        Console.WriteLine("Zipファイルからフォーム情報取得開始")

        Try
            'コンストラクタでファイルパスを指定
            clsZipOpen = New ZipOpen()
            clsZipOpen.zipWorks(sFilePath)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

        'タグ名取得
        clsZipOpen.tagCreate(clsZipOpen.BookMarkName)
        tagname_lst = clsZipOpen.Tag

        'カテゴリ取得
        setTagCat(tagname_lst, tagcat_lst)

        'タイトルを取得
        title = clsZipOpen.FileName

        'ファイル名を取得
        file_name = clsZipOpen.FileMei

        bookmark_file_name = clsZipOpen.FileMei

        'イメージを取得
        thumbnail = clsZipOpen.Thumbs

        'ファイルサイズを取得
        'fi = New System.IO.FileInfo(sFilePath)
        'file_size = ChangeFileSize(fi.Length)

        'フルパス
        fullpath = sFilePath

        'ランクは１を設定
        rank = 1

    End Sub

    Public Overrides Sub getOraData(ByVal sFilePath As String)

        Dim clsOraAccess As OraAccess
        'DBアクセス用クラスのインスタンスを作成
        clsOraAccess = New OraAccess()

        Dim readerFilePath As OracleDataReader = Nothing

        Console.WriteLine("DBからフォーム情報取得開始")

        Try
            'パスからファイル情報を取得
            clsOraAccess.queryBookMarkPath(sFilePath, readerFilePath)

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try


        'DB取得値をBeanに設定
        While (readerFilePath.Read())
            file_id = readerFilePath.GetValue(0)
            folder_id = readerFilePath.GetValue(6)


            'パス、タイトル、ファイルサイズ設定
            fullpath = sFilePath
            title = readerFilePath.GetString(1)
            file_size = ChangeFileSize(readerFilePath.GetValue(3))
            bookmark_id = readerFilePath.GetValue(7)
            bookmark_file_name = readerFilePath.GetString(8)

            'ランクを設定
            rank = readerFilePath.GetValue(5)

            'サムネイルを設定
            Dim blob As OracleBlob = readerFilePath.GetOracleBlob(4)
            Dim ms As New System.IO.MemoryStream(blob.Value)
            thumbnail = Image.FromStream(ms)
        End While

        'タグ取得
        setTagData(bookmark_id, tagname_lst, tagcat_lst, 3)

    End Sub

    'フォルダID検索
    Public Sub getOraDataList(ByVal iFileId As Integer)

        Dim clsOraAccess As OraAccess
        'DBアクセス用クラスのインスタンスを作成
        clsOraAccess = New OraAccess()

        Dim readerFileList As OracleDataReader = Nothing

        Console.WriteLine("DBからイメージリスト情報取得開始(フォルダID検索)")

        Try
            clsOraAccess.queryBookMarkListKensaku(iFileId, readerFileList)

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

        setBean(readerFileList)

    End Sub

    Private Sub setBean(ByRef readerFileList As OracleDataReader)

        'カレントファイル、フォルダを変数に格納
        While (readerFileList.Read())
            Dim clsBookmarkBean As New BookMarkBean

            clsBookmarkBean.file_id = readerFileList.GetValue(0)
            clsBookmarkBean.folder_id = readerFileList.GetValue(7)


            'パス、タイトル、ファイルサイズ設定
            clsBookmarkBean.fullpath = readerFileList.GetString(2)
            clsBookmarkBean.title = readerFileList.GetString(1)
            clsBookmarkBean.file_size = ChangeFileSize(readerFileList.GetValue(3))
            clsBookmarkBean.bookmark_id = readerFileList.GetValue(8)
            clsBookmarkBean.bookmark_file_name = readerFileList.GetString(9)

            'ランクを設定
            clsBookmarkBean.rank = readerFileList.GetValue(5)

            'サムネイルを設定
            Dim blob As OracleBlob = readerFileList.GetOracleBlob(4)
            Dim ms As New System.IO.MemoryStream(blob.Value)
            clsBookmarkBean.thumbnail = Image.FromStream(ms)

            _bookmark_imageListBeans.Add(clsBookmarkBean)
        End While

    End Sub

    Public Overrides Function FileExistCheck(ByVal sPath As String) As Boolean

        Dim clsOraAccess As OraAccess

        Try
            'DBアクセス用クラスのインスタンスを作成
            clsOraAccess = New OraAccess()
            'タグ取得

            If clsOraAccess.querBookMarkPathCount(sPath) = 0 Then
                Return False
            Else
                Return True
            End If

        Catch ex As Exception
            MsgBox(ex.Message)
        Finally

        End Try

    End Function


End Class
