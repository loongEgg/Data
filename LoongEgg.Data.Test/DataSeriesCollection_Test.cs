using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoongEgg.Data.Test
{
    [TestClass]
    public class DataSeriesCollection_Test
    {
        
        [TestMethod]
        public void Instance_CanBeRemove_When_Disposed()
        {
            // make sure the instance has clear
            DataSeriesCollection.Instances.Clear();
            Assert.AreEqual(0, DataSeriesCollection.Instances.Count);

            var group = new DataSeriesCollection();

            int id = group.Id;
            Assert.AreEqual(1, id);
            Assert.AreEqual(1, DataSeriesCollection.Instances.Count);

            // dipose manully
            group.Dispose();
            Assert.AreEqual(0, DataSeriesCollection.Instances.Count);

            // asing to a new instance
            group = new DataSeriesCollection();

            // auto disposed
            using (var autoDisposed = new DataSeriesCollection())
            {
                Assert.AreEqual(2, DataSeriesCollection.Instances.Count);
                Assert.AreEqual(2, autoDisposed.Id);
            }
            Assert.AreEqual(1, DataSeriesCollection.Instances.Count);
        }
    }
}
