using Org.BouncyCastle.Bcpg.OpenPgp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Estatistica.BusinessLogicLayer.Utils
{
    public static class XmlUtils
    {
        public static XmlDocument LoadFromContent(string xmlContent)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlContent);
            return doc;
        }
        public static bool ValidateSigniture(XmlDocument doc)
        {
            var signedXml = new SignedXml(doc);
            XmlNodeList nodeList = doc.GetElementsByTagName("Signature");

            if (nodeList.Count == 0)
                throw new Exception("Assinatura digital não encontrada.");

            signedXml.LoadXml((XmlElement)nodeList[0]);

            return signedXml.CheckSignature();
        }
    }
}
