using System;
using System.Collections.Generic;
using SecondHand.model;

namespace SecondHand.Model
{
    public enum Platform
    {
        Android,
        IPhone,
        Web
    }

    public class StudentCredential
    {
        public string Token { get; set; }
        public Student Credential { get; set; }
        public DateTimeOffset ExpireDate { get; set; } = DateTimeOffset.Now.AddDays(15);
        public Platform Platform { get; set; } = Platform.Web;
    }

    public class AdminCredential
    {
        public string Token { get; set; }
        public Admin Credential { get; set; }
        public DateTimeOffset ExpireDate { get; set; } = DateTimeOffset.Now.AddDays(15);
        public Platform Platform { get; set; } = Platform.Web;
    }

    public class AddressDetail
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
    }

    public class MyGoods
    {
        //我发布的
        public List<Commodity> AllMyCommodities { get; set; } = new List<Commodity>();

        //我卖出去的
        public List<SalesRecord> Sold { get; set; } = new List<SalesRecord>();

        //我买到的，过滤一下其中的check字段即可得到已付款但未签收的商品
        public List<SalesRecord> Bought { get; set; } = new List<SalesRecord>();
    }
}