Public Class BookMarkImateListView

    Private clsFileMoveForm As FileMoveForm

    Public Sub New(ByRef clsFMF As FileMoveForm)
        clsFileMoveForm = clsFMF
    End Sub

    '' <summary>
    '' イメージリスト作成
    '' </summary>
    Public Sub BookMarkImageListCreate(ByRef clsBookMarkImageListBeans As List(Of BookMarkBean))


        'clsFileMoveForm.TabControl1.SelectedTab = clsFileMoveForm.TabPage2
        clsFileMoveForm.lstBookMark.Clear()
        clsFileMoveForm.ilstBMThumbs.Images.Clear()

        For Each clsBookMarkImageListBean As BookMarkBean In clsBookMarkImageListBeans

            AddThumnail(clsBookMarkImageListBean.thumbnail, clsFileMoveForm.ilstBMThumbs, clsFileMoveForm.lstBookMark, clsBookMarkImageListBean.title, clsBookMarkImageListBean.file_size, clsBookMarkImageListBean.rank, clsBookMarkImageListBean.fullpath)

        Next

    End Sub

    'サムネイル追加
    Private icount As Integer
    Public Sub AddThumnail(ByRef bmp As Image, ByRef imgLstThumbs As ImageList, ByRef lstViewThumbs As ListView, ByVal sFileName As String, ByVal sFileSize As String, ByVal lRank As Long, ByVal sFilePath As String)

        Dim width As Integer = 96
        Dim height As Integer = 96

        imgLstThumbs.ImageSize = New Size(width, height)
        lstViewThumbs.LargeImageList = imgLstThumbs

        imgLstThumbs.Images.Add(bmp)
        Dim lvi As New ListViewItem(sFileName, icount)
        'lvi.ToolTipText = sFileSize
        lvi.SubItems.Add(sFilePath)

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

        lstViewThumbs.Items.Add(lvi)
        icount = icount + 1

    End Sub
End Class
