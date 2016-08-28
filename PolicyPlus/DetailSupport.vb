﻿Public Class DetailSupport
    Public Sub PresentDialog(Supported As PolicyPlusSupport)
        NameTextbox.Text = Supported.DisplayName
        IdTextbox.Text = Supported.UniqueID
        DefinedTextbox.Text = Supported.RawSupport.DefinedIn.SourceFile
        DisplayCodeTextbox.Text = Supported.RawSupport.DisplayCode
        Select Case Supported.RawSupport.Logic
            Case AdmxSupportLogicType.AllOf
                LogicTextbox.Text = "Match all the referenced products"
            Case AdmxSupportLogicType.AnyOf
                LogicTextbox.Text = "Match any of the referenced products"
            Case AdmxSupportLogicType.Blank
                LogicTextbox.Text = "Do not match products"
        End Select
        EntriesListview.Items.Clear()
        If Supported.Elements IsNot Nothing Then
            For Each element In Supported.Elements
                If element.Product Is Nothing Then
                    ' Some ADMX writers thought support definitions could refer to other support definitions
                    EntriesListview.Items.Add("<missing: " & element.RawSupportEntry.ProductID & ">")
                Else
                    Dim lsvi = EntriesListview.Items.Add(element.Product.DisplayName)
                    If element.RawSupportEntry.IsRange Then
                        If element.RawSupportEntry.MinVersion.HasValue Then lsvi.SubItems.Add(element.RawSupportEntry.MinVersion.Value) Else lsvi.SubItems.Add("")
                        If element.RawSupportEntry.MaxVersion.HasValue Then lsvi.SubItems.Add(element.RawSupportEntry.MaxVersion.Value) Else lsvi.SubItems.Add("")
                    End If
                    lsvi.Tag = element.Product
                End If
            Next
        End If
        ShowDialog()
    End Sub
    Private Sub EntriesListview_ClientSizeChanged(sender As Object, e As EventArgs) Handles EntriesListview.ClientSizeChanged, Me.Shown
        ChName.Width = EntriesListview.ClientSize.Width - ChMinVer.Width - ChMaxVer.Width
    End Sub
    Private Sub EntriesListview_DoubleClick(sender As Object, e As EventArgs) Handles EntriesListview.DoubleClick
        If EntriesListview.SelectedItems.Count = 0 Then Exit Sub
        Dim product = EntriesListview.SelectedItems(0).Tag
        If product Is Nothing Then Exit Sub
        DetailProduct.PresentDialog(product)
    End Sub
End Class