using MimeKit;
using System;
using System.Globalization;
using System.Windows.Controls;

namespace emailpipe.Helper
{
    public static class HelperListView
    {
        public static ListViewItem ConvertToListViewItem<T>(object data)
        {
            if (!(data is MimeMessage))
                return null;

            var dataForListView = (MimeMessage)data;

            var listViewItem = new ListViewItem();

            var emailDate = DateTime.Now.ToString(CultureInfo.CurrentCulture);

            if (dataForListView.Date != null)
                emailDate = ((DateTimeOffset)dataForListView.Date).ToString("yyyy-MM-dd HH:mm:ss");

            listViewItem.Content = new EmailListViewItem { Subject = dataForListView.Subject, Date = emailDate };
            listViewItem.Tag = dataForListView;

            return listViewItem;
        }
    }

    public class EmailListViewItem
    {
        public string Subject { get; set; }
        public string Date { get; set; }
    }
}
