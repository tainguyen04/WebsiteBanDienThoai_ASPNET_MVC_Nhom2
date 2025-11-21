namespace QLCHBanDienThoaiMoi.Helpers
{
    public class SessionHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public SessionHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public string EnsureSessionIdExists()
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            if(session == null)
            {
                throw new InvalidOperationException("Session is not available.");
            }
            var sesionId = session.GetString("SessionId")?.Trim();
            if(string.IsNullOrEmpty(sesionId))
            {
                sesionId = Guid.NewGuid().ToString();
                session.SetString("SessionId", sesionId);
            }
            return sesionId;
        }
        public int? GetKhachHangId()
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            if (session == null)
            {
                throw new InvalidOperationException("Session is not available.");
            }
            return session.GetInt32("KhachHangId");
        }
    }
}
