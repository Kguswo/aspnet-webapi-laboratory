using Microsoft.Data.SqlClient;
using System.Data;

namespace DapperMvcDemo.Data
{
    /*
    Dapper를 사용하여 데이터베이스 연결을 관리하는 DbContext 클래스 - 앱이 db 접속할 때 필요한 연결정보 보관하고 필요할 때 연결 만들어주는 역할

    appsettings.json             DapperDbContext              Repository
    ┌─────────────────────┐      ┌─────────────────────┐         ┌─────────────────┐
    │ "DefaultConnection":       │                     │         │                 │
    │ "Server=..."        │ ──▶  │ 연결문자열 저장     │ ──▶     │ DB 쿼리 실행    │
    └─────────────────────┘      │                     │         │                 │
                                 │ CreateConnection()  │          connection.Query()
                                 │ → SqlConnection 반환│         │
                                 └─────────────────────┘         └─────────────────┘
    */
    public class DapperDbContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        // 생성자 - 설정 파일에서 연결 문자열 가져오기
        public DapperDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        // DB 연결 객체 만들어서 반환
        public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
    }
}
