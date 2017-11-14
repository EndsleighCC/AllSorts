using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Text.RegularExpressions;

namespace TestXML
{
    public partial class Mainform : Form
    {
        public Mainform()
        {
            InitializeComponent();
        }

        public string TestHostName
        {
            get { /*System.Net.Dns.GetHostName()*/ return "adebs03"; }
        }

        public string PromotionLevel
        {
            get { return "st04"; }
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            XmlDocument buildEnvironmentDocument = new XmlDocument();
            buildEnvironmentDocument.Load(@"C:\ST04\Endsleigh\Build\Console\buildenvironment.xml");

            XmlNodeList buildNodeList =
                buildEnvironmentDocument.SelectNodes("/buildenvironment/server[@name='" + TestHostName +
                                                     "']");

            string sysbuild =
                buildEnvironmentDocument.SelectSingleNode("/buildenvironment/server[@name='" +
                                                          TestHostName + "']").Attributes.GetNamedItem(
                                                              "sysbuild").Value;

            foreach ( XmlElement xmlelement in buildNodeList )
            {
                string BuildPath =
                    xmlelement.SelectSingleNode("promotion[@level='" + PromotionLevel + "']").Attributes.GetNamedItem(
                        "path").Value;

                string BuildConfigPath = System.IO.Path.Combine(BuildPath,
                                                                xmlelement.SelectSingleNode("buildfile").Attributes.
                                                                    GetNamedItem("relativepath").Value);

                XmlNodeList environmentNodeList = xmlelement.SelectNodes("environment");

                foreach ( XmlNode xmlenvelement in environmentNodeList)
                {
                    XmlNodeList pathNodeList = xmlenvelement.SelectNodes("searchpath");

                    foreach (XmlElement xmlpathelement in pathNodeList)
                    {
                        string searchPath = xmlpathelement.GetAttribute("path");
                        if ( ! (xmlpathelement.GetAttribute("relative") == null ))
                        {
                            if (xmlpathelement.GetAttribute("relative").ToLower() == "true" )
                            {
                                searchPath = String.Format("{0}\\{1}", BuildPath, searchPath);
                            }
                            
                        }
                    }

                    XmlNodeList envvarNodeList = xmlenvelement.SelectNodes("environmentvariablepath");

                    foreach (XmlElement xmlpathelement in envvarNodeList)
                    {
                        string name = xmlpathelement.GetAttribute("name");
                        string path = xmlpathelement.GetAttribute("path");
                        if ( ! Regex.IsMatch(path,"[a-z]:.*",RegexOptions.IgnoreCase))
                        {
                            // Relative path
                            path = Path.Combine(BuildPath, path);
                        }
                        txtOutput.Text += String.Format("{0} = \"{1}\"",name,path)+Environment.NewLine;
                    }
                }
            }
        }
    }
}
