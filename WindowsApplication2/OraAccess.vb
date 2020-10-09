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
        cnn.ConnectionString =
        "User Id=scott; Password=tiger; Data Source=orcl"
        cnn.Open()
        cmd.Connection = cnn
    End Sub



    '**************************************************************
    '********************** 　　　参照 　　　********************** 
    '**************************************************************

    'システム管理マスタからタグのジャンル用プルダウンリスト取得
    Public Function queryDropList(ByRef reader As OracleDataReader) As Integer

        Dim sDropListQuery As String = Nothing

        sDropListQuery = "select FLAG_01,KOBETU_01 from SYSTEM_KANRI_MST " &
                        "where KANRI_CODE='SE' AND KANRI_PG = 'FILE_CATEGORY' order by KANRI_KBN"

        cmd.CommandText = sDropListQuery

        reader = cmd.ExecuteReader()

        Return 0

    End Function


    'ファイルタグIDを取得
    'iCategory ファイルタグテーブルのカテゴリ
    'sData  ファイルタグテーブルのファイルタグ名
    'ファイルタグテーブルを参照し、パラメータで取得したカテゴリ、タイトルを条件に、ファイルタグIDを取得
    Public Function queryFileTagID(ByVal iCategory As Integer, ByVal sData As String) As Integer

        Dim sFileTagSelectSql As String = Nothing
        Dim iFileTagId As Integer

        'パラメータで取得したカテゴリ、タイトルを条件に、ファイルタグテーブルを参照しファイルタグIDを取得
        sFileTagSelectSql = "select FILE_TAG_ID from FILE_TAG_TBL " &
        "where FILE_TAG_NAME =:FILE_TAG_NAME and CATEGORY = :CATEGORY and rownum = 1 " &
        "order by FILE_TAG_ID "

        cmd.CommandText = sFileTagSelectSql
        cmd.Parameters.Clear()
        cmd.Parameters.Add(New OracleParameter("FILE_TAG_NAME", sData))
        cmd.Parameters.Add(New OracleParameter("CATEGORY", iCategory))

        If (Integer.TryParse(cmd.ExecuteScalar(), iFileTagId)) Then
            Return iFileTagId
        Else
            Return -1
        End If

    End Function


    'ファイルタグ紐づけIDを取得
    'iFileTagId ファイルタグID
    'iFileId  ファイルID
    'ファイルタグ紐づけテーブルを参照し、パラメータで取得したファイルタグID、ファイルIDを条件に、ファイルタグIDを取得
    Public Function queryFileTagHimoID(ByVal iFileTagId As Integer, ByVal iFileFolderId As Integer, ByVal iFile_Kbn As Integer) As Integer

        Dim sFileTagHimoSelectSql As String = Nothing
        Dim iFileTagHimoId As Integer

        cmd.Parameters.Clear()
        cmd.Parameters.Add(New OracleParameter("TAG_ID", iFileTagId))

        Select Case iFile_Kbn
            Case 1
                'パラメータで取得したカテゴリ、タイトルを条件に、ファイルタグテーブルを参照しファイルタグIDを取得
                sFileTagHimoSelectSql = "select HIMO_ID from FILE_TAG_HIMO_TBL " &
                                "where TAG_ID =:TAG_ID and FILE_ID = :FILE_ID and FILE_KBN = 1 and rownum = 1 " &
                                "order by HIMO_ID "
                cmd.Parameters.Add(New OracleParameter("FILE_ID", iFileFolderId))
            Case 2
                'パラメータで取得したカテゴリ、タイトルを条件に、ファイルタグテーブルを参照しファイルタグIDを取得
                sFileTagHimoSelectSql = "select HIMO_ID from FILE_TAG_HIMO_TBL " &
                                "where TAG_ID =:TAG_ID and FOLDER_ID = :FOLDER_ID and FILE_KBN = 2 and rownum = 1 " &
                                "order by HIMO_ID "
                cmd.Parameters.Add(New OracleParameter("FOLDER_ID", iFileFolderId))
        End Select


        cmd.CommandText = sFileTagHimoSelectSql

        If (Integer.TryParse(cmd.ExecuteScalar(), iFileTagHimoId)) Then
            Return iFileTagHimoId
        Else
            Return -1
        End If

    End Function


    'ファイルタグ紐づけテーブルのファイルタグIDの件数を取得
    'iFileTagId ファイルタグID
    'パラメータで取得したファイルタグIDを条件に、ファイルタグ紐づけテーブルを参照し、レコード件数を取得
    Public Function queryFileTagHimoCount(ByVal iFileTagId As Integer) As Integer

        Dim sFileTagHimoSelectSql As String = Nothing
        Dim iFileTagHimoId As Integer

        'パラメータで取得したカテゴリ、タイトルを条件に、ファイルタグテーブルを参照しファイルタグIDを取得
        sFileTagHimoSelectSql = "select count(HIMO_ID) from FILE_TAG_HIMO_TBL " &
                                "where TAG_ID =:TAG_ID "

        cmd.CommandText = sFileTagHimoSelectSql
        cmd.Parameters.Clear()
        cmd.Parameters.Add(New OracleParameter("TAG_ID", iFileTagId))

        If (Integer.TryParse(cmd.ExecuteScalar(), iFileTagHimoId)) Then
            Return iFileTagHimoId
        Else
            Return -1
        End If

    End Function

    'ファイルタグ紐づけテーブルから、ファイルIDをキーにファイルタグIDの一覧を取得
    'iFileId ファイルID
    'パラメータで取得したファイルIDを条件に、ファイルタグ紐づけテーブルを参照し、タグID、ファイルタグIDを取得
    Public Function queryFileTagHimoID(ByVal iFileFolderId As Integer, ByVal iFile_Kbn As Integer, ByRef reader As OracleDataReader) As Integer

        Dim syFileTagQuery As String = Nothing

        cmd.Parameters.Clear()


        If (iFile_Kbn = 1) Then
            syFileTagQuery = "select TAG_ID,HIMO_ID from FILE_TAG_HIMO_TBL " &
                         "where FILE_ID = :FILE_ID and FILE_KBN = :FILE_KBN "

            cmd.Parameters.Add(New OracleParameter("FILE_ID", iFileFolderId))

        ElseIf iFile_Kbn = 2 Then
            syFileTagQuery = "select TAG_ID,HIMO_ID from FILE_TAG_HIMO_TBL " &
                         "where FOLDER_ID = :FOLDER_ID and FILE_KBN = :FILE_KBN "
            cmd.Parameters.Add(New OracleParameter("FOLDER_ID", iFileFolderId))
        End If

        cmd.CommandText = syFileTagQuery
        cmd.Parameters.Add(New OracleParameter("FILE_KBN", iFile_Kbn))
        reader = cmd.ExecuteReader()
        Return 0

    End Function

    'ファイルタグ紐づけテーブルから、ファイルIDをキーにファイルタグの一覧を取得
    'iFileId ファイルID
    'パラメータで取得したファイルIDを条件に、ファイルタグ紐づけテーブルを参照し、タグ名、カテゴリを取得
    Public Function queryFileTag(ByVal iFileFolderId As Integer, ByVal iFile_Kbn As Integer, ByRef reader As OracleDataReader) As Integer

        Dim syFileTagQuery As String = Nothing

        cmd.Parameters.Clear()


        If (iFile_Kbn = 1) Then
            syFileTagQuery = "select B.CATEGORY,B.FILE_TAG_NAME from FILE_TAG_HIMO_TBL A,FILE_TAG_TBL B " &
                         "where A.TAG_ID = B.FILE_TAG_ID and FILE_ID = :FILE_ID and FILE_KBN = :FILE_KBN "

            cmd.Parameters.Add(New OracleParameter("FILE_ID", iFileFolderId))

        ElseIf iFile_Kbn = 2 Then
            syFileTagQuery = "select B.CATEGORY,B.FILE_TAG_NAME from FILE_TAG_HIMO_TBL A,FILE_TAG_TBL B " &
                         "where A.TAG_ID = B.FILE_TAG_ID and A.FOLDER_ID = :FOLDER_ID and A.FILE_KBN = :FILE_KBN "
            cmd.Parameters.Add(New OracleParameter("FOLDER_ID", iFileFolderId))
        End If

        cmd.CommandText = syFileTagQuery
        cmd.Parameters.Add(New OracleParameter("FILE_KBN", iFile_Kbn))
        reader = cmd.ExecuteReader()
        Return 0

    End Function



    '********************************************************************************************

    'ジャンルを取得
    'sCondition　フォルダタグテーブルのフォルダ名
    '条件なければジャンルテーブルのジャンル名、ジャンルIDの一覧を取得
    '条件ある場合はフォルダタグテーブルのデータに一致するジャンル名、ジャンルIDの一覧を取得
    Public Function queryGenre(ByVal sCondition As String, ByRef reader As OracleDataReader) As Integer

        If (sCondition = "") Then

            sGenreQuery = "select distinct C.ID,C.TITLE from FOLDER_TBL A,GENRE_TBL C " &
                        "WHERE C.ID = A.GENRE_ID ORDER BY C.ID"
        Else
            sGenreQuery = "select distinct C.ID,C.TITLE from FOLDER_TBL A,FILE_TAG_TBL B,GENRE_TBL C ,FILE_TAG_HIMO_TBL D " &
                        "WHERE C.ID = A.GENRE_ID and D.FOLDER_ID = A.ID and B.FILE_TAG_ID = D.TAG_ID and B.FILE_TAG_NAME like :FOLDER_NAME ORDER BY C.ID"
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

            sFolderQuery = "select ID,TITLE,GENRE_ID from FOLDER_TBL " &
                                        "where GENRE_ID IS NOT NULL order by GENRE_ID,TITLE"
        Else
            sFolderQuery = "select distinct A.ID,A.TITLE,A.GENRE_ID from FOLDER_TBL A,FILE_TAG_TBL B ,FILE_TAG_HIMO_TBL C " &
                                        "WHERE C.FOLDER_ID = A.ID and B.FILE_TAG_ID = C.TAG_ID and B.FILE_TAG_NAME like :FOLDER_NAME ORDER BY A.GENRE_ID,A.TITLE" ' ORDER BY C.ID"
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
    'iKensakuKbn　検索区分（0：最終更新日順、1：フォルダ検索（ランク以下）
    'iRank　ランク
    'iFolderId　フォルダID
    '条件に一致するファイルテーブルのID,ファイル名、パス、サイズ、画像、ランク、追加日、フォルダIDの一覧を取得
    Public Function queryFileListKensaku(ByVal iKensakuKbn As Integer, ByRef lstZokusei As List(Of String), ByVal iRank As Integer, ByVal iFolderId As Integer, ByRef reader As OracleDataReader) As Integer

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


        '属性SQL作成
        Dim i As Integer
        Dim sZokuCondition As String
        For Each sZokusei In lstZokusei
            If (i = 0) Then
                sZokuCondition = "SELECT T.ID, T.TITLE, T.A ,T.FILE_SIZE ,T.THUMBNAIL ,T.RANK ,T.ADD_DATE ,T.FOLDER_ID from FILE_TAG_TBL A,FILE_TAG_HIMO_TBL B,tmp T " &
                    "where A.FILE_TAG_ID = B.TAG_ID and T.ID = B.FILE_ID and A.CATEGORY = 5 and a.FILE_TAG_NAME = '" & sZokusei & "'"
            Else
                sZokuCondition = sZokuCondition & " and B.FILE_ID in ( " &
                                                    "SELECT T.ID from FILE_TAG_TBL A,FILE_TAG_HIMO_TBL B,tmp T " &
                                                    "where A.FILE_TAG_ID = B.TAG_ID and T.ID = B.FILE_ID and A.CATEGORY = 5 and a.FILE_TAG_NAME = '" & sZokusei & "'"
            End If
            i = i + 1
        Next

        If (i > 1) Then
            For ii = 2 To i
                sZokuCondition = sZokuCondition & ")"
            Next
        End If

        If (i > 0) Then
            sFileListQuery = "with tmp as ( " & sFileListQuery
            sFileListQuery = sFileListQuery & ")" & sZokuCondition
        End If


        Console.WriteLine(sFileListQuery)
        cmd.CommandText = sFileListQuery

        reader = cmd.ExecuteReader()

        Return 0

    End Function

    'ファイルの一覧を取得
    'iKensakuKbn　検索区分（フォルダ検索（ランク以下）
    'iRank　ランク
    'sTagName　タグ名
    '条件に一致するファイルテーブルのID,ファイル名、パス、サイズ、画像、ランク、追加日、フォルダIDの一覧を取得
    Public Function queryFileListKensaku(ByRef lstZokusei As List(Of String), ByVal iRank As Integer, ByVal sTagName As String, ByRef reader As OracleDataReader) As Integer

        Dim sFileListQuery As String = Nothing
        Dim sTagCondition As String = Nothing
        Dim sZokuCondition As String = Nothing


        'タグSQL作成
        If (sTagName <> "") Then
            sTagCondition = "select FILE_ID from FILE_TAG_TBL A,FILE_TAG_HIMO_TBL B " &
            "where A.FILE_TAG_ID = B.TAG_ID And B.FILE_KBN = 1 And A.FILE_TAG_NAME like '" & sTagName & "%'" &
            "union " &
            "select D.ID AS FILE_ID from FILE_TAG_TBL A,FILE_TAG_HIMO_TBL B ,FOLDER_TBL C ,FILE_TBL D " &
            "where A.FILE_TAG_ID = B.TAG_ID And B.FOLDER_ID = C.ID And C.ID = D.FOLDER_ID and B.FILE_KBN = 2 and A.FILE_TAG_NAME like '" & sTagName & "%'" &
            "union " &
            "select D.ID FILE_ID from FILE_TAG_TBL A, FILE_TAG_HIMO_TBL B, FOLDER_TBL C , FILE_TBL D " &
            "where A.FILE_TAG_ID = B.TAG_ID And C.ID = D.FOLDER_ID And B.FOLDER_ID = C.PARENT_FOLDER_ID and B.FILE_KBN = 2 and A.FILE_TAG_NAME like '" & sTagName & "%'"
        End If

        '属性SQL作成
        Dim i As Integer
        For Each sZokusei In lstZokusei
            If (i > 0) Then
                sZokuCondition = sZokuCondition & " and B.FILE_ID in ( "
            End If
            If (sTagName = "") Then
                sZokuCondition = sZokuCondition & "select B.FILE_ID from FILE_TAG_TBL A,FILE_TAG_HIMO_TBL B " &
                                                "where A.FILE_TAG_ID = B.TAG_ID and A.CATEGORY = 5 and a.FILE_TAG_NAME = '" & sZokusei & "'"
            Else
                sZokuCondition = sZokuCondition & "select B.FILE_ID from FILE_TAG_TBL A,FILE_TAG_HIMO_TBL B,tmp T " &
                                                "where A.FILE_TAG_ID = B.TAG_ID and T.FILE_ID = B.FILE_ID and A.CATEGORY = 5 and a.FILE_TAG_NAME = '" & sZokusei & "'"
            End If
            i = i + 1
        Next

        If (i > 1) Then
            For ii = 2 To i
                sZokuCondition = sZokuCondition & ")"
            Next
        End If



        If (sTagName <> "" And lstZokusei.Count > 0) Then
            'タグあり、属性ありの場合
            sFileListQuery = "with tmp as(" & sTagCondition & ") ,tmp2 as(" & sZokuCondition & ")"
        ElseIf (sTagName <> "" And lstZokusei.Count = 0) Then
            'タグあり、属性なしの場合
            sFileListQuery = "with tmp2 as(" & sTagCondition & ") "
        ElseIf (sTagName = "" And lstZokusei.Count > 0) Then
            'タグなし、属性ありの場合
            sFileListQuery = "with tmp2 as(" & sZokuCondition & ")"
        End If

        sFileListQuery = sFileListQuery &
                        "select A.ID, A.TITLE, C.PATH || '\' || B.PATH || '\' || A.PATH AS A ,A.FILE_SIZE ,A.THUMBNAIL ,A.RANK ,A.ADD_DATE ,A.FOLDER_ID " &
                        "from FILE_TBL A , FOLDER_TBL B , GENRE_TBL C , tmp2 T " &
                        "where B.GENRE_ID = C.ID and A.FOLDER_ID = B.ID and A.ID In T.FILE_ID and A.RANK >= " & iRank & " " &
                        "union all " &
                        "select A.ID, A.TITLE, D.PATH || '\' || C.PATH || '\' || B.PATH || '\' || A.PATH AS A ,A.FILE_SIZE ,A.THUMBNAIL ,A.RANK ,A.ADD_DATE ,A.FOLDER_ID " &
                        "from FILE_TBL A , FOLDER_TBL B , FOLDER_TBL C , GENRE_TBL D , tmp2 T " &
                        "where C.GENRE_ID = D.ID and B.PARENT_FOLDER_ID = C.ID and A.FOLDER_ID = B.ID and A.ID In T.FILE_ID and A.RANK >= " & iRank & " " &
                        "order by RANK desc"

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

        Console.WriteLine(sFilePathQuery)

        cmd.CommandText = sFilePathQuery
        cmd.Parameters.Clear()
        cmd.Parameters.Add(New OracleParameter("FILEPATH", sPath))
        reader = cmd.ExecuteReader()

        Return 0

    End Function

    'パスからファイルを取得
    'sPath　パス
    '条件に一致するファイルテーブルのID,ファイル名、パス、サイズ、画像、ランク、追加日、フォルダIDの一覧を取得
    Public Function queryBookMarkPath(ByVal sPath As String, ByRef reader As OracleDataReader) As Integer

        Dim sFilePathQuery As String = Nothing

        sFilePathQuery = "SELECT D.BOOKMARK_ID AS ID, D.TITLE, C.PATH || '\' || B.PATH || '\' || A.PATH || '\' || D.PATH AS A ,D.FILE_SIZE ,D.THUMBNAIL ,D.RANK ,A.FOLDER_ID ,B.TITLE AS NOW_FOLDER, B.TITLE AS PARENT_TITLE " &
                            "FROM FILE_TBL A ,FOLDER_TBL B ,GENRE_TBL C ,BOOKMARK_TBL D " &
                            "WHERE B.GENRE_ID = C.ID And A.FOLDER_ID = B.ID And D.FILE_ID = A.ID and " &
                            "C.PATH || '\' || B.PATH || '\' || A.PATH  || '\' || D.PATH = ':FILEPATH' " &
                        "UNION ALL " &
                        "SELECT E.BOOKMARK_ID AS ID, E.TITLE, D.PATH || '\' || C.PATH || '\' || B.PATH || '\' || A.PATH || '\' || E.PATH AS A ,E.FILE_SIZE ,E.THUMBNAIL ,E.RANK ,A.FOLDER_ID ,B.TITLE AS NOW_FOLDER ,C.TITLE AS PARENT_TITLE " &
                            "FROM FILE_TBL A ,FOLDER_TBL B ,FOLDER_TBL C ,GENRE_TBL D ,BOOKMARK_TBL E " &
                            "WHERE C.GENRE_ID = D.ID AND B.PARENT_FOLDER_ID = C.ID AND A.FOLDER_ID = B.ID AND E.FILE_ID = A.ID AND " &
                             "D.PATH || '\' || C.PATH || '\' || B.PATH || '\' || A.PATH || '\' || E.PATH = ':FILEPATH'"


        Console.WriteLine(sFilePathQuery)

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

    'ファイルタグ名に一番多く登録されているカテゴリを取得 
    'sTagName　ファイルタグ名
    'ファイルタグタグテーブルからファイルタグ名をキーにファイルタグＩＤを取得し、取得したファイルタグIＤをキーにファイルタグ紐づけテーブルを検索し、最も登録されているカテゴリを返却
    Public Function queryDefaultCategory(ByVal sTagName As String) As Integer

        Dim sDefaultCategoryQuery As String = Nothing


        sDefaultCategoryQuery = "select * from (" &
                                "select A.CATEGORY from FILE_TAG_TBL A, FILE_TAG_HIMO_TBL B " &
                                "where A.FILE_TAG_ID = B.TAG_ID And A.FILE_TAG_NAME =:TAG_NAME " &
                                "group by A.CATEGORY " &
                                "order by count(A.CATEGORY) desc) " &
                                "where rownum = 1"

        cmd.Parameters.Clear()
        cmd.Parameters.Add(New OracleParameter("TAG_NAME", sTagName))
        cmd.CommandText = sDefaultCategoryQuery

        Return cmd.ExecuteScalar()

    End Function


    '属性の上位ｎ件を取得
    'iCount 取得件数
    'ファイルタグタグテーブルからカテゴリが属性のファイルＩＤを取得し、取得したファイルタグＩＤをキーにファイルタグ紐づけテーブルを検索し、最も登録されているファイルタグ名を返却
    Public Function queryZokuseiList(ByVal iCount As Integer, ByRef reader As OracleDataReader) As Integer

        Dim sZokuseiQuery As String = Nothing

        sZokuseiQuery = "select * from (" &
                                "select A.FILE_TAG_NAME from FILE_TAG_TBL A, FILE_TAG_HIMO_TBL B " &
                                "where A.FILE_TAG_ID = B.TAG_ID and A.CATEGORY = 5 " &
                                "group by A.FILE_TAG_NAME " &
                                "order by COUNT(A.FILE_TAG_NAME) desc) " &
                                "where rownum <= :COUNT"

        cmd.CommandText = sZokuseiQuery
        cmd.Parameters.Clear()
        cmd.Parameters.Add(New OracleParameter("COUNT", iCount))
        reader = cmd.ExecuteReader()

        Return 0

    End Function





    '**************************************************************
    '********************** 　　　追加 　　　********************** 
    '**************************************************************


    'ファイルタグテーブルへレコード追加
    Public Function insertFileTagTBL(ByVal iCategory As Integer, ByVal sData As String) As Integer

        Dim sFileTagQuery As String = Nothing



        sFileTagQuery = "insert into FILE_TAG_TBL values (FILE_TAG_TBL_SEQ.NEXTVAL,:FILE_TAG_NAME,:CATEGORY,'')"

        cmd.Parameters.Clear()
        cmd.CommandText = sFileTagQuery
        cmd.Parameters.Add(New OracleParameter(":FILE_TAG_NAME", sData))
        cmd.Parameters.Add(New OracleParameter(":CATEGORY", iCategory))

        cmd.ExecuteNonQuery()

        Return 0

    End Function



    'ファイルタグ紐づけテーブルへのレコード追加
    Public Function insertFileTagHimoTBL(ByVal iFileTagId As Integer, ByVal iFileFolderId As Integer, ByVal iFile_Kbn As Integer) As Integer

        Dim sFileTagHimoQuery As String = Nothing

        sFileTagHimoQuery = "insert into file_tag_himo_tbl values (file_tag_himo_tbl_seq.nextval,:FILE_TAG_ID,:FILE_ID,:FOLDER_ID,:BOOKMARK_ID,:FILE_KBN,'')"
        cmd.CommandText = sFileTagHimoQuery
        cmd.Parameters.Clear()
        cmd.Parameters.Add(New OracleParameter(":FILE_TAG_ID", iFileTagId))

        Select Case iFile_Kbn
            Case 1
                cmd.Parameters.Add(New OracleParameter(":FILE_ID", iFileFolderId))
                cmd.Parameters.Add(New OracleParameter(":FOLDER_ID", DBNull.Value))
                cmd.Parameters.Add(New OracleParameter(":BOOKMARK_ID", DBNull.Value))
            Case 2
                cmd.Parameters.Add(New OracleParameter(":FILE_ID", DBNull.Value))
                cmd.Parameters.Add(New OracleParameter(":FOLDER_ID", iFileFolderId))
                cmd.Parameters.Add(New OracleParameter(":BOOKMARK_ID", DBNull.Value))
            Case 3
                cmd.Parameters.Add(New OracleParameter(":FILE_ID", DBNull.Value))
                cmd.Parameters.Add(New OracleParameter(":FOLDER_ID", DBNull.Value))
                cmd.Parameters.Add(New OracleParameter(":BOOKMARK_ID", iFileFolderId))
        End Select


        cmd.Parameters.Add(New OracleParameter(":FILE_KBN", iFile_Kbn))
        cmd.ExecuteNonQuery()

        Return 0

    End Function



    'フォルダテーブルのレコード追加。
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
            cmd.Parameters.Add(New OracleParameter("GENRE_ID", DBNull.Value))
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
            cmd.Parameters.Add(New OracleParameter("PARENT_FOLDER", DBNull.Value))
            cmd.ExecuteNonQuery()


        End If
        Return iFolderId

    End Function



    'ファイルテーブルのレコード追加
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

        Return iId

    End Function





    '**************************************************************
    '********************** 　　　更新 　　　********************** 
    '**************************************************************

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


    '**************************************************************
    '********************** 　　　削除 　　　********************** 
    '**************************************************************
    'ファイルタグ紐づけテーブルのレコード削除
    'iFileTagId ファイルタグID
    'iFileId ファイルID
    'iFileKbn ファイル区分(1:ファイル 2:フォルダ 3:ブックマーク）
    Public Function delFileTagHimoTBL(ByVal iFileTagId As Integer, ByVal iFileFolderId As Integer, ByVal iFileKbn As Integer) As Integer

        Dim sFileTagHimoDelQuery As String = Nothing
        sFileTagHimoDelQuery = "delete FILE_TAG_HIMO_TBL where TAG_ID =:TAG_ID and FILE_KBN = :FILE_KBN "


        cmd.Parameters.Clear()
        cmd.Parameters.Add(New OracleParameter("TAG_ID", iFileTagId))
        cmd.Parameters.Add(New OracleParameter("FILE_KBN", iFileKbn))

        Select Case iFileKbn
            Case 1
                sFileTagHimoDelQuery = sFileTagHimoDelQuery & " and FILE_ID = :FILE_ID "
                cmd.Parameters.Add(New OracleParameter("FILE_ID", iFileFolderId))
            Case 2
                sFileTagHimoDelQuery = sFileTagHimoDelQuery & " and FOLDER_ID = :FOLDER_ID "
                cmd.Parameters.Add(New OracleParameter("FOLDER_ID", iFileFolderId))
            Case 3
            Case 2
                sFileTagHimoDelQuery = sFileTagHimoDelQuery & " and BOOKMARK_ID = :BOOKMARK_ID "
                cmd.Parameters.Add(New OracleParameter("BOOKMARK_ID", iFileFolderId))

        End Select

        cmd.CommandText = sFileTagHimoDelQuery
        cmd.ExecuteNonQuery()

        Return 0

    End Function

    'ファイルタグテーブルのレコード削除
    'iFileTagId ファイルタグID
    Public Function delFileTagTBL(ByVal iFileTagId As Integer) As Integer

        Dim sFileTagDelQuery As String = Nothing
        sFileTagDelQuery = "delete FILE_TAG_TBL where FILE_TAG_ID =:TAG_ID "


        cmd.Parameters.Clear()
        cmd.CommandText = sFileTagDelQuery
        cmd.Parameters.Add(New OracleParameter("TAG_ID", iFileTagId))
        cmd.ExecuteNonQuery()

        Return 0

    End Function

    'ファイルテーブルのレコード削除
    'iFileId　ファイルID
    Public Function deleteFile(ByVal iFileId As Integer) As String

        Dim sFileQuery As String = Nothing
        Dim sFileTagQuery As String = Nothing

        sFileQuery = "DELETE file_tbl where id = :FILEID"

        cmd.Parameters.Clear()
        cmd.Parameters.Add(New OracleParameter("FILEID", iFileId))
        cmd.CommandText = sFileQuery
        cmd.ExecuteNonQuery()

        Return 0

    End Function

    'フォルダテーブルのレコード削除
    'iFolderId　フォルダID
    Public Function deleteFolder(ByVal iFolderId As Integer) As Integer

        Dim sFolderTagQuery As String = Nothing
        Dim sFolderQuery As String = Nothing

        Dim txn As OracleTransaction = cnn.BeginTransaction()

        sFolderQuery = "DELETE FROM FOLDER_TBL WHERE ID = " & iFolderId
        cmd.CommandText = sFolderQuery
        cmd.ExecuteNonQuery()
        txn.Commit()
        txn.Dispose()

        Return 0
    End Function

    Protected Overrides Sub Finalize()
        cmd = Nothing
        cnn.Close()
        MyBase.Finalize()
    End Sub
End Class
