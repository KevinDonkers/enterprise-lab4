<%@ Page Title="Register" Language="C#" MasterPageFile="~/monday.Master" AutoEventWireup="true" CodeBehind="register.aspx.cs" Inherits="lesson11.register" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <h1>Register</h1>
    <h6>All fields are required</h6>

    <div class="forn-group-lg">
        <asp:Label ID="lblStatus" runat="server" CssClass="label label-info" />
    </div>

    <div class="form-group">
        <label for="txtUsername" class="col-sm-2">UserName:</label>
        <asp:TextBox ID="txtUsername" runat="server" />
    </div>

    <div class="form-group">
        <label for="txtPassword" class="col-sm-2">Password:</label>
        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" />
    </div>

    <div class="form-group">
        <label for="txtConfirm" class="col-sm-2">Confirm Password:</label>
        <asp:TextBox ID="txtConfirm" runat="server" TextMode="Password" />
        <asp:CompareValidator runat="server" ControlToValidate="txtPassword" 
            ControlToCompare="txtConfirm" Operator="Equal" ErrorMessage="Passwords must Match" />
    </div>

    <div class="col-sm-offset-2">
        <asp:Button ID="btnRegister" runat="server" Text="Register" CssClass="btn btn-primary" OnClick="btnRegister_Click" />
    </div>
</asp:Content>
