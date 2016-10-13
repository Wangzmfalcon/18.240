<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link rel="icon" href="imges/airmacau.ico" />
    <title>Login</title>

    <%-- css --%>
    <link href="bootstrap-3.3.5-dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="bootstrap-3.3.5-dist/css/signin.css" rel="stylesheet" />

    <style>
        #login {
  
        }
    </style>
    <%-- script --%>
    <script src="bootstrap-3.3.5-dist/js/bootstrap.js"></script>
    <script src="bootstrap-3.3.5-dist/js/bootstrap.min.js"></script>
</head>
<body>
    <form runat="server">
        <div id="layout">
            <div id="systitle">
                <div class="logo">
                    <img src="imges/logo.gif" width="152" height="40" />
                </div>
                <div class="toptitle">Crew Qualification Monitoring System<span style="font-size: 46px; font-weight: normal; padding: 5px"></span></div>

            </div>
        </div>
        <div id="mainbody">
            <div style="border-bottom: 4px solid #D80425;"></div>
            <%--<div style="border-bottom: 8px solid #D80425;"></div>--%>
            <div style="border-bottom: 8px solid #322a65;"></div>

            <div class="container" style="margin-top: 400px; margin-left: 200px">
                <div id="form1" class="form-signin">
                    <h2 class="form-signin-heading">Login</h2>
                    <label for="inputEmail" class="sr-only">Email address</label>
                    <input type="email" id="inputEmail" class="form-control" placeholder="User Name" >
                    <label for="inputPassword" class="sr-only">Password</label>
                    <input type="password" id="inputPassword" class="form-control" placeholder="Password" >

                    <button class="btn btn-lg btn-primary btn-block" type="submit" id="login">Sign in</button>
                </div>
            </div>

        </div>
        <footer class="footer">
            <div class="container">
                <p class="text-center">Copyright © 2016 - All Rights Reserved Air Macau Company IT Division.</p>
            </div>
        </footer>


    </form>

</body>
</html>

