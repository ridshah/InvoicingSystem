<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="InvoicingSystem.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--
        The stylesheet and the javascript files can also be downloaded and added to the project's Scripts and Styles folder.
        The reference should then point to the local path.
        This will let us manage the code locally.
    --%>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/2.1.3/jquery.min.js"></script>
    <script type="text/javascript" charset="utf8" src="https://cdn.datatables.net/1.10.19/js/jquery.dataTables.js"></script>
    <script type="text/javascript" charset="utf8" src="/Scripts/Default.js"></script>
    <link rel="stylesheet" type="text/css" href="https://cdn.datatables.net/1.10.19/css/jquery.dataTables.css" />
    <div id="dataSection" runat="server">
    </div>
    <div>
        <table>
            <tr>
                <td>
                    <asp:Button ID="DownloadCSV" Text="Download All As CSV" OnClick="DownloadCSV_Click" runat="server" /></td>
                <td>
                    <asp:Button ID="DownloadCustomerCSV" Text="Download Customer Report As CSV" OnClick="DownloadCustomerCSV_Click" runat="server" /></td>
            </tr>
        </table>
    </div>
</asp:Content>
