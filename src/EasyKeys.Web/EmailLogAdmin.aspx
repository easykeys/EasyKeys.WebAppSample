<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Plain.master" CodeBehind="EmailLogAdmin.aspx.vb" Inherits=".EmailLogAdmin" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table cellpadding="0" cellspacing="0" style="width:100%">
        <tr>
            <td style="height:30px; background-color:#F59F1A;text-align:center">
                <span style="font-size:large;color:white">EMail Log Admin</span>
            </td>
        </tr>
        <tr>
            <td>
                <br />
                <asp:DataList ID="dlEmailLog" Width="60%" RepeatDirection="Vertical" OnItemCommand="dlEmailLog_ItemCommand" BorderColor="#F59F1A" BorderWidth="1px" runat="server">
                    <ItemStyle BackColor="#FFFFFF" />
                    <AlternatingItemStyle BackColor="#F5CC8B" />
                    <ItemTemplate>
                        <asp:HiddenField ID="hfEmailLogID" Value='<%# DataBinder.Eval(Container.DataItem, "EmailLogID") %>' runat="server" />
                        <table cellpadding="0" cellspacing="0" border="0" style="width: 100%">
                            <tr>
                                <td style="width:27%; height:36px; text-align:center">
                                    <asp:TextBox ID="txtToEmailList" MaxLength="500" Width="200px" Text='<%# DataBinder.Eval(Container.DataItem, "ToEmailList") %>' runat="server" />
                                </td>
                                <td style="width:27%; text-align:center">
                                    <asp:TextBox ID="txtFromEmail" MaxLength="500" Width="200px" Text='<%# DataBinder.Eval(Container.DataItem, "FromEmail") %>' runat="server" />
                                </td>
                                <td style="width:30%; text-align:center">
                                    <asp:TextBox ID="txtSubject" MaxLength="500" Width="300px" Text='<%# DataBinder.Eval(Container.DataItem, "Subject") %>' runat="server" />
                                </td>
                                <td style="width:8%; text-align:center">
                                    <asp:Button ID="btnUpdate" CommandName="Update" Text="Update" runat="server" />
                                </td>
                                <td style="width:8%; text-align:center">
                                    <asp:Button ID="btnDelete" CommandName="Delete" Text="Delete" runat="server" />
                                </td>
                                
                            </tr>
                        </table>
                    </ItemTemplate>
                </asp:DataList>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">
</asp:Content>
