Imports Oracle.DataAccess.Client
Imports Oracle.DataAccess.Types

Public Class FormView

    Private clsFileMoveForm As FileMoveForm
    Public Sub New()

    End Sub


    Public Sub New(ByRef clsFMF As FileMoveForm)
        clsFileMoveForm = clsFMF
    End Sub

    Public Sub TagCreate(ByVal iFileID As Integer, ByVal iKensakuKbn As Integer)

        Dim clsOraAccess As OraAccess
        Dim readerFileTag As OracleDataReader

        Try
            'DBアクセス用クラスのインスタンスを作成
            clsOraAccess = New OraAccess()
            'タグ取得
            clsOraAccess.queryFileTag(iFileID, iKensakuKbn, readerFileTag)

            'タグを設定
            Dim ilistIdx As Integer = 1
            Dim cmb As ComboBox
            Dim chk As CheckBox
            While (readerFileTag.Read())
                Dim chkFlag As Boolean = False
                If (readerFileTag.GetValue(0) = 5) Then
                    For i = 1 To 10
                        chk = CType(clsFileMoveForm.Controls("chkZoku" & i), CheckBox)
                        If (chk.Text = readerFileTag.GetValue(1)) Then
                            chk.Checked = True
                            chkFlag = True
                            Exit For
                        End If
                    Next
                End If
                If (chkFlag = False) Then
                    clsFileMoveForm.Controls("txtTag" & ilistIdx).Text = readerFileTag.GetString(1)
                    cmb = clsFileMoveForm.Controls("cmbTag" & ilistIdx)
                    cmb.SelectedValue = readerFileTag.GetValue(0)
                    ilistIdx += 1
                End If

            End While
            readerFileTag.Close()

        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            readerFileTag.Close()

        End Try
    End Sub

    ''' <summary>
    ''' タグのテキストボックス、プルダウン、属性チェックボックスをクリアする
    ''' </summary>
    Public Sub DataClear()
        '初期化
        ClearTextBox(clsFileMoveForm)
        ClearCombotBox(clsFileMoveForm)
        ClearCheckBox(clsFileMoveForm)

        clsFileMoveForm.txtFileName.BackColor = Color.White

    End Sub

    ''' <summary>
    ''' 更新モードのボタン設定
    ''' 
    ''' </summary>
    Public Sub BtnUpdateMode()
        clsFileMoveForm.btnMove.Visible = False
        clsFileMoveForm.btnDelete.Visible = False
        clsFileMoveForm.btnUpdate.Visible = True
        clsFileMoveForm.btnNowDel.Visible = True
        clsFileMoveForm.btnBookMarkAdd.Visible = False
        clsFileMoveForm.btnBookMarkUpdate.Visible = False
        clsFileMoveForm.btnBookMarkDel.Visible = False
        clsFileMoveForm.AcceptButton = clsFileMoveForm.btnUpdate
    End Sub

    Public Sub BtnAddMode()
        clsFileMoveForm.btnMove.Visible = True
        clsFileMoveForm.btnDelete.Visible = True
        clsFileMoveForm.btnUpdate.Visible = False
        clsFileMoveForm.btnNowDel.Visible = False
        clsFileMoveForm.btnBookMarkAdd.Visible = False
        clsFileMoveForm.btnBookMarkUpdate.Visible = False
        clsFileMoveForm.btnBookMarkDel.Visible = False
        clsFileMoveForm.AcceptButton = clsFileMoveForm.btnMove
    End Sub

    ''' <summary>
    ''' ブックマーク更新モードのボタン設定
    ''' 
    ''' </summary>
    Public Sub BtnBookMarkUpdateMode()
        clsFileMoveForm.btnMove.Visible = False
        clsFileMoveForm.btnDelete.Visible = False
        clsFileMoveForm.btnUpdate.Visible = False
        clsFileMoveForm.btnNowDel.Visible = False
        clsFileMoveForm.btnBookMarkAdd.Visible = False
        clsFileMoveForm.btnBookMarkUpdate.Visible = True
        clsFileMoveForm.btnBookMarkDel.Visible = True
        clsFileMoveForm.AcceptButton = clsFileMoveForm.btnBookMarkUpdate
    End Sub

    ''' <summary>
    ''' ブックマーク追加モードのボタン設定
    ''' 
    ''' </summary>
    Public Sub BtnBookMarkAddMode()
        clsFileMoveForm.btnMove.Visible = False
        clsFileMoveForm.btnDelete.Visible = False
        clsFileMoveForm.btnUpdate.Visible = False
        clsFileMoveForm.btnNowDel.Visible = False
        clsFileMoveForm.btnBookMarkAdd.Visible = True
        clsFileMoveForm.btnBookMarkUpdate.Visible = False
        clsFileMoveForm.btnBookMarkDel.Visible = False
        clsFileMoveForm.AcceptButton = clsFileMoveForm.btnBookMarkAdd
    End Sub

    Public Sub ImageGet(ByVal sFilePath As String, ByRef image As Image)

        Dim clsZipOpen As ZipOpen

        'コンストラクタでファイルパスを指定
        clsZipOpen = New ZipOpen(sFilePath)
        clsZipOpen.tagCreate(clsZipOpen.FileName)

        'イメージを取得し、フォームに設定
        image = clsZipOpen.Thumbs

    End Sub


    Public Sub ShowBookMark(ByVal bDataFlag As Boolean)
        If bDataFlag = True Then

            clsFileMoveForm.lstBookMark.Show()
            clsFileMoveForm.TableLayoutPanel1.SetRowSpan(clsFileMoveForm.TabControl1, 1)

            clsFileMoveForm.lstBookMark.Select()

        Else
            clsFileMoveForm.lstBookMark.Hide()
            clsFileMoveForm.TableLayoutPanel1.SetRowSpan(clsFileMoveForm.TabControl1, 4)
        End If

    End Sub

    Public Sub SetZokusei(ByRef lstZokusei As List(Of String))
        '属性チェックを配列に格納
        Dim chk As CheckBox
        For i = 1 To 15
            chk = CType(clsFileMoveForm.Controls("chkZoku" & i), CheckBox)
            If (chk.Checked = True) Then
                lstZokusei.Add(chk.Text)
            End If
        Next
    End Sub

    ''' <summary>
    ''' ランキング用の☆マーク（白抜き）
    ''' </summary>
    Private imgWhite As System.Drawing.Image = System.Drawing.Image.FromFile("C:\ProgramData\NeeView\Plugin\星白.png")

    ''' <summary>
    ''' ''' ランキング用の☆マーク（黒埋め）
    ''' </summary>
    Private imgBlack As System.Drawing.Image = System.Drawing.Image.FromFile("C:\ProgramData\NeeView\Plugin\星黒.png")

    Private Sub RankImageChange(ByVal iIdx As Integer)


        clsFileMoveForm.picRank1.Image = imgWhite
        clsFileMoveForm.picRank2.Image = imgWhite
        clsFileMoveForm.picRank3.Image = imgWhite
        clsFileMoveForm.picRank4.Image = imgWhite
        clsFileMoveForm.picRank5.Image = imgWhite

        If (iIdx = 0) Then
            Exit Sub
        End If

        Dim pic As PictureBox
        For i = 1 To iIdx
            pic = CType(clsFileMoveForm.Controls("picRank" & i), PictureBox)
            pic.Image = imgBlack
        Next

    End Sub

    Public Sub TagSet(ByRef lstTagName As List(Of String), ByRef lstTagCat As List(Of Integer))

        'タグ取得し、フォームに値を設定
        Dim clsOraAccess As OraAccess
        'DBアクセス用クラスのインスタンスを作成
        clsOraAccess = New OraAccess()

        Dim cmb As ComboBox
        Dim chk As CheckBox
        Dim iTagidx As Integer = 1

        For idx = 0 To lstTagName.Count - 1

            If lstTagCat.Count = 0 Then
                clsFileMoveForm.Controls("txtTag" & iTagidx).Text = lstTagName(idx)
                iTagidx = iTagidx + 1
                Continue For
            End If

            Dim chkFlag As Boolean = False
            '属性の場合
            If lstTagCat(idx) = 5 Then
                For i = 1 To 15
                    chk = CType(clsFileMoveForm.Controls("chkZoku" & i), CheckBox)
                    If chk.Text = lstTagName(idx) Then
                        chk.Checked = True
                        chkFlag = True
                        Exit For
                    End If
                Next
            End If
            If chkFlag = False Then
                clsFileMoveForm.Controls("txtTag" & iTagidx).Text = lstTagName(idx)
                cmb = CType(clsFileMoveForm.Controls("cmbTag" & iTagidx), ComboBox)
                cmb.SelectedValue = lstTagCat(idx)
                iTagidx = iTagidx + 1
            End If
        Next

    End Sub

    Public Function TreeCondGet(ByRef lstTagName As List(Of String), ByRef lstTagCat As List(Of Integer)) As String


        Dim sSakusya As String = Nothing

        For idx = 0 To lstTagName.Count - 1

            Select Case lstTagCat(idx)
                Case 1
                    Return lstTagName(idx)
                Case 2
                    If sSakusya Is Nothing Then
                        sSakusya = lstTagName(idx)
                    End If
            End Select
        Next

        Return sSakusya

    End Function

    Public Sub FormSet(ByRef clsFormBean As FormBean)

        'タグやチェックをクリア
        DataClear()

        clsFileMoveForm.txtFileName.Text = clsFormBean.title

        ''絞込み条件をタグから取得
        'sConditionValue = Nothing

        'イメージを取得し、フォームに設定
        clsFileMoveForm.picThumbs.Image = clsFormBean.thumbnail

        'パスをフォームに設定
        clsFileMoveForm.lblFilePath.Text = clsFormBean.fullpath

        'ファイルのサイズを取得
        clsFileMoveForm.lblFileSize.Text = clsFormBean.file_size

        'ランクを設定
        RankImageChange(clsFormBean.rank)

        'ファイル名をテキストボックスに設定
        clsFileMoveForm.txtTagSetting.Text = clsFormBean.file_name

        'フォルダ名をテキストボックスに設定
        clsFileMoveForm.lblNowFolder.Text = clsFormBean.folder_name

        'タグを設定
        TagSet(clsFormBean.tagname_lst, clsFormBean.tagcat_lst)

    End Sub

    Public Sub FormSet(ByRef clsBookMarkBean As BookMarkBean)

        'タグやチェックをクリア
        DataClear()

        clsFileMoveForm.txtFileName.Text = clsBookMarkBean.title

        ''絞込み条件をタグから取得
        'sConditionValue = Nothing


        'イメージを取得し、フォームに設定
        clsFileMoveForm.picThumbs.Image = clsBookMarkBean.thumbnail

        'パスをフォームに設定
        clsFileMoveForm.lblFilePath.Text = clsBookMarkBean.fullpath

        'ファイルのサイズを取得
        clsFileMoveForm.lblFileSize.Text = clsBookMarkBean.file_size

        'ランクを設定
        RankImageChange(clsBookMarkBean.rank)

        'ファイル名をテキストボックスに設定
        clsFileMoveForm.txtTagSetting.Text = clsBookMarkBean.file_name

        'タグを設定
        TagSet(clsBookMarkBean.tagname_lst, clsBookMarkBean.tagcat_lst)

    End Sub

End Class
