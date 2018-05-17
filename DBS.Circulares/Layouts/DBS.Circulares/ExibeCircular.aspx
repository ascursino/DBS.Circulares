<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExibeCircular.aspx.cs" Inherits="DBS.Circulares.Layouts.DBS.Circulares.ExibeCircular" DynamicMasterPageFile="~masterurl/default.master" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">

<script type="text/javascript">

    //Close popup on cancel button click

    function CloseForm() {

        window.frameElement.cancelPopUp();
        return false;

    }
    
</script>

</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">

<asp:Label ID="lblErro" runat="server"></asp:Label>

<table>
    <tr>
        <td class="style1" colspan="2">
            <asp:Label ID="lblTitulo" runat="server"></asp:Label></td>
    </tr>
    <tr>
        <td class="style1" colspan="2">
            <asp:Label ID="lblDescricao" runat="server"></asp:Label></td>
    </tr>
    <tr>
        <td class="style2">
            <asp:Label ID="lblCriadoEm" runat="server"></asp:Label></td>
        <td>
            Ciente: <asp:Label ID="lblCiente" runat="server"></asp:Label></td>
    </tr>
    <tr>
        <td class="style2">
            <asp:Label ID="lblLinks" runat="server"></asp:Label></td>
        <td>
            &nbsp;</td>
    </tr>
    <tr>
        <td class="style2">
            &nbsp;</td>
        <td>
            &nbsp;</td>
    </tr>
    <tr>
        <td class="style2">
            <asp:Button ID="btnMarcarCiente" runat="server" Text="Marcar Ciente" OnClick="btnMarcarCiente_Click"/>
        </td>
        <td>
            <asp:Button ID="btnFechar" runat="server" Text="Fechar" OnClick="btnFechar_Click"/>
        </td>
    </tr>
</table>


</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
Application Page
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server" >
My Application Page
</asp:Content>
