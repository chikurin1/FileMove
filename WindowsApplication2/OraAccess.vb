Imports Oracle.DataAccess.Client
Imports Oracle.DataAccess.Types

Public Class OraAccess

    'Connectionオブジェクトの生成
    Private cnn As New OracleConnection
    Private cmd As New OracleCommand
    Private sFilePath As String = Nothing

    '検索条件
    Private sGenreQuery As String
    Private sFolderQuery As String

    Private iId As Integer

    'コンストラクタ
    Public Sub New()
        'Oracleへのコネクションの確立
        cnn.ConnectionString = _
        "User Id=scott; Password=tiger; Data Source=orcl"
        cnn.Open()
        cmd.Connection = cnn
    End Sub

    'システム管理マスタからタグのジャンル用プルダウンリスト取得
    Public Function queryDropList(ByRef reader As OracleDataReader) As Integer

        Dim sDropListQuery As String = Nothing

        sDropListQuery = "select FLAG_01,KOBETU_01 from SYSTEM_KANRI_MST " &
                        "where KANRI_CODE='SE' AND KANRI_PG = 'FILE_CATEGORY' order by KANRI_KBN"

        cmd.CommandText = sDropListQuery

        reader = cmd.ExecuteReader()

        Return 0

    End Function

    'ジャンルを取得
    'sCondition　フォルダタグテーブルのフォルダ名
    '条件なければジャンルテーブルのジャンル名、ジャンルIDの一覧を取得
    '条件ある場合はフォルダタグテーブルのデータに一致するジャンル名、ジャンルIDの一覧を取得
    Public Function queryGenre(ByVal sCondition As String, ByRef reader As OracleDataReader) As Integer

        If (sCondition = "") Then

            sGenreQuery = "select distinct C.ID,C.TITLE from FOLDER_TBL A,FOLDER_TAG_TBL B,GENRE_TBL C " &
                        "WHERE C.ID = A.GENRE_ID AND B.FOLDER_ID = A.ID ORDER BY C.ID"
        Else
            sGenreQuery = "select distinct C.ID,C.TITLE from FOLDER_TBL A,FOLDER_TAG_TBL B,GENRE_TBL C " &
                        "WHERE C.ID = A.GENRE_ID AND B.FOLDER_ID = A.ID AND B.DATA like :FOLDER_NAME ORDER BY C.ID"
            cmd.Parameters.Clear()
            cmd.Parameters.Add(New OracleParameter("FOLDER_NAME", sCondition & "%"))
        End If

        cmd.CommandText = sGenreQuery

        reader = cmd.ExecuteReader()

        Return 0

    End Function

    'フォルダを取得
    'sCondition　フォルダタグテーブルのフォルダ名
    '条件なければフォルダテーブルのID,フォルダ名、ジャンルIDの一覧を取得
    '条件ある場合はフォルダタグテーブルのデータに一致するフォルダテーブルのID,フォルダ名、ジャンルIDの一覧を取得
    Public Function queryFolder(ByVal sCondition As String, ByRef reader As OracleDataReader) As Integer

        If (sCondition = "") Then

            sFolderQuery = "select distinct A.ID,A.TITLE,A.GENRE_ID from FOLDER_TBL A,FOLDER_TAG_TBL B " &
                                        "WHERE B.FOLDER_ID = A.ID order by A.GENRE_ID,A.TITLE"
        Else
            sFolderQuery = "select distinct A.ID,A.TITLE,A.GENRE_ID from FOLDER_TBL A,FOLDER_TAG_TBL B " &
                                        "WHERE B.FOLDER_ID = A.ID AND B.DATA like :FOLDER_NAME ORDER BY A.GENRE_ID,A.TITLE" ' ORDER BY C.ID"
            cmd.Parameters.Clear()
            cmd.Parameters.Add(New OracleParameter("FOLDER_NAME", sCondition & "%"))
        End If

        cmd.CommandText = sFolderQuery

        reader = cmd.ExecuteReader()

        Return 0

    End Function

    '子フォルダの一覧を取得
    'sCondition　フォルダテーブルの親フォルダID
    'フォルグテーブルのデータに一致するフォルダテーブルのID,フォルダ名の一覧を取得
    Public Function querySubFolder(ByVal sCondition As String, ByRef reader As OracleDataReader) As Integer

        Dim sSubFolderQuery As String = Nothing

        If (sCondition = "") Then
            MessageBox.Show("パラメタが不正です。")
            Return -1
        Else
            sSubFolderQuery = "select B.ID,B.TITLE from FOLDER_TBL A,FOLDER_TBL B " &
                            "WHERE B.PARENT_FOLDER_ID = A.ID AND B.PARENT_FOLDER_ID = :FOLDER_NAME " &
                            "ORDER BY B.TITLE"
            cmd.Parameters.Clear()
            cmd.Parameters.Add(New OracleParameter("FOLDER_NAME", sCondition))
        End If

        cmd.CommandText = sSubFolderQuery

        reader = cmd.ExecuteReader()

        Return 0

    End Function

    'フォルダ配下のファイルの一覧を取得
    'sCondition　フォルダID
    'フォルグテーブルのデータに一致するファイルテーブルのID,ファイル名、パス、サイズ、画像、ランク、追加日、フォルダIDの一覧を取得
    Public Function queryFileList(ByVal iCondition As Integer, ByRef reader As OracleDataReader) As Integer

        Dim sFileListQuery As String = Nothing

        'sFileListQuery = "SELECT A.ID, A.TITLE, C.PATH || '\' || B.PATH || '\' || A.PATH AS A ,A.FILE_SIZE " & _
        '            "FROM FILE_TBL A ,FOLDER_TBL B ,GENRE_TBL C " & _
        '            "WHERE B.GENRE_ID = C.ID AND A.FOLDER_ID = B.ID AND " & _
        '            "A.FOLDER_ID = " & iCondition & " " & _
        '            "UNION " & _
        '            "SELECT A.ID, A.TITLE, D.PATH || '\' || C.PATH || '\' || B.PATH || '\' || A.PATH AS A ,A.FILE_SIZE " & _
        '            "FROM FILE_TBL A ,FOLDER_TBL B ,FOLDER_TBL C ,GENRE_TBL D " & _
        '            "WHERE C.GENRE_ID = D.ID AND B.PARENT_FOLDER_ID = C.ID AND A.FOLDER_ID = B.ID AND " & _
        '            "A.FOLDER_ID = " & iCondition

        'BLOBでサムネイルを取得
        sFileListQuery = "SELECT A.ID, A.TITLE, C.PATH || '\' || B.PATH || '\' || A.PATH AS A ,A.FILE_SIZE ,A.THUMBNAIL ,A.RANK ,A.ADD_DATE ,A.FOLDER_ID " &
            "FROM FILE_TBL A ,FOLDER_TBL B ,GENRE_TBL C " &
            "WHERE B.GENRE_ID = C.ID AND A.FOLDER_ID = B.ID AND " &
            "A.FOLDER_ID = " & iCondition & " " &
            "UNION ALL " &
            "SELECT A.ID, A.TITLE, D.PATH || '\' || C.PATH || '\' || B.PATH || '\' || A.PATH AS A ,A.FILE_SIZE ,A.THUMBNAIL ,A.RANK ,A.ADD_DATE ,A.FOLDER_ID " &
            "FROM FILE_TBL A ,FOLDER_TBL B ,FOLDER_TBL C ,GENRE_TBL D " &
            "WHERE C.GENRE_ID = D.ID AND B.PARENT_FOLDER_ID = C.ID AND A.FOLDER_ID = B.ID AND " &
            "A.FOLDER_ID = " & iCondition & " ORDER BY TITLE "


        '全件取得
        'sFileListQuery = "SELECT A.ID, A.TITLE, C.PATH || '\' || B.PATH || '\' || A.PATH AS A ,A.FILE_SIZE ,A.THUMBNAIL " & _
        '    "FROM FILE_TBL A ,FOLDER_TBL B ,GENRE_TBL C " & _
        '    "WHERE B.GENRE_ID = C.ID AND A.FOLDER_ID = B.ID " & _
        '    "UNION ALL " & _
        '    "SELECT A.ID, A.TITLE, D.PATH || '\' || C.PATH || '\' || B.PATH || '\' || A.PATH AS A ,A.FILE_SIZE,A.THUMBNAIL " & _
        '    "FROM FILE_TBL A ,FOLDER_TBL B ,FOLDER_TBL C ,GENRE_TBL D " & _
        '    "WHERE C.GENRE_ID = D.ID AND B.PARENT_FOLDER_ID = C.ID AND A.FOLDER_ID = B.ID"

        cmd.CommandText = sFileListQuery

        reader = cmd.ExecuteReader()

        Return 0

    End Function


    'ファイルの一覧を取得
    'iKensakuKbn　検索区分（0：最終更新日順、1：フォルダ検索（ランクと一致）、2：フォルダ検索（ランク以下）
    'iRank　ランク
    'iFolderId　フォルダID
    '条件に一致するファイルテーブルのID,ファイル名、パス、サイズ、画像、ランク、追加日、フォルダIDの一覧を取得
    Public Function queryFileListKensaku(ByVal iKensakuKbn As Integer, ByVal iRank As Integer, ByVal iFolderId As Integer, ByRef reader As OracleDataReader) As Integer

        Dim sFileListQuery As String = Nothing

        Select Case iKensakuKbn

            Case 0
                '最新更新日　上位200件を取得
                sFileListQuery =
                    "SELECT * FROM (SELECT A.ID, A.TITLE, C.PATH || '\' || B.PATH || '\' || A.PATH AS A ,A.FILE_SIZE ,A.THUMBNAIL ,A.RANK ,A.ADD_DATE ,A.FOLDER_ID " &
                    "FROM FILE_TBL A ,FOLDER_TBL B ,GENRE_TBL C " &
                    "WHERE B.GENRE_ID = C.ID AND A.FOLDER_ID = B.ID " &
                    "AND A.RANK >= " & iRank & " " &
                    "UNION ALL " &
                    "SELECT A.ID, A.TITLE, D.PATH || '\' || C.PATH || '\' || B.PATH || '\' || A.PATH AS A ,A.FILE_SIZE ,A.THUMBNAIL ,A.RANK ,A.ADD_DATE ,A.FOLDER_ID " &
                    "FROM FILE_TBL A ,FOLDER_TBL B ,FOLDER_TBL C ,GENRE_TBL D " &
                    "WHERE C.GENRE_ID = D.ID AND B.PARENT_FOLDER_ID = C.ID AND A.FOLDER_ID = B.ID " &
                    "AND A.RANK >= " & iRank & " " &
                    "ORDER BY ADD_DATE DESC )WHERE ROWNUM <= 200"
            Case 1
                'フォルダ検索　ランクと一致
                sFileListQuery =
                    "SELECT A.ID, A.TITLE, C.PATH || '\' || B.PATH || '\' || A.PATH AS A ,A.FILE_SIZE ,A.THUMBNAIL ,A.RANK ,A.ADD_DATE ,A.FOLDER_ID " &
                    "FROM FILE_TBL A ,FOLDER_TBL B ,GENRE_TBL C " &
                    "WHERE B.GENRE_ID = C.ID AND A.FOLDER_ID = B.ID " &
                    "AND A.RANK = " & iRank & " " &
                    "AND (B.ID = " & iFolderId & " OR C.ID = " & iFolderId & ") " &
                    "UNION ALL " &
                    "SELECT A.ID, A.TITLE, D.PATH || '\' || C.PATH || '\' || B.PATH || '\' || A.PATH AS A ,A.FILE_SIZE ,A.THUMBNAIL ,A.RANK ,A.ADD_DATE ,A.FOLDER_ID " &
                    "FROM FILE_TBL A ,FOLDER_TBL B ,FOLDER_TBL C ,GENRE_TBL D " &
                    "WHERE C.GENRE_ID = D.ID AND B.PARENT_FOLDER_ID = C.ID AND A.FOLDER_ID = B.ID " &
                    "AND A.RANK = " & iRank & " " &
                    "AND (B.ID = " & iFolderId & " OR C.ID = " & iFolderId & " OR D.ID = " & iFolderId & ") " &
                    "ORDER BY RANK DESC"
            Case 2
                'フォルダ検索　ランク以下
                sFileListQuery =
                    "SELECT A.ID, A.TITLE, C.PATH || '\' || B.PATH || '\' || A.PATH AS A ,A.FILE_SIZE ,A.THUMBNAIL ,A.RANK ,A.ADD_DATE ,A.FOLDER_ID " &
                    "FROM FILE_TBL A ,FOLDER_TBL B ,GENRE_TBL C " &
                    "WHERE B.GENRE_ID = C.ID AND A.FOLDER_ID = B.ID " &
                    "AND A.RANK >= " & iRank & " " &
                    "AND (B.ID = " & iFolderId & " OR C.ID = " & iFolderId & ") " &
                    "UNION ALL " &
                    "SELECT A.ID, A.TITLE, D.PATH || '\' || C.PATH || '\' || B.PATH || '\' || A.PATH AS A ,A.FILE_SIZE ,A.THUMBNAIL ,A.RANK ,A.ADD_DATE ,A.FOLDER_ID " &
                    "FROM FILE_TBL A ,FOLDER_TBL B ,FOLDER_TBL C ,GENRE_TBL D " &
                    "WHERE C.GENRE_ID = D.ID AND B.PARENT_FOLDER_ID = C.ID AND A.FOLDER_ID = B.ID " &
                    "AND A.RANK >= " & iRank & " " &
                    "AND (B.ID = " & iFolderId & " OR C.ID = " & iFolderId & " OR D.ID = " & iFolderId & ") " &
                    "ORDER BY RANK DESC"
        End Select

        Console.WriteLine(sFileListQuery)
        cmd.CommandText = sFileListQuery

        reader = cmd.ExecuteReader()

        Return 0

    End Function

    'ファイルの一覧を取得
    'iKensakuKbn　検索区分（3：フォルダ検索（ランクと一致）、4：フォルダ検索（ランク以下）
    'iRank　ランク
    'sTagName　タグ名
    '条件に一致するファイルテーブルのID,ファイル名、パス、サイズ、画像、ランク、追加日、フォルダIDの一覧を取得
    Public Function queryFileListKensaku(ByVal iKensakuKbn As Integer, ByVal iRank As Integer, ByVal sTagName As String, ByRef reader As OracleDataReader) As Integer

        Dim sFileListQuery As String = Nothing

        Select Case iKensakuKbn

            Case 3
                'ファイルタグ、またはフォルダタグ、またはフォルダ名が前方一致、かつランクが一致した、ファイルIDを返却する
                sFileListQuery =
                    "WITH tmp AS( " &
                    "SELECT A.ID  " &
                    "FROM FILE_TBL A ,FILETAG_TBL E " &
                    "WHERE A.ID = E.FILE_ID " &
                    "AND (E.DATA LIKE '" & sTagName & "%') " &
                    "AND A.RANK = " & iRank & " " &
                    "UNION " &
                    "SELECT A.ID " &
                    "FROM FILE_TBL A ,FOLDER_TBL B ,FOLDER_TBL C ,FOLDER_TAG_TBL F " &
                    "WHERE B.PARENT_FOLDER_ID = C.ID AND A.FOLDER_ID = B.ID AND F.FOLDER_ID = C.ID " &
                    "AND (F.DATA LIKE '" & sTagName & "%' OR C.TITLE LIKE '" & sTagName & "%') " &
                    "AND A.RANK = " & iRank & " " &
                    "UNION " &
                    "SELECT A.ID " &
                    "FROM FILE_TBL A ,FOLDER_TBL B ,FOLDER_TAG_TBL F " &
                    "WHERE A.FOLDER_ID = B.ID AND F.FOLDER_ID = B.ID " &
                    "AND (F.DATA LIKE '" & sTagName & "%' OR B.TITLE LIKE '" & sTagName & "%') " &
                    "AND A.RANK = " & iRank & ") " &
                    "SELECT A.ID, A.TITLE, C.PATH || '\' || B.PATH || '\' || A.PATH AS A ,A.FILE_SIZE ,A.THUMBNAIL ,A.RANK ,A.ADD_DATE ,A.FOLDER_ID " &
                    "FROM FILE_TBL A ,FOLDER_TBL B ,GENRE_TBL C ,tmp T " &
                    "WHERE B.GENRE_ID = C.ID AND A.FOLDER_ID = B.ID AND A.ID IN T.ID " &
                    "UNION ALL " &
                    "SELECT A.ID, A.TITLE, D.PATH || '\' || C.PATH || '\' || B.PATH || '\' || A.PATH AS A ,A.FILE_SIZE ,A.THUMBNAIL ,A.RANK ,A.ADD_DATE ,A.FOLDER_ID " &
                    "FROM FILE_TBL A ,FOLDER_TBL B ,FOLDER_TBL C ,GENRE_TBL D ,tmp T " &
                    "WHERE C.GENRE_ID = D.ID AND B.PARENT_FOLDER_ID = C.ID AND A.FOLDER_ID = B.ID AND A.ID IN T.ID " &
                    "ORDER BY RANK DESC"
            Case 4
                'ファイルタグ、またはフォルダタグ、またはフォルダ名が前方一致、かつランクがパラメータ以下の、ファイルIDを返却する
                sFileListQuery =
                    "WITH tmp AS( " &
                    "SELECT A.ID  " &
                    "FROM FILE_TBL A ,FILETAG_TBL E " &
                    "WHERE A.ID = E.FILE_ID " &
                    "AND (E.DATA LIKE '" & sTagName & "%') " &
                    "AND A.RANK >= " & iRank & " " &
                    "UNION " &
                    "SELECT A.ID " &
                    "FROM FILE_TBL A ,FOLDER_TBL B ,FOLDER_TBL C ,FOLDER_TAG_TBL F " &
                    "WHERE B.PARENT_FOLDER_ID = C.ID AND A.FOLDER_ID = B.ID AND F.FOLDER_ID = C.ID " &
                    "AND (F.DATA LIKE '" & sTagName & "%' OR C.TITLE LIKE '" & sTagName & "%') " &
                    "AND A.RANK >= " & iRank & " " &
                    "UNION " &
                    "SELECT A.ID " &
                    "FROM FILE_TBL A ,FOLDER_TBL B ,FOLDER_TAG_TBL F " &
                    "WHERE A.FOLDER_ID = B.ID AND F.FOLDER_ID = B.ID " &
                    "AND (F.DATA LIKE '" & sTagName & "%' OR B.TITLE LIKE '" & sTagName & "%') " &
                    "AND A.RANK >= " & iRank & ") " &
                    "SELECT A.ID, A.TITLE, C.PATH || '\' || B.PATH || '\' || A.PATH AS A ,A.FILE_SIZE ,A.THUMBNAIL ,A.RANK ,A.ADD_DATE ,A.FOLDER_ID " &
                    "FROM FILE_TBL A ,FOLDER_TBL B ,GENRE_TBL C ,tmp T " &
                    "WHERE B.GENRE_ID = C.ID AND A.FOLDER_ID = B.ID AND A.ID IN T.ID " &
                    "UNION ALL " &
                    "SELECT A.ID, A.TITLE, D.PATH || '\' || C.PATH || '\' || B.PATH || '\' || A.PATH AS A ,A.FILE_SIZE ,A.THUMBNAIL ,A.RANK ,A.ADD_DATE ,A.FOLDER_ID " &
                    "FROM FILE_TBL A ,FOLDER_TBL B ,FOLDER_TBL C ,GENRE_TBL D ,tmp T " &
                    "WHERE C.GENRE_ID = D.ID AND B.PARENT_FOLDER_ID = C.ID AND A.FOLDER_ID = B.ID AND A.ID IN T.ID " &
                    "ORDER BY RANK DESC"
        End Select

        Console.WriteLine(sFileListQuery)
        cmd.CommandText = sFileListQuery

        reader = cmd.ExecuteReader()

        Return 0

    End Function

    'パスからファイルを取得
    'sPath　パス
    '条件に一致するファイルテーブルのID,ファイル名、パス、サイズ、画像、ランク、追加日、フォルダIDの一覧を取得
    Public Function queryFilePath(ByVal sPath As String, ByRef reader As OracleDataReader) As Integer

        Dim sFilePathQuery As String = Nothing

        sFilePathQuery = "SELECT A.ID, A.TITLE, C.PATH || '\' || B.PATH || '\' || A.PATH AS A ,A.FILE_SIZE ,A.THUMBNAIL ,A.RANK ,A.FOLDER_ID ,B.TITLE AS NOW_FOLDER, B.TITLE AS PARENT_TITLE " &
                            "FROM FILE_TBL A ,FOLDER_TBL B ,GENRE_TBL C " &
                            "WHERE B.GENRE_ID = C.ID And A.FOLDER_ID = B.ID And " &
                            "C.PATH || '\' || B.PATH || '\' || A.PATH  = ':FILEPATH' " &
                        "UNION ALL " &
                        "SELECT A.ID, A.TITLE, D.PATH || '\' || C.PATH || '\' || B.PATH || '\' || A.PATH AS A ,A.FILE_SIZE ,A.THUMBNAIL ,A.RANK ,A.FOLDER_ID ,B.TITLE AS NOW_FOLDER ,C.TITLE AS PARENT_TITLE " &
                            "FROM FILE_TBL A ,FOLDER_TBL B ,FOLDER_TBL C ,GENRE_TBL D " &
                            "WHERE C.GENRE_ID = D.ID AND B.PARENT_FOLDER_ID = C.ID AND A.FOLDER_ID = B.ID AND " &
                             "D.PATH || '\' || C.PATH || '\' || B.PATH || '\' || A.PATH = ':FILEPATH'"

        cmd.CommandText = sFilePathQuery
        cmd.Parameters.Clear()
        cmd.Parameters.Add(New OracleParameter("FILEPATH", sPath))
        reader = cmd.ExecuteReader()

        Return 0

    End Function

    'フォルダIDからフォルダのパスを取得
    'iFolderId フォルダID
    '条件に一致するフォルダテーブル、ジャンルテーブルのパスを取得
    'フォルダIDが10以下の場合、ジャンルテーブルのパスを取得
    Public Function queryFolderPath(ByVal iFolderId As Integer) As String

        Dim sFolderPathQuery As String = Nothing

        If (iFolderId > 10) Then
            sFolderPathQuery = "SELECT C.PATH || '\' || B.PATH || '\' AS A " &
                        "FROM FOLDER_TBL B ,GENRE_TBL C " &
                        "WHERE B.GENRE_ID = C.ID AND " &
                        "B.ID = " & iFolderId & " " &
                        "UNION " &
                        "SELECT D.PATH || '\' || C.PATH || '\' || B.PATH || '\' AS A " &
                        "FROM FOLDER_TBL B ,FOLDER_TBL C ,GENRE_TBL D " &
                        "WHERE C.GENRE_ID = D.ID AND B.PARENT_FOLDER_ID = C.ID AND " &
                        "B.ID = " & iFolderId
        Else
            sFolderPathQuery = "SELECT PATH || '\' AS A FROM GENRE_TBL WHERE ID = " & iFolderId
        End If
        cmd.CommandText = sFolderPathQuery

        Return cmd.ExecuteScalar()

    End Function

    'ファイルIDから、ファイルタグの一覧を取得
    'iFileId ファイルID
    '条件に一致するファイルタグテーブルのカテゴリ、タグ名を取得
    Public Function queryFileTag(ByVal iFileId As Integer, ByRef reader As OracleDataReader) As String

        Dim syFileTagQuery As String = Nothing


        syFileTagQuery = "SELECT CATEGORY,DATA " &
                    "FROM FILETAG_TBL " &
                    "WHERE FILE_ID = " & iFileId
        cmd.CommandText = syFileTagQuery
        reader = cmd.ExecuteReader()
        Return 0

    End Function

    'ファイルタグ名に一番多く登録されているカテゴリを取得
    'sTagName　ファイルタグ名
    'ファイルタグ名からファイルタグ名を検索し、最大のカテゴリを返却
    Public Function queryDefaultCategory(ByVal sTagName As String) As Integer

        Dim sDefaultCategoryQuery As String = Nothing


        sDefaultCategoryQuery = "SELECT * FROM (SELECT category FROM filetag_tbl " &
            "WHERE data = :TAG_NAME " &
            "GROUP BY category ORDER BY COUNT(category) DESC) WHERE rownum = 1"

        cmd.Parameters.Clear()
        cmd.Parameters.Add(New OracleParameter("TAG_NAME", sTagName))
        cmd.CommandText = sDefaultCategoryQuery

        Return cmd.ExecuteScalar()

    End Function

    'ファイルの追加
    Public Function insertFile(ByVal sTitle As String, ByVal sPath As String, ByVal iFolderId As Integer, ByVal lFileSize As Long, ByVal iRank As Integer, ByRef imgPic As Image) As Integer

        Dim sFileQuery As String = Nothing
        Dim sIdSeqQuery As String = Nothing

        sIdSeqQuery = "SELECT FILE_TBL_SEQ.nextval AS ID FROM dual"
        cmd.CommandText = sIdSeqQuery
        iId = CInt(cmd.ExecuteScalar)

        sFileQuery = "INSERT INTO file_tbl VALUES (:FILEID,:TITLE,:PATH,:FOLDER_ID,:FILE_SIZE,:RANK,sysdate,:THUMBNAIL)"
        cmd.CommandText = sFileQuery
        cmd.Parameters.Clear()
        cmd.Parameters.Add(New OracleParameter("FILEID", iId))
        cmd.Parameters.Add(New OracleParameter("TITLE", sTitle))
        cmd.Parameters.Add(New OracleParameter("PATH", sPath))
        cmd.Parameters.Add(New OracleParameter("FOLDER_ID", iFolderId))
        cmd.Parameters.Add(New OracleParameter("FILE_SIZE", lFileSize))
        cmd.Parameters.Add(New OracleParameter("RANK", iRank))
        Dim pBlob As OracleParameter =
                cmd.Parameters.Add("THUMBNAIL", OracleDbType.Blob)
        Dim imgconv As New ImageConverter()
        Dim b As Byte() =
            CType(imgconv.ConvertTo(imgPic, GetType(Byte())), Byte())
        pBlob.Value = b

        cmd.ExecuteNonQuery()

        Return 0

    End Function

    'ファイルの更新
    'iFileId　ファイルID
    'iFolderId フォルダID
    'stitle　ファイル名
    'sPath　パス
    'iRank ランク
    'imgPic　サムネイル
    Public Function updateFile(ByVal iFileId As Integer, ByVal iFolderId As Integer, ByVal stitle As String, ByVal sPath As String, ByVal iRank As Integer, ByRef imgPic As Image) As Integer

        Dim sFileQuery As String = Nothing

        sFileQuery = "UPDATE file_tbl SET folder_id = :FOLDER_ID,title = :TITLE,path = :PATH,rank=:RANK,thumbnail = :THUMBNAIL where id = :FILEID"
        cmd.CommandText = sFileQuery
        cmd.Parameters.Clear()

        iId = iFileId
        cmd.Parameters.Add(New OracleParameter("FOLDER_ID", iFolderId))
        cmd.Parameters.Add(New OracleParameter("TITLE", stitle))
        cmd.Parameters.Add(New OracleParameter("PATH", sPath))
        cmd.Parameters.Add(New OracleParameter("RANK", iRank))
        Dim pBlob As OracleParameter =
        cmd.Parameters.Add("THUMBNAIL", OracleDbType.Blob)
        Dim imgconv As New ImageConverter()
        Dim b As Byte() =
            CType(imgconv.ConvertTo(imgPic, GetType(Byte())), Byte())
        pBlob.Value = b
        cmd.Parameters.Add(New OracleParameter("FILEID", iId))
        cmd.ExecuteNonQuery()

        b = Nothing
        imgconv = Nothing

        Return 0

    End Function


    'ファイルの更新（サムネイル）
    'iId　ファイルID
    'imgPic　サムネイル
    Public Function updateFile(ByVal iId As Integer, ByRef imgPic As Image) As Integer

        Dim sFileQuery As String = Nothing

        sFileQuery = "UPDATE file_tbl SET thumbnail = :THUMBNAIL where id = :FILEID"
        cmd.CommandText = sFileQuery
        cmd.Parameters.Clear()

        Dim pBlob As OracleParameter =
        cmd.Parameters.Add("THUMBNAIL", OracleDbType.Blob)
        Dim imgconv As New ImageConverter()
        Dim b As Byte() =
            CType(imgconv.ConvertTo(imgPic, GetType(Byte())), Byte())
        pBlob.Value = b
        cmd.Parameters.Add(New OracleParameter("FILEID", iId))
        cmd.ExecuteNonQuery()

        Return 0

    End Function

    'ファイルの削除。削除するファイルのファイルタグも削除する
    'iFileId　ファイルID
    Public Function deleteFile(ByVal iFileId As Integer) As String

        Dim sFileQuery As String = Nothing
        Dim sFileTagQuery As String = Nothing

        sFileQuery = "DELETE file_tbl where id = :FILEID"
        sFileTagQuery = "DELETE filetag_tbl where file_id = :FILEID"


        cmd.Parameters.Clear()
        cmd.CommandText = sFileTagQuery
        cmd.Parameters.Add(New OracleParameter("FILEID", iFileId))
        cmd.ExecuteNonQuery()

        cmd.CommandText = sFileQuery
        cmd.ExecuteNonQuery()

        Return 0

    End Function

    'ファイルタグの追加
    'iCategory カテゴリ
    'sData 　ファイルタグ名
    'idはグローバル変数を利用（ファイルID)
    Public Function insertFileTag(ByVal iCategory As Integer, ByVal sData As String) As Integer

        Dim sFileTagQuery As String = Nothing

        sFileTagQuery = "INSERT INTO filetag_tbl VALUES (file_tag_seq.nextval,:FILE_ID,:CATEGORY,:FILE_DATA)"
        cmd.CommandText = sFileTagQuery
        cmd.Parameters.Clear()
        cmd.Parameters.Add(New OracleParameter("FILE_ID", iId))
        cmd.Parameters.Add(New OracleParameter("CATEGORY", iCategory))
        cmd.Parameters.Add(New OracleParameter("FILE_DATA", sData))

        cmd.ExecuteNonQuery()

        Return 0

    End Function

    'ファイルIDからファイルタグの削除
    'iFileId ファイルID
    Public Function deleteFileTag(ByVal iFileId As Integer) As String

        Dim sFileTagQuery As String = Nothing
        sFileTagQuery = "DELETE filetag_tbl where file_id = :FILEID"

        cmd.Parameters.Clear()
        cmd.CommandText = sFileTagQuery
        cmd.Parameters.Add(New OracleParameter("FILEID", iFileId))
        cmd.ExecuteNonQuery()
        Return 0

    End Function

    'フォルダの追加。フォルダタグテーブルへの追加もする。
    'iParentFolderId 親フォルダID
    'sName　フォルダ名
    Public Function insertFolder(ByVal iParentFolderId As Integer, ByVal sName As String) As Integer

        Dim sIdSeqQuery As String = "SELECT folder_tbl_seq.nextval AS ID FROM dual"
        cmd.CommandText = sIdSeqQuery
        Dim iFolderId As Integer = CInt(cmd.ExecuteScalar)

        If (iParentFolderId > 10) Then
            Dim sFoldergQuery As String = "INSERT INTO folder_tbl VALUES (:ID,:FOLDER_NAME,:PATH,:GENRE_ID,:PARENT_FOLDER)"
            cmd.CommandText = sFoldergQuery
            cmd.Parameters.Clear()
            cmd.Parameters.Add(New OracleParameter("ID", iFolderId))
            cmd.Parameters.Add(New OracleParameter("FOLDER_NAME", sName))
            cmd.Parameters.Add(New OracleParameter("PATH", sName))
            cmd.Parameters.Add(New OracleParameter("GENRE_ID", ""))
            cmd.Parameters.Add(New OracleParameter("PARENT_FOLDER", iParentFolderId))
            cmd.ExecuteNonQuery()
        Else

            Dim sFoldergQuery As String = "INSERT INTO folder_tbl VALUES (:ID,:FOLDER_NAME,:PATH,:GENRE_ID,:PARENT_FOLDER)"
            cmd.CommandText = sFoldergQuery
            cmd.Parameters.Clear()
            cmd.Parameters.Add(New OracleParameter("ID", iFolderId))
            cmd.Parameters.Add(New OracleParameter("FOLDER_NAME", sName))
            cmd.Parameters.Add(New OracleParameter("PATH", sName))
            cmd.Parameters.Add(New OracleParameter("GENRE_ID", iParentFolderId))
            cmd.Parameters.Add(New OracleParameter("PARENT_FOLDER", ""))
            cmd.ExecuteNonQuery()

            Dim sFoldergTagQuery As String = "INSERT INTO folder_tag_tbl VALUES (folder_tag_seq.nextval,:FOLDER_ID,1,:DATA_VALUE,'')"
            cmd.CommandText = sFoldergTagQuery
            cmd.Parameters.Clear()
            cmd.Parameters.Add(New OracleParameter("FOLDER_ID", iFolderId))
            cmd.Parameters.Add(New OracleParameter("DATA_VALUE", sName))
            cmd.ExecuteNonQuery()
        End If
        Return iFolderId

    End Function

    'フォルダの削除。フォルダタグテーブルの削除もする
    'iFolderId　フォルダID
    Public Function deleteFolder(ByVal iFolderId As Integer) As Integer

        Dim sFolderTagQuery As String = Nothing
        Dim sFolderQuery As String = Nothing

        Dim txn As OracleTransaction = cnn.BeginTransaction()

        sFolderTagQuery = "DELETE FROM folder_tag_tbl WHERE FOLDER_ID = " & iFolderId
        cmd.CommandText = sFolderTagQuery
        cmd.ExecuteNonQuery()
        sFolderQuery = "DELETE FROM FOLDER_TBL WHERE ID = " & iFolderId
        cmd.CommandText = sFolderQuery
        cmd.ExecuteNonQuery()
        txn.Commit()
        txn.Dispose()

        Return 0
    End Function

    'フォルダタグの追加（カテゴリがなぜか1固定）
    'iFolderId　ファルダID
    'sName　フォルダタグ名
    Public Function insertFolderTag(ByVal iFolderId As Integer, ByVal sName As String) As Integer

        Dim sFoldergTagQuery As String = "INSERT INTO folder_tag_tbl VALUES (folder_tag_seq.nextval,:FOLDER_ID,1,:DATA_VALUE,'')"
        cmd.CommandText = sFoldergTagQuery
        cmd.Parameters.Clear()
        cmd.Parameters.Add(New OracleParameter("FOLDER_ID", iFolderId))
        cmd.Parameters.Add(New OracleParameter("DATA_VALUE", sName))
        cmd.ExecuteNonQuery()

        Return 0
    End Function


    Protected Overrides Sub Finalize()
        cmd = Nothing
        cnn.Close()
        MyBase.Finalize()
    End Sub
End Class
