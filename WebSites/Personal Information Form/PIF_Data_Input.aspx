<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PIF_Data_Input.aspx.cs" Inherits="PIF_Date_Input" %>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8" content="text/html; charset=utf-8">
    <title>Personal Information</title>
    <script src="./js/jquery.js"></script>
    <script src="./js/jquery-ui.min.js"></script>
    <script src="./js/gen_validatorv4.js"></script>
    <link rel="stylesheet" href="./css/jquery-ui.min.css">
    <link rel="stylesheet" href="./css/jquery-ui.theme.min.css">
    <link rel="stylesheet" href="./css/jquery-ui.structure.min.css">
</head>
<body>
    <form>
        <div class="wrapper">
            <div class="center-block">
                <img src="./images/title.jpg" alt="">
            </div>
            <div class="section">
                <div action="" class="form-basic" id="personalform">
                    <h3>1.Personal Particulars</h3>
                    <div class="form-group">
                        <label for="Name"><span>*</span>Name:</label>
                        <input type="text" class="form-input textonly" name="UserName" required="required" min="2" title="">
                        <label for="" class="righthint hiddenelement">must not null</label>
                    </div>
                    <div class="form-group">
                        <label for="Name"><span>*</span>Staff No:</label>
                        <input type="text" class="form-input numberonly" name="UserStaffNumber" required="required" title="">
                        <label for="" class="righthint hiddenelement">must not null</label>
                    </div>
                    <div class="form-group ">
                        <label for="Name"><span>*</span>Marital Status:</label>
                        <select name="MaritalStatus" class="form-select" onchange="marriedSt(this)" required="required">
                            <option value="Single">Single</option>
                            <option value="Married">Married</option>
                        </select>
                        <label for="" class="righthint hiddenelement">must not null</label>
                    </div>
                    <div class="form-group disabel-function">
                        <label for="Name">Marital Date:</label>
                        <input type="text" class="form-input datepicker" name="MaritalDate">
                    </div>
                    <h4>Address in Macau:</h4>
                    <div class="form-group">
                        <label for="home in Macau">Address:</label>
                        <textarea name="AddressinMacau" id="" cols="55" rows="4" wrap="hard"></textarea>
                    </div>
                    <div class="form-group">
                        <label for="PhoneinMacau">Phone:</label>
                        <input type="text" name="PhoneinMacau" class="form-input phone">
                    </div>
                    <div class="form-group">
                        <label for="MacauExtention">Extention:</label>
                        <input type="text" name="MacauExtention" class="form-input numberonly">
                    </div>
                    <div class="form-group">
                        <label for="prefer-address">Prefered :</label>
                        <%--       <select name="Macau" class="form-select" onchange="marriedSt(this)" required="required">
                            <option value="Single">Single</option>
                            <option value="Married">Married</option>
                        </select>--%>
                        <input class="prefer" type="checkbox" name="MAddressprefered" onclick="checkgrouponly(this)" id="prefermacau"></input>
                    </div>
                    <h4>Address in Homebase:</h4>
                    <div class="form-group">
                        <label for="homecountry">Country:</label>
                        <select name="CountryinHomebase" id="" class="form-select">
                            <asp:Repeater ID="countoption1" runat="server">
                                <%--         <option value="*">*</option>--%>
                                <ItemTemplate>
                                    <%# countoptionstyle((Container.ItemIndex+1))%>
                                </ItemTemplate>
                            </asp:Repeater>
                        </select>
                    </div>
                    <div class="form-group">
                        <label for="homeinhomebase">Address:</label>
                        <textarea name="AddressinHomebase" id="" cols="55" rows="4" wrap="hard"></textarea>
                    </div>
                    <div class="form-group">
                        <label for="phoneinhomebase">Phone:</label>
                        <input type="text" class="form-input" name="PhoneinHomebase">
                    </div>
                    <div class="form-group">
                        <label for="hombasicextention">extention:</label>
                        <input type="text" class="form-input numberonly" name="HomebaseExtention">
                    </div>
                    <div class="form-group">
                        <label for="prefer-address">Prefered:</label>
                        <input class="prefer" type="checkbox" id="preferhombase" name="HAddressprefered" onclick="checkgrouponly(this)">
                    </div>
                    <div class="section" id="section2">


                        <h3>2.Family contact</h3>
                        <div action="" class="form-basic" name="familyform">
                            <div class="row">
                                <div class=" form-group inline-group">
                                    <label for="family-relationship"><span>*</span>Relationship:</label>
                                    <select name="Familyrelationship" id="" class="form-select" required="required">
                                        <asp:Repeater ID="familyop" runat="server">
                                            <%--         <option value="*">*</option>--%>
                                            <ItemTemplate>
                                                <%# familyoptionstyle((Container.ItemIndex+1))%>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </select>
                                </div>
                                <div class=" form-group inline-group">
                                    <label for="familyNameType"><span>*</span>NameType:</label>
                                    <select name="FamilyNameType" class="form-select" required="required" name="nametype">
                                        <option value="Chinese">Chinese</option>
                                        <option value="Chinese">English</option>
                                    </select>
                                </div>
                            </div>
                            <div class="row">
                                <div class="form-group inline-group">
                                    <label for=""><span>*</span>Last name:</label>
                                    <input type="text" class="form-input" required="required" name="RelationshipLastname">
                                </div>
                                <div class="form-group inline-group">
                                    <label for=""><span>*</span>First name:</label>
                                    <input type="text" class="form-input" required="required" name="RelationshipFirstname">
                                </div>
                            </div>
                            <div class="row">
                                <div class="form-group inline-group">
                                    <label for=""><span>*</span>Sex:</label>
                                    <select name="FamilySex" id="" class="form-select" required="required">
                                        <%--  <option value="">*</option>--%>
                                        <option value="Male">Male</option>
                                        <option value="Female">Female</option>
                                    </select>
                                </div>
                                <div class="form-group inline-group">
                                    <label for="dateofbirth"><span>*</span>Date Of Birth:</label>
                                    <input type="text" class="form-input datepicker " required="required" name="FamilyDateofBirth">
                                </div>
                                <div class="form-group inline-group">
                                    <label for="occupation">Occupation:</label>
                                    <input type="text" class="form-input" name="FamilyOccupation">
                                </div>

                                  <div class="form-group inline-group">
                                    <label for="occupation"><span>*</span>Marital Status:</label>
                                     <select name="FMaritalStatus" class="form-select" onchange="marriedSt(this)" required="required">
                            <option value="Single">Single</option>
                            <option value="Married">Married</option>
                        </select>
                                </div>
                            </div>
                            <div class="row">
                        <%--        <div class="form-group inline-group">
                                    <label for="addresstype">Address Type</label>
                                    <select class="form-select" name="FamilyAddressType">
                                        <asp:Repeater ID="addressopt1" runat="server">
                                            <%--         <option value="*">*</option>--%>
                                            <%--  <ItemTemplate>
                                              <%--  <%# addressoptstyle((Container.ItemIndex+1))%>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </select>
                                </div>--%>
                                <div class="form-group inline-group">
                                    <label for="">Country</label>
                                    <select class="form-select" name="FamilyCountry">
                                        <asp:Repeater ID="countoption2" runat="server">
                                            <%--         <option value="*">*</option>--%>
                                            <ItemTemplate>
                                                <%# countoptionstyle((Container.ItemIndex+1))%>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </select>
                                </div>
                                <div class="form-group inline-group">
                                    <label for="City">City</label>
                                    <input type="text" name="FamilyCity" class="form-input">
                                    <%--    <select name="" class="form-select">
                           <option value="*">*</option>
                        </select>--%>
                                </div>
                            </div>
                            <div class="row">
                                <div class="form-group">
                                    <label for="phoneinMacau">Phone:</label>
                                    <input name="FamilyPhone" type="text" class="form-input numberonly">
                                </div>
                            </div>
                            <div class="row">
                                <div class="form-group">
                                    <label for="home in Macau">Address:</label>
                                    <textarea name="FamilyAddress" id="" cols="55" rows="4" wrap="hard" style="margin-left: 0"></textarea>
                                </div>
                            </div>
                        </div>
                        <div class="center-block" id="addfamilybutton"><a href="#" class="addformbutton btn" onclick="addform('familyform','Family Contact','addfamilybutton')">Add Family Contact</a><a href="#" class="addformbutton btn" onclick="removeform('familyform')" style="margin-left: 15px;">Delete one Family Contact</a></div>
                    </div>
                    <div class="section" id="section3">

                        <h3>3.Emergency contact</h3>
                        <div class="form-basic" name="emerform">
                            <div class="row">
                                <div class=" form-group inline-group">
                                    <label for="family-relationship">Relationship:</label>
                                    <select name="Emergencyrelationship" id="" class="form-select" required="required">
                                        <asp:Repeater ID="relationop" runat="server">
                                            <%--         <option value="*">*</option>--%>
                                            <ItemTemplate>
                                                <%# relationoptionstyle((Container.ItemIndex+1))%>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </select>
                                </div>
                                <div class=" form-group inline-group">
                                    <label for="familyNameType"><span>*</span>NameType:</label>
                                    <select name="EmergencyNameType" id="" class="form-select" required="required">
                                        <option value="Chinese">Chinese</option>
                                        <option value="Chinese">English</option>
                                    </select>
                                </div>
                            </div>
                            <div class="row">
                                <div class="form-group inline-group">
                                    <label for=""><span>*</span>Last name:</label>
                                    <input name="EmergencyLastName" type="text" class="form-input" required="required">
                                </div>
                                <div class="form-group inline-group">
                                    <label for=""><span>*</span>First name:</label>
                                    <input name="EmergencyFirstName" type="text" class="form-input" required="required">
                                </div>
                            </div>
                            <div class="row">
                                <div class="form-group inline-group">
                                    <label for=""><span>*</span>Sex:</label>
                                    <select name="EmergencySex" id="" class="form-select" required="required">
                                        <option value="Male">Male</option>
                                        <option value="Female">Female</option>
                                    </select>
                                </div>
                                <div class="form-group inline-group">
                                    <label for="occupation"><span>*</span>Marital Status:</label>
                                     <select name="EMaritalStatus" class="form-select" onchange="marriedSt(this)" required="required">
                            <option value="Single">Single</option>
                            <option value="Married">Married</option>
                        </select>
                                </div>
                            </div>
                            <div class="row">
                                <%--<div class="form-group inline-group">
                                    <label for="addresstype">Address Type</label>
                                    <select name="EmergencyAddType" class="form-select">
                                        <asp:Repeater ID="addressopt" runat="server">
                                            <%--         <option value="*">*</option>--%>
                                        <%--      <ItemTemplate>
                                         <%--       <%# addressoptstyle((Container.ItemIndex+1))%>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </select>
                                </div>--%>
                                <div class="form-group inline-group">
                                    <label for="">Country</label>
                                    <select name="EmergencyCountry" class="form-select">
                                        <asp:Repeater ID="countoption" runat="server">
                                            <%--         <option value="*">*</option>--%>
                                            <ItemTemplate>
                                                <%# countoptionstyle((Container.ItemIndex+1))%>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </select>
                                </div>
                                <div class="form-group inline-group">
                                    <label for="City">City</label>
                                    <input type="text" name="EmergencyCity" class="form-input">
                                    <%--    <select name="" class="form-select">
                           <option value="">*</option>
                        </select>--%>
                                </div>
                            </div>
                            <div class="row">
                                <div class="form-group">
                                    <label for="phoneinMacau"><span>*</span>Phone:</label>
                                    <input type="text" name="EmergencyPhone" class="form-input numberonly">
                                </div>
                            </div>
                            <div class="row">
                                <div class="form-group">
                                    <label for="home in Macau">Address:</label>
                                    <textarea name="EmergencyAddress" id="" cols="55" rows="4" wrap="hard" style="margin-left: 0"></textarea>
                                </div>
                            </div>
                        </div>
                        <div class="center-block" id="addemerbutton"><a href="#" class="addformbutton btn" onclick="addform('emerform','Emergency Contact','addemerbutton')">Add Emergency contact</a><a href="#" class="addformbutton btn" onclick="removeform('emerform')" style="margin-left: 15px;">Delete one Emerg contact</a></div>
                    </div>
                    <div class="signature">
                        <input type="checkbox" id="confirmcheck" style="margin-left: 50px; float: left"><span>I hereby confirm that all information provided in this form is true and genuine. I understand that disciplinary action may be taken against me for any declaration.</span>
                    </div>
                    <div class="center-block"><a href="#" id="submitall" class="btn" onclick="submitall()">Submit</a></div>
                </div>
                <span id="formval" class="display:ture"></span>
    </form>
</body>
</html>
<style>
    *, *:before, *:after {
        -moz-box-sizing: border-box;
        box-sizing: border-box;
    }

    body {
        margin: 0;
        padding: 0;
        width: 100%;
        min-width: 100%;
        height: 100%;
        display: block;
        background-color: #fbfbfb;
        */ font-size: 13px;
        box-sizing: border-box;
    }

    h3 {
        font-size: 15px;
        border-top: 5px solid #6d5e8d;
    }

    h4 {
        font-size: 14px;
    }

    .wrapper {
        width: 720px;
        margin-top: 5px;
        margin-left: auto;
        margin-right: auto;
        background-color: #fff;
    }

    .hiddenelement {
        display: none;
    }

    .section {
        margin-top: 20px;
        border-bottom: 1px solid #ccc;
    }

    .form-basic {
        display: block;
        padding-bottom: 10px;
        border: 1px solid #eee;
        border-bottom: 1px solid #ccc;
    }

    .righthint {
        margin: 0;
        padding-left: 0;
        padding-right: 50px;
        /*		display: hidden;*/
        float: right;
        color: #cc3333;
        white-space: nowrap;
    }

    h3, h4 {
        margin: 0;
        padding-top: 5px;
        padding-bottom: 5px;
        padding-left: 15px;
        font-weight: 600;
        color: #333;
        background-color: #E0E0E0;
    }

    .form-group {
        display: block;
        clear: both;
        margin-top: 5px;
    }

    label {
        margin-left: 50px;
        float: left;
        width: 150px;
        font-size: 14px;
        font-weight: 600;
        color: #333;
    }

    textarea {
        margin-left: 20px;
        width: 250px;
        font-size: 13.5px;
        font-weight: 600;
        color: #555;
        background-color: #fff;
        border-radius: 4px;
        outline: 0;
        border: 1px solid #aaa;
        box-shadow: inset 0 1px 1px rgba(0,0,0,.075);
        -webkit-transition: border-color ease-in-out .15s,-webkit-box-shadow ease-in-out .15s, color .15s;
        transition: border-color ease-in-out .20s,box-shadow ease-in-out .20s, color: ease-in-out .20s;
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

    .form-select {
        margin-left: 20px;
        width: 200px;
        font-size: 13px;
        padding-left: 6px;
        font-weight: 600;
        color: #555;
        border-radius: 4px;
        outline: 0;
        border: 1px solid #aaa;
        box-shadow: inset 0 1px 1px rgba(0,0,0,.075);
        -webkit-transition: border-color ease-in-out .15s,-webkit-box-shadow ease-in-out .15s;
        transition: border-color ease-in-out .20s,box-shadow ease-in-out .20s,color: ease-in-out .20s;
    }

        .form-select:focus {
            font-size: 13px;
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

    .row {
        padding-top: 10px;
        padding-bottom: 10px;
        clear: both;
        border-top: 1px solid #eee;
    }

        .row label {
            display: inline-block;
            vertical-align: central;
            width: 100px;
        }

        .row input {
            display: inline-block;
            width: 150px;
            margin-left: 0;
        }

        .inline-group select, .row select {
            margin-left: 0;
            width: 150px;
        }

    .inline-group {
        display: inline-block;
        margin-left: auto;
        margin-right: auto;
    }

        .inline-group input:focus {
            font-size: 13px;
        }

    .hidden-form {
        display: none;
    }

    .signature {
        margin-left: auto;
        margin-right: auto;
        margin-top: 20px;
        margin-bottom: 20px;
        font-weight: 600;
        color: #563d7c;
        text-align: center;
    }

        .signature input[type = "checkbox"] {
            display: block;
            margin-left: auto;
            margin-right: auto;
            width: 20px;
            height: 20px;
            background: #fff;
        }

    .center-block {
        clear: both;
        text-align: center;
    }

    .btn {
        display: inline-block;
        margin-bottom: 20px;
        margin-top: 20px;
        margin-left: auto;
        margin-right: auto;
        vertical-align: central;
        cursor: pointer;
        padding: 10px 26px;
        font-size: 18px;
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


     

        .btn:hover {
            color: #fff;
            background-color: #563d7c;
            border-color: #563d7c;
        }

    .has-error {
        border-color: #a94442;
        -webkit-box-shadow: inset 0 1px 1px rgba(0,0,0,.075);
        box-shadow: inset 0 1px 1px rgba(0,0,0,.075);
    }

        .has-error:focus {
            border-color: #843534;
            -webkit-box-shadow: inset 0 1px 1px rgba(0,0,0,.075),0 0 6px #ce8483;
            box-shadow: inset 0 1px 1px rgba(0,0,0,.075),0 0 6px #ce8483;
        }
    /*	Custom Jquery ui*/
    #ui-datepicker-div {
        width: 250px;
        font-size: 13px;
    }

    .ui-datepicker-header {
        background: #513c8a;
    }

    .ui-tooltip {
        font-size: 12px;
        width: 200px;
    }

    .ui-datepicker .ui-datepicker-prev span, .ui-datepicker .ui-datepicker-next span {
        background-color: #fff;
    }
</style>
<script>
    $(function () {
        $(document).tooltip();
    });

    $("body").on("focus", ".datepicker", function () {
        $(this).datepicker({ changeMonth: true, changeYear: true });
    });
    $("body").on("focusout", ".has-error", function () {
        reValiateElement($(this));
    })
    function addform(formname, header, btnid) {
        var formname = formname;
        var btnid = btnid;
        var header = header;
        var maxformnum = 0;
        var form = $("div[name =" + formname + "]")[0];
        var formnum = $("div[name = " + formname + "]").length;
        var formheader = null;
        $(".datepicker").datepicker("destroy");
        var formHtml = $($("div[name =" + formname + "]")[0]).html();
        event.preventDefault ? event.preventDefault() : (event.returnValue = false);
        if (formname === 'familyform') { maxformnum = 10; }
        else if (formname === 'emerform') { maxformnum = 3; }
        if (formnum < maxformnum) {
            formnum++;
            var formheader = "<h3>" + header + formnum + "</h3>";
            $newform = $("<div  class='form-basic' name= " + formname + ">" + formheader + formHtml + "</div>");
            $newform.insertBefore($("#" + btnid));
            $newform.find(".has-error").removeClass("has-error");
            $('html,body').animate({
                scrollTop: ($newform.offset().top - 50)
            }, 500);
            // $($(".datepikcer")[formnum-1]).removeAttr('id');
            $(".datepicker").removeAttr('id');
        }
    }

    function removeform(formname) {
        event.preventDefault ? event.preventDefault() : (event.returnValue = false);
        var formname = formname;
        if ((formnum = $("div[name = " + formname + "]").length) > 1) {
            $($("div[name =" + formname + "]")[formnum - 1]).detach();
        }
    }

    function checkgrouponly(o) {

        //var checkgroup = document.getElementsByName(o.name);
        var checkgroup = $(".prefer");
        if (o.checked) {

            for (var i = checkgroup.length - 1; i >= 0; i--) {
                if (checkgroup[i] != o) {

                    if (checkgroup[i].checked)
                    { checkgroup[i].checked = false; }



                }

            }
        }
    }
    function marriedSt(o) {
        var mstatus = o.options[o.selectedIndex].text;
        if (mstatus === 'Married') {
            $('input[name = marriedDate]').prop("disabled", false);
        } else { $('input[name = marriedDate]').prop("disabled", true); }
    }

    function date(o) {
        o.datepicker();
    }
    function submitall() {

        event.returnValue = false;
        event.preventDefault = false;


        ////跳过检查测试
        //var inputdate = "";
        //var forms = $("form");            
        //x = $(forms[0]).serializeArray();
        //$.each(x, function (i, field) { 
        //   if(field.name=="Username"||field.name=="Family-relationship"||field.name=="Emergency-relationship")
        //        inputdate += "||";
        //    inputdate += field.name + "^^" + field.value + "#$#";
        //});
        //console.log(inputdate);
        //$.post('PIF_Data_Save.aspx', {
        //    inputdate1: inputdate
        //}, function (data) {
        //    alert(data);
           
        //});



        //检查
        //清空错误
        $(".has-error").each(function (index, element) { $(element).removeClass("has-error"); });
        //检查必填
        $("input[required = 'required'],select[required = 'required']").each(function (index, element) {
            var inputval = $(element).val();
            if (!inputval || inputval === null) { $(element).addClass("has-error") }; $(element).prop('title', 'must filled')
        });
        //检查文本
        $(".textonly").each(function (index, element) {
            var element = $(element);
            var namevalue = element.val();
            if ((namevalue != "") && !valiateName(namevalue, "textonly")) {
                element.attr('title', 'required and english only');
                //console.log();
                element.addClass("has-error");
            }
        });
        //检查电话
        $(".phone").each(function (index, element) {
            var element = $(element);
            var namevalue = element.val();
            if ((namevalue != "") && !valiateName(namevalue, "phone")) {
                element.attr('title', 'required and numbers only');
                element.addClass("has-error");
            }
        });

        //检查数字
        $(".numberonly").each(function (index, element) {
            var element = $(element);
            var namevalue = element.val();
            if ((namevalue != "") && !valiateName(namevalue, "numberonly")) {
                element.attr('title', 'required and numbers only');
                element.addClass("has-error");
            }
        });

        //检查prefer
        var checkgroup = $(".prefer");
        var checkcount = 0;
        for (var i = checkgroup.length - 1; i >= 0; i--) {
            if (checkgroup[i].checked)
            { checkcount++ }
        }

        if (checkcount == 0) {
            checkgroup.attr('title', 'required to select at least one between Macau and Homebase');
            checkgroup.addClass("has-error");
        }

        //要勾选confirm
        if (!$("#confirmcheck").is(':checked')) {

            alert("Please confirm your form in the last checkbox");

        } else {
            //如果有错
            if ($(".has-error").length) {
                $($(".has-error")[0]).focus();
            } else {
                //无错
                var inputdate = "";
                var forms = $("form");
                for (var i = 0; i < forms.length; i++) {
                    inputdate += "||";
                    x = $(forms[i]).serializeArray();
                    $.each(x, function (i, field) {
                        if (field.name == "Username" || field.name == "Familyrelationship" || field.name == "Emergencyrelationship")
                            inputdate += "||";
                        inputdate += field.name + "^^" + field.value + "#$#";
                    });

                }
       
                console.log(inputdate);

                $.post('PIF_Data_Save.aspx', {
                    inputdate1: inputdate
                }, function (result) {
                 
                    alert(result);
                    if (result == "Insert success.")
                    $("form").trigger('reset');
                });
                //alert("Submit success");
            };
        }
       
        //$("form").each(function (index, form) {
        //    $(form).find("input").val("");
        //    $(form).find("select").val("");
        //    $(form).find("checkbox").val("false")
        //    $(form).find("textarea").val("");
        //});


    }

    function reValiateElement(element) {
        var element = $(element);

        if (element.attr("required") === "required") {
            if ((element.val() != "") && (element.hasClass("has-error")) && !element.hasClass("textonly") && !element.hasClass("numberonly")) {

                element.removeClass("has-error");
            }
        }

        if (element.hasClass("textonly")) {
            if (valiateName(element.val(), "textonly")) {

                if (element.hasClass("has-error")) {
                    element.removeClass("has-error");
                }
            } else {

            }
        }
        if (element.hasClass("numberonly")) {
            if (valiateName(element.val(), "numberonly")) {
                if (element.hasClass("has-error")) {
                    element.removeClass("has-error");
                }
            }
        }
        if (element.hasClass("phone")) {
            if (valiateName(element.val(), "phone")) {
                if (element.hasClass("has-error")) {
                    element.removeClass("has-error");
                }
            }
        }
    }

    //form valiation function` 
    function valiateName(value, content) {
        switch (content) {
            case 'textonly':
                if (/^[a-zA-Z][a-zA-z._, \-]+$/.test(value) ) {
                    return true;
                }
                else {
                  
                    return false;
                }
                break;
            case 'numberonly':
                if (/^[0-9]+$/.test(value)) {
                    return true;
                }
                else {
                    return false;
                }
            case 'phone':
                if (/^[0-9.-]+$/.test(value)) {
                    return true;
                }
                else {
                    return false;
                }
        }
    }


</script>
