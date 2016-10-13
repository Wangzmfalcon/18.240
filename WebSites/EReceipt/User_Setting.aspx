﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="User_Setting.aspx.cs" Inherits="User_Setting" %>

<%@ Register Src="~/NAV.ascx" TagName="NAV" TagPrefix="uc1" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta content='IE=edge,chrome=1' http-equiv='X-UA-Compatible' />
    <link rel="icon" href="images/airmacau.ico" />
    <title>Station Setting</title>


    <%-- css --%>
    <link href="css/font-awesome/css/font-awesome.min.css" rel="stylesheet" />
    <link href="css/jquery-ui.css" rel="stylesheet" />
    <link href="css/Style.css" rel="stylesheet" />
    <%-- js --%>


    <script src="js/jquery.js" type="text/javascript"></script>
    <script src="js/jquery-ui.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <uc1:NAV ID="NAV" runat="server" />
        </div>
        <div class="mainbody">
            <div class="bodydiv">

                <div style="padding-top: 20px; padding-bottom: 20px; font-family: Cambria, Helvetica, sans-serif;">
                    <div class="col-md-4">Login Name:<asp:TextBox ID="ID" runat="server"></asp:TextBox></div>
                    <div class="col-md-4">Password:<asp:TextBox ID="password" runat="server" TextMode="Password"></asp:TextBox></div>
                    <div class="col-md-4">Staff Name:<asp:TextBox ID="Staff_Name" runat="server"></asp:TextBox></div>




                </div>
                <div style="padding-bottom: 20px; padding-top: 20px; font-family: Cambria, Helvetica, sans-serif;">
                    <div class="col-md-4">
                        Admin Level:<asp:DropDownList ID="Level" runat="server"></asp:DropDownList>

                    </div>
                    <div class="col-md-4">
                        Station:<asp:DropDownList ID="Station" runat="server"></asp:DropDownList>
                        Receipt:<asp:DropDownList ID="Receipt" runat="server"></asp:DropDownList>
                    </div>
                    <div class="col-md-4">
                        <asp:LinkButton class="gridbutton" ID="Add" runat="server" Text="Add" OnClick="save_Click" />
                        <asp:LinkButton class="gridbutton" ID="Edit" runat="server" Text="Edit" OnClick="Edit_Click" />
                        <asp:LinkButton class="gridbutton" ID="Delete" runat="server" Text="Delete" OnClick="delete_Click" />
                    </div>
                </div>

                <div>

                    <asp:Repeater ID="User" runat="server">

                        <HeaderTemplate>
                            <table class="grid grid-striped grid-hover">
                                <tr>
                                    <th>Login Name</th>
                                    <th>Staff Name </th>
                                    <th>Last Login Time</th>
                                    <th>Admin Level</th>
                                    <th>Station</th>
                                    <th>Receipt</th>
                                    <%-- <th>Delete</th>--%>
                                </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td><a href="#" onclick="deleteitem('<%#Eval("User_Id") %>','<%#Eval("Password") %>','<%#Eval("User_Name") %>','<%#Eval("Manage_Level") %>','<%#Eval("Station") %>','<%#Eval("Receipt") %>')"><%#Eval("User_Id")%></a></td>

                                <td><font> <%#Eval("User_Name")%></font></td>
                                <td><font> <%#Eval("Last_Login_Time")%></font></td>
                                <td><font> <%#Eval("Manage_Level")%></font></td>
                                <td><font> <%#Eval("Station")%></font></td>
                                <td><font> <%#Eval("Receipt")%></font></td>
                                <%-- <td>
                            <a class="gridbutton" role="button" onclick="deleteitem('<%#Eval("id") %>','<%#Eval("STATION")%>','<%#Eval("STATION_CODE")%>','<%#Eval("STATION_NAME")%>')">Delete</a>

                        </td>--%>
                            </tr>
                        </ItemTemplate>
                        <%--   <AlternatingItemTemplate>
                    <tr>
                        <td><a href='Default.aspx?id=<%#"databaselogid" %>'><%#("STATION_NAME")%></a></td>
                    </tr>
                </AlternatingItemTemplate>--%>
                        <FooterTemplate>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>

                </div>





                <div class="pagebreak">

                    <div class="col-md-3">
                        <asp:Label ID="lblCurrentPage" runat="server"
                            Text="Label"></asp:Label>&nbsp;
                Total :<asp:Label ID="labPage" runat="server" Text="Label"></asp:Label>
                    </div>
                    <div class="col-md-3" style="float: right">
                        <div class="col-md-3">
                            <asp:HyperLink ID="first" runat="server"><i class="fa fa-angle-double-left" aria-hidden="true"></i></asp:HyperLink>

                            <asp:HyperLink ID="up" runat="server"><i class="fa fa-angle-left" aria-hidden="true"></i></asp:HyperLink>
                        </div>
                        <div class="col-md-3" style="float: right">
                            <asp:HyperLink ID="next" runat="server"><i class="fa fa-angle-right" aria-hidden="true"></i></asp:HyperLink>

                            <asp:HyperLink ID="last" runat="server"><i class="fa fa-angle-double-right" aria-hidden="true"></i></asp:HyperLink>
                        </div>
                    </div>
                </div>

            </div>

        </div>
    </form>

    <script>



        function deleteitem(i, a, b, c, d, e) {
            $("#ID").val(i);
            $("#password").val(a);
            $("#Staff_Name").val(b);
            $("#Level").val(c);
            $("#Station").val(d);
            $("#Receipt").val(e);
        }
    </script>
</body>
</html>
