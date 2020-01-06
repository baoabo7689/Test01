using MySql.Data.MySqlClient;
using NovaCash.Sportsbook.Clients.Criteria;

namespace NovaCash.Sportsbook.Clients.Repositories
{
    public interface IBetDetailRepository
    {
        void InsertBetDetailBatch(InsertBetDetailBatchCriteria criteria);

        int SelectBetDetailLastVersion(SelectBetDetailLastVersionCriteria criteria);

        void UpdateBetDetailLastVersion(MySqlConnection conn, MySqlTransaction sqlTxn, UpdateBetDetailLastVersionCriteria criteria);
    }
}