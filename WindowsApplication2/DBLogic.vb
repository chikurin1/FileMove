Imports Oracle.DataAccess.Client
Imports Oracle.DataAccess.Types
Public Class DBLogic

    'ファイルタグの追加
    'iCategory カテゴリ
    'sData 　ファイルタグ名
    'iFileId ファイルID
    Public Function insertFileTag(ByRef clsOraAccess As OraAccess, ByVal iCategory As Integer, ByVal sData As String, ByVal iFileId As Integer, ByVal iFileKbn As Integer) As Integer

        Dim iFileTagId As Integer
        Dim iFileTagHimoId As Integer

        'ファイルタグテーブルからカテゴリ、タイトルを条件にファイルタグIDを取得
        iFileTagId = clsOraAccess.queryFileTagID(iCategory, sData)

        'ファイルタグIDが取得できなかったた場合、ファイルタグテーブルに追加
        If (iFileTagId = -1) Then
            clsOraAccess.insertFileTagTBL(iCategory, sData)

            '追加したファイルタグIDを取得
            iFileTagId = clsOraAccess.queryFileTagID(iCategory, sData)
        End If

        'ファイルタグ紐づけテーブルからファイルID,ファイルタグIDを条件にレコードの存在チェック
        iFileTagHimoId = clsOraAccess.queryFileTagHimoID(iFileTagId, iFileId, iFileKbn)

        'ファイルタグ紐づけIDが取得できなかった場合、ファイルタグ紐づけテーブルに追加
        If (iFileTagHimoId = -1) Then
            iFileTagHimoId = clsOraAccess.insertFileTagHimoTBL(iFileTagId, iFileId, iFileKbn)
        End If

        Return 0

    End Function


    'ファイルタグの削除
    'iCategory カテゴリ
    'sData 　ファイルタグ名
    'iDelId ファイルID or フォルダID
    'iFileKbn 1:ファイル、2:フォルダ
    Public Function delFileTag(ByRef clsOraAccess As OraAccess, ByVal iCategory As Integer, ByVal sData As String, ByVal iFileFolderId As Integer, ByVal iFileKbn As Integer) As Integer

        Dim iFileTagId As Integer
        Dim iFileTagHimoCount As Integer

        'ファイルタグテーブルからカテゴリ、タイトルを条件にファイルタグIDを取得
        iFileTagId = clsOraAccess.queryFileTagID(iCategory, sData)

        'ファイルタグIDが取得できなかったた場合、なにもしない
        If (iFileTagId = -1) Then
            Return 0
            Exit Function
        End If

        'ファイルタグ紐づけテーブルからファイルタグID、ファイルIDに一致するレコードを削除
        clsOraAccess.delFileTagHimoTBL(iFileTagId, iFileFolderId, iFileKbn)

        'ファイルタグ紐づけテーブルからファイルタグIDを条件にレコードの存在チェック
        iFileTagHimoCount = clsOraAccess.queryFileTagHimoCount(iFileTagId)

        'ファイルタグ紐づけテーブルのレコードが0件の場合、ファイルタグテーブルのレコードを削除
        If (iFileTagHimoCount <= 0) Then
            iFileTagHimoCount = clsOraAccess.delFileTagTBL(iFileTagId)
        End If

        Return 0

    End Function


    'ファイルタグの削除
    'iFileId ファイルID
    Public Function delFileTag(ByRef clsOraAccess As OraAccess, ByVal iFileId As Integer, ByVal iFileKbn As Integer) As Integer

        Dim iFileTagId As Integer
        Dim iFileTagHimoId As Integer
        Dim iFileTagHimoCount As Integer
        Dim readerTagIdList As OracleDataReader = Nothing

        'ファイルタグ紐づけテーブルからファイルIDを条件にタグIDを取得
        iFileTagId = clsOraAccess.queryFileTagHimoID(iFileId, iFileKbn, readerTagIdList)


        '取得したレコード分繰り返し
        While (readerTagIdList.Read())

            iFileTagId = readerTagIdList.GetValue(0)
            iFileTagHimoId = readerTagIdList.GetValue(1)

            'ファイルタグ紐づけテーブルからファイルタグID、ファイルIDに一致するレコードを削除
            clsOraAccess.delFileTagHimoTBL(iFileTagId, iFileId, iFileKbn)

            'ファイルタグ紐づけテーブルからファイルタグIDを条件にレコードの存在チェック
            iFileTagHimoCount = clsOraAccess.queryFileTagHimoCount(iFileTagId)

            'ファイルタグ紐づけテーブルのレコードが0件の場合、ファイルタグテーブルのレコードを削除
            If (iFileTagHimoCount <= 0) Then
                iFileTagHimoCount = clsOraAccess.delFileTagTBL(iFileTagId)
            End If

        End While

        Return 0

    End Function

End Class
