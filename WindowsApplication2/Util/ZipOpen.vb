Public Class ZipOpen

    Private sFilePath As String
    Private listTag As New List(Of String)
    Private sFileName As String
    Private sFileMei As String
    Private imgThumbs As Image

    Private sBookMarkDir As String
    Private sBookMarkFile As String

    '暫定
    Private sFirstFile As String

    Public ReadOnly Property FirstFile() As String
        Get
            Return sFirstFile
        End Get
    End Property

    'value:zipファイル
    Public Sub New(ByVal value As String)
        sFilePath = value
        zipWorks()
    End Sub

    'value:zipファイル
    'bm:Bookmark
    Public Sub New(ByVal value As String, ByVal bm As String)
        sFilePath = value
        zipWorks(bm)
    End Sub

    Public Sub New()

    End Sub


    Public ReadOnly Property Thumbs() As Image

        Get
            Return imgThumbs
        End Get
    End Property


    Public ReadOnly Property Tag() As List(Of String)
        Get
            Return listTag
        End Get
    End Property

    Public ReadOnly Property FileName() As String
        Get
            Return sFileName
        End Get
    End Property


    Public ReadOnly Property FileMei() As String
        Get
            Return sFileMei
        End Get
    End Property


    Public ReadOnly Property BookMarkName() As String
        Get
            Return sBookMarkFile
        End Get
    End Property

    Private Sub zipWorks()

        Dim width As Integer = 96
        Dim height As Integer = 96

        Dim bFlag As Boolean = False

        'ZipFileオブジェクトの作成 
        Dim zf As New ICSharpCode.SharpZipLib.Zip.ZipFile(sFilePath)

        'ZIP内のエントリを列挙
        Dim ze As ICSharpCode.SharpZipLib.Zip.ZipEntry

        For Each ze In zf

            '情報を表示する 
            If ze.IsFile Then

                '☆暫定☆
                sFirstFile = ze.Name

                'ファイルのとき 
                '閲覧するZIPエントリのStreamを取得 
                Dim reader As System.IO.Stream = zf.GetInputStream(ze)

                'サムネイルを作成
                Dim bmpThumbs As Bitmap = New Bitmap(reader)
                imgThumbs = createThumbnail(bmpThumbs, width, height)

                reader.Close()

                'Zip書庫のフォルダ名を取得
                'フォルダがない場合は、元ファイルのファイル名を取得
                Dim iSplitIdx As Integer = ze.Name.IndexOf("/")
                If (iSplitIdx = -1) Then
                    sFileName = Trim(System.IO.Path.GetFileNameWithoutExtension(sFilePath))
                Else
                    sFileName = Trim(ze.Name.Substring(0, iSplitIdx))
                End If

                Exit For
            ElseIf ze.IsDirectory Then
                'ディレクトリのとき 
                sFileName = Trim(ze.Name)
                If (bFlag = True) Then
                    'Exit While
                End If

            End If
        Next

        sFileMei = sFileName
        'ファイル名、タグを取得
        'tagCreate(sFileName)

        '閉じる 
        ze = Nothing
        zf.Close()
    End Sub

    Public Sub zipWorks(ByVal sBookMarkPath As String)

        Dim width As Integer = 96
        Dim height As Integer = 96

        Dim bFlag As Boolean = False


        sBookMarkDir = sBookMarkPath.Substring(0, sBookMarkPath.LastIndexOf("\"))
        sBookMarkFile = sBookMarkPath.Substring(sBookMarkPath.LastIndexOf("\") + 1)


        'ZipFileオブジェクトの作成 
        Dim zf As New ICSharpCode.SharpZipLib.Zip.ZipFile(sBookMarkDir)

        'ZIP内のエントリを列挙
        Dim ze As ICSharpCode.SharpZipLib.Zip.ZipEntry

        For Each ze In zf

            '情報を表示する 
            If ze.IsFile Then

                If ze.Name = sBookMarkFile Then

                    'ファイルのとき 
                    '閲覧するZIPエントリのStreamを取得 
                    Dim reader As System.IO.Stream = zf.GetInputStream(ze)

                    'サムネイルを作成
                    Dim bmpThumbs As Bitmap = New Bitmap(reader)
                    imgThumbs = createThumbnail(bmpThumbs, width, height)

                    reader.Close()


                    sFileName = sBookMarkFile

                    bFlag = True

                    Exit For
                End If

            ElseIf ze.IsDirectory Then
                If (ze.Name = sBookMarkDir) Then
                    'ディレクトリのとき 
                    sFileName = Trim(ze.Name)

                    '処理追加したいけど。

                End If
            End If
        Next

        If bFlag = False Then
            MsgBox("ブックマークファイルが存在しません")
            'NGを返したい

        End If

        sFileMei = sFileName

        '閉じる 
        ze = Nothing
        zf.Close()
    End Sub


    'ファイル名から、タグを作成
    Public Sub tagCreate(ByVal sFileName1 As String)
        Console.WriteLine("tagCreate" & sFileName1)
        Dim arrayKakko As Char(,) = {{"(", ")"}, {"［", "］"}, {"（", "）"}, {"『", "』"}, {"【", "】"}, {"[", "]"}, {"「", "」"}, {"〈", "〉"}, {"《", "》"}, {"｛", "｝"}, {"{", "}"}, {"〔", "〕"}, {"〘", "〙"}, {"〚", "〛"}}

        'Dim listTag As New ArrayList

        Dim i As Integer
        Dim iColumn As Integer = -1
        Dim iStartColumn As Integer = 0
        Dim bFlag As Boolean

        'listTag.Add(sFileName)

        '1文字ずつ繰り返し
        For i = 0 To sFileName1.Length - 1

            If (iColumn = -1) Then
                '括弧の配列を１つずつ繰り返し
                For j = 0 To arrayKakko.GetLength(0) - 1
                    '前括弧が存在した場合
                    If (arrayKakko(j, 0) = sFileName1(i)) Then
                        If (bFlag = True And iStartColumn <> i) Then

                            Dim sbuf As String = Trim(ChangeKinsoku(sFileName1.Substring(iStartColumn, i - iStartColumn)))
                            If (sbuf <> "") Then
                                sFileName = sbuf
                            End If
                            bFlag = False
                        End If
                        iColumn = j
                        Exit For
                    End If
                Next
                If bFlag = False Then
                    If (iColumn = -1) Then
                        iStartColumn = i
                    Else
                        iStartColumn = i + 1
                    End If
                    bFlag = True
                End If
            Else
                If (arrayKakko(iColumn, 1) = sFileName1(i)) Then
                    If ((Trim(sFileName1.Substring(iStartColumn, i - iStartColumn)) <> "同人誌") And
                        (Trim(sFileName1.Substring(iStartColumn, i - iStartColumn)) <> "成年コミック")) Then
                        listTag.Add(Trim(ChangeKinsoku(sFileName1.Substring(iStartColumn, i - iStartColumn))))
                    End If
                    iColumn = -1
                    bFlag = False
                End If
            End If
        Next
        If (bFlag = True) Then
            If (iColumn = -1) Then
                Dim sBuf As String = Trim(ChangeKinsoku(sFileName1.Substring(iStartColumn, i - iStartColumn)))
                If (sBuf <> "") Then
                    sFileName = sBuf
                End If
            Else
                iStartColumn = iStartColumn - 1
                listTag.Add(Trim(ChangeKinsoku(sFileName1.Substring(iStartColumn, i - iStartColumn))))
            End If
        End If

        If (listTag.Count = 0) Then
            Exit Sub
        End If

        Dim sTag As String
        '
        For index = 0 To listTag.Count - 1
            iStartColumn = 0
            sTag = listTag(index)
            For ii = 0 To sTag.Length - 1
                For j = 0 To arrayKakko.GetLength(0) - 1
                    '括弧ありの場合
                    Select Case sTag(ii)
                        Case arrayKakko(j, 0)
                            iStartColumn = ii
                            Exit For
                        Case arrayKakko(j, 1)
                            If (iStartColumn <> 0) Then
                                listTag(index) = Trim(ChangeKinsoku(sTag.Substring(0, iStartColumn - 1)))
                                listTag.Add(Trim(ChangeKinsoku(sTag.Substring(iStartColumn + 1, ii - iStartColumn - 1))))
                                Exit For
                            End If
                    End Select
                Next
            Next
        Next


    End Sub

    ' 幅w、高さhのImageオブジェクトを作成
    Function createThumbnail(ByRef image As Image, ByVal w As Integer, ByVal h As Integer) As Image
        Dim canvas As New Bitmap(w, h)

        Dim g As Graphics = Graphics.FromImage(canvas)
        g.FillRectangle(New SolidBrush(Color.White), 0, 0, w, h)

        Dim fw As Double = CDbl(w) / CDbl(image.Width)
        Dim fh As Double = CDbl(h) / CDbl(image.Height)
        Dim scale As Double = Math.Min(fw, fh)

        Dim w2 As Integer = CInt(image.Width * scale)
        Dim h2 As Integer = CInt(image.Height * scale)

        g.DrawImage(image, (w - w2) \ 2, (h - h2) \ 2, w2, h2)
        g.Dispose()

        Return canvas
    End Function

End Class
