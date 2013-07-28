using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace oyster.web.codegenerator
{
    class RazorResolver
    {
        public RazorResolver(string codeText)
        {
            UsingNames = new List<string>
            {
                "System",
                "System.Web",
                "System.Linq",
                "System.Text",
                "System.Collections.Generic",
            };
            OriginCode = codeText;
            StaticHtmlList = new Dictionary<int, string>();
            CodeList = new Dictionary<int, string>();
            OutCodeList = new Dictionary<int, string>();
            sectionCodeList = new Dictionary<int, KeyValuePair<string, RazorResolver>>();
            Init();
        }

        public string OriginCode { get; set; }
        public List<string> UsingNames { get; set; }
        public Dictionary<int, string> StaticHtmlList { get; set; }
        public Dictionary<int, string> CodeList { get; set; }
        public Dictionary<int, string> OutCodeList { get; set; }
        public Dictionary<int, KeyValuePair<string, RazorResolver>> sectionCodeList { get; set; }


        void Init()
        {
            DoResolve();
        }


        int _ndCount = 0;
        int NewNodeIndex
        {
            get
            {
                return _ndCount++;
            }
        }

        public int NodeCount
        {
            get { return _ndCount; }
        }

        public bool DoResolve()
        {
            string codeBody = DoUsingResolve();

            return DoHtmlCodeResolve(codeBody);
        }

        string DoUsingResolve()
        {
            int firstCodeIndex = OriginCode.IndexOf('<');
            int tindx = OriginCode.IndexOf("@{");
            if (tindx > 0)
            {
                firstCodeIndex = Math.Min(tindx, firstCodeIndex);
            }
            if (firstCodeIndex > 0)
            {
                string usingstr = OriginCode.Substring(0, firstCodeIndex);
                Regex regusing = new Regex("@using\\s+([^;]+);", RegexOptions.Singleline);

                regusing.Replace(usingstr, new MatchEvaluator((m) =>
                {
                    if (m.Success && m.Groups.Count > 1)
                    {
                        string nsp = m.Groups[1].Value;
                        if (!UsingNames.Contains(nsp))
                        {
                            UsingNames.Add(nsp);
                        }
                    }
                    return m.Value;
                }));
                UsingNames.Sort();
            }

            return OriginCode.Substring(firstCodeIndex > 0 ? firstCodeIndex : 0);
        }

        bool DoHtmlCodeResolve(string code)
        {
            int i = 0;
            int ii = 0;

            int bgkh = 0;
            for (; i < code.Length; i++)
            {
                if (
                    (code[i] == '@' && (i == 0 || code[i - 1] != '@'))
                    || (bgkh > 0 && code[i] == '}')
                    || i > code.Length - 2
                    )
                {
                    if (i - ii > 0)
                        StaticHtmlList.Add(NewNodeIndex, code.Substring(ii, i - ii));
                    i += DoCSharpCodeResolve(code.Substring(i), out bgkh, bgkh);
                    bgkh = bgkh > 0 ? bgkh : 0;
                    ii = i;
                }
            }
            return true;
        }

        int DoCSharpCodeResolve(string code, out int inbgkh, int bgkh = 0)
        {
            //string codeEchoFormat = "\r\n            Echo(html, {0});";
            int i = 0;
            int ii = 0;
            bool bgyhleft = false;

            int bgkhleft = bgkh;
            bool canHtml = false;
            bool bigyhleft = false;
            bool inSection = false;

            for (; i < code.Length; i++)
            {
                if (code[i] == '"' && (i == 0 || code[i - 1] != '\\'))
                {
                    bgyhleft = !bgyhleft;
                }
                if (bgyhleft)
                    continue;

                if (code[i] == '@' && i < code.Length - 1)
                {
                    bool yhleft = false;
                    int khleft = 0;
                    int j = 0;
                    switch (code[i + 1])
                    {
                        //@(XXX)
                        case '(':
                            khleft++;
                            j = i + 2;
                            for (; j < code.Length; j++)
                            {
                                if (code[j] == '"' && code[j - 1] != '\\')
                                {
                                    yhleft = !yhleft;
                                }
                                if (yhleft)
                                    continue;
                                if (code[j] == '(')
                                {
                                    khleft++;
                                }
                                if (code[j] == ')')
                                {
                                    khleft--;
                                }
                                if (khleft > 0)
                                    continue;
                                j++;
                                break;
                            }
                            ii = i + 2;
                            int len1 = j - ii - 1;
                            if (len1 > 0 && code.Substring(ii, len1).Trim().Length > 0)
                                OutCodeList.Add(NewNodeIndex, code.Substring(ii, len1));
                            ii = j;
                            goto DoHtmlCodeResolve;
                        //@{XXX}
                        case '{':
                            j = i + 2;
                            ii = j;
                            break;

                        //@section XXX{}
                        case 's':
                            if (code[j + 1] == 's' && code[j + 2] == 'e' && code[j + 3] == 'c' && code[j + 4] == 't' && code[j + 5] == 'i' && code[j + 6] == 'o' && code[j + 7] == 'n' && code[j + 8] == ' ')
                            {
                                string sectionName = "";
                                string secText = "";
                                int secKh = 0;
                                int secBgidx = -1;
                                for (int secI = j + 9; secI < code.Length; secI++)
                                {
                                    if (code[secI] == '{')
                                    {
                                        if (secKh == 0)
                                        {
                                            sectionName = code.Substring(j + 9, secI - j - 9);
                                            secBgidx = secI + 1;
                                        }
                                        secKh++;
                                    }
                                    if (code[secI] == '}')
                                    {
                                        secKh--;
                                        if (secKh == 0)
                                        {
                                            secText = code.Substring(secBgidx, secI - secBgidx);
                                            j = secI + 1;
                                            ii = j;
                                            var sectionRz = new RazorResolver(secText);

                                            sectionCodeList.Add(NewNodeIndex,
                                                new KeyValuePair<string, RazorResolver>(sectionName, sectionRz));

                                            goto DoHtmlCodeResolve;
                                        }
                                    }
                                }
                            }
                            goto default;
                            break;
                        //@XXXX
                        default:

                            j = i + 1;
                            for (; j < code.Length; j++)
                            {
                                if (code[j] == '"' && code[j - 1] != '\\')
                                {
                                    yhleft = !yhleft;
                                }
                                if (yhleft)
                                    continue;
                                if (code[j] == '(')
                                {
                                    khleft++;
                                }
                                if (code[j] == ')')
                                {
                                    khleft--;
                                }
                                if (khleft > 0)
                                    continue;

                                int casc = (int)code[j];
                                //允许有.
                                if (casc == 46)
                                    continue;
                                //允许有0-9
                                if (casc > 47 && casc < 58)
                                    continue;
                                //允许有A-z
                                if (casc > 64 && casc < 123)
                                    continue;

                                j++;
                                break;
                            }

                            ii = i + 1;
                            int len2 = j - ii;
                            if (len2 > 0 && code.Substring(ii, len2).Trim().Length > 0)
                                OutCodeList.Add(NewNodeIndex, code.Substring(ii, len2));
                            ii = j;
                            goto DoHtmlCodeResolve;
                    }
                    //跳过处理的段
                    i = j;
                }
                if (code[i] == '{')
                {
                    bgkhleft++;
                }
                if (code[i] == '}')
                {
                    bgkhleft--;
                }

                if (code[i] == '"' && code[i - 1] != '\\')
                {
                    bigyhleft = !bigyhleft;
                }

                if (!bigyhleft && (code[i] == '\n' || code[i] == ';'))
                    canHtml = true;
                else if (!canHtml || (code[i] != ' ' && code[i] != '\r' && code[i] != '\n' && code[i] != '\t'
                    && code[i] != '<'))
                    canHtml = false;

                if (bgkhleft < 0 || (code[i] == '<' && canHtml))
                {
                    int len3 = i - ii;
                    if (len3 > 0)
                        CodeList.Add(NewNodeIndex, code.Substring(ii, len3));
                    ii = i + (bgkhleft < 0 ? 1 : 0);

                    goto DoHtmlCodeResolve;
                }
            }

            //外层用循环避免递归
        DoHtmlCodeResolve:
            inbgkh = bgkhleft;
            return ii;
        }
    }
}
