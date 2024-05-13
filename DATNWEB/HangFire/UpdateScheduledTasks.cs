using DATNWEB.Models;

namespace DATNWEB.HangFire
{
    public class UpdateScheduledTasks
    {
        QlPhimAnimeContext db = new QlPhimAnimeContext();
        public void RunDailyTask()
        {
            var lst = (from u in db.Users
                       join us in db.UserSubscriptions on u.UserId equals us.UserId
                       where us.ExpirationDate > DateTime.Now.AddDays(-1)
                       group us by new { u.UserId, us.PackageId } into grouped
                       select new
                       {
                           UserId = grouped.Key.UserId,
                           PackageID = grouped.Key.PackageId,
                           ExpirationDate = grouped.Max(x => x.ExpirationDate)
                       }).Distinct().ToList();

            lst = lst.OrderByDescending(x => x.UserId).ThenByDescending(x => x.PackageID).ToList();

            var lst1 = (from u in db.Users.ToList() // Chuyển danh sách User thành danh sách trong bộ nhớ
                        where u.UserType != 0 && !lst.Any(x => x.UserId == u.UserId)
                        select u).ToList();
            foreach (var a in lst1)
            {
                a.UserType = 0;
                db.Update(a);
            }
            foreach (var a in lst)
            {
                var user = db.Users.Find(a.UserId);
                if (a.PackageID == "SP0002" && user.UserType == 2)
                {
                    if (a.ExpirationDate < DateTime.Now)
                    {
                        user.UserType = 0;
                    }
                }
                if (a.PackageID == "SP0001" && user.UserType == 1)
                {
                    if(a.ExpirationDate < DateTime.Now)
                    {
                        user.UserType = 0;
                    }
                }
                if(a.PackageID == "SP0001" && user.UserType == 0)
                {
                    if(a.ExpirationDate > DateTime.Now)
                    {
                        user.UserType = 1;
                    }
                }
            }
            db.SaveChanges();

        }
    }
}
