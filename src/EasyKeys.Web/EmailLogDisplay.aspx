<%@ Page Title="Email Log Display" Language="vb" AutoEventWireup="false" Debug="true" %>
<%@ Import Namespace="DAL" %>
<%@ Import Namespace="System" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<script runat="server">
    Public Body as String = String.Empty
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            pnlShowEmail.Visible = False
            BindddlEmailLogID()
        End If
    End Sub
    Sub BindddlEmailLogID()
        Dim EML As List(Of DAL.EmailLog) = DAL.EmailLog.GetAllEmailLogs()
        If EML?.Count > 0 Then
            ddlEmailLogID.Items.Insert(0, New ListItem("Select Email Log ID", "-1"))
            ddlEmailLogID.DataValueField = "EmailLogID"
            ddlEmailLogID.DataTextField = "EmailLogID"
            ddlEmailLogID.DataSource = EML
            ddlEmailLogID.DataBind()
        End If
    End Sub
    Protected Sub ddlEmailLogID_SelectedIndexChanged(sender As Object, e As EventArgs)

        If ddlEmailLogID.SelectedValue <> "-1" then

            Try
                lblTitle.Text = "Email for Email Log ID: " & ddlEmailLogID.SelectedValue
                pnlShowEmail.Visible = True
                Dim EML As DAL.EmailLog = DAL.EmailLog.GetEmailLog(CInt(ddlEmailLogID.SelectedValue))
                lblFrom.Text = EML.FromEmail
                lblTo.Text = EML.ToEmailList
                lblSubject.Text = EML.Subject
                Body = EML.Body

            Catch ex As Exception
                Dim exceptionAudit As New DAL.Audit(DAL.AuditEvent.EXCEPTION_EVENT,
                   "Email Log Display" & vbCrLf & "-------" & ex.ToString())
                exceptionAudit.Add()
            End Try
        Else
            pnlShowEmail.Visible = False
        End If

    End Sub
</script>
    <table cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td style="height:30px; background-color:#F59F1A;text-align:center">
                <span style="font-size:large;color:white">EMail Log Display</span>
            </td>
        </tr>
        <tr>
            <td style="text-align:center">
                <br />
                <asp:DropDownList ID="ddlEmailLogID" Width="200px" Font-Bold="true" ValidateRequestMode="Disabled" Font-Size="medium" Font-Names="Arial, Sans-Serif, Verdana" AppendDataBoundItems="true" AutoPostBack="true" OnSelectedIndexChanged="ddlEmailLogID_SelectedIndexChanged"  runat="server" />
            </td>
        </tr>
    </table>
    <asp:Panel ID="pnlShowEmail" runat="server">
        <br />
        <table cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td style="width:25%">
                    &nbsp;
                </td>
                <td style="width:50%">
                    <table style="font-family:Arial" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td colspan="2" style="text-align:center">
                                <asp:Label ID="lblTitle" ForeColor="#F59F1A" ValidateRequestMode="Disabled" Font-Bold="true" Font-Size="large" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td style="color:#F59F1A;font-weight:bold;font-size:medium;text-align:right">
                                <br />
                                From:&nbsp
                            </td>
                            <td>
                                <br />
                                &nbsp;<asp:Label ID="lblFrom" ValidateRequestMode="Disabled" ForeColor="#000000" Font-Bold="true" Font-Size="medium" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td style="color:#F59F1A;font-weight:bold;font-size:medium;text-align:right">
                                <br />
                                To:&nbsp;
                            </td>
                            <td>
                                <br />
                                &nbsp;<asp:Label ID="lblTo" ForeColor="#000000" ValidateRequestMode="Disabled" Font-Bold="true" Font-Size="medium" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td style="color:#F59F1A;font-weight:bold;font-size:medium;text-align:right">
                                <br />
                                Subject:&nbsp;
                            </td>
                            <td>
                                <br />
                                &nbsp;<asp:Label ID="lblSubject" ForeColor="#000000" ValidateRequestMode="Disabled" Font-Bold="true" Font-Size="medium" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td style="color:#F59F1A;font-weight:bold;font-size:medium;text-align:right;vertical-align:top">
                                <br />
                                Body:&nbsp;
                            </td>
                            <td>
                                <br />
                                <%If Not Body Is Nothing Then
                                        litBody.Text = Body
                                    ElseIf Body.Trim() = String.Empty Then
                                        litBody.Text = "NO BODY FOR THIS EMAIL"
                                    End IF
                                %>
                                    <asp:Literal ID="litBody" ValidateRequestMode="Disabled" runat="server" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="width:25%">
                    &nbsp;
                </td>
            </tr>
        </table>
    </asp:Panel>

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">
</asp:Content>
