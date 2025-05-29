export interface PrivateChatMembers{
    firstMember: PrivateChatMember;
    secondMember: PrivateChatMember;
}

export interface PrivateChatMember{
    memberId: number;
    userId: number;
    firstname: string;
    lastname?: string;
    username?: string;
}