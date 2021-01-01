Imports Oracle.DataAccess.Client
Imports Oracle.DataAccess.Types
Public Class FormBean

    Private _title As String
    Public Property title() As String
        Get
            Return _title
        End Get
        Set(ByVal value As String)
            _title = value
        End Set
    End Property

    Private _file_id As Integer
    Public Property file_id() As Integer
        Get
            Return _file_id
        End Get
        Set(ByVal value As Integer)
            _file_id = value
        End Set
    End Property

    Private _fullpath As String
    Public Property fullpath() As String
        Get
            Return _fullpath
        End Get
        Set(ByVal value As String)
            _fullpath = value
        End Set
    End Property

    Private _file_size As String
    Public Property file_size() As String
        Get
            Return _file_size
        End Get
        Set(ByVal value As String)
            _file_size = value
        End Set
    End Property

    Private _thumbnail As Image
    Public Property thumbnail() As Image
        Get
            Return _thumbnail
        End Get
        Set(ByVal value As Image)
            _thumbnail = value
        End Set
    End Property
    Private _rank As Integer
    Public Property rank() As Integer
        Get
            Return _rank
        End Get
        Set(ByVal value As Integer)
            _rank = value
        End Set
    End Property
    Private _folder_id As Integer
    Public Property folder_id() As Integer
        Get
            Return _folder_id
        End Get
        Set(ByVal value As Integer)
            _folder_id = value
        End Set
    End Property
    Private _folder_name As String
    Public Property folder_name() As String
        Get
            Return _folder_name
        End Get
        Set(ByVal value As String)
            _folder_name = value
        End Set
    End Property
    Private _parent_folder_name As String
    Public Property parent_folder_name() As String
        Get
            Return _parent_folder_name
        End Get
        Set(ByVal value As String)
            _parent_folder_name = value
        End Set
    End Property

    Private _tagname_lst As New List(Of String)
    Public Property tagname_lst() As List(Of String)
        Get
            Return _tagname_lst
        End Get
        Set(ByVal value As List(Of String))
            _tagname_lst = value
        End Set
    End Property

    Private _tagcat_lst As New List(Of Integer)
    Public Property tagcat_lst() As List(Of Integer)
        Get
            Return _tagcat_lst
        End Get
        Set(ByVal value As List(Of Integer))
            _tagcat_lst = value
        End Set
    End Property


    Private _file_name As String
    Public Property file_name() As String
        Get
            Return _file_name
        End Get
        Set(ByVal value As String)
            _file_name = value
        End Set
    End Property

    '☆暫定☆
    Private _first_file As String
    Public Property first_file() As String
        Get
            Return _first_file
        End Get
        Set(ByVal value As String)
            _first_file = value
        End Set
    End Property

    Public Overridable Sub getZipData(ByVal sFilePath As String)

        Dim clsZipOpen As ZipOpen
        Dim fi As System.IO.FileInfo

        Console.WriteLine("Zipファイルからフォーム情報取得開始")

        Try
            'コンストラクタでファイルパスを指定
            clsZipOpen = New ZipOpen(sFilePath)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

        'タグ名取得
        clsZipOpen.tagCreate(clsZipOpen.FileName)
        tagname_lst = clsZipOpen.Tag

        'カテゴリ取得
        setTagCat(tagname_lst, tagcat_lst)

        'タイトルを取得
        title = clsZipOpen.FileName

        'ファイル名を取得
        file_name = clsZipOpen.FileMei

        'イメージを取得
        thumbnail = clsZipOpen.Thumbs

        'ファイルサイズを取得
        fi = New System.IO.FileInfo(sFilePath)
        file_size = ChangeFileSize(fi.Length)

        'フルパス
        fullpath = sFilePath

        'ランクは１を設定
        rank = 1

    End Sub

    Public Overridable Sub getOraData(ByVal sFilePath As String)

        Dim clsOraAccess As OraAccess
        'DBアクセス用クラスのインスタンスを作成
        clsOraAccess = New OraAccess()

        Dim readerFilePath As OracleDataReader = Nothing

        Console.WriteLine("DBからフォーム情報取得開始")

        Try
            'パスからファイル情報を取得
            clsOraAccess.queryFilePath(sFilePath, readerFilePath)

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

        Dim clsFormView As New FormView

        'カレントファイル、フォルダを変数に格納
        While (readerFilePath.Read())
            file_id = readerFilePath.GetValue(0)
            folder_id = readerFilePath.GetValue(6)

            'パス、タイトル、ファイルサイズ設定
            fullpath = sFilePath
            title = readerFilePath.GetString(1)
            file_size = ChangeFileSize(readerFilePath.GetValue(3))
            folder_name = readerFilePath.GetString(7)

            'サムネイルを設定
            Dim blob As OracleBlob = readerFilePath.GetOracleBlob(4)
            Dim ms As New System.IO.MemoryStream(blob.Value)
            thumbnail = Image.FromStream(ms)

            'ランクを設定
            rank = readerFilePath.GetValue(5)

        End While

        Dim clsZipOpen As New ZipOpen

        'タグ取得
        setTagData(file_id, tagname_lst, tagcat_lst, 1)

    End Sub


    Public Overridable Sub getOraDataImageChange(ByVal sFilePath As String)

        Dim clsOraAccess As OraAccess
        'DBアクセス用クラスのインスタンスを作成
        clsOraAccess = New OraAccess()

        Dim readerFilePath As OracleDataReader = Nothing

        Console.WriteLine("DBからフォーム情報取得開始")

        Try
            'パスからファイル情報を取得
            clsOraAccess.queryFilePath(sFilePath, readerFilePath)

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

        Dim clsFormView As New FormView
        'サムネイルを設定
        '☆暫定☆
        clsFormView.ImageGet(sFilePath, thumbnail, first_file)
        '暫定解除後は↓を正とする
        'clsFormView.ImageGet(sFilePath, thumbnail)

        'カレントファイル、フォルダを変数に格納
        While (readerFilePath.Read())
            file_id = readerFilePath.GetValue(0)
            folder_id = readerFilePath.GetValue(6)


            'パス、タイトル、ファイルサイズ設定
            fullpath = sFilePath
            title = readerFilePath.GetString(1)
            file_size = ChangeFileSize(readerFilePath.GetValue(3))
            folder_name = readerFilePath.GetString(7)

            'ランクを設定
            rank = readerFilePath.GetValue(5)

        End While

        Dim clsZipOpen As New ZipOpen

        'タグ取得
        setTagData(file_id, tagname_lst, tagcat_lst, 1)

    End Sub

    Public Sub setTagData(ByVal iFileID As Integer, ByRef lstTagName As List(Of String), ByRef lstTagCat As List(Of Integer), ByVal iFileKbn As Integer)

        Dim clsOraAccess As OraAccess
        Dim readerFileTag As OracleDataReader

        Try
            'DBアクセス用クラスのインスタンスを作成
            clsOraAccess = New OraAccess()
            'タグ取得
            clsOraAccess.queryFileTag(iFileID, iFileKbn, readerFileTag)

            'タグを設定
            While (readerFileTag.Read())
                lstTagCat.Add(readerFileTag.GetValue(0))
                lstTagName.Add(readerFileTag.GetString(1))
            End While
            readerFileTag.Close()
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            readerFileTag.Close()

        End Try
    End Sub


    Public Sub insertFileForm()

        Dim clsOraAccess As New OraAccess

        'ファイルをTBLに追加
        _file_id = clsOraAccess.insertFile(_title, _file_name, _folder_id, _file_size, _rank, _thumbnail)

        Dim clsDBLogic As New DBLogic

        'タグテーブルに追加
        For i = 0 To _tagname_lst.Count - 1
            clsDBLogic.insertFileTag(clsOraAccess, _tagcat_lst.Item(i), _tagname_lst.Item(i), _file_id, 1)
        Next

    End Sub

    Public Sub updateFileForm()

        Dim clsOraAccess As New OraAccess

        'ファイルをTBLに追加
        clsOraAccess.updateFile(_file_id, _folder_id, _title, _file_name, _rank, _thumbnail)

        'タグテーブルに追加
        Dim clsDBLogic As New DBLogic

        'ファイルタグをTBLから削除
        clsDBLogic.delFileTag(clsOraAccess, _file_id, 1)

        For i = 0 To _tagname_lst.Count - 1
            clsDBLogic.insertFileTag(clsOraAccess, _tagcat_lst.Item(i), _tagname_lst.Item(i), _file_id, 1)
        Next

    End Sub

    Public Sub deleteFileForm()

        Dim clsDBLogic As New DBLogic
        Dim clsOraAccess As New OraAccess

        'タグテーブルを削除
        clsDBLogic.delFileTag(clsOraAccess, _file_id, 1)

        'ファイルをTBLを削除
        clsOraAccess.deleteFile(_file_id)


    End Sub


    Public Sub setTagCat(ByRef lstTagName As List(Of String), ByRef lstTagCat As List(Of Integer))

        Dim clsOraAccess As OraAccess

        Try
            'DBアクセス用クラスのインスタンスを作成
            clsOraAccess = New OraAccess()
            'タグ取得

            For Each sTagName In lstTagName
                lstTagCat.Add(clsOraAccess.queryDefaultCategory(sTagName))
            Next
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally

        End Try
    End Sub

    Public Overloads Sub AddTag(ByVal sTagName As String, ByVal iTagCat As Integer)

        _tagname_lst.Add(sTagName)
        _tagcat_lst.Add(iTagCat)

    End Sub


    Public Overridable Function FileExistCheck(ByVal sPath As String) As Boolean

        Dim clsOraAccess As OraAccess

        Try
            'DBアクセス用クラスのインスタンスを作成
            clsOraAccess = New OraAccess()
            'タグ取得

            If clsOraAccess.queryFilePathCount(sPath) = 0 Then
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
