using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Collections.Generic;
using System.Collections;
using System.Data;
using System.Xml;
using System.Data.SqlClient;
namespace WebApplication1
{
    /// <summary>
    /// WebService1 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class WebService1 : System.Web.Services.WebService
    {
        
       
   
        

        
       

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod]
        public List<string> Noemail()
        {
            List<string> noemail = new List<string> ();

           // string connString = @"Data Source=192.168.101.149;Initial Catalog=HCMPRD;Persist Security Info=True;User ID=ehr;Password=nxhr2015";
            string connString = "Server=192.168.101.149;uid=ehr;pwd=nxhr2015;database=HCMPRD";

            SqlConnection connection = new SqlConnection(connString); //连接到引入的数据库
            connection.Open();  // 打开数据库连接 
            string sql = String.Format("select EMPLID from AM_STAFF where EMAIL is null");  //获取
            SqlCommand command = new SqlCommand(sql, connection);   //创建 Command 对象
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                noemail.Add(reader.GetString(0).ToString());
            }
            reader.Close();
            connection.Close();
            return noemail;
        }
   
     



    }
}
