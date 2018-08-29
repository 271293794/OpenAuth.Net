using System;
using System.Collections.Generic;
using System.Linq;
using OpenAuth.App.Request;
using OpenAuth.App.Response;
using OpenAuth.App.SSO;
using OpenAuth.Repository.Domain;


namespace OpenAuth.App
{
    public class UserManagerApp : BaseApp<User>
    {
        public RevelanceManagerApp ReleManagerApp { get; set; }

        public User Get(string account)
        {
            return Repository.FindSingle(u => u.Account == account);
        }

        /// <summary>
        /// 加载当前登录用户可访问的一个部门及子部门全部用户
        /// </summary>
        public TableData Load(QueryUserListReq request)
        {
            var loginUser = AuthUtil.GetCurrentUser();
            // 节点层次ID，如 【集团总部】为【.0.1.】
            // 【研发部】为【.0.1.3.】，子节点【研发小组】为【.0.1.3.1.】
            string cascadeId = ".0.";
            if (!string.IsNullOrEmpty(request.orgId))
            {
                var org = loginUser.Orgs.SingleOrDefault(u => u.Id == request.orgId);
                cascadeId = org.CascadeId;
            }
            // 用户所属部门ID数组
            var ids = loginUser.Orgs.Where(u => u.CascadeId.Contains(cascadeId)).Select(u => u.Id).ToArray();
            // 与此用户同部门的所有用户的ID
            var userIds = ReleManagerApp.Get(Define.USERORG, false, ids);

            var users = UnitWork.Find<User>(u => userIds.Contains(u.Id))
                   .OrderBy(u => u.Name)
                   .Skip((request.page - 1) * request.limit)
                   .Take(request.limit);
            // Repository 为父类【BaseApp<User>】中的属性
            var records = Repository.GetCount(u => userIds.Contains(u.Id));
            


            var userviews = new List<UserView>();
            foreach (var user in users.ToList())
            {
                UserView uv = user;
                var orgs = LoadByUser(user.Id);
                uv.Organizations = string.Join(",", orgs.Select(u => u.Name).ToList());
                uv.OrganizationIds = string.Join(",", orgs.Select(u => u.Id).ToList());
                userviews.Add(uv);
            }

            return new TableData
            {
                count = records,
                data = userviews,
            };
        }

        public void AddOrUpdate(UserView view)
        {
            if (string.IsNullOrEmpty(view.OrganizationIds))
                throw new Exception("请为用户分配机构");
            // 隐式转换，见【UserView】类，UserView类比 User 类多了部门信息
            User user = view;
            if (string.IsNullOrEmpty(view.Id))
            {
                if (UnitWork.IsExist<User>(u => u.Account == view.Account))
                {
                    throw new Exception("用户账号已存在");
                }
                user.CreateTime = DateTime.Now;
                user.Password = user.Account; //初始密码与账号相同
                Repository.Add(user);
                view.Id = user.Id;   //要把保存后的ID存入view
            }
            else
            {
                UnitWork.Update<User>(u => u.Id == view.Id, u => new User
                {
                    Account = user.Account,
                    BizCode = user.BizCode,
                    Name = user.Name,
                    Sex = user.Sex,
                    Status = user.Status
                });
            }
            string[] orgIds = view.OrganizationIds.Split(',').ToArray();

            ReleManagerApp.DeleteBy(Define.USERORG, user.Id);
            // ToLookup方法：创建一个1 => n 的映射。 它可以方便的将数据分类成组
            // 就像在字典中字母D对应很多字，如：邓、大、当
            // 添加部门信息
            ReleManagerApp.AddRelevance(Define.USERORG, orgIds.ToLookup(u => user.Id));
        }

        /// <summary>
        /// 加载用户所属的所有机构
        /// </summary>
        public IEnumerable<Org> LoadByUser(string userId)
        {
            /* 等价 SQL 
             * SELECT  o.*
             * FROM    [OpenAuthDB].[dbo].[Relevance] AS r
             *         JOIN dbo.Org AS o ON r.SecondId = o.Id
             * WHERE   r.FirstId = '6ba79766-faa0-4259-8139-a4a6d35784e0'
             *         AND r.[Key] = 'UserOrg';
             *
             */
            var result = from userorg in UnitWork.Find<Relevance>(null)
                         join org in UnitWork.Find<Org>(null) on userorg.SecondId equals org.Id
                         where userorg.FirstId == userId && userorg.Key == Define.USERORG
                         select org;
            return result;
        }


    }
}