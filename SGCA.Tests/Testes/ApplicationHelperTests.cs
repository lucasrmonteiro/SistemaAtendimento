using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SGCA.Models.Util;

namespace SGCA.Tests.Testes
{
    class ApplicationHelperTests
    {
        [Test]
        public void TesteLimpaCaracteresCpf()
        {
            Assert.AreEqual("11626296731",CpfUtil.LimpaCarateresCpf("116.262.967-31"));
        }

        [Test]
        public void ValidaCPFReal()
        {
            Assert.AreEqual(true, CpfUtil.ValidaCPF("116.262.967-31"));
        }

        [Test]
        public void ValidaCPFFalso()
        {
            Assert.AreEqual(false, CpfUtil.ValidaCPF("111.111.111-11"));
        }
    }
}
