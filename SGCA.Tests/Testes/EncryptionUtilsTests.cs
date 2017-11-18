using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SGCA.Models.Util;
using NUnit.Framework;

namespace SGCA.Tests.Testes
{
    class EncryptionUtilsTests
    {
        [Test]
        public void TestaSHA256Hash()
        {
           String senha =  EncryptionUtils.SHA256Hash("1");
           Assert.AreEqual("6b86b273ff34fce19d6b804eff5a3f5747ada4eaa22f1d49c01e52ddb7875b4b", senha);
        }
    }
}
