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

    Public Function queryDropList(ByRef reader As OracleDataReader) As Integer

        Dim sDropListQuery As String = Nothing

        sDropListQuery = "select FLAG_01,KOBETU_01 from SYSTEM_KANRI_MST " & _
                        "where KANRI_CODE='SE' AND KANRI_PG = 'FILE_CATEGORY' order by KANRI_KBN"

        cmd.CommandText = sDropListQuery

        reader = cmd.ExecuteReader()

        Return 0

    End Function

    Public Function queryGenre(ByVal sCondition As String, ByRef reader As OracleDataReader) As Integer

        If (sCondition = "") Then

            sGenreQuery = "select distinct C.ID,C.TITLE from FOLDER_TBL A,FOLDER_TAG_TBL B,GENRE_TBL C " & _
                        "WHERE C.ID = A.GENRE_ID AND B.FOLDER_ID = A.ID ORDER BY C.ID"
        Else
            sGenreQuery = "select distinct C.ID,C.TITLE from FOLDER_TBL A,FOLDER_TAG_TBL B,GENRE_TBL C " & _
                        "WHERE C.ID = A.GENRE_ID AND B.FOLDER_ID = A.ID AND B.DATA like :FOLDER_NAME ORDER BY C.ID"
            cmd.Parameters.Clear()
            cmd.Parameters.Add(New OracleParameter("FOLDER_NAME", sCondition & "%"))
        End If

        cmd.CommandText = sGenreQuery

        reader = cmd.ExecuteReader()

        Return 0

    End Function

    Public Function queryFolder(ByVal sCondition As String, ByRef reader As OracleDataReader) As Integer

        If (sCondition = "") Then

            sFolderQuery = "select distinct A.ID,A.TITLE,A.GENRE_ID from FOLDER_TBL A,FOLDER_TAG_TBL B " & _
                                        "WHERE B.FOLDER_ID = A.ID order by A.GENRE_ID,A.TITLE"
        Else
            sFolderQuery = "select distinct A.ID,A.TITLE,A.GENRE_ID from FOLDER_TBL A,FOLDER_TAG_TBL B " & _
                                        "WHERE B.FOLDER_ID = A.ID AND B.DATA like :FOLDER_NAME ORDER BY A.GENRE_ID,A.TITLE" ' ORDER BY C.ID"
            cmd.Parameters.Clear()
            cmd.Parameters.Add(New OracleParameter("FOLDER_NAME", sCondition & "%"))
        End If

        cmd.CommandText = sFolderQuery

        reader = cmd.ExecuteReader()

        Return 0

    End Function

    Public Function querySubFolder(ByVal sCondition As String, ByRef reader As OracleDataReader) As Integer

        Dim sSubFolderQuery As String = Nothing

        If (sCondition = "") Then
            MessageBox.Show("パラメタが不正です。")
            Return -1
        Else
            sSubFolderQuery = "select B.ID,B.TITLE from FOLDER_TBL A,FOLDER_TBL B " & _
                            "WHERE B.PARENT_FOLDER_ID = A.ID AND B.PARENT_FOLDER_ID = :FOLDER_NAME " & _
                            "ORDER BY B.TITLE"
            cmd.Parameters.Clear()
            cmd.Parameters.Add(New OracleParameter("FOLDER_NAME", sCondition))
        End If

        cmd.CommandText = sSubFolderQuery

        reader = cmd.ExecuteReader()

        Return 0

    End Function

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
        sFileListQuery = "SELECT A.ID, A.TITLE, C.PATH || '\' || B.PATH || '\' || A.PATH AS A ,A.FILE_SIZE ,A.THUMBNAIL ,A.RANK ,A.ADD_DATE ,A.FOLDER_ID " & _
            "FROM FILE_TBL A ,FOLDER_TBL B ,GENRE_TBL C " & _
            "WHERE B.GENRE_ID = C.ID AND A.FOLDER_ID = B.ID AND " & _
            "A.FOLDER_ID = " & iCondition & " " & _
            "UNION ALL " & _
            "SELECT A.ID, A.TITLE, D.PATH || '\' || C.PATH || '\' || B.PATH || '\' || A.PATH AS A ,A.FILE_SIZE ,A.THUMBNAIL ,A.RANK ,A.ADD_DATE ,A.FOLDER_ID " & _
            "FROM FILE_TBL A ,FOLDER_TBL B ,FOLDER_TBL C ,GENRE_TBL D " & _
            "WHERE C.GENRE_ID = D.ID AND B.PARENT_FOLDER_ID = C.ID AND A.FOLDER_ID = B.ID AND " & _
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


    Public Function queryFileListKensaku(ByVal iKensakuKbn As Integer, ByVal iRank As Integer, ByVal iFolderId As Integer, ByRef reader As OracleDataReader) As Integer

        Dim sFileListQuery As String = Nothing

        Select Case iKensakuKbn

            Case 0
                '最新更新日　上位100件を取得
                sFileListQuery = _
                    "SELECT * FROM (SELECT A.ID, A.TITLE, C.PATH || '\' || B.PATH || '\' || A.PATH AS A ,A.FILE_SIZE ,A.THUMBNAIL ,A.RANK ,A.ADD_DATE ,A.FOLDER_ID " & _
                    "FROM FILE_TBL A ,FOLDER_TBL B ,GENRE_TBL C " & _
                    "WHERE B.GENRE_ID = C.ID AND A.FOLDER_ID = B.ID " & _
                    "UNION ALL " & _
                    "SELECT A.ID, A.TITLE, D.PATH || '\' || C.PATH || '\' || B.PATH || '\' || A.PATH AS A ,A.FILE_SIZE ,A.THUMBNAIL ,A.RANK ,A.ADD_DATE ,A.FOLDER_ID " & _
                    "FROM FILE_TBL A ,FOLDER_TBL B ,FOLDER_TBL C ,GENRE_TBL D " & _
                    "WHERE C.GENRE_ID = D.ID AND B.PARENT_FOLDER_ID = C.ID AND A.FOLDER_ID = B.ID " & _
                    "ORDER BY ADD_DATE DESC )WHERE ROWNUM <= 100"
            Case 1
                '最新更新日　上位100件を取得
                sFileListQuery = _
                    "SELECT A.ID, A.TITLE, C.PATH || '\' || B.PATH || '\' || A.PATH AS A ,A.FILE_SIZE ,A.THUMBNAIL ,A.RANK ,A.ADD_DATE ,A.FOLDER_ID " & _
                    "FROM FILE_TBL A ,FOLDER_TBL B ,GENRE_TBL C " & _
                    "WHERE B.GENRE_ID = C.ID AND A.FOLDER_ID = B.ID " & _
                    "AND A.RANK = " & iRank & " " & _
                    "AND (B.ID = " & iFolderId & " OR C.ID = " & iFolderId & ") " & _
                    "UNION ALL " & _
                    "SELECT A.ID, A.TITLE, D.PATH || '\' || C.PATH || '\' || B.PATH || '\' || A.PATH AS A ,A.FILE_SIZE ,A.THUMBNAIL ,A.RANK ,A.ADD_DATE ,A.FOLDER_ID " & _
                    "FROM FILE_TBL A ,FOLDER_TBL B ,FOLDER_TBL C ,GENRE_TBL D " & _
                    "WHERE C.GENRE_ID = D.ID AND B.PARENT_FOLDER_ID = C.ID AND A.FOLDER_ID = B.ID " & _
                    "AND A.RANK = " & iRank & " " & _
                    "AND (B.ID = " & iFolderId & " OR C.ID = " & iFolderId & " OR D.ID = " & iFolderId & ") " & _
                    "ORDER BY RANK DESC"
            Case 2
                '最新更新日　上位100件を取得
                sFileListQuery = _
                    "SELECT A.ID, A.TITLE, C.PATH || '\' || B.PATH || '\' || A.PATH AS A ,A.FILE_SIZE ,A.THUMBNAIL ,A.RANK ,A.ADD_DATE ,A.FOLDER_ID " & _
                    "FROM FILE_TBL A ,FOLDER_TBL B ,GENRE_TBL C " & _
                    "WHERE B.GENRE_ID = C.ID AND A.FOLDER_ID = B.ID " & _
                    "AND A.RANK >= " & iRank & " " & _
                    "AND (B.ID = " & iFolderId & " OR C.ID = " & iFolderId & ") " & _
                    "UNION ALL " & _
                    "SELECT A.ID, A.TITLE, D.PATH || '\' || C.PATH || '\' || B.PATH || '\' || A.PATH AS A ,A.FILE_SIZE ,A.THUMBNAIL ,A.RANK ,A.ADD_DATE ,A.FOLDER_ID " & _
                    "FROM FILE_TBL A ,FOLDER_TBL B ,FOLDER_TBL C ,GENRE_TBL D " & _
                    "WHERE C.GENRE_ID = D.ID AND B.PARENT_FOLDER_ID = C.ID AND A.FOLDER_ID = B.ID " & _
                    "AND A.RANK >= " & iRank & " " & _
                    "AND (B.ID = " & iFolderId & " OR C.ID = " & iFolderId & " OR D.ID = " & iFolderId & ") " & _
                    "ORDER BY RANK DESC"
        End Select

        cmd.CommandText = sFileListQuery

        reader = cmd.ExecuteReader()

        Return 0

    End Function

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

    Public Function queryFolderPath(ByVal iFolderId As Integer) As String

        Dim sFolderPathQuery As String = Nothing

        If (iFolderId > 10) Then
            sFolderPathQuery = "SELECT C.PATH || '\' || B.PATH || '\' AS A " & _
                        "FROM FOLDER_TBL B ,GENRE_TBL C " & _
                        "WHERE B.GENRE_ID = C.ID AND " & _
                        "B.ID = " & iFolderId & " " & _
                        "UNION " & _
                        "SELECT D.PATH || '\' || C.PATH || '\' || B.PATH || '\' AS A " & _
                        "FROM FOLDER_TBL B ,FOLDER_TBL C ,GENRE_TBL D " & _
                        "WHERE C.GENRE_ID = D.ID AND B.PARENT_FOLDER_ID = C.ID AND " & _
                        "B.ID = " & iFolderId
        Else
            sFolderPathQuery = "SELECT PATH || '\' AS A FROM GENRE_TBL WHERE ID = " & iFolderId
        End If
        cmd.CommandText = sFolderPathQuery

        Return cmd.ExecuteScalar()

    End Function

    Public Function queryFileTag(ByVal iFileId As Integer, ByRef reader As OracleDataReader) As String

        Dim syFileTagQuery As String = Nothing


        syFileTagQuery = "SELECT CATEGORY,DATA " & _
                    "FROM FILETAG_TBL " & _
                    "WHERE FILE_ID = " & iFileId
        cmd.CommandText = syFileTagQuery
        reader = cmd.ExecuteReader()
        Return 0

    End Function


    Public Function queryDefaultCategory(ByVal sCategory As String) As Integer

        Dim sDefaultCategoryQuery As String = Nothing


        sDefaultCategoryQuery = "SELECT * FROM (SELECT category FROM filetag_tbl " & _
            "WHERE data = :CATEGORY_NAME " & _
            "GROUP BY category ORDER BY COUNT(category) DESC) WHERE rownum = 1"

        cmd.Parameters.Clear()
        cmd.Parameters.Add(New OracleParameter("CATEGORY_NAME", sCategory))
        cmd.CommandText = sDefaultCategoryQuery

        Return cmd.ExecuteScalar()

    End Function

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
        Dim pBlob As OracleParameter = _
                cmd.Parameters.Add("THUMBNAIL", OracleDbType.Blob)
        Dim imgconv As New ImageConverter()
        Dim b As Byte() = _
            CType(imgconv.ConvertTo(imgPic, GetType(Byte())), Byte())
        pBlob.Value = b

        cmd.ExecuteNonQuery()

        Return 0

    End Function

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
        Dim pBlob As OracleParameter = _
        cmd.Parameters.Add("THUMBNAIL", OracleDbType.Blob)
        Dim imgconv As New ImageConverter()
        Dim b As Byte() = _
            CType(imgconv.ConvertTo(imgPic, GetType(Byte())), Byte())
        pBlob.Value = b
        cmd.Parameters.Add(New OracleParameter("FILEID", iId))
        cmd.ExecuteNonQuery()

        b = Nothing
        imgconv = Nothing

        Return 0

    End Function

    Public Function updateFile(ByVal iId As Integer, ByRef imgPic As Image) As Integer

        Dim sFileQuery As String = Nothing

        sFileQuery = "UPDATE file_tbl SET thumbnail = :THUMBNAIL where id = :FILEID"
        cmd.CommandText = sFileQuery
        cmd.Parameters.Clear()

        Dim pBlob As OracleParameter = _
        cmd.Parameters.Add("THUMBNAIL", OracleDbType.Blob)
        Dim imgconv As New ImageConverter()
        Dim b As Byte() = _
            CType(imgconv.ConvertTo(imgPic, GetType(Byte())), Byte())
        pBlob.Value = b
        cmd.Parameters.Add(New OracleParameter("FILEID", iId))
        cmd.ExecuteNonQuery()

        Return 0

    End Function

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


    Public Function deleteFileTag(ByVal iFileId As Integer) As String

        Dim sFileTagQuery As String = Nothing
        sFileTagQuery = "DELETE filetag_tbl where file_id = :FILEID"

        cmd.Parameters.Clear()
        cmd.CommandText = sFileTagQuery
        cmd.Parameters.Add(New OracleParameter("FILEID", iFileId))
        cmd.ExecuteNonQuery()
        Return 0

    End Function

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
