<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CCLFQ.aspx.cs" Inherits="Home" %>

<%@ Register Src="~/NAV.ascx" TagName="NAV" TagPrefix="uc1" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta content='IE=edge,chrome=1' http-equiv='X-UA-Compatible' />
    <link rel="icon" href="images/airmacau.ico" />
    <title>Home</title>


    <%-- css --%>
    <link href="css/font-awesome/css/font-awesome.min.css" rel="stylesheet" />
    <link href="css/style.css" rel="stylesheet" />
    <link href="css/jquery-ui.css" rel="stylesheet" />

    <style type="text/css">
     
    </style>
    <%-- js--%>
    <script src="js/jquery.js" type="text/javascript"></script>
    <script src="js/jquery-ui.js" type="text/javascript"></script>

    <script>
        $(function () {
            $("#tabs").tabs();
            $("#From").datepicker(
                {

                });
            $("#To").datepicker();
        });


        function checkForm() {
            pass = true;
            $(".reqiredfield").each(function () {
                if (this.value == '') {
                    text = $(this).prev().text();
                    alert(text + " is required");
                    this.focus();
                    pass = false;
                    return false;//跳出each
                }
            });
            return pass;
        }


    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <uc1:NAV ID="NAV" runat="server" />
        </div>
        <div class="mainbody">
            <div class="bodydiv">
                <div style="font-family: Perpetua,AkzidenzGrotesk,Cambria; margin: auto; height: 50px; padding-top: 20px">
                    <div class="col-md-3">
                        <span style="color: red">*</span> <span>From</span>:<asp:TextBox ID="From" CssClass="reqiredfield" runat="server" Width="150px"></asp:TextBox>
                    </div>

                    <div class="col-md-3">
                        <span style="color: red">*</span><span>To</span>:<asp:TextBox ID="To" CssClass="reqiredfield" runat="server" Width="150px"></asp:TextBox>
                    </div>
                    <div class="col-md-2">
                        Load Factor:<asp:DropDownList ID="LF" runat="server">

                            <asp:ListItem Value="0.85">>=85%</asp:ListItem>
                            <asp:ListItem Value="0.6">>=60%</asp:ListItem>
                            <asp:ListItem Value="0.5">>=50%</asp:ListItem>

                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2" style="text-align:center">
                        <asp:LinkButton ID="Search" runat="server" Text="Search" CssClass="fa fa-search" OnClick="Search_Click" />
                    </div>
                    <div class="col-md-2">
                        <asp:LinkButton ID="Download" runat="server" Text="Download" CssClass="fa fa-download" OnClick="Download_Click" />
                    </div>
                </div>
                <div style="font-family: Perpetua,AkzidenzGrotesk,Cambria; margin: auto">
                    <div id="tabs">
                        <ul>
                            <li><a href="#tabs-1">Statistical Results</a></li>
                            <li><a href="#tabs-2">Detail Report</a></li>

                        </ul>
                        <div id="tabs-1" style="height: 300px; overflow: scroll">

                            <asp:Repeater ID="Report" runat="server">

                                <HeaderTemplate>
                                    <table class="grid grid-striped grid-hover" cellspacing="1" cellpadding="3" rules="cols" border="1" id="GridView1" style="border-color: #999999; border-width: 1px; border-style: solid; width: 800px; height: 300px; border-collapse: collapse; margin: auto">
                                        <tr>
                                            <th>StaffID</th>
                                            <th>Name</th>
                                            <th>Count</th>


                                        </tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <%--<td><a href="Training_Records_Print.aspx?staffno=<%#Eval("StaffID")%>"><%#Eval("StaffID")%></a></td>--%>
                                        <%--  <td><font><%#Eval("StaffID")%></font></td>--%>
                                        <td><font><%#Eval("StaffID")%></font></td>
                                        <td><font><%#Eval("Name")%></font></td>
                                        <td><font><%#Eval("Count")%></font></td>


                                    </tr>

                                </ItemTemplate>
                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                        </div>
                        <div id="tabs-2" style="height: 300px; overflow: scroll" hidden="hidden">

                            <asp:Repeater ID="Report_detail" runat="server">

                                <HeaderTemplate>
                                    <table class="grid grid-striped grid-hover" cellspacing="1" cellpadding="3" rules="cols" border="1" id="GridView1" style="border-color: #999999; border-width: 1px; border-style: solid; width: 800px; height: 300px; border-collapse: collapse; margin: auto">
                                        <tr>
                                            <th style="white-space: nowrap">FLT_DATE</th>
                                            <th>IATA_C</th>
                                            <th>FLT_ID</th>
                                            <th>AC_ID</th>
                                            <th>AC_TYPE</th>
                                            <th>DEP_APT</th>
                                            <th>ARR_APT</th>
                                            <th>CANCEL_FLAG</th>
                                            <th>SEAT</th>
                                            <th>PAX</th>
                                            <th>CABIN_STAFFNO</th>
                                            <th>CABIN_NAME</th>
                                            <th>CREW_NUMBER</th>

                                        </tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <%--<td><a href="Training_Records_Print.aspx?staffno=<%#Eval("StaffID")%>"><%#Eval("StaffID")%></a></td>--%>
                                        <%--  <td><font><%#Eval("StaffID")%></font></td>--%>
                                        <td style="white-space: nowrap"><font><%#Convert.ToDateTime(Eval("FLT_DATE")).ToString("dd-MM-yyyy", System.Globalization.DateTimeFormatInfo.InvariantInfo)%></font></td>
                                        <td><font><%#Eval("IATA_C")%></font></td>
                                        <td><font><%#Eval("FLT_ID")%></font></td>
                                        <td><font><%#Eval("AC_ID")%></font></td>
                                        <td><font><%#Eval("AC_TYPE")%></font></td>
                                        <td><font><%#Eval("DEP_APT")%></font></td>
                                        <td><font><%#Eval("ARR_APT")%></font></td>
                                        <td><font><%#Eval("CANCEL_FLAG")%></font></td>
                                        <td><font><%#Eval("SEAT")%></font></td>
                                        <td><font><%#Eval("PAX")%></font></td>
                                        <td><font><%#Eval("CABIN_STAFFNO")%></font></td>
                                        <td style="white-space: nowrap"><font><%#Eval("CABIN_NAME")%></font></td>
                                        <td><font><%#Eval("CREW_NUMBER")%></font></td>

                                    </tr>

                                </ItemTemplate>
                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>

                        </div>

                    </div>
                </div>


            </div>
        </div>
    </form>
</body>
</html>
