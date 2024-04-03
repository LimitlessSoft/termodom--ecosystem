using FirebirdSql.Data.FirebirdClient;
using LSCore.Contracts.Http;
using TD.OfficeServer.Contracts.Helpers;
using TD.OfficeServer.Contracts.IManagers;
using TD.OfficeServer.Contracts.Requests.SMS;

namespace TD.OfficeServer.Domain.Managers
{
    public class SMSManager : ISMSManager
    {
        public string ConnectionString { get; set; }

        private static List<string> ControlNumbers { get; set; } = new List<string>()
        {
            "0693691472",
            "063245200",
            "0641083932",
            "0698801503",
            "0698801508"
        };

        public LSCoreResponse Queue(SMSQueueRequest request)
        {
            var response = new LSCoreResponse();

            foreach(var control in ControlNumbers)
            {
                if (request.Numbers.All(x => MobilePhoneHelpers.GenarateValidNumber(x) != MobilePhoneHelpers.GenarateValidNumber(control)))
                    request.Numbers.Add(control);

            }

            using (var con = new FbConnection(ConnectionString))
            {
                con.Open();
                using(var cmd = new FbCommand("INSERT INTO SMS(ID, MOBILE, TEXT, IS_READ, STATUS, \"DATE\") VALUES(((SELECT COALESCE(MAX(ID),0) FROM SMS)+1) ,@MOBILE, @TEXT, 0, 2, @DATE)", con))
                {
                    cmd.Parameters.Add("@MOBILE", FbDbType.VarChar);
                    cmd.Parameters.AddWithValue("@DATE", DateTime.UtcNow);
                    cmd.Parameters.AddWithValue("@TEXT", request.Text);

                    foreach(var mobile in request.Numbers)
                    {
                        cmd.Parameters["@MOBILE"].Value = MobilePhoneHelpers.GenarateValidNumber(mobile);

                        cmd.ExecuteNonQuery();
                    }

                }
            }
            return response;
        }
    }
}
