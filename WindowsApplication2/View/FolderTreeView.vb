Imports Oracle.DataAccess.Client
Imports Oracle.DataAccess.Types

Public Class FolderTreeView

    Private clsFileMoveForm As FileMoveForm

    Public Sub New(ByRef clsFMF As FileMoveForm)
        clsFileMoveForm = clsFMF
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
        Dim clsOraAccess As New OraAccess

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

            clsFileMoveForm.treeDir.Nodes.Clear()
            'ツリーノード生成
            Dim iNowGroup As Integer
            Dim nowNode As TreeNode = Nothing
            Dim iNodeIdx As Integer = -1
            Dim iSubNodeIdx As Integer = -1

            While (readerFolder.Read())
                If Not (iNowGroup = readerFolder.GetValue(2)) Then
                    While (readerGenre.Read())
                        clsFileMoveForm.treeDir.Nodes.Add(readerGenre.GetValue(0).ToString, readerGenre.GetString(1))
                        iNowGroup = readerGenre.GetValue(0)
                        iNodeIdx = iNodeIdx + 1
                        iSubNodeIdx = -1
                        nowNode = clsFileMoveForm.treeDir.Nodes(iNodeIdx)
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
                clsFileMoveForm.treeDir.ExpandAll()
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



End Class
