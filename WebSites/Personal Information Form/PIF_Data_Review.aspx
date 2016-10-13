<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PIF_Data_Review.aspx.cs" Inherits="PIF_Data_Review" %>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8" content="text/html; charset=utf-8">
    <title>Personal Information</title>

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
                        <label><%=username%></label>
                        <label for="" class="righthint hiddenelement">must not null</label>
                    </div>
                    <div class="form-group">
                        <label for="Name"><span>*</span>Staff No:</label>
                          <label><%=staffid%></label>
                        <label for="" class="righthint hiddenelement">must not null</label>
                    </div>
                    <div class="form-group ">
                        <label for="Name"><span>*</span>Marital Status:</label>
                        <label><%=MarS%></label>
                        <label for="" class="righthint hiddenelement">must not null</label>
                    </div>
                    <div class="form-group disabel-function">
                        <label for="Name">Marital Date:</label>
                  <label><%=MarD%></label>
                    </div>
                    <br>
                    <h4>Address in Macau:</h4>
                    <div class="form-group">
                        <label for="home in Macau">Address:</label>
                        <textarea name="AddressinMacau" id="" cols="55" rows="4" wrap="hard"  disabled="disabled"><%=Macauadd%></textarea>
                    </div>
                    <div class="form-group">
                        <label for="PhoneinMacau">Phone:</label>
                      <label><%=Macauphone%></label>
                    </div>
                    <div class="form-group">
                        <label for="MacauExtention">Extention:</label>
                        <label><%=Macauexention%></label>
                    </div>
                    <div class="form-group">
                        <label for="prefer-address">Prefered :</label>
                    
                        <input class="prefer" id="prefermacau" runat="server" type="checkbox" name="MAddressprefered" disabled="disabled" ></input>
                    </div>
                    <h4>Address in Homebase:</h4>
                    <div class="form-group">
                        <label for="homecountry">Country:</label>
                      <label><%=HomeCountry%></label>
                    </div>
                    <div class="form-group">
                        <label for="homeinhomebase">Address:</label>
                        <textarea name="AddressinHomebase" id="" cols="55" rows="4" wrap="hard" disabled="disabled">       <%=Homeadd%></textarea>
                    </div>
                    <div class="form-group">
                        <label for="phoneinhomebase">Phone:</label>
                            <label><%=Homephone%></label>
                    </div>
                    <div class="form-group">
                        <label for="hombasicextention">extention:</label>
                            <label><%=Homeexention%></label>
                    </div>
                    <div class="form-group">
                        <label for="prefer-address">Prefered:</label>
                        <input class="prefer" type="checkbox"  runat="server" id="preferhombase" name="HAddressprefered" disabled="disabled">
                    </div>
                    <div class="section" id="section2">

                                <asp:Repeater ID="family" runat="server">
                                            <%--         <option value="*">*</option>--%>
                                            <ItemTemplate>
                                                <%# familystyle((Container.ItemIndex+1))%>
                                            </ItemTemplate>
                                        </asp:Repeater>
                        


                           <asp:Repeater ID="Emergency" runat="server">
                                   
                                            <ItemTemplate>
                                                <%# Emergencystyle((Container.ItemIndex+1))%>
                                            </ItemTemplate>
                                        </asp:Repeater>
                        

                      
                       
                        
                    </div>
                   
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

