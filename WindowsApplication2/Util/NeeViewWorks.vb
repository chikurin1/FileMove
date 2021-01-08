Public Class NeeViewWorks

    Private sScriptPath As String = "C:\ProgramData\NeeView\Scripts\OnBookLoaded.nvjs"

    Private sw As System.IO.StreamWriter

    'コンストラクタ
    Public Sub New()
        'BOMなしUTF-8で書き込む
        '書き込むファイルが既に存在している場合は、上書きする
        sw = New System.IO.StreamWriter(sScriptPath, False)
    End Sub

    Private Sub FileWrite(ByRef lstData As List(Of String))

        'TextBox1.Textの内容を書き込む
        For Each writedata In lstData
            sw.WriteLine(writedata)
        Next
    End Sub

    Public Sub ScriptCreate(ByVal sFileName As String, ByRef lstView As ListView)

        Dim arrayData As New List(Of String)
        arrayData.Add("// @name PlaylistOpen")
        arrayData.Add("// @description FileMove Renkei")

        Dim lItemsCount As Integer = lstView.Items.Count
        For ii = 0 To lItemsCount - 1

            If lItemsCount <= 0 Then
                'なにもしない
                Exit For
            End If

            'ファイル名と同じ場合
            If lstView.Items(ii).SubItems(1).Text = sFileName Then

                Dim dt1 As DateTime = DateTime.Now

                arrayData.Add("if(!nv.Values.nv" & dt1.ToString("yyyyMMddHHmmss") & "){ ")

                If Not lstView.Items.Count = 1 Then

                    If lstView.Items.Count - 1 = ii Then
                        arrayData.Add("if(nv.Bookshelf.Items.filter(e => e.Name.startsWith('" & lstView.Items(ii - 1).SubItems(0).Text & "'))!=''){")
                        arrayData.Add("nv.Bookshelf.SelectedItems = nv.Bookshelf.Items.filter(e => e.Name.startsWith('" & lstView.Items(ii - 1).SubItems(0).Text & ".zip'))")
                        arrayData.Add("nv.Command.NextBook.Execute()")
                    Else
                        arrayData.Add("if(nv.Bookshelf.Items.filter(e => e.Name.startsWith('" & lstView.Items(ii + 1).SubItems(0).Text & "'))!=''){")
                        arrayData.Add("nv.Bookshelf.SelectedItems = nv.Bookshelf.Items.filter(e => e.Name.startsWith('" & lstView.Items(ii + 1).SubItems(0).Text & ".zip'))")
                        arrayData.Add("nv.Command.PrevBook.Execute()")

                    End If
                End If

                arrayData.Add("for (let i = 0; i < 3; i++) {")
                arrayData.Add("if(nv.Book.Path ==" & """" & lstView.Items(ii).SubItems(1).Text & """" & "){")
                arrayData.Add("break")
                arrayData.Add("}")
                arrayData.Add("sleep(100)")
                arrayData.Add("}")
                arrayData.Add("for (let ii = 0; ii < 10; ii++) {")
                arrayData.Add("nv.Command.FirstPage.Execute()")
                arrayData.Add("if(nv.Book.Pages[0].Path == nv.Book.ViewPages[0].Path){")
                arrayData.Add("break")
                arrayData.Add("}")
                arrayData.Add("sleep(100)")
                arrayData.Add("}")
                arrayData.Add("}")
                arrayData.Add("}")
                arrayData.Add("nv.Values.nv" & dt1.ToString("yyyyMMddHHmmss") & "= true")

                Exit For
            End If

        Next

        FileWrite(arrayData)

        sw.Close()

    End Sub

    Public Sub KidouHikisuuCreate(ByRef lstView As ListView, ByVal sFilePath As String, ByRef sAllPath As String)

        Dim ii As Integer = 0
        Dim bFileFlag As Boolean = False
        For Each a In lstView.Items
            sAllPath = sAllPath & " " & """" & a.SubItems(1).text.ToString & """"
            If a.SubItems(1).text.ToString = sFilePath Then
                bFileFlag = True
            End If

            ii = ii + 1
        Next

    End Sub

End Class
