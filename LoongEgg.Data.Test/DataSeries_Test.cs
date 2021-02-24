using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;

namespace LoongEgg.Data.Test
{
    [TestClass]
    public class DataSeries_Test
    {
        [TestMethod]
        public void Group_IsSetInInnerHandler_Before_OuterCollectionChangedEventRaised()
        {
            var group = new DataSeriesCollection();
            var series = new DataSeries();

            Assert.AreEqual(1, group.Id);
            Assert.AreEqual(0, series.GroupId);

            group.CollectionChanged += (s, e) =>
            {
                var newItems = e.NewItems as IEnumerable<DataSeries>;
                if (newItems == null) return;

                foreach (var item in newItems)
                {
                    Assert.AreEqual(group.Id, item.GroupId);
                }
            };

            group.Add(series);
            group = null;
        }
    }
}
