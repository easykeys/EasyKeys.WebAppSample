Imports DAL
Public Class EmailLogAdmin
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            BinddlEmailLog()
        End If
    End Sub
    Sub BinddlEmailLog()
        Dim EML As List(Of EmailLog) = EmailLog.GetAllEmailLogs()
        If EML?.Count > 0 Then
            dlEmailLog.DataSource = EML
            dlEmailLog.DataBind()
        End If
    End Sub
    Protected Sub dlEmailLog_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataListCommandEventArgs)
        Dim hfEmailLogID As HiddenField = TryCast(e.Item.FindControl("hfEmailLogID"), HiddenField)
        Dim EmailLogID As Integer = CInt(hfEmailLogID.Value)

        Dim txtToEmailList As TextBox = TryCast(e.Item.FindControl("txtToEmailList"), TextBox)
        Dim txtFromEmail As TextBox = TryCast(e.Item.FindControl("txtFromEmail"), TextBox)
        Dim txtSubject As TextBox = TryCast(e.Item.FindControl("txtSubject"), TextBox)

        If e.CommandName = "Update" Then
            Dim EditEmailLog As EmailLog = EmailLog.GetEmailLog(EmailLogID)
            EditEmailLog.ToEmailList = txtToEmailList.Text.Trim()
            EditEmailLog.FromEmail = txtFromEmail.Text.Trim()
            EditEmailLog.Subject = txtSubject.Text.Trim()
            EditEmailLog.Update()
            BinddlEmailLog()
        ElseIf e.CommandName = "Delete" Then
            Dim DeleteEmailLog As EmailLog = EmailLog.GetEmailLog(EmailLogID)
            DeleteEmailLog.Delete()
            BinddlEmailLog()
        End If
    End Sub
End Class
