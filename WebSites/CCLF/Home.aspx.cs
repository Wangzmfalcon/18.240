﻿using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Home : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["CCLF_Manage_Code"] == null )
        {
            Server.Transfer("Login.aspx");
        }
        else
        { 
        
        }
    }
}