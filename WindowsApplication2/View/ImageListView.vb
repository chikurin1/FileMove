Public Class ImageListView

    Private clsFileMoveForm As FileMoveForm

    Public Sub New(ByRef clsFMF As FileMoveForm)
        clsFileMoveForm = clsFMF
    End Sub

    '' <summary>
    '' イメージリスト作成
    '' </summary>
    Public Sub ImageListCreate(ByRef clsImageListBeans As List(Of ImageListBean))

        'clsFileMoveForm.TabControl1.SelectedTab = clsFileMoveForm.TabPage2
        clsFileMoveForm.lstThumbs.Clear()
        clsFileMoveForm.ilstThumbs.Images.Clear()

        For Each clsImageListBean As ImageListBean In clsImageListBeans

            AddThumnail(clsImageListBean.thumbnail, clsFileMoveForm.ilstThumbs, clsFileMoveForm.lstThumbs, clsImageListBean.title, clsImageListBean.file_size, clsImageListBean.rank, clsImageListBean.fullpath, clsImageListBean.file_id)

        Next

    End Sub

    'サムネイル追加
    Private icount As Integer
    Public Sub AddThumnail(ByRef bmp As Image, ByRef imgLstThumbs As ImageList, ByRef lstViewThumbs As ListView, ByVal sFileName As String, ByVal sFileSize As String, ByVal lRank As Long, ByVal sFilePath As String, ByVal iFIleId As Integer)

        Dim width As Integer = 96
        Dim height As Integer = 96

        imgLstThumbs.ImageSize = New Size(width, height)
        lstViewThumbs.LargeImageList = imgLstThumbs


        imgLstThumbs.Images.Add(bmp)
        Dim lvi As New ListViewItem(sFileName, icount)
        lvi.ToolTipText = sFileSize
        lvi.SubItems.Add(sFilePath)
        lvi.SubItems.Add(iFIleId)

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

    Public Sub ImageListChange(ByRef clsFormBean As FormBean)

        If clsFormBean Is Nothing Then
            Exit Sub
        End If

        For i = 0 To clsFileMoveForm.lstThumbs.Items.Count - 1

            If clsFileMoveForm.lstThumbs.Items(i).SubItems(2).Text = clsFormBean.file_id Then

                clsFileMoveForm.lstThumbs.Items(i).Text = clsFormBean.title
                clsFileMoveForm.lstThumbs.Items(i).SubItems(1).Text = clsFormBean.fullpath

                Select Case clsFormBean.rank
                    Case 5
                        clsFileMoveForm.lstThumbs.Items(i).BackColor = Color.Gold
                    Case 4
                        clsFileMoveForm.lstThumbs.Items(i).BackColor = Color.Silver
                    Case 3
                        clsFileMoveForm.lstThumbs.Items(i).BackColor = Color.Orange
                    Case 2
                        clsFileMoveForm.lstThumbs.Items(i).BackColor = Color.Aqua
                    Case 1
                        clsFileMoveForm.lstThumbs.Items(i).BackColor = Color.White
                    Case Else
                        clsFileMoveForm.lstThumbs.Items(i).BackColor = Color.Gray
                End Select

                Exit For

            End If
        Next
    End Sub

End Class
