
namespace oyster.web.themes
{
    using System;
    public class Default
    {
        public string DoAction()
        {
            string html = ";
html+=@"<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Transitional//EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"">
<html xmlns=""http://www.w3.org/1999/xhtml"">
<head>
    <title></title>
</head>
<body>
    ";
        int t = 4 + 6;
        System.Console.WriteLine("out:" + t.ToString());
    
html+=@"
</body>
</html>
";
            return html;
        }
    }
}
