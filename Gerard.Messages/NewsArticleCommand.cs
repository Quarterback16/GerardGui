using System;
//using System.Xml;

namespace Gerard.Messages
{
    public class NewsArticleCommand : ICommand
    {
        public DateTime ArticleDate { get; set; }

        public string ArticleText { get; set; }

        public override string ToString()
        {
            return $"Article {ArticleDate:u} {ArticleText}";
        }

        //public NewsArticleCommand(XmlNode node)
        //{
        //    foreach (XmlNode n in node.ChildNodes)
        //    {
        //        switch (n.Name)
        //        {
        //            case "text":
        //                ArticleText = n.InnerText;
        //                break;

        //            case "date":
        //                ArticleDate = DateTime.Parse(n.InnerText);
        //                break;
        //        }
        //    }
        //}

        public bool IsInDraftSeason()
        {
            var articleMonth = ArticleDate.Month;
            if (articleMonth > 3)
                return true;
            return false;
        }
    }
}
