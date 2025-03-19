namespace RChat.Domain.Members.Repository;

public interface IMemberRepository
{
    Task<Member?> GetAsync(GetMemberParameters parameters);
    
    Task<List<Member>> GetListAsync(GetMemberListParameters parameters);
    
    Task<int> CreateAsync(Member member);

    Task<int[]> CreateAsync(IEnumerable<Member> members);
    
    Task DeleteAsync(int memberId);
}