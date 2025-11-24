
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
            var claim = _httpContextAccessor.HttpContext?.User.FindFirst("KhachHangId");
            if (claim == null) return null;
            if(int.TryParse(claim.Value,out int id)) return id;
            return null;
        }
        public int? GetUserIdFromClaim()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user == null) return null;
            var userIdClaim = user.FindFirst("KhachHangId")?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                return null;
            return userId;
        }
    }
}
