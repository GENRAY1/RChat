namespace RChat.Domain.Members;

public interface IMemberRepository
{
    Task<Member?> GetByIdAsync(int memberId);
    
    Task<List<Member>> GetListAsync(GetMemberListParameters parameters);
    
    Task<int> CreateAsync(Member member);

    Task<int[]> CreateAsync(IEnumerable<Member> members);
    
    Task DeleteAsync(int memberId);
}