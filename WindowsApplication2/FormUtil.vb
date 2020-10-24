Imports Oracle.DataAccess.Client
Imports Oracle.DataAccess.Types

Public Class FormUtil

    Private clsFileMoveForm As FileMoveForm

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

    Public Sub ImageSetFile(ByVal sFilePath As String)

        Dim clsZipOpen As ZipOpen

        'コンストラクタでファイルパスを指定
        clsZipOpen = New ZipOpen(sFilePath)
        clsZipOpen.tagCreate(clsZipOpen.FileName)

        'イメージを取得し、フォームに設定
        clsFileMoveForm.picThumbs.Image = clsZipOpen.Thumbs

    End Sub

    Public Sub ImageSetBookMark(ByVal sFilePath As String)

        Dim clsZipOpen As ZipOpen

        'コンストラクタでファイルパスを指定
        clsZipOpen = New ZipOpen()
        clsZipOpen.zipWorks(sFilePath)

        'イメージを取得し、フォームに設定
        clsFileMoveForm.picThumbs.Image = clsZipOpen.Thumbs
    End Sub


    'サムネイル追加
    Private icount As Integer
    Public Sub AddThumnail(ByRef bmp As Bitmap, ByRef imgLstThumbs As ImageList, ByRef lstViewThumbs As ListView, ByVal sFileName As String, ByVal lFileSize As Long, ByVal lRank As Long)

        Dim width As Integer = 96
        Dim height As Integer = 96

        imgLstThumbs.ImageSize = New Size(width, height)
        lstViewThumbs.LargeImageList = imgLstThumbs

        Dim original As Image = bmp
        Dim thumbnail As Image = createThumbnail(original, width, height)

        '一括登録用
        'clsOraAccess.updateFile(iNowId, thumbnail)

        imgLstThumbs.Images.Add(thumbnail)
        Dim lvi As New ListViewItem(sFileName, icount)
        lvi.ToolTipText = ChangeFileSize(lFileSize)

        'ランクにより文字背景色をかえる
        Select Case lRank
            Case 5
                lvi.BackColor = Color.Gold
            Case 4
                lvi.BackColor = Color.Silver
            Case 3
                lvi.BackColor = Color.Orange
            Case 2
                lvi.BackColor = Color.Aqua
            Case 1
                lvi.BackColor = Color.White
            Case Else
                lvi.BackColor = Color.Gray
        End Select

        '        lvi.BackColor = Color.Aqua
        lstViewThumbs.Items.Add(lvi)
        icount = icount + 1
        original.Dispose()
        thumbnail.Dispose()

    End Sub

    Public Sub AddThumnailClear()
        icount = 0
    End Sub



    Public Sub lstImageListInit(ByRef lstImgPath() As ArrayList)
        lstImgPath(0) = New ArrayList
        lstImgPath(1) = New ArrayList
        lstImgPath(2) = New ArrayList
        lstImgPath(3) = New ArrayList
        lstImgPath(4) = New ArrayList
    End Sub

    Public Sub lstImageListSet(ByRef lstImgPath() As ArrayList, ByRef readerFileList As OracleDataReader, ByRef ilstThumbs As ImageList, ByRef lstThumbs As ListView)

        While (readerFileList.Read())
            Dim blob As OracleBlob = readerFileList.GetOracleBlob(4)
            Dim ms As New System.IO.MemoryStream(blob.Value)
            AddThumnail(New Bitmap(ms), ilstThumbs, lstThumbs, readerFileList.GetString(1), readerFileList.GetValue(3), readerFileList.GetValue(5))

            lstImgPath(0).Add(readerFileList.GetValue(0))
            lstImgPath(1).Add(readerFileList.GetString(2))
            lstImgPath(2).Add(readerFileList.GetValue(5))
            lstImgPath(3).Add(readerFileList.GetValue(3))
            lstImgPath(4).Add(readerFileList.GetValue(7))
        End While
    End Sub

    Public Sub lstBookMarkPathInit(ByRef lstBookMarkPath() As ArrayList)
        lstBookMarkPath(0) = New ArrayList
        lstBookMarkPath(1) = New ArrayList
        lstBookMarkPath(2) = New ArrayList
        lstBookMarkPath(3) = New ArrayList
        lstBookMarkPath(4) = New ArrayList
        lstBookMarkPath(5) = New ArrayList
        lstBookMarkPath(6) = New ArrayList
    End Sub

    Public Sub lstBookMarkPathSet(ByRef lstBookMarkPath() As ArrayList, ByRef readerFileList As OracleDataReader, ByRef ilstBMThumbs As ImageList, ByRef lstBookMark As ListView)
        lstBookMarkPath(0) = New ArrayList

        While (readerFileList.Read())
            Dim blob As OracleBlob = readerFileList.GetOracleBlob(4)
            Dim ms As New System.IO.MemoryStream(blob.Value)
            AddThumnail(New Bitmap(ms), ilstBMThumbs, lstBookMark, readerFileList.GetString(1), readerFileList.GetValue(3), readerFileList.GetValue(5))

            lstBookMarkPath(0).Add(readerFileList.GetValue(0))
            lstBookMarkPath(1).Add(readerFileList.GetString(2))
            lstBookMarkPath(2).Add(readerFileList.GetValue(5))
            lstBookMarkPath(3).Add(readerFileList.GetValue(3))
            lstBookMarkPath(4).Add(readerFileList.GetValue(7))
            lstBookMarkPath(5).Add(readerFileList.GetValue(8))
            lstBookMarkPath(6).Add(readerFileList.GetString(9))
        End While
    End Sub

    Public Sub FileIDGet(ByRef readerFileList As OracleDataReader, ByRef iFileID As Integer, ByRef iFolderId As Integer)

        While (readerFileList.Read())

            iFileID = readerFileList.GetValue(0)
            iFolderId = readerFileList.GetValue(6)
            'sFolderName = readerFileList.GetString(8)

        End While

    End Sub

    Public Sub ShowBookMark(ByVal bDataFlag As Boolean)
        If bDataFlag = True Then

            clsFileMoveForm.lstBookMark.Show()
            clsFileMoveForm.TableLayoutPanel1.SetRowSpan(clsFileMoveForm.TabControl1, 1)
        Else
            clsFileMoveForm.lstBookMark.Hide()
            clsFileMoveForm.TableLayoutPanel1.SetRowSpan(clsFileMoveForm.TabControl1, 4)
        End If

    End Sub

    Public Sub SetZokusei(ByRef lstZokusei As List(Of String))
        '属性チェックを配列に格納
        Dim chk As CheckBox
        For i = 1 To 10
            chk = CType(clsFileMoveForm.Controls("chkZoku" & i), CheckBox)
            If (chk.Checked = True) Then
                lstZokusei.Add(chk.Text)
            End If
        Next
    End Sub

End Class
