﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Course_ref_search.aspx.cs" Inherits="Course_ref_search" %>

<%@ Register Src="Airmacau.ascx" TagName="Airmacau1" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">

    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="Content-Language" content="zh-CN" />
    <meta name="author" content="Airmacau/ITD" />
    <meta name="Copyright" content="Airmacau" />
    <meta name="description" content="Airmacau" />
    <meta name="keywords" content="Airmacau" />
    <!--css-->
    <link href="css/style.css" rel="stylesheet" type="text/css" />



    <title>Course Reference Setting</title>


</head>

<body>

    <form id="form1" runat="server">

        <div id="backpanel">
            <uc1:Airmacau1 ID="Airmacau1" runat="server" />
            <%--<div class="menu">

<ul>
<li><a id="staffs" class="hide" href="Main.aspx" runat="server">Staff Settings</a>

    <ul>
    <li><a href="Department.aspx" title="Department">Department Setting</a></li>
    <li><a href="Position.aspx" title="Wrapping text around images">Division Setting</a></li>
    <li><a href="Title.aspx" title="Wrapping text around images">Title Setting</a></li>
    <li><a href="Staffin.aspx" title="Image Map for detailed information">Employee Setting</a></li>

    </ul>

</li>
<li><a id="Authors" style="width:150px" class="hide" href="Main.aspx" runat="server">Authorization Settings</a>

    <ul>
    <li><a href="Project.aspx" title="Styling forms">Project Setting</a></li>  
    <li><a href="Range.aspx" title="Multi-position drop shadow">Range Setting</a></li>
    <li><a href="Level.aspx" title="Removing active/focus borders">Level Setting</a></li>

    </ul>

</li>
<li><a id="Author" class="hide" href="Authorization.aspx" runat="server" >Authorization</a>

</li>
<li><a id="Authorss" class="hide" href="AuthorizationSelect.aspx" runat="server">AuthorizationSearch</a>

</li>
<li><a class="hide" href="../mozilla/index.html">Test</a>

</li>
</ul>
<!-- clear the floats if required -->
<div class="clear"> </div>
</div>--%>
            <div id="pagebody">


                <div id="Contentpanel">
                    <div id="linkweb" style="float: left; width: 500px; height: 25px; font-size: 14px;">
                    </div>
                    <script>    setInterval("linkweb.innerHTML=new Date().toLocaleString()+' 星期'+'日一二三四五六'.charAt(new Date().getDay());", 1000);
                    </script>
                    <div id="welcome" style="width: 500px; height: 25px; text-align: right; font-size: 14px; float: left;">
                        <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
                    </div>

                    <div style="height: 30px; text-align: center; color: black; font-size: large">
                        Course Reference Setting</div>
                    </br>
</br>

    <div style="height: 30px; text-align: center; color: black; font-size: large">
      Course Reference：<asp:TextBox ID="Class_Type" runat="server"></asp:TextBox>
             Course Type：<asp:TextBox ID="Course_Type" runat="server"></asp:TextBox>
        <asp:Button ID="search" runat="server" Height="25px" UseSubmitBehavior="false" OnClick="btn_search"
            Text="Search" Width="82px" />
         <asp:Button ID="add" runat="server" Height="25px" UseSubmitBehavior="false" OnClick="add_class"
            Text="Add" Width="82px" />
        <div style="text-align: center; color: black;">
            <table cellspacing="3" cellpadding="1" align="center" border="0"
                style="width: 800px">
                <td>
                       <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" AllowPaging="True" PagerSettings-Visible="False" PageSize="10" 
            onrowdeleting="GridView1_RowDeleting" Width="950px" 
            onrowdatabound="GridView1_RowDataBound" 
            onrowediting="GridView1_RowEditing"  
            onrowcancelingedit="GridView1_RowCancelingEdit" 
             OnSelectedIndexChanged="GridView1_SelectedIndexChanged"
            onrowupdating="GridView1_RowUpdating" BackColor="White" BorderColor="#999999" 
                        BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Vertical">

                        <RowStyle BackColor="#EEEEEE" ForeColor="Black" />

                        <Columns>
                            <asp:TemplateField HeaderText="ID" Visible="false">
                                <%--                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox1" MaxLength="20" Width="80" runat="server" Text='<%# Bind("ID") %>'></asp:TextBox>
                    </EditItemTemplate>--%>
                                <ItemTemplate>
                                    <asp:Label ID="Course_Ref_id" runat="server"  Text='<%# Bind("ID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Course Reference">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txClass_Type" MaxLength="80" Width="200" runat="server" Text='<%# Bind("Course_Ref") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton ID="laClass_Type" CommandName="Select" Width="200" CausesValidation="False" runat="server" Text='<%# Bind("Course_Ref") %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>

                                 <asp:TemplateField HeaderText="Course Type">
                                <EditItemTemplate>
                                    <asp:TextBox ID="ctxClass_Type" MaxLength="80" Width="200" runat="server" Text='<%# Bind("Course") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="claClass_Type" CommandName="Select" Width="200" CausesValidation="False" runat="server" Text='<%# Bind("Course") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                               <asp:TemplateField HeaderText="Training Type">
                                <EditItemTemplate>
                                    <asp:TextBox ID="ttxClass_Type" MaxLength="80" Width="80" runat="server" Text='<%# Bind("Training_Type") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="tlaClass_Type" CommandName="Select" Width="80" CausesValidation="False" runat="server" Text='<%# Bind("Training_Type") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>


                            <asp:TemplateField HeaderText="Action" ShowHeader="False">
                                <EditItemTemplate>
                                    <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="True"
                                        CommandName="Update" Text="update"></asp:LinkButton>
                                    &nbsp;<asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False"
                                        CommandName="Cancel" Text="cancel"></asp:LinkButton>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <%--<asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False"
                                        CommandName="Edit" Text="edit"></asp:LinkButton>--%>
                                    &nbsp;<asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False"
                                        CommandName="Delete" Text="Delete"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                        <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                        <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
                        <AlternatingRowStyle BackColor="#DCDCDC" />
                    </asp:GridView>
                    <asp:Label ID="Lab_PageCount" runat="server" Text="<%# ((GridView)Container.NamingContainer).PageCount %>"></asp:Label>
                    <asp:Label ID="Lab_CurrentPage" runat="server" Text="<%# ((GridView)Container.NamingContainer).PageIndex + 1 %>"></asp:Label>
                    &nbsp;<asp:Button ID="Button3" runat="server" OnClick="Button3_Click" Text="First Page" />
                    &nbsp;
                     <asp:Button ID="Button2" runat="server" UseSubmitBehavior="false" OnClick="Button2_Click" Text="〈" />
                    <asp:Button ID="Button1" runat="server" UseSubmitBehavior="false" OnClick="Button1_Click" Text="〉" />
                    <asp:Button ID="Button4" runat="server" UseSubmitBehavior="false" OnClick="Button4_Click" Text="Last Page" />
                    <asp:Button ID="Button5" runat="server" UseSubmitBehavior="false" OnClick="Button5_Click" Text="Turn To Page" />
                    <asp:TextBox ID="TextBox1" runat="server" Width="51px"></asp:TextBox>
                    <%-- <tr>
    <td colspan="2" style="padding:0 20px;text-align:center;"><a href="changepwd.aspx" target="mainFrame">Change Password</a></td>
    </tr>--%>
                </td>
            </table>
        </div>
    </div>
                </div>
            </div>
            <div id="copyright">
                Copyright © 2013 - All Rights Reserved Air Macau Company IT Division
            </div>
        </div>

    </form>
</body>
</html>
