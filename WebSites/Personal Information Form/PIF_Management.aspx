<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PIF_Management.aspx.cs" Inherits="PIF_Management" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Personal Information Management</title>
    <script src="./js/jquery.js"></script>
    <script src="./js/jquery-ui.min.js"></script>
  <%--   <script type="text/javascript" src="./js/jquery-2.2.1.min.js" charset="UTF-8"></script>--%>
    <script src="./js/gen_validatorv4.js"></script>
    <link rel="stylesheet" href="./css/jquery-ui.min.css">
    <link rel="stylesheet" href="./css/jquery-ui.theme.min.css">
    <link rel="stylesheet" href="./css/jquery-ui.structure.min.css">
</head>
<body>
    <form id="form1" runat="server">
        <div class="wrapper">
            <div class="center-block">
                <img src="./images/title.jpg" alt="">
            </div>
            <div class="form-basic">
                <div class="center-block" style="font-weight: 600">
                   Staff id<asp:TextBox ID="TextBox1" runat="server" CssClass="form-input"></asp:TextBox>
                    
                     <input type="checkbox" runat="server" id="only" /><span style="font-weight: 600">Can Download  </span>
                    <asp:Button ID="Button1" runat="server" Text="Button" class="btn" OnClick="Button1_Click" />
                </div>
                <input type="checkbox" id="selectall1" /><span style="font-weight: 600" >Select All  </span>
                
                <a href="#" onclick="downloadexcel()" style ="margin-left:500px;font-weight: 600">Download</a>
                <asp:Repeater ID="Repeater1" runat="server">
                    <HeaderTemplate>
                        <table id="table" class="table">
                            <tr class="tr">
                                <td class="td"></td>
                                <td class="td">Staff ID</td>
                                <td class="td">Staff Name</td>
                                <td class="td">Delete</td>
                                <td class="td">Release</td>                                
                            </tr>
                    </HeaderTemplate>
                    <ItemTemplate>

                        <%# trstyle((Container.ItemIndex+1))%>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
    </form>
    <!-- style -->
    <style>
        .center-block {
            clear: both;
            text-align: center;
        }

        .td {
            border: solid /*#add9c0*/;
            border-width: 0px 0px 1px 0px;
            font-size: 15px;
            border-top: 5px solid #6d5e8d;
            background-color: #E0E0E0;
        }

      
        .table {
            border: double /*#add9c0*/;
            border-width: 1px 1px 1px 1px;
            width: 720px;
             font-weight: 600;
        }

        .wrapper {
            width: 720px;
            margin-top: 5px;
            margin-left: auto;
            margin-right: auto;
            background-color: #fff;
        }

        .form-basic {
            display: block;
            padding-bottom: 10px;
            border: 1px solid #eee;
            border-bottom: 1px solid #ccc;
        }

            .form-basic textarea {
                margin-left: 20px;
                padding-left: 12px;
                padding-top: 6px;
                width: 250px;
                height: 60px;
                outline: 0;
            }

            .form-basic input[type="checkbox"] {
                margin-left: 20px;
                height: 20px;
                width: 20px;
                border: 1px solid #aaa;
                border-top: 0;
                color: #513c8a;
            }

            .form-basic input[type="radio"] {
                margin-left: 20px;
            }


        .form-input {
            width: 250px;
            height: 24px;
            margin-left: 20px;
            padding: 4px 12px;
            font-size: 13px;
            font-weight: 600;
            line-height: 18px;
            border-radius: 4px;
            border: 1px solid #aaa;
            color: #555;
            background-color: #fff;
            box-shadow: inset 0 1px 1px rgba(0,0,0,.075);
            -webkit-transition: border-color ease-in-out .15s,-webkit-box-shadow ease-in-out .15s, color .15s;
            transition: border-color ease-in-out .15s,box-shadow ease-in-out .15s, color: ease-in-out .15s;
        }

            .form-input:focus, .form-select:focus, textarea:focus {
                font-size: 13px;
                font-weight: 600;
                color: #513c8a;
                border-color: #9f54ad;
                outline: 0;
                box-shadow: inset 0 1px 1px rgba(0,0,0,.075),0 0 7px rgba(102,175,233,.5);
            }

        .btn {
            display: inline-block;
            margin-bottom: 10px;
            margin-top: 10px;
            margin-left: 10px;
            margin-right: auto;
            vertical-align: central;
            cursor: pointer;
            padding: 8px 10px;
            font-size: 12px;
            font-weight: 600;
            line-height: 1.333;
            border-radius: 6px;
            color: #563d7c;
            text-decoration: none;
            background-color: transparent;
            border-color: #563d7c;
            border: 1px solid #563d7c;
            touch-action: manipulation;
        }

          .btndis {
            display: inline-block;
            margin-bottom: 10px;
            margin-top: 10px;
            margin-left: 10px;
            margin-right: auto;
            vertical-align: central;
            cursor: pointer;
            padding: 8px 10px;
            font-size: 12px;
            font-weight: 600;
            line-height: 1.333;
            border-radius: 6px;
            color: #563d7c;
            text-decoration: none;
            background-color: transparent;
            border-color: #563d7c;
            border: 1px solid #563d7c;
            touch-action: manipulation;
         background-color: #E0E0E0;
    }
            .btn:hover {
                color: #fff;
                background-color: #563d7c;
                border-color: #563d7c;
            }
    </style>
    <!-- script -->
   
    <script>

        $("#selectall1").click(function () {
            if ($(this).is(":checked")) {

                $("[name=items1]:checkbox").prop("checked", true);

            } else {
                $("[name=items1]:checkbox").prop("checked", false);
            }




        });

        function downloadexcel() {
            var count = 0;
            var staffid = "";


            //console.log($("[name=items1]:checkbox").length);
            for (i = 0; i < $("[name=items1]:checkbox").length; i++) {

                var a = $("[name=items1]:checkbox")[i];
                //console.log(a.id);
                //console.log($("#"+a.id).attr("checked"));
                if (a.checked == true) {
                    //console.log(a.id);
                    if (count == 0)
                        staffid = staffid + a.id;
                    else
                        staffid = staffid + "|" + a.id;
                    count++;

                }


            }

            if (count == 0)
                alert("Please select at least one staff")
            else {
                //$.post("DownloadExcel.aspx?StaffID=" + staffid, function (data) { });
                window.open("DownloadExcel.aspx?StaffID=" + staffid)

            }
            console.log("count:" + count);
            console.log("staffid:" + staffid);


        }
        function release(id) {

            $.post("ReleastRecord.aspx?StaffID=" + id,
                function (data) {
                    if (data == "Y")
                        document.getElementById(id).disabled = false;
       
                    alert("Release success!")
                  
                });

        }


        function Delete(id) {

            $.post("DeleteRecord.aspx?StaffID=" + id,
                function (data) {
                    if (data == "Y")
                        document.getElementById(id).disabled = false;

                    alert("Delete success!")

                });

        }

        
    </script>


</body>

</html>
