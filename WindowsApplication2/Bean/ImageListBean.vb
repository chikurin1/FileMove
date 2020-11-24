Imports Oracle.DataAccess.Client
Imports Oracle.DataAccess.Types
Public Class ImageListBean

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

    Private _imageListBeans As New List(Of ImageListBean)
    Public Property imageListBeans() As List(Of ImageListBean)
        Get
            Return _imageListBeans
        End Get
        Set(ByVal value As List(Of ImageListBean))
            _imageListBeans = value
        End Set
    End Property


    'フォルダID検索
    Public Sub getOraData(ByVal iFolderId As Integer, ByVal iRank As Integer, ByRef lstZokusei As List(Of String))

        Dim clsOraAccess As OraAccess
        'DBアクセス用クラスのインスタンスを作成
        clsOraAccess = New OraAccess()

        Dim readerFileList As OracleDataReader = Nothing

        Console.WriteLine("DBからイメージリスト情報取得開始(フォルダID検索)")

        Try
            If iRank = -1 Then
                clsOraAccess.queryFileList(iFolderId, readerFileList)
            Else
                clsOraAccess.queryFileListKensaku(1, lstZokusei, iRank, iFolderId, readerFileList)
            End If

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

        setBean(readerFileList)

    End Sub

    'タグ名検索
    Public Sub getOraData(ByVal sTagName As String, ByVal iRank As Integer, ByRef lstZokusei As List(Of String))

        Dim clsOraAccess As OraAccess
        'DBアクセス用クラスのインスタンスを作成
        clsOraAccess = New OraAccess()

        Dim readerFileList As OracleDataReader = Nothing

        Console.WriteLine("DBからイメージリスト情報取得開始(タグ名検索)")

        Try
            clsOraAccess.queryFileListKensaku(lstZokusei, iRank, sTagName, readerFileList)

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

        setBean(readerFileList)

    End Sub

    '更新日順検索
    Public Sub getOraData(ByVal iRank As Integer, ByRef lstZokusei As List(Of String))

        Dim clsOraAccess As OraAccess
        'DBアクセス用クラスのインスタンスを作成
        clsOraAccess = New OraAccess()

        Dim readerFileList As OracleDataReader = Nothing

        Console.WriteLine("DBからイメージリスト情報取得開始(タグ名検索)")

        Try
            clsOraAccess.queryFileListKensaku(0, lstZokusei, iRank, 0, readerFileList)

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

        setBean(readerFileList)

    End Sub

    Private Sub setBean(ByRef readerFileList As OracleDataReader)

        If readerFileList Is Nothing Then
            Exit Sub
        End If

        'カレントファイル、フォルダを変数に格納
        While (readerFileList.Read())
            Dim clsImageListBean As New ImageListBean

            clsImageListBean.file_id = readerFileList.GetValue(0)
            clsImageListBean.folder_id = readerFileList.GetValue(7)


            'パス、タイトル、ファイルサイズ設定
            clsImageListBean.fullpath = readerFileList.GetString(2)
            clsImageListBean.title = readerFileList.GetString(1)
            clsImageListBean.file_size = ChangeFileSize(readerFileList.GetValue(3))

            'ランクを設定
            clsImageListBean.rank = readerFileList.GetValue(5)

            'サムネイルを設定
            Dim blob As OracleBlob = readerFileList.GetOracleBlob(4)
            Dim ms As New System.IO.MemoryStream(blob.Value)
            clsImageListBean.thumbnail = Image.FromStream(ms)

            _imageListBeans.Add(clsImageListBean)
        End While

    End Sub

End Class
