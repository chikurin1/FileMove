
Module Util

    Function ChangeFileSize(ByVal FileSize As Object) As String
        Dim dFileSize = CType(FileSize, Double)
        Select Case dFileSize
            Case 0 To 1024
                Return "1 KB"
            Case (1024 + 1) To (1024 ^ 2)
                Return Math.Round((dFileSize / 1024), 0) & " KB"
            Case ((1024 ^ 2) + 1) To (1024 ^ 4)
                Return Math.Round((dFileSize / (1024 ^ 2)), 2) & " MB"
            Case ((1024 ^ 4) + 1) To (1024 ^ 8)
                Return Math.Round((dFileSize / (1024 ^ 4)), 2) & " GB"
            Case ((1024 ^ 8) + 1) To (1024 ^ 16)
                Return Math.Round((dFileSize / (1024 ^ 8)), 2) & " TB"
            Case Else
                Return dFileSize
        End Select
    End Function



    '禁則文字を半角から全角に置換
    Public Function ChangeKinsoku(ByVal sBef As String) As String

        Dim arrayKinsoku As Char(,) = {{"?", "？"}, {"*", "＊"}, {":", "："}, {"/", "／"}, {"<", "＜"}, {">", "＞"}, {"\", "￥"}}

        Dim sAft As String = Nothing
        Dim sBuf As String = Nothing

        sBuf = alphaHenkan(sBef)

        Dim c As Char

        For Each c In sBuf
            For j = 0 To arrayKinsoku.GetLength(0) - 1
                If (c = arrayKinsoku(j, 0)) Then
                    sAft = sAft & arrayKinsoku(j, 1)
                    Exit For
                End If
                If (j = arrayKinsoku.GetLength(0) - 1) Then
                    sAft = sAft & c
                End If
            Next
        Next

        Return sAft

    End Function

    Function alphaHenkan(ByVal s As String) As String

        Dim re As System.Text.RegularExpressions.Regex = New System.Text.RegularExpressions.Regex("[０-９Ａ-Ｚａ-ｚ’－　？＠．，]+")
        Dim output As String = re.Replace(s, AddressOf myReplacer)

        Return output
    End Function

    Function myReplacer(ByVal m As System.Text.RegularExpressions.Match) As String
        Return Strings.StrConv(m.Value, VbStrConv.Narrow, 0)
    End Function



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

    Public Sub ClearTextBox(ByVal hParent As Control)
        ' hParent 内のすべてのコントロールを列挙する
        For Each cControl As Control In hParent.Controls
            ' 列挙したコントロールにコントロールが含まれている場合は再帰呼び出しする
            If cControl.HasChildren Then
                ClearTextBox(cControl)
            End If

            ' コントロールの型が TextBoxBase からの派生型の場合は Text をクリアする
            If TypeOf cControl Is TextBoxBase Then
                cControl.Text = String.Empty
            End If
        Next cControl
    End Sub

    Public Sub ClearTextBox2(ByVal hParent As Control)
        ' hParent 内のすべてのコントロールを列挙する
        For Each cControl As Control In hParent.Controls
            ' 列挙したコントロールにコントロールが含まれている場合は再帰呼び出しする
            If cControl.HasChildren Then
                ClearTextBox2(cControl)
            End If
            ' コントロールの型が TextBoxBase からの派生型の場合は Text をクリアする

            If TypeOf cControl Is TextBoxBase Then
                'タグ再設定テキストボックスはクリアしない
                If (cControl.Name <> "txtTagSetting") Then
                    cControl.Text = String.Empty
                End If
            End If
        Next cControl
    End Sub

    Public Sub ClearCombotBox(ByVal hParent As Control)
        ' hParent 内のすべてのコントロールを列挙する
        For Each cControl As Control In hParent.Controls
            ' 列挙したコントロールにコントロールが含まれている場合は再帰呼び出しする
            If cControl.HasChildren Then
                ClearCombotBox(cControl)
            End If

            ' コントロールの型が TextBoxBase からの派生型の場合は Text をクリアする
            If TypeOf cControl Is ComboBox Then
                Dim cmb As ComboBox
                cmb = cControl
                cmb.SelectedIndex = 0
            End If
        Next cControl
    End Sub

    Public Sub ClearCheckBox(ByVal hParent As Control)
        ' hParent 内のすべてのコントロールを列挙する
        For Each cControl As Control In hParent.Controls
            ' 列挙したコントロールにコントロールが含まれている場合は再帰呼び出しする
            If cControl.HasChildren Then
                ClearCombotBox(cControl)
            End If

            ' コントロールの型が TextBoxBase からの派生型の場合は Text をクリアする
            If TypeOf cControl Is CheckBox Then
                Dim chk As CheckBox
                chk = cControl
                If (chk.Name <> "chkOver") Then
                    chk.Checked = False
                End If
            End If
        Next cControl
    End Sub

    Public Sub ChangeBookmark()

    End Sub



End Module
