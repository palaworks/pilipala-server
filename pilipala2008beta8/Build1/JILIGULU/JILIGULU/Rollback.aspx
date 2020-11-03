<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Rollback.aspx.cs" Inherits="JILIGULU.Rollback" %>

<!DOCTYPE html>
<html lang="zh-CN">

<head>
    <script src="https://cdn.jsdelivr.net/npm/vue/dist/vue.js"></script>
    <script src="https://cdn.bootcss.com/jquery/1.12.4/jquery.min.js"></script>

    <link rel="stylesheet" href="UI/css/global.css">
    <link rel="stylesheet" href="UI/css/font.css">

    <link rel="stylesheet" href="UI/css/Home.css">

    <link rel="stylesheet" href="UI/css/Reg.css">
    <link rel="stylesheet" href="UI/css/Dispose.css">
    <link rel="stylesheet" href="UI/css/Update.css">
    
    <link rel="stylesheet" href="UI/css/Delete.css">
    <link rel="stylesheet" href="UI/css/Apply.css">
    <link rel="stylesheet" href="UI/css/Rollback.css">
    <link rel="stylesheet" href="UI/css/Release.css">
</head>

<body>
    <button class="Home" onclick="location.href='Home.aspx'">Home</button>

    <button class="Reg" onclick="location.href='Reg.aspx'">Reg</button>
    <button class="Dispose" onclick="location.href='Dispose.aspx'">Dispose</button>
    <button class="Update" onclick="location.href='Update.aspx'">Update</button>

    <button class="Delete" onclick="location.href='Delete.aspx'">Delete</button>
    <button class="Apply" onclick="location.href='Apply.aspx'">Apply</button>
    <button class="Rollback" onclick="location.href='Rollback.aspx'">Rollback</button>
    <button class="Release" onclick="location.href='Release.aspx'">Release</button>

    <form class="Box" runat="server">
        <asp:Button ID="CommitBtn" runat="server" Text="提交" CssClass="CommitBtn" OnClick="CommitBtn_Click" />

        <textarea runat="server" id="ID" class="ID" placeholder="ID"></textarea>
    </form>
</body>