<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PrintTest.aspx.cs" Inherits="PrintTest" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>


    <style media="print">
        .Noprint {
            display: none;
        }

        .PageNext {
            page-break-after: always;
        }
    </style>

    <script>
        A = "ZERO^ONE^TWO^THREE^FOUR^FIVE^SIX^SEVEN^EIGHT^NINE^TEN^ELEVEN^TWELVE^THIRTEEN^FOURTEEN^FIFTEEN^SIXTEEN^SEVENTEEN^EIGHTEEN^NINETEEN^^TWENTY^THIRTY^FORTY^FIFTY^SIXTY^SEVENTY^EIGHTY^NINETY^HUNDRED^^THOUSAND^MILLION^BILLION^TRILLION^QUADRILLION^QUINTI LLION".split("^");
        B = new Array();
        for (i = 0; i < 1000; i++) {
            B[i] = i < 20 ? A[i] : i < 100 ? A[19 + Math.floor(i / 10)] + (i % 10 == 0 ? "" : "-" + B[i % 10]) : A[Math.floor(i / 100)] + " " + A[29] + (i % 100 == 0 ? "" : " and " + B[i % 100]);
        }
        function readNum() {
            text = "";
            
            with (num2En.arabNum.value.split(".")[0].split(",").join("")) {
                for (i = 0; i < Math.ceil(length / 3) ; i++) {
                    sec = parseInt(substring(length - 3 * i - 3, length - 3 * i), 10);
                    text = (i == 0 && sec > 0 && sec < 100 && parseInt(substring(0, length - 3), 10) > 0 ? " and " : "") + (sec == 0 && (i > 0 || toString(10) != 0) ? "" : B[sec]) + (sec == 0 ? "" : " " + (typeof A[30 + i] == "undefined" ? "undefined".fontcolor("red") : A[30 + i])) + (i == 0 || sec == 0 || (sec > 0 && text == "") ? "" : " ".fontcolor("black") + " ") + text;
                }
            }
    
            text = text + ' ONLY'
            return readout.innerHTML = (text.indexOf("undefined") == -1 ? "" : typeof alert("Digital long, will not correct output！") == "undefined" && num2En.arabNum.focus() ? "" : "") + "&nbsp;" + text;
        }

    </script>
</head>
<body>
    <form id="num2En" runat="server">
        
        <div class=" Noprint">
            <p>不想打印这个</p>
            <input id="arabNum"  />

        </div>
        <div class=" PageNext">
            <p>第一页</p>
            <input type="button" onclick="readNum()"/>
            <div id="readout"></div>
        </div>
        <div class=" PageNext">
            <p>第二页</p>
        </div>
        <div>
            <p>第三页</p>
        </div>
    </form>
</body>
</html>
